using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Abloom.Cryptography
{
    internal class PasswordGenerator
    {
        const string NUMBERS = "0123456789";
        const string ALPHABET = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string PUNCTUATION = "!@#$%^&*()_+";
        private string Chars { get; set; } = ALPHABET.ToLower() + ALPHABET.ToUpper() + NUMBERS + PUNCTUATION;

        public BigInteger NumberOfCombinations { get; private set; }
        private BigInteger LastCombination { get; set; } = 0;
        private int PasswordLength { get; set; }


        public PasswordGenerator(int passwordLength)
        {
            PasswordLength = passwordLength;
            NumberOfCombinations = BigInteger.Pow(Chars.Length, PasswordLength);
        }

        private List<string> GeneratePasswords(int numberOfPasswords)
        {
            var passwords = new List<string>();

            for (BigInteger i = LastCombination; i < NumberOfCombinations; i++, LastCombination++)
            {
                var password = "";
                var val = i;

                for (int j = 0; j < PasswordLength; j++)
                {
                    var ch = (int)(val % Chars.Length);

                    if (j == 3 && !ALPHABET.ToLower().Contains(Chars[ch]))
                        break;
                    if (j == 2 && !NUMBERS.Contains(Chars[ch]))
                        break;
                    if (j == 1 && !ALPHABET.ToUpper().Contains(Chars[ch]))
                        break;
                    if (j == 0 && !PUNCTUATION.Contains(Chars[ch]))
                        break;

                    password = Chars[ch] + password;
                    val /= Chars.Length;
                }


                if (password.Length == PasswordLength)
                {
                    passwords.Add(new String(password));

                    if (passwords.Count == numberOfPasswords)
                        break;
                }
            }

            LastCombination++;

            return passwords;
        }

        public List<string> GetPasswords(int numebrOfPasswords)
        {
            return GeneratePasswords(numebrOfPasswords);
        }
    }
}
