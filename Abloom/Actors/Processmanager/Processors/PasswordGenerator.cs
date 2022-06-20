using Abloom.Messages;
using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Abloom.Actors.Processmanager.Processors
{
    internal class PasswordGenerator : UntypedActor
    {
        const string NUMBERS = "0123456789";
        const string ALPHABET = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string PUNCTUATION = "!@#$%^&*()_+";
        private string Chars { get; set; } = ALPHABET.ToLower() + ALPHABET.ToUpper() + NUMBERS + PUNCTUATION;
        private BigInteger NumberOfCombinations { get; set; } = 0;
        private BigInteger LastCombination { get; set; }
        private int PasswordLength { get; set; } = 0;

        private List<List<string>> GetPasswordIntervals(int numberOfIntervals, int numberOfPasswordsInInterval)
        {
            var partOfPasswords = new List<string>();
            var data = new List<List<string>>();

            for (BigInteger i = LastCombination; i < NumberOfCombinations; i++, LastCombination++)
            {
                var password = "";
                var val = i;

                for (int j = 0; j < PasswordLength; j++)
                {
                    var ch = (int)(val % Chars.Length);

                    bool resultOfchecking;
                    if (PasswordLength >= 4 && j == 3)
                    {
                        resultOfchecking = ConstraintCheck(4, Chars[ch]);
                        if (!resultOfchecking)
                            break;
                    }
                    if (PasswordLength >= 3 && j == 2)
                    {
                        resultOfchecking = ConstraintCheck(3, Chars[ch]);
                        if (!resultOfchecking)
                            break;
                    }
                    if (PasswordLength >= 2 && j == 1)
                    {
                        resultOfchecking = ConstraintCheck(2, Chars[ch]);
                        if (!resultOfchecking)
                            break;
                    }
                    if (PasswordLength >= 1 && j == 0)
                    {
                        resultOfchecking = ConstraintCheck(1, Chars[ch]);
                        if (!resultOfchecking)
                            break;
                    }

                    password = Chars[ch] + password;
                    val /= Chars.Length;
                }


                if (password.Length == PasswordLength)
                {
                    partOfPasswords.Add(new String(password));

                    if (partOfPasswords.Count == numberOfPasswordsInInterval)
                    {
                        data.Add(partOfPasswords.ToList());
                        partOfPasswords.Clear();
                    }

                    if (data.Count == numberOfIntervals)
                        break;
                }
            }

            LastCombination++;

            return data;
        }
        private static bool ConstraintCheck(int index, char symbol)
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
                case SetInitialData data:
                    NumberOfCombinations = data.NumberOfPassCombinations;
                    PasswordLength = data.PasswordLength;
                    break;

                case GetPasswordsIntervals data:
                    if (PasswordLength != 0 && NumberOfCombinations != 0)
                    {
                        var result = GetPasswordIntervals(data.NumberOfIntervals, data.NumebrOfPasswordsInTheInterval);
                        Sender.Tell(new RespondPasswordIntervals(result));
                    }
                    else
                        Self.Forward(data);
                    break;
            }
        }
    }
}
