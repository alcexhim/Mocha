//
//  SlickBinaryDataFormat.cs
//
//  Author:
//       Michael Becker <alcexhim@gmail.com>
//
//  Copyright (c) 2020 Mike Becker's Software
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
using UniversalEditor.IO;
using UniversalEditor.ObjectModels.FileSystem;

namespace UniversalEditor.Plugins.Mocha.DataFormats.MochaBinary.Slick
{
	public class SlickBinaryDataFormat : DataFormat
	{
		public SlickBinaryDataFormat()
		{
		}

		public int FileNameSize { get; set; } = 128;

		private static DataFormatReference _dfr = null;
		protected override DataFormatReference MakeReferenceInternal()
		{
			if (_dfr == null)
			{
				_dfr = base.MakeReferenceInternal();
				_dfr.Capabilities.Add(typeof(FileSystemObjectModel), DataFormatCapabilities.All);
			}
			return _dfr;
		}

		protected override void LoadInternal(ref ObjectModel objectModel)
		{
			FileSystemObjectModel fsom = (objectModel as FileSystemObjectModel);
			if (fsom == null)
				throw new ObjectModelNotSupportedException();

			Reader reader = Accessor.Reader;
			string signature = reader.ReadFixedLengthString(8);
			if (!signature.Equals("slick!\0\0"))
			{
				Console.Error.WriteLine("expected 'slick!\\0\\0', got '{0}'", signature);
				throw new InvalidDataFormatException();
			}

			float version = reader.ReadSingle();
			if (version != 1.0f)
				throw new InvalidDataFormatException();

			FileNameSize = reader.ReadInt32();

			int fileCount = reader.ReadInt32();
			for (int i = 0; i < fileCount; i++)
			{
				string fileName = reader.ReadFixedLengthString(FileNameSize).TrimNull();
				int offset = reader.ReadInt32();
				int length = reader.ReadInt32();

				File file = fsom.AddFile(fileName);
				file.Properties["reader"] = reader;
				file.Properties["offset"] = offset;
				file.Properties["length"] = length;
				file.DataRequest += File_DataRequest;
			}
		}

		void File_DataRequest(object sender, DataRequestEventArgs e)
		{
			File file = (sender as File);
			int offset = (int)file.Properties["offset"];
			int length = (int)file.Properties["length"];
			Reader reader = (Reader)file.Properties["reader"];

			reader.Accessor.Seek(offset, SeekOrigin.Begin);
			e.Data = reader.ReadBytes(length);
		}

		protected override void SaveInternal(ObjectModel objectModel)
		{
			FileSystemObjectModel fsom = (objectModel as FileSystemObjectModel);
			if (fsom == null)
				throw new ObjectModelNotSupportedException();

			Writer writer = Accessor.Writer;
			writer.WriteFixedLengthString("slick!\0\0");
			writer.WriteSingle(1.0f);

			writer.WriteInt32(FileNameSize);

			File[] files = fsom.GetAllFiles();
			writer.WriteInt32(files.Length);

			int foffset = 20 + ((FileNameSize + 8) * files.Length);
			for (int i = 0; i < files.Length; i++)
			{
				writer.WriteFixedLengthString(files[i].Name, FileNameSize);
				writer.WriteInt32(foffset);

				int datalen = files[i].GetData().Length;
				writer.WriteInt32(datalen);

				foffset += datalen;
			}

			for (int i = 0; i < files.Length; i++)
			{
				writer.WriteBytes(files[i].GetData());
			}
		}
	}
}
