using NBitcoin;
using NBitcoin.DataEncoders;
using Nethereum.Hex.HexConvertors.Extensions;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using System;
using System.Text;

namespace Pickaxe.Blockchain.Common
{
    public static class HashUtils
    {
        /// <summary>
        /// Creates a Bitcoin address step by step.
        /// </summary>
        /// <param name="publicKey"></param>
        /// <returns></returns>
        /// <remarks>
        /// See here for more details:
        /// https://en.bitcoin.it/wiki/Technical_background_of_version_1_Bitcoin_addresses
        /// </remarks>
        public static string CreateBitcoinAddressVerbose(byte[] publicKey)
        {
            StringBuilder output = new StringBuilder();
            byte[] step1 = publicKey;
            output.AppendLine("1. public_key:\r\n" + step1.ToHex());

            byte[] step2_SHA256 = ComputeSha256(step1);
            output.AppendLine("2. SHA256(public_key):\r\n" + step2_SHA256.ToHex());

            byte[] step3_RIPEMD160 = ComputeRIPEMD160(step2_SHA256);
            output.AppendLine("3. RIPEMD160(SHA256(public_key)):\r\n" + step3_RIPEMD160.ToHex());

            byte[] step4_prepend_byte = ArrayUtils.ConcatArrays(new byte[1], step3_RIPEMD160);
            output.AppendLine("4. [0x00] + RIPEMD160(SHA256(public_key)):\r\n" + step4_prepend_byte.ToHex());

            byte[] step5_SHA256 = ComputeSha256(step4_prepend_byte);
            output.AppendLine("5. SHA256([0x00] + RIPEMD160(SHA256(public_key))):\r\n" + step5_SHA256.ToHex());

            byte[] step6_SHA256 = ComputeSha256(step5_SHA256);
            output.AppendLine("6. SHA256(SHA256([0x00] + RIPEMD160(SHA256(public_key)))):\r\n" + step6_SHA256.ToHex());

            byte[] step7_checksum = ArrayUtils.SubArray(step6_SHA256, 0, 4);
            output.AppendLine("7. checksum = First4Bytes(SHA256(SHA256([0x00] + RIPEMD160(SHA256(public_key))))):\r\n" + step7_checksum.ToHex());

            byte[] step8_BTC_address = ArrayUtils.ConcatArrays(step4_prepend_byte, step7_checksum);
            output.AppendLine("8. BTC_address = [0x00] + RIPEMD160(SHA256(public_key)) + checksum:\r\n" + step8_BTC_address.ToHex());

            string step9_Base58Check_encode = new Base58Encoder().EncodeData(step8_BTC_address);
            output.AppendLine("9. Base58CheckEncode(BTC_address):\r\n" + step9_Base58Check_encode);

            return output.ToString();
        }

        public static string WifToBitcoinAddress(string wif, Network network = null)
        {
            Key privateKey = Key.Parse(wif);
            return CreateBitcoinAddress(privateKey.PubKey, network);
        }

        public static string PublicKeyToBitcoinAddress(string hex, Network network = null)
        {
            PubKey publicKey = new PubKey(hex);
            return CreateBitcoinAddress(publicKey, network);
        }

        public static byte[] ComputeRIPEMD160(byte[] data)
        {
            IDigest digest = new RipeMD160Digest();
            byte[] buffer = new byte[digest.GetDigestSize()];
            digest.BlockUpdate(data, 0, data.Length);
            digest.DoFinal(buffer, 0);
            return buffer;
        }

        public static byte[] ComputeSha256(byte[] data)
        {
            IDigest digest = new Sha256Digest();
            byte[] buffer = new byte[digest.GetDigestSize()];
            digest.BlockUpdate(data, 0, data.Length);
            digest.DoFinal(buffer, 0);
            return buffer;
        }

        public static byte[] ComputeSha384(byte[] data)
        {
            IDigest digest = new Sha384Digest();
            byte[] buffer = new byte[digest.GetDigestSize()];
            digest.BlockUpdate(data, 0, data.Length);
            digest.DoFinal(buffer, 0);
            return buffer;
        }

        public static byte[] ComputeSha512(byte[] data)
        {
            IDigest digest = new Sha512Digest();
            byte[] buffer = new byte[digest.GetDigestSize()];
            digest.BlockUpdate(data, 0, data.Length);
            digest.DoFinal(buffer, 0);
            return buffer;
        }

        public static byte[] ComputeSha3Digest(byte[] data, int bitLength)
        {
            var digest = new Sha3Digest(bitLength);
            byte[] buffer = new byte[digest.GetDigestSize()];
            digest.BlockUpdate(data, 0, data.Length);
            digest.DoFinal(buffer, 0);
            return buffer;
        }

        public static byte[] ComputeSha3KeccakDigest(byte[] data, int bitLength)
        {
            var digest = new KeccakDigest(bitLength);
            byte[] buffer = new byte[digest.GetDigestSize()];
            digest.BlockUpdate(data, 0, data.Length);
            digest.DoFinal(buffer, 0);
            return buffer;
        }

        public static byte[] ComputeWhirlpoolDigest(byte[] data)
        {
            IDigest digest = new WhirlpoolDigest();
            byte[] buffer = new byte[digest.GetDigestSize()];
            digest.BlockUpdate(data, 0, data.Length);
            digest.DoFinal(buffer, 0);
            return buffer;
        }

        public static byte[] ComputeHmac(byte[] data, byte[] key, int bitLength)
        {
            IDigest digest;
            if (bitLength == 256)
            {
                digest = new Sha256Digest();
            }
            else if (bitLength == 512)
            {
                digest = new Sha512Digest();
            }
            else
            {
                throw new ArgumentException("SHA digest restricted to one of [256, 512]");
            }

            HMac hmac = new HMac(digest);
            hmac.Init(new KeyParameter(key));

            byte[] buffer = new byte[hmac.GetMacSize()];
            hmac.BlockUpdate(data, 0, data.Length);
            hmac.DoFinal(buffer, 0);
            return buffer;
        }

        public static string ToHexCompressed(ECPoint point)
        {
            BigInteger yCoord = point.YCoord.ToBigInteger();
            return point.XCoord.ToString() + Convert.ToInt32(IsOdd(yCoord));
        }

        private static bool IsOdd(BigInteger value)
        {
            BigInteger remainder = value.Remainder(BigInteger.Two);
            return remainder.CompareTo(BigInteger.One) == 0;
        }

        private static string CreateBitcoinAddress(PubKey publicKey, Network network = null)
        {
            return publicKey.GetAddress(network ?? Network.Main).ToString();
        }
    }
}
