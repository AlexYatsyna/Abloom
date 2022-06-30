using Abloom.Cryptography;
using Abloom.Messages;
using Abloom.Models;
using AbloomWorkingNode.Messages;
using Akka.Actor;
using Akka.Util.Internal;

namespace Abloom.Actors.Processmanager.Processors
{
    internal class SendReceivePassProcessor : UntypedActor
    {
        private Dictionary<Guid, SentPassword> SentPasswords { get; set; } = new();
        private PasswordGenerator Generator { get; set; }
        private string? Hash { get; set; }
        private bool IsFound { get; set; } = false;
        private TimeSpan CriticalResponseTime { get; set; } = TimeSpan.FromMilliseconds(60000);

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case "Ready for checking":

                    if (Hash != null && !IsFound)
                    {
                        var respondPath = Sender.Path.Address + "/user/node";
                        foreach (var item in SentPasswords)
                        {
                            if (item.Value.ExpectedResponseTime < DateTime.Now)
                            {
                                SentPasswords.AddOrSet(item.Key, new SentPassword(DateTime.Now, DateTime.Now + CriticalResponseTime, item.Value.Passwords));
                                SentPasswords.TryGetValue(item.Key, out var sentPassword);

                                if (sentPassword != null)
                                    Context.ActorSelection(respondPath)
                                        .Tell(new SendToWorkinNode(sentPassword.Passwords, Hash, item.Key));
                                return;
                            }

                        }

                        var id = Guid.NewGuid();
                        var passwords = Generator.GetPasswords(250);

                        SentPasswords.Add(id, new SentPassword(DateTime.Now, DateTime.Now + CriticalResponseTime, passwords));

                        var data = new SendToWorkinNode(passwords, Hash, id);
                        Context.ActorSelection(respondPath).Tell(data);

                        break;
                    }

                    Self.Forward(message);
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
                    Generator = new PasswordGenerator(data.PasswordLength);
                    Context.ActorSelection("../display-processor").Tell(new SetNumberOfCombinations(Generator.NumberOfCombinations));
                    break;

                case Exception ex:
                    Console.WriteLine(ex);
                    Context.Parent.Tell("End");
                    break;
            }
        }
    }
}
