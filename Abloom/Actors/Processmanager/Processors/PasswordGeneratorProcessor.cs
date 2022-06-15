using Abloom.Messages;
using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Abloom.Actors.Processmanager.Processors
{
    public class PasswordGeneratorProcessor : UntypedActor
    {
        const string NUMBERS = "0123456789";
        const string ALPHABET = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string PUNCTUATION = "!@#$%^&*()_+";
        List<string> partOfPasswords = new List<string>();
        private string Chars { get; set; } = ALPHABET.ToLower() + ALPHABET.ToUpper() + NUMBERS + PUNCTUATION;
        private Dictionary<Guid, List<string>> PreparedToSend { get; set; } = new Dictionary<Guid, List<string>>();
        private Dictionary<Guid, List<string>> SentPasswords { get; set; } = new Dictionary<Guid, List<string>>();
        private string Hash { get; set; }
        private CancellationTokenSource TokenSource { get; set; }
        private CancellationToken Token { get; set; }
        private bool isFound = false;
        private bool isExit = false;

        protected override void PreStart()
        {
            TokenSource = new CancellationTokenSource();
            Token = TokenSource.Token;
        }
        public void StartCheck(int estimatedPasswordLength = 0)
        {

            var alphabetSize = NUMBERS.Length + 2 * ALPHABET.Length + PUNCTUATION.Length;

            //if (estimatedPasswordLength == 0)
            //{
            //    while (!(isFound || isExit))
            //    {
            //        estimatedPasswordLength++;
            //        CommonData.NumberOfPassCombinations = BigInteger.Pow(alphabetSize, estimatedPasswordLength);
            //        CommonData.PasswordLength = estimatedPasswordLength;
            //        StartBruteForce(estimatedPasswordLength);
            //    }
            //    return;
            //}

            StartBruteForce(estimatedPasswordLength);
        }

        private void StartBruteForce(int passlength)
        {
            var passChars = new char[passlength];

            for (int i = 0; i < passlength; i++)
            {
                passChars[i] = Chars[0];
            }

            CreatePassword(0, passChars);
        }

        private void CreatePassword(int currentCharPosition, char[] passChars)
        {

            var indexOfLastChar = passChars.Length - 1;
            var nextCharPosition = currentCharPosition + 1;

            for (int i = 0; i < Chars.Length; i++)
            {
                passChars[currentCharPosition] = Chars[i];

                if (isFound || isExit)
                    return;

                if (currentCharPosition < indexOfLastChar)
                {
                    CreatePassword(nextCharPosition, passChars);
                }
                else
                {
                    bool resultOfchecking;
                    if (passChars.Length >= 4)
                    {
                        resultOfchecking = ConstraintCheck(4, passChars[passChars.Length - 4]);
                        if (!resultOfchecking)
                            continue;
                    }
                    if (passChars.Length >= 3)
                    {
                        resultOfchecking = ConstraintCheck(3, passChars[passChars.Length - 3]);
                        if (!resultOfchecking)
                            continue;
                    }
                    if (passChars.Length >= 2)
                    {
                        resultOfchecking = ConstraintCheck(2, passChars[passChars.Length - 2]);
                        if (!resultOfchecking)
                            continue;
                    }
                    if (passChars.Length >= 1)
                    {
                        resultOfchecking = ConstraintCheck(1, passChars[passChars.Length - 1]);
                        if (!resultOfchecking)
                            continue;
                    }

                    var password = new string(passChars);

                    if (partOfPasswords.Count >= 50)
                    {
                        lock (this)
                        {
                            var id = Guid.NewGuid();
                            //PasswordForWorkerNodes.Add(id, partOfPasswords.ToList());
                            PreparedToSend.Add(id, partOfPasswords.ToList());
                            //Context.Parent.Tell(new SendToWorkinNode(partOfPasswords.ToList(), Hash, id));
                            //ParentRef.Tell(new SendToWorkinNode(partOfPasswords.ToList(), Hash, id));
                            partOfPasswords.Clear();
                        }

                    }
                    partOfPasswords.Add(password);
                    return;

                }
            }
        }

        private bool ConstraintCheck(int index, char symbol)
        {
            return index switch
            {
                1 => PUNCTUATION.Contains(symbol),
                2 => ALPHABET.ToUpper().Contains(symbol),
                3 => NUMBERS.Contains(symbol),
                4 => ALPHABET.ToLower().Contains(symbol),
                _ => false,
            };
        }

        private void SendPasswords()
        {
            lock (this)
            {
                if (PreparedToSend.Count != 0)
                {
                    Context.Parent.Tell(new SendToWorkinNode(PreparedToSend.First().Value, Hash, PreparedToSend.First().Key));
                    SentPasswords.Add(PreparedToSend.First().Key, PreparedToSend.First().Value);
                    PreparedToSend.Remove(PreparedToSend.First().Key);
                }
            }
        }

        

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case SendToGeneratePasswords data:
                    Hash = data.CorrectHash;
                    Task.Run(() => StartCheck(data.EstimatedPassword), Token);
                    break;

                case RespondPassword data:
                    SentPasswords.Remove(data.Id);
                    var displayProcessor = Context.ActorSelection("../display-processor");

                    displayProcessor.Tell(new SetCurrentCombination(data.IntervalSize));

                    if (data.IsFound && data.Password != "")
                    {
                        TokenSource.Cancel();
                        Context.Parent.Tell("StopSending");
                        displayProcessor.Tell(new RespondFinishExecution(data.Password, data.IsFound));
                    }
                    //TODO? : send this notification to workers
                    break;

                case "Send":
                    SendPasswords();
                    break;
            }
        }
    }
}
