using Abloom2.Messages;
using Abloom2.Models;
using AbloomWorkingNode.Messages;
using Akka.Actor;
using Akka.Util.Internal;
using System.Collections.Concurrent;

namespace Abloom2.Actors.Processmanager.Processors
{
    internal class SendReceivePassProcessor : UntypedActor
    {
        private IActorRef? PasswordgeneratorRef { get; set; }
        private ConcurrentDictionary<Guid, List<string>> PreparedToSend { get; set; } = new ConcurrentDictionary<Guid, List<string>>();
        private Dictionary<Guid, SentPassword> SentPasswords { get; set; } = new Dictionary<Guid, SentPassword>();
        private string? Hash { get; set; }
        private bool isRequested = false;
        private bool IsFound { get; set; } = false;
        private TimeSpan ResponseTime { get; set; } = TimeSpan.FromMilliseconds(60000);


        protected override void PreStart()
        {
            PasswordgeneratorRef = Context.ActorOf<PasswordGenerator>("password-generator");
        }

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case "Ready for checking":

                    if (!PreparedToSend.IsEmpty && Hash != null && !IsFound)
                    {
                        var respondPath = Sender.Path.Address + "/user/node";
                        foreach (var item in SentPasswords)
                        {
                            if (item.Value.ExpectedResponseTime < DateTime.Now)
                            {
                                SentPasswords.AddOrSet(item.Key, new SentPassword(DateTime.Now, DateTime.Now + ResponseTime, item.Value.Passwords));
                                SentPasswords.TryGetValue(item.Key, out var sentPassword);

                                if (sentPassword != null)
                                    Context.ActorSelection(respondPath)
                                        .Tell(new SendToWorkinNode(sentPassword.Passwords, Hash, item.Key));
                                return;
                            }

                        }

                        var id = PreparedToSend.First().Key;
                        var passwords = PreparedToSend.First().Value;

                        PreparedToSend.TryRemove(new KeyValuePair<Guid, List<string>>(id, passwords));
                        SentPasswords.Add(id, new SentPassword(DateTime.Now, DateTime.Now + ResponseTime, passwords));

                        var data = new SendToWorkinNode(passwords, Hash, id);
                        Context.ActorSelection(respondPath).Tell(data);

                        if (PreparedToSend.IsEmpty)
                        {
                            PasswordgeneratorRef.Tell(new GetPasswordsIntervals(5, 250));
                            isRequested = true;
                        }

                        break;
                    }

                    if (PreparedToSend.IsEmpty && !isRequested)
                    {
                        PasswordgeneratorRef.Tell(new GetPasswordsIntervals(5, 250));
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
                        {
                            displayProcessor.Tell(new RespondFinishExecution(data.Password, data.IsFound));
                            IsFound = true;
                        }
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
