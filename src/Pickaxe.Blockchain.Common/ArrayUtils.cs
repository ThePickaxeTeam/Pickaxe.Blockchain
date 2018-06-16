using System;
using System.Linq;

namespace Pickaxe.Blockchain.Common
{
    public static class ArrayUtils
    {
        public static T[] ConcatArrays<T>(T[] arr1, T[] arr2)
        {
            var result = new T[arr1.Length + arr2.Length];
            arr1.CopyTo(result, 0);
            arr2.CopyTo(result, arr1.Length);

            return result;
        }

        public static T[] Combine<T>(params T[][] arrays)
        {
            T[] result = new T[arrays.Sum(a => a.Length)];
            int offset = 0;
            foreach (T[] array in arrays)
            {
                Buffer.BlockCopy(array, 0, result, offset, array.Length);
                offset += array.Length;
            }
            return result;
        }

        public static T[] SubArray<T>(T[] arr, int start, int length)
        {
            var result = new T[length];
            Array.Copy(arr, start, result, 0, length);

            return result;
        }

        public static T[] SubArray<T>(T[] arr, int start)
        {
            return SubArray(arr, start, arr.Length - start);
        }
    }
}
