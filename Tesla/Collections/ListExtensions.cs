using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Tesla.Collections
{
    public static class ListExtensions
    {
        /// <summary>
        /// Randomly shuffles elements of the list.
        /// </summary>
        /// <typeparam name="T">The type of the list.</typeparam>
        /// <param name="list">The list to shuffle.</param>
        public static void Shuffle<T>(this IList<T> list)
        {
            var provider = new RNGCryptoServiceProvider();
            var n = list.Count;

            while (n > 1)
            {
                var box = new byte[1];

                do provider.GetBytes(box);
                while (!(box[0] < n * (Byte.MaxValue / n)));

                var k = (box[0] % n);
                n--;
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
