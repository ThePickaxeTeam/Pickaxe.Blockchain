using Nethereum.Hex.HexConvertors.Extensions;
using Pickaxe.Blockchain.Common;
using Pickaxe.Blockchain.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pickaxe.Blockchain.Mining
{
    public class Miner : IMiner
    {
        private IBlockchain _blockchain;

        public Miner(IBlockchain blockchain)
        {
            _blockchain = blockchain;
        }

        public Block MineNewBlock(
            int previousBlockIndex,
            byte[] previousBlockHash,
            byte[] previousBlockData)
        {
            int index = previousBlockIndex + 1;
            long nonce = 0;
            long timestamp = DateTime.UtcNow.Ticks;

            byte[] data = GetData(
                index,
                previousBlockHash,
                timestamp,
                previousBlockData,
                nonce);
            byte[] guess = HashUtils.ComputeSha256(data);
            while (!guess.ToHex().StartsWith("0000"))
            {
                nonce++;
                timestamp = DateTime.UtcNow.Ticks;
                data = GetData(
                    index,
                    previousBlockHash,
                    timestamp,
                    previousBlockData,
                    nonce);
                guess = HashUtils.ComputeSha256(data);
            }

            return new Block();
        }

        private static byte[] GetData(
            int index,
            byte[] previousBlockHash,
            long timestamp,
            byte[] previousBlockData,
            long nonce)
        {
            return Combine(
                BitConverter.GetBytes(index),
                previousBlockHash,
                BitConverter.GetBytes(timestamp),
                previousBlockData,
                BitConverter.GetBytes(nonce));
        }

        private static byte[] Combine(params byte[][] arrays)
        {
            byte[] result = new byte[arrays.Sum(a => a.Length)];
            int offset = 0;
            foreach (byte[] array in arrays)
            {
                Buffer.BlockCopy(array, 0, result, offset, array.Length);
                offset += array.Length;
            }
            return result;
        }
    }
}
