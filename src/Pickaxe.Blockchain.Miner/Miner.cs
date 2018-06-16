using Nethereum.Hex.HexConvertors.Extensions;
using Pickaxe.Blockchain.Clients;
using Pickaxe.Blockchain.Common;
using Pickaxe.Blockchain.Contracts;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Pickaxe.Blockchain.Miner
{
    internal class Miner
    {
        private static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

        private static async Task MainAsync(string[] args)
        {
            string nodeBaseUrl = args[0];
            string minerId = args[1];
            TimeSpan maxJobDuration = new TimeSpan(0, 0, 5); // 5 seconds
            NodeClient nodeClient = new NodeClient(nodeBaseUrl);
            Stopwatch stopwatch = new Stopwatch();

            do
            {
                stopwatch.Start();

                Response<MiningJob> response;
                do
                {
                    response = await nodeClient.GetMiningJob(minerId).ConfigureAwait(false);
                } while (response.Status == Status.Failed);

                // BlockHash = SHA256(BlockIndex|TransactionsHash|PreviousBlockHash|Timestamp|Nonce);

                MiningJob job = response.Payload;
                string difficultyCheck = new string('0', job.Difficulty);
                byte[] precomputedData = GetPrecomputedData(job);
                ulong nonce = 0;
                while (nonce < ulong.MaxValue)
                {


                    if (maxJobDuration < stopwatch.Elapsed)
                    {
                        stopwatch.Reset();
                        break;
                    }
                }

            } while (true);
        }

        private static byte[] GetPrecomputedData(MiningJob job)
        {
            return ArrayUtils.Combine(
                BitConverter.GetBytes(job.BlockIndex),
                job.TransactionsHash.HexToByteArray(),
                job.PreviousBlockHash.HexToByteArray());
        }

        //private static byte[] GetData(
        //    int blockIndex,
        //    byte[] previousBlockHash,
        //    long timestamp,
        //    byte[] previousBlockData,
        //    long nonce)
        //{
        //    return ArrayUtils.Combine(
        //        BitConverter.GetBytes(index),
        //        previousBlockHash,
        //        BitConverter.GetBytes(timestamp),
        //        previousBlockData,
        //        BitConverter.GetBytes(nonce));
        //}
    }
}
