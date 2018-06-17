using Nethereum.Hex.HexConvertors.Extensions;
using Pickaxe.Blockchain.Clients;
using Pickaxe.Blockchain.Common;
using Pickaxe.Blockchain.Contracts;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Pickaxe.Blockchain.Miner
{
    /// <summary>
    /// Computes block hashes of the following type:
    /// SHA256([block_index][transactions_hash][previous_block_hash][timestamp][nonce])
    /// </summary>
    internal class Miner
    {
        private static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

        private static async Task MainAsync(string[] args)
        {
            string nodeBaseUrl = "http://localhost:64149";//args[0];
            string minerId = "1";//args[1];
            TimeSpan maxJobDuration = new TimeSpan(0, 0, 5);
            NodeClient nodeClient = new NodeClient(nodeBaseUrl);
            Stopwatch stopwatch = new Stopwatch();

            do
            {
                stopwatch.Start();

                MiningJob job = await GetMiningJob(nodeClient, minerId).ConfigureAwait(false);

                string difficultyCheck = new string('0', job.Difficulty);
                byte[] precomputedData = GetPrecomputedData(job);
                long ticks = DateTime.UtcNow.Ticks;
                ulong nonce = 0;

                while (nonce < ulong.MaxValue)
                {
                    string guess = ComputeHash(precomputedData, ticks, nonce);
                    if (guess.StartsWith(difficultyCheck))
                    {
                        // block found, send it to the node
                        bool submitted = await SubmitMiningResult(
                            nodeClient,
                            nonce,
                            ticks,
                            guess,
                            minerId,
                            5).ConfigureAwait(false);
                        Console.WriteLine($"Submitting mining result {(submitted ? "successful" : "failed")}.");
                        break;
                    }

                    if (maxJobDuration < stopwatch.Elapsed)
                    {
                        stopwatch.Reset();
                        break;
                    }

                    // get new timestamp on every 100,000 iterations
                    if (nonce % 100000 == 0)
                    {
                        ticks = DateTime.UtcNow.Ticks;
                    }

                    nonce++;
                }

            } while (true);
        }

        private static async Task<bool> SubmitMiningResult(
            NodeClient nodeClient,
            ulong nonce,
            long ticks,
            string hash,
            string minerId,
            int maxRetries)
        {
            MiningJobResult result = new MiningJobResult
            {
                Nonce = nonce.ToString(),
                TimestampTicks = ticks.ToString(),
                BlockHash = hash
            };

            Response<EmptyPayload> response;
            int retries = 0;
            do
            {
                retries++;
                response = await nodeClient.SubmitMiningJob(result, minerId).ConfigureAwait(false);
            } while (response.Status == Status.Failed && retries < maxRetries);

            return response.Status == Status.Success;
        }

        private static async Task<MiningJob> GetMiningJob(
            NodeClient nodeClient,
            string minerId)
        {
            Response<MiningJob> response;
            do
            {
                response = await nodeClient.GetMiningJob(minerId).ConfigureAwait(false);
            } while (response.Status == Status.Failed);

            return response.Payload;
        }

        private static byte[] GetPrecomputedData(MiningJob job)
        {
            return ArrayUtils.Combine(
                BitConverter.GetBytes(job.BlockIndex),
                job.TransactionsHash.HexToByteArray(),
                job.PreviousBlockHash.HexToByteArray());
        }

        private static string ComputeHash(
            byte[] precomputedData,
            long ticks,
            ulong nonce)
        {
            byte[] data = ArrayUtils.Combine(
                precomputedData,
                BitConverter.GetBytes(ticks),
                BitConverter.GetBytes(nonce));
            byte[] hash = HashUtils.ComputeSha256(data);
            return hash.ToHex();
        }
    }
}
