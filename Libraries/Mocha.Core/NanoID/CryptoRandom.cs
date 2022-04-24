//
//  CryptoRandom.cs - from the nanoid project, https://github.com/ai/nanoid - MIT License
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
using System.Security.Cryptography;

namespace Mocha.Core.NanoID
{
	public class CryptoRandom : Random
	{
		private static RandomNumberGenerator _r;
#if !NETSTANDARD2_1
		private readonly byte[] _uint32Buffer = new byte[4];
#endif
		/// <inheritdoc />
		/// <summary>
		/// </summary>
		public CryptoRandom()
		{
			_r = RandomNumberGenerator.Create();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="buffer"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public override void NextBytes(byte[] buffer)
		{
			if (buffer == null) throw new ArgumentNullException(nameof(buffer));
			_r.GetBytes(buffer);
		}

#if NETSTANDARD2_1
    /// <inheritdoc/>
    public override void NextBytes(Span<byte> buffer)
    {
        RandomNumberGenerator.Fill(buffer);
    }
#endif

		/// <inheritdoc />
		/// <summary>
		/// </summary>
		/// <returns></returns>
		public override double NextDouble()
		{
#if NETSTANDARD2_1
        Span<byte> uint32Buffer = stackalloc byte[4];
        RandomNumberGenerator.Fill(uint32Buffer);
        return BitConverter.ToUInt32(uint32Buffer) / (1.0 + UInt32.MaxValue);
#else
			_r.GetBytes(_uint32Buffer);
			return BitConverter.ToUInt32(_uint32Buffer, 0) / (1.0 + UInt32.MaxValue);
#endif
		}
		/// <inheritdoc />
		/// <summary>
		/// </summary>
		/// <param name="minValue"></param>
		/// <param name="maxValue"></param>
		/// <returns></returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException"></exception>
		public override int Next(int minValue, int maxValue)
		{
			if (minValue > maxValue) throw new ArgumentOutOfRangeException(nameof(minValue));
			if (minValue == maxValue) return minValue;
			var range = (long)maxValue - minValue;
			return (int)((long)Math.Floor(NextDouble() * range) + minValue);
		}
		/// <inheritdoc />
		/// <summary>
		/// </summary>
		/// <returns></returns>
		public override int Next()
		{
			return Next(0, int.MaxValue);
		}
		/// <inheritdoc />
		/// <summary>
		/// </summary>
		/// <param name="maxValue"></param>
		/// <returns></returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException"></exception>
		public override int Next(int maxValue)
		{
			if (maxValue < 0) throw new ArgumentOutOfRangeException(nameof(maxValue));
			return Next(0, maxValue);
		}
	}
}
