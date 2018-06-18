namespace Pickaxe.Blockchain.Contracts
{
    public class Error
    {
        public Error(string message)
        {
            Message = message;
        }

        public string Message { get; private set; }
    }
}
