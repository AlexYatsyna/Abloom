using Abloom.Messages;
using Abloom.Models;
using AbloomWorkingNode.Messages;
using Akka.Actor;
using Akka.Util.Internal;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Abloom.Actors.Processmanager.Processors
{
    public class SendReceivePassProcessor : UntypedActor
    {
        private IActorRef PasswordgeneratorRef { get; set; }
        private ConcurrentDictionary<Guid, List<string>> PreparedToSend { get; set; } = new ConcurrentDictionary<Guid, List<string>>();
        private Dictionary<Guid, SentPassword> SentPasswords { get; set; } = new Dictionary<Guid,SentPassword>();
        private string Hash { get; set; }
        private bool isRequested = false;


        protected override void PreStart()
        {
            PasswordgeneratorRef = Context.ActorOf<PasswordGenerator>("password-generator");
        }

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case "Ready for checking":

                    if (!PreparedToSend.IsEmpty && Hash != null)
                    {
                        var respondPath = Sender.Path.Address + "/user/node";
                        foreach (var item in SentPasswords)
                        {
                            if (item.Value.ExpectedResponseTime < DateTime.Now)
                            {
                                SentPasswords.AddOrSet(item.Key, new SentPassword(DateTime.Now, DateTime.Now + TimeSpan.FromMilliseconds(5500), item.Value.Passwords));
                                SentPasswords.TryGetValue(item.Key, out SentPassword sentPassword);

                                if (sentPassword != null)
                                    Context.ActorSelection(respondPath)
                                        .Tell(new SendToWorkinNode(sentPassword.Passwords, Hash, item.Key));
                                return;
                            }

                        }

                        var id = PreparedToSend.First().Key;
                        var passwords = PreparedToSend.First().Value;

                        PreparedToSend.TryRemove(new KeyValuePair<Guid, List<string>>(id, passwords));
                        SentPasswords.Add(id, new SentPassword(DateTime.Now, DateTime.Now + TimeSpan.FromMilliseconds(5500), passwords));

                        var data = new SendToWorkinNode(passwords, Hash, id);
                        Context.ActorSelection(respondPath).Tell(data);

                        if (PreparedToSend.IsEmpty)
                        {
                            PasswordgeneratorRef.Tell(new GetPasswordsIntervals(10, 25));
                            isRequested = true;
                        }

                        break;
                    }

                    if (PreparedToSend.IsEmpty && !isRequested)
                    {
                        PasswordgeneratorRef.Tell(new GetPasswordsIntervals(10, 25));
                        isRequested = true;
                    }

                    Self.Forward(message);
                    break;

                case RespondPasswordIntervals data:
                    Task.Run(() =>
                    {
                        foreach (var item in data.Intervals)
                        {
                            PreparedToSend.GetOrAdd(Guid.NewGuid(), item);
                        }
                        isRequested = false;
                    });
                    break;

                case RespondPassword data:
                    if (SentPasswords.Remove(data.Id))
                    {
                        var displayProcessor = Context.ActorSelection("../display-processor");
                        displayProcessor.Tell(new SetCurrentCombination(data.IntervalSize));

                        if (data.IsFound && data.Password != "")
                            displayProcessor.Tell(new RespondFinishExecution(data.Password, data.IsFound));
                    }
                    break;

                case SetInitialData data:
                    Hash = data.Hash;
                    PasswordgeneratorRef.Tell(data);
                    break;
            }
        }
    }
}
