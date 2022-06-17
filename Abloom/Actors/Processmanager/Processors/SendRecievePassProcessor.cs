using Abloom.Messages;
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
    public class SendRecievePassProcessor : UntypedActor
    {
        private IActorRef PasswordgeneratorRef { get; set; }
        private ConcurrentDictionary<Guid, List<string>> PreparedToSend { get; set; } = new ConcurrentDictionary<Guid, List<string>>();
        private Dictionary<Guid, List<string>> SentPasswords { get; set; } = new Dictionary<Guid, List<string>>();
        private string Hash { get; set; }


        protected override void PreStart()
        {
            PasswordgeneratorRef = Context.ActorOf<PasswordGenerator>("password-generator");
        }

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case "Ready for checking":
                    
                    if(PreparedToSend.Count == 0 || Hash == null)
                    {
                        PasswordgeneratorRef.Tell(new GetPasswordsIntervals(10, 25));
                        Self.Forward(message);
                    }
                    else
                    {
                        var id = PreparedToSend.First().Key;
                        var passwords = PreparedToSend.First().Value;
                        PreparedToSend.TryRemove(new KeyValuePair<Guid, List<string>>(id, passwords));
                        SentPasswords.Add(id, passwords);
                        Sender.Tell(new SendToWorkinNode(passwords, Hash, id, Self));
                    }
                    break;

                case RespondPasswordIntervals data:
                    Task.Run(() =>
                    {
                        foreach (var item in data.Intervals)
                        {
                            PreparedToSend.GetOrAdd(Guid.NewGuid(), item);
                        }

                    });
                    break;

                case RespondPassword data:
                    SentPasswords.Remove(data.Id);

                    var displayProcessor = Context.ActorSelection("../display-processor");
                    displayProcessor.Tell(new SetCurrentCombination(data.IntervalSize));

                    if (data.IsFound && data.Password != "")
                        displayProcessor.Tell(new RespondFinishExecution(data.Password, data.IsFound));

                    break;

                case SetInitialData data:
                    Hash = data.Hash;
                    PasswordgeneratorRef.Tell(data);
                    break;

                //case RespondPassword data:
                //    SentPasswords.Remove(data.Id);
                //    Console.WriteLine(Context.AsInstanceOf<ActorCell>().Mailbox.MessageQueue.Count);
                //    var displayProcessor = Context.ActorSelection("../display-processor");

                //    displayProcessor.Tell(new SetCurrentCombination(data.IntervalSize));

                //    if (data.IsFound && data.Password != "")
                //    {
                //        TokenSource.Cancel();
                //        Context.Parent.Tell("StopSending");
                //        displayProcessor.Tell(new RespondFinishExecution(data.Password, data.IsFound));
                //    }
                //    //TODO? : send this notification to workers
                //    break;
            }
        }
    }
}
