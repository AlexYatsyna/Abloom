using Identity.PasswordHasher;

namespace AbloomWorkingNode.Hashers
{
    public class CustomPasswordHasher : PasswordHasher
    {
        private const string Salt = "4GH220AT-8723-49AW-R5FP-23D2332323813";

        public override string HashPassword(string password)
        {
            return CryptographyExtention.CreateHash(password + Salt);
        }

        public override bool VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            return CryptographyExtention.Verify(providedPassword + Salt, hashedPassword) ? true : false;
        }
    }
}
