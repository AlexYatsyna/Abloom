using Abloom.Messages;
using Akka.Actor;
using Akka.Util.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Abloom.Actors.Processmanager.Processors
{
    public class SendRecievePassProcessor : UntypedActor
    {
        private IActorRef PasswordgeneratorRef { get; set; }
        private Dictionary<Guid, List<string>> PreparedToSend { get; set; } = new Dictionary<Guid, List<string>>();
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
                    //TODO : Get data and send to worker
                    break;
                //case SendToGeneratePasswords data:
                //    Hash = data.CorrectHash;
                //    Task.Run(() => StartCheck(data.EstimatedPassword), Token);
                //    break;

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
