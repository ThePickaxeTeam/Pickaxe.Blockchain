﻿using Pickaxe.Blockchain.Clients;
using Pickaxe.Blockchain.Common;
using Pickaxe.Blockchain.Common.Extensions;
using Pickaxe.Blockchain.Contracts;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Pickaxe.Blockchain.Miner
{
    /// <summary>
    /// Computes block hashes of the following type:
    /// SHA256([block_data_hash][timestamp][nonce])
    /// </summary>
    internal class Miner
    {
        private static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

        private static async Task MainAsync(string[] args)
        {
            string nodeBaseUrl;
            string minerAddress;
            if (args.Length < 2)
            {
                nodeBaseUrl = "http://localhost:64149";
                minerAddress = "9a9f082f37270ff54c5ca4204a0e4da6951fe917";
            }
            else
            {
                nodeBaseUrl = args[0];
                minerAddress = args[1];
            }

            TimeSpan maxJobDuration = new TimeSpan(0, 0, 5);
            INodeClient nodeClient = new NodeClient(nodeBaseUrl);
            Stopwatch stopwatch = new Stopwatch();

            do
            {
                stopwatch.Start();

                MiningJob job = await GetMiningJob(nodeClient, minerAddress).ConfigureAwait(false);

                string difficultyCheck = new string('0', job.Difficulty);
                DateTime dateCreated = DateTime.UtcNow;
                ulong nonce = 0;

                while (nonce < ulong.MaxValue)
                {
                    string guess = HashUtils.ComputeBlockSha256Hash(job.BlockDataHash, dateCreated, nonce);
                    if (guess.StartsWith(difficultyCheck))
                    {
                        // block found, send it to the node
                        bool submitted = await SubmitMinedBlock(
                            nodeClient,
                            minerAddress,
                            5,
                            job.BlockDataHash,
                            dateCreated,
                            nonce,
                            guess).ConfigureAwait(false);
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
                        dateCreated = DateTime.UtcNow;
                    }

                    nonce++;
                }

            } while (true);
        }

        private static async Task<bool> SubmitMinedBlock(
            INodeClient nodeClient,
            string minerAddress,
            int maxRetries,
            string blockDataHash,
            DateTime dateCreated,
            ulong nonce,
            string computedHash)
        {
            MinedBlock block = new MinedBlock
            {
                BlockDataHash = blockDataHash,
                DateCreated = dateCreated.Iso8601Formatted(),
                Nonce = nonce.ToString(),
                BlockHash = computedHash
            };

            Response<Block> response;
            int retries = 0;
            do
            {
                retries++;
                response = await nodeClient.SubmitMinedBlock(block, minerAddress).ConfigureAwait(false);
            } while (response.Status == Status.Failed && retries < maxRetries);

            return response.Status == Status.Success;
        }

        private static async Task<MiningJob> GetMiningJob(
            INodeClient nodeClient,
            string minerAddress)
        {
            Response<MiningJob> response;
            do
            {
                response = await nodeClient.GetMiningJob(minerAddress).ConfigureAwait(false);
            } while (response.Status == Status.Failed);

            return response.Payload;
        }
    }
}
