using Nethereum.Hex.HexConvertors.Extensions;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Pickaxe.Blockchain.Clients;
using Pickaxe.Blockchain.Common;
using Pickaxe.Blockchain.Common.Extensions;
using Pickaxe.Blockchain.Contracts;
using Pickaxe.Blockchain.Contracts.Serialization;
using System;
using System.Threading.Tasks;

namespace Pickaxe.Blockchain.TransactionSigner
{
    internal class TransactionSigner
    {
        private static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

        private static async Task MainAsync(string[] args)
        {
            string nodeBaseUrl = "http://localhost:64149";
            INodeClient nodeClient = new NodeClient(nodeBaseUrl);

            CreateTransactionRequest request = CreateAndSignTransaction(
                "7e4670ae70c98d24f3662c172dc510a085578b9ccc717e6c2f4e547edd960a34",
                "f51362b7351ef62253a227a77751ad9b2302f911",
                "2018-02-10T17:53:48.972Z",
                25000,
                10,
                "funds");

            Response<Transaction> resposne =
                await nodeClient.CreateTransaction(request).ConfigureAwait(false);
            Console.WriteLine(resposne.Status);

            VerifyTransaction(request);
        }

        private static CreateTransactionRequest CreateAndSignTransaction(
            string privateKey,
            string recipientAddress,
            string dateCreatedIso8601,
            long value,
            long fee,
            string data)
        {
            Console.WriteLine("Generate and sign a transaction");
            Console.WriteLine("-------------------------------");

            Console.WriteLine("Sender private key:\r\n" + privateKey);

            BigInteger senderPrivateKey = new BigInteger(privateKey, 16);
            ECPoint senderPublicKey = EncryptionUtils.GetPublicKey(senderPrivateKey);
            string senderPublicKeyCompressed = EncryptionUtils.ToHexCompressed(senderPublicKey);
            Console.WriteLine("Sender public key compressed (65 hex digits):\r\n" +
                senderPublicKeyCompressed);

            string senderAddress = HashUtils.ComputeRIPEMD160(
                senderPublicKeyCompressed.GetBytes()).ToHex();
            Console.WriteLine("Sender address (40 hex digits):\r\n" + senderAddress);

            TransactionData transaction = new TransactionData
            {
                From = senderAddress,
                To = recipientAddress,
                Value = value,
                Fee = fee,
                DateCreated = dateCreatedIso8601,
                Data = data,
                SenderPubKey = senderPublicKeyCompressed
            };

            string transactionJsonFormatted = JsonUtils.Serialize(transaction);
            Console.WriteLine("Transaction (JSON, formatted):\r\n" + transactionJsonFormatted);

            string transactionJson = JsonUtils.Serialize(transaction, false);
            Console.WriteLine("Transaction (JSON):\r\n" + transactionJson);

            byte[] transactionDataHash = HashUtils.ComputeSha256(transactionJson.GetBytes());
            Console.WriteLine("Transaction data hash (SHA256):\r\n" + transactionDataHash.ToHex());

            BigInteger[] transactionSignature =
                EncryptionUtils.Sign(transactionDataHash, senderPrivateKey);
            string r = transactionSignature[0].ToString(16);
            string s = transactionSignature[1].ToString(16);

            Console.WriteLine("Transaction signature:\r\n({0}, {1})", r, s);

            CreateTransactionRequest request =
                CreateTransactionRequest.FromTransactionData(transaction);
            request.SenderSignature = new string[] { r, s };

            string signedTransactionJson = JsonUtils.Serialize(request);
            Console.WriteLine("Signed transaction (JSON):\r\n" + signedTransactionJson);

            return request;
        }

        private static void VerifyTransaction(CreateTransactionRequest request)
        {
            TransactionData transaction = new TransactionData
            {
                From = request.From,
                To = request.To,
                Value = request.Value,
                Fee = request.Fee,
                DateCreated = request.DateCreated,
                Data = request.Data,
                SenderPubKey = request.SenderPubKey
            };

            string transactionJson = JsonUtils.Serialize(transaction, false);
            byte[] transactionDataHash = HashUtils.ComputeSha256(transactionJson.GetBytes());
            BigInteger r = new BigInteger(request.SenderSignature[0], 16);
            BigInteger s = new BigInteger(request.SenderSignature[1], 16);
            ECPoint publicKey = EncryptionUtils.DecompressKey(request.SenderPubKey);

            bool valid = EncryptionUtils.VerifySignature(publicKey, r, s, transactionDataHash);
            Console.WriteLine("Signature valid: " + valid);
        }
    }
}
