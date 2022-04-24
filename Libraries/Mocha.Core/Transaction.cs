//
//  Transaction.cs
//
//  Author:
//       Michael Becker <alcexhim@gmail.com>
//
//  Copyright (c) 2021 Mike Becker's Software
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Collections;
using System.Collections.Generic;
using MBS.Framework;
using MBS.Framework.Collections.Generic;

namespace Mocha.Core
{
	// FIXME: this should be extrapolated into a Collections.Generic.Blockchain<T> implementation
	public class Transaction
	{
		private Transaction _next = null;
		private Transaction _prev = null;

		private byte[] hash = null;
		public string Hashstr { get { return ToHexstring(hash); } }

		private string ToHexstring(byte[] data)
		{
			// 00 AA CC DD
			// data.len == 4, str.len == 8
			char[] cc = new char[data.Length * 2];
			for (int i = 0; i < data.Length; i++)
			{
				string t = data[i].ToString("x").PadLeft(2, '0');
				cc[(i * 2)] = t[0];
				cc[(i * 2) + 1] = t[1];
			}

			// 0    1    2    3   
			//  0 1  2 3  4 5  6 7
			return new string(cc);
		}

		private byte[] GetData()
		{
			System.IO.MemoryStream ms = new System.IO.MemoryStream();
			System.IO.BinaryWriter bw = new System.IO.BinaryWriter(ms);
			bw.Write(Operations.Count);
			foreach (TransactionOperation op in Operations)
			{
				bw.Write(op.GetData());
			}
			bw.Close();
			return ms.ToArray();
		}

		public class TransactionCollection : AppendOnlyLinkedList<Transaction>
		{
			protected virtual System.Security.Cryptography.HashAlgorithm CreateHashAlgorithm()
			{
				return new System.Security.Cryptography.SHA512Managed();
			}

			protected override void InsertItem(Transaction item)
			{
				System.Security.Cryptography.HashAlgorithm sha = CreateHashAlgorithm();

				LinkedListNode<Transaction> prev = Last;

				System.IO.MemoryStream ms = new System.IO.MemoryStream();
				System.IO.BinaryWriter bw = new System.IO.BinaryWriter(ms);
				bw.Write(item.GetData());
				while (prev != null)
				{
					bw.Write(prev.Value.GetData());

					prev = prev.Previous;
				}
				bw.Close();

				item.hash = sha.ComputeHash(ms.ToArray());

				base.InsertItem(item);
			}

			public bool Verify()
			{
				System.Security.Cryptography.HashAlgorithm sha = CreateHashAlgorithm();
				LinkedListNode<Transaction> prev = Last;

				System.IO.MemoryStream ms = new System.IO.MemoryStream();
				System.IO.BinaryWriter bw = new System.IO.BinaryWriter(ms);
				while (prev != null)
				{
					bw.Write(prev.Value.GetData());

					prev = prev.Previous;
				}
				bw.Close();

				if (Last.Value.hash.Matches(sha.ComputeHash(ms.ToArray())))
					return true;
				return false;
			}
		}

		public TenantHandle Tenant { get; } = TenantHandle.Empty;
		public Transaction(TenantHandle tenant)
		{
			Tenant = tenant;
		}

		public TransactionOperation.TransactionOperationCollection Operations { get; } = new TransactionOperation.TransactionOperationCollection();
	}
}
