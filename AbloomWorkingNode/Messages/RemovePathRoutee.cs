namespace AbloomWorkingNode.Messages
{
    public sealed class RemovePathRoutee
    {
        public string Path { get; }

        public RemovePathRoutee(string path)
        {
            Path = path;
        }
    }
}
