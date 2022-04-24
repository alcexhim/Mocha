//
//  NanoID.cs - from the nanoid project, https://github.com/ai/nanoid - MIT License
//
//  Author:
//       Andrey Sitnik <andrey@sitnik.ru> (original)
//       zhu yu (C# translation)
// 	
// 	Original NanoID copyright (c) 2017 Andrey Sitnik <andrey@sitnik.ru>
// 	C# translation copyright (c) 2017 zhu yu
// 	
// 	Permission is hereby granted, free of charge, to any person obtaining a copy
// 	of this software and associated documentation files (the "Software"), to deal
// 	in the Software without restriction, including without limitation the rights
// 	to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// 	copies of the Software, and to permit persons to whom the Software is
// 	furnished to do so, subject to the following conditions:
// 	
// 	The above copyright notice and this permission notice shall be included in all
// 	copies or substantial portions of the Software.
// 	
// 	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// 	IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// 	FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// 	AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// 	LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// 	OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// 	SOFTWARE.

using System;
using System.Threading.Tasks;

namespace Mocha.Core.NanoID
{
	/// <summary>
	/// 
	/// </summary>
	public static class Nanoid
	{
		private const string DefaultAlphabet = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ"; // "_-0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
		private static readonly CryptoRandom Random = new CryptoRandom();

		// /// <summary>
		// /// 
		// /// </summary>
		// /// <param name="alphabet"></param>
		// /// <param name="size"></param>
		// /// <returns></returns>
		// public static async Task<string> GenerateAsync(string alphabet = DefaultAlphabet, int size = 21) => await Task.Run(() => Generate(Random, alphabet, size));

		/// <summary>
		/// 
		/// </summary>
		/// <param name="alphabet"></param>
		/// <param name="size"></param>
		/// <returns></returns>
		public static string Generate(string alphabet = DefaultAlphabet, int? size = null) => Generate(Random, alphabet, size);

		public static int DefaultSize { get; set; } = 21;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="random"></param>
		/// <param name="alphabet"></param>
		/// <param name="size"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public static string Generate(Random random, string alphabet = DefaultAlphabet, int? size = null)
		{
			if (size == null)
				size = DefaultSize;

			if (random == null)
			{
				throw new ArgumentNullException("random cannot be null.");
			}

			if (alphabet == null)
			{
				throw new ArgumentNullException("alphabet cannot be null.");
			}

			if (alphabet.Length <= 0 || alphabet.Length >= 256)
			{
				throw new ArgumentOutOfRangeException("alphabet must contain between 1 and 255 symbols.");
			}

			if (size <= 0)
			{
				throw new ArgumentOutOfRangeException("size must be greater than zero.");
			}

			// See https://github.com/ai/nanoid/blob/master/format.js for
			// explanation why masking is use (`random % alphabet` is a common
			// mistake security-wise).
			var mask = (2 << 31 - Clz32((alphabet.Length - 1) | 1)) - 1;
			var step = (int)Math.Ceiling(1.6 * mask * size.Value / alphabet.Length);

#if NETSTANDARD2_1
        Span<char> idBuilder = stackalloc char[size];
        Span<byte> bytes = stackalloc byte[step];
#else
			var idBuilder = new char[size.Value];
			var bytes = new byte[step];
#endif

			int cnt = 0;

			while (true)
			{

				random.NextBytes(bytes);

				for (var i = 0; i < step; i++)
				{

					var alphabetIndex = bytes[i] & mask;

					if (alphabetIndex >= alphabet.Length) continue;
					idBuilder[cnt] = alphabet[alphabetIndex];
					if (++cnt == size)
					{
						return new string(idBuilder);
					}

				}

			}

		}

		/// <summary>
		/// Counts leading zeros of <paramref name="x"/>.
		/// </summary>
		/// <param name="x">Input number.</param>
		/// <returns>Number of leading zeros.</returns>
		/// <remarks>
		/// Courtesy of spender/Sunsetquest see https://stackoverflow.com/a/10439333/623392.
		/// </remarks>
		internal static int Clz32(int x)
		{
			const int numIntBits = sizeof(int) * 8; //compile time constant
													//do the smearing
			x |= x >> 1;
			x |= x >> 2;
			x |= x >> 4;
			x |= x >> 8;
			x |= x >> 16;
			//count the ones
			x -= x >> 1 & 0x55555555;
			x = (x >> 2 & 0x33333333) + (x & 0x33333333);
			x = (x >> 4) + x & 0x0f0f0f0f;
			x += x >> 8;
			x += x >> 16;
			return numIntBits - (x & 0x0000003f); //subtract # of 1s from 32
		}
	}
}
