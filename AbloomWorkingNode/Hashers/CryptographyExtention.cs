namespace AbloomWorkingNode.Hashers
{
    public class CryptographyExtention
    {
        public static string CreateHash(string input)
        {
            const int WORK_FACTOR = 12; // Hash computation takes about 0.8 sec on 'Intel (R) i7-2600 CPU 3.4Ghz'

            var hash = BCrypt.Net.BCrypt.HashPassword(input, WORK_FACTOR);

            return hash;
        }

        public static bool Verify(string input, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(input, hash);
        }
    }
}
