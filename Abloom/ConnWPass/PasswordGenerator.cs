using Abloom.Data;
using Microsoft.AspNet.Identity;
using System;
using System.Numerics;


namespace Abloom.ConnWPass
{
    internal class PasswordGenerator
    {
        const string NUMBERS = "0123456789";
        const string ALPHABET = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string PUNCTUATION = "!@#$%^&*()_+";
        private string Chars { get; set; }
        //private CustomPasswordHasher hasher;

        public PasswordGenerator()
        {
            Chars = ALPHABET.ToLower() + ALPHABET.ToUpper() + NUMBERS + PUNCTUATION;
            //hasher = new CustomPasswordHasher();
        }

        public void StartCheck(int estimatedPasswordLength = 0)
        {

            var alphabetSize = NUMBERS.Length + 2 * ALPHABET.Length + PUNCTUATION.Length;

            if (estimatedPasswordLength == 0)
            {
                while (!(CommonData.isFound || CommonData.isExit))
                {
                    estimatedPasswordLength++;
                    CommonData.NumberOfPassCombinations = BigInteger.Pow(alphabetSize, estimatedPasswordLength);
                    CommonData.PasswordLength = estimatedPasswordLength;
                    StartBruteForce(estimatedPasswordLength);
                }
                return;
            }

            CommonData.NumberOfPassCombinations = BigInteger.Pow(alphabetSize, estimatedPasswordLength);
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

                if (CommonData.isFound || CommonData.isExit)
                    return;

                if (currentCharPosition < indexOfLastChar)
                {
                    CreatePassword(nextCharPosition, passChars);
                }
                else
                {
                    CommonData.CurrentNumberOfComb++;

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

                    //if (hasher.VerifyHashedPassword(CommonData.InputHash, password) == PasswordVerificationResult.Success)
                    //{
                    //CommonData.isFound = true;
                    //Console.WriteLine($"Result: {password}");

                    return;
                    //}
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
    }
}
