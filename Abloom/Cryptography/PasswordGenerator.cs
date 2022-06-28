using System.Numerics;

namespace Abloom.Cryptography
{
    internal class PasswordGenerator
    {
        const string NUMBERS = "0123456789";
        const string ALPHABETL = "abcdefghijklmnopqrstuvwxyz";
        const string ALPHABETU = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string PUNCTUATION = "!@#$%^&*()_+";
        private string Chars { get; set; } = ALPHABETL + ALPHABETU + NUMBERS + PUNCTUATION;
        public BigInteger NumberOfCombinations { get; private set; }
        private BigInteger LastCombination { get; set; } = 0;
        private int PasswordLength { get; set; }


        public PasswordGenerator(int passwordLength)
        {
            PasswordLength = passwordLength;
            NumberOfCombinations = BigInteger.Pow(Chars.Length, PasswordLength);
        }

        public List<string> GetPasswords(int numberOfPasswords)
        {
            var passwords = new List<string>(numberOfPasswords);

            for (var i = LastCombination; i < NumberOfCombinations; i++, LastCombination++)
            {
                var password = "";
                var val = i;

                for (var j = 0; j < PasswordLength; j++)
                {
                    var ch = (int)(val % Chars.Length);

                    if (j == 3 && !ALPHABETL.Contains(Chars[ch]))
                        break;
                    if (j == 2 && !NUMBERS.Contains(Chars[ch]))
                        break;
                    if (j == 1 && !ALPHABETU.Contains(Chars[ch]))
                        break;
                    if (j == 0 && !PUNCTUATION.Contains(Chars[ch]))
                        break;

                    password = Chars[ch] + password;
                    val /= Chars.Length;
                }


                if (password.Length == PasswordLength)
                {
                    passwords.Add(password);

                    if (passwords.Count == numberOfPasswords)
                        break;
                }
            }

            LastCombination++;

            return passwords;
        }
    }
}
