
namespace Abloom.Messages
{
    internal class RespondFinishExecution
    {
        public string Password { get; }
        public bool IsCompleted { get; }

        public RespondFinishExecution(string password, bool isCompleted)
        {
            Password = password;
            this.IsCompleted = isCompleted;
        }
    }
}
