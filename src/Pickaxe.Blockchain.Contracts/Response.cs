using System.Collections.Generic;

namespace Pickaxe.Blockchain.Contracts
{
    public class Response<T>
    {
        public Status Status { get; set; }

        public IList<string> Errors { get; set; }

        public T Payload { get; set; }
    }
}
