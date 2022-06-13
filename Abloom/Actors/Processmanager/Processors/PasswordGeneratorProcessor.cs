using Abloom.Messages;
using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private string Hash { get; set; }

        private Dictionary<Guid, List<string>> PasswordForWorkerNodes { get; set; } = new Dictionary<Guid, List<string>>();

        private bool isFound = false;
        private bool isExit = false;


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
                    //CurrentNumberOfComb++;

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
                    //if (partOfPasswords.Count > 5000)
                    //{
                    //    var id = Guid.NewGuid();
                    //    PasswordForWorkerNodes.Add(id, partOfPasswords);

                    //    Context.Parent.Tell(new SendToWorkinNode(partOfPasswords, Hash, id));

                    //    partOfPasswords.Clear();

                    //}
                    //partOfPasswords.Add(password);
                    //Console.WriteLine(password);


                    //TODO : send part of passwords to worker
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

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case SendToGeneratePasswords data:
                    Hash = data.CorrectHash;
                    StartCheck(data.estimatedPassword);
                    break;

            }
        }
    }
}
