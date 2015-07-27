using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TruckEntryList {
    public static class FileManager {

        public static bool AddToFile(string file, TruckInfo entry) {
            using (Stream stream = File.Open(file, FileMode.Open, FileAccess.ReadWrite)) {
                return AddToFile(stream, entry);
            }
        }

        public static bool AddToFile(Stream stream, TruckInfo entry) {
            if (!stream.CanRead || !stream.CanWrite || !stream.CanSeek)
                return false;

            stream.Seek(0, SeekOrigin.Begin);
            byte[] posByte = new byte[4];
            if (stream.Length != 0) {
                stream.Read(posByte, 0, 4);
            } else posByte = BitConverter.GetBytes(0);
            int pos = BitConverter.ToInt32(posByte, 0);
            entry.nrCrt = ++pos;
            stream.Seek(0, SeekOrigin.Begin);
            stream.Write(BitConverter.GetBytes(pos), 0, 4);
            stream.Seek(0, SeekOrigin.End);
            entry.WriteObject(stream);
            return true;
        }

        public static bool ReplaceInFile() {
            return true;
        }

    }

    public class FixedObjectFileStream : FileStream {
        public static string TempStreamFile = Path.GetTempFileName();
        private int numberOfObjects;
        public int NumberOfObjects { get { return numberOfObjects; } }

        public FixedObjectFileStream(string file, FileMode mode, FileAccess access, bool append = false) : base(file, mode, access) {

            if (mode == FileMode.Open && access == FileAccess.Read && base.Length == 0) {
                throw new ArgumentException("Not a FixedObject File!");
            } else if (base.Length == 0) {
                numberOfObjects = 0;
                base.Write(BitConverter.GetBytes(0), 0, 4);
            } else if (mode == FileMode.Append) {
                throw new ArgumentException("FixedObject File cannot be opened in Append mode!");
            } else {
                byte[] buff = new byte[4];
                base.Read(buff, 0, 4);
                numberOfObjects = BitConverter.ToInt32(buff, 0);
            }

            if (append)
                base.Seek(0, SeekOrigin.End);
        }

        public FixedObjectFileStream(Stream stream) : base(TempStreamFile, FileMode.Create, FileAccess.ReadWrite, FileShare.None, 4096, FileOptions.DeleteOnClose) {

            byte[] buffer = new byte[4096];
            int sz;
            bool empty = true;
            while ((sz = stream.Read(buffer, 0, 4096)) > 0) {
                base.Write(buffer, 0, sz);
                empty = false;
            }

            if (empty) {
                base.Seek(0, SeekOrigin.Begin);
                base.Write(BitConverter.GetBytes(0), 0, 4);
            }

            base.Seek(0, SeekOrigin.Begin);

            base.Read(buffer, 0, 4);
            numberOfObjects = BitConverter.ToInt32(buffer, 0);
        }

        public void FillTestStream(Stream ms) {
            ms.Seek(0, SeekOrigin.Begin);
            long pos = base.Position;
            base.Seek(0, SeekOrigin.Begin);
            byte[] buff = new byte[4096];
            int sz;
            while ((sz = base.Read(buff, 0, 4096)) > 0) {
                ms.Write(buff, 0, sz);
            }
            ms.Seek(0, SeekOrigin.Begin);

            base.Position = pos;
        }

        public override long Length {
            get {
                return (base.Length - 4) / TruckInfo.sizeInBytes;
            }
        }

        public override long Position {
            get {
                return (base.Position - 4) / TruckInfo.sizeInBytes;
            }

            set {
                base.Position = value * TruckInfo.sizeInBytes + 4;
            }
        }

        public override int Read(byte[] buffer, int offset, int count) {
            throw new Exception("You can't read bytes from this stream.");
        }

        private int Read(TruckInfo[] entries, int offset, int count) {
            int actualCount = offset;

            Seek(offset, SeekOrigin.Current);

            while (base.Position < base.Length && actualCount < count + offset) {
                byte[] buffer = new byte[TruckInfo.sizeInBytes];
                base.Read(buffer, 0, TruckInfo.sizeInBytes);
                MemoryStream ms = new MemoryStream(buffer);

                entries[actualCount++] = new TruckInfo(ms);
            }
            return actualCount - offset;
        }

        public override long Seek(long offset, SeekOrigin origin) {
            offset *= TruckInfo.sizeInBytes;
            if (origin == SeekOrigin.Begin)
                if (offset < 0)
                    return base.Seek(offset, origin);
                else
                    return base.Seek(offset + 4, origin);
            else if (origin == SeekOrigin.Current && base.Position + offset < 4)
                return base.Seek(offset - 4, origin);
            else if (origin == SeekOrigin.End && base.Length + offset < 4)
                return base.Seek(offset - 4, origin);
            else return base.Seek(offset, origin);
        }

        public override void SetLength(long value) {
            if (value >= 0)
                base.SetLength(value * TruckInfo.sizeInBytes + 4);
            else base.SetLength(value * TruckInfo.sizeInBytes);
        }

        public override void Write(byte[] buffer, int offset, int count) {
            throw new Exception("You can't write bytes to this stream.");
        }

        private void Write(TruckInfo[] entries, int offset, int count) {
            int actualOffset = offset;
            byte[] buffer = new byte[TruckInfo.sizeInBytes];
            MemoryStream ms = new MemoryStream(buffer);

            while (actualOffset < count + offset) {
                ms.Seek(0, SeekOrigin.Begin);
                entries[actualOffset++].WriteObject(ms);

                base.Write(buffer, 0, TruckInfo.sizeInBytes);
            }
        }


        public void Add(TruckInfo entry) {
            Add(new TruckInfo[] { entry });
        }

        public void Add(TruckInfo[] entries) {
            base.Seek(0, SeekOrigin.End);
            Write(entries, 0, entries.Length);

            long pos = base.Position;
            base.Seek(0, SeekOrigin.Begin);
            base.Write(BitConverter.GetBytes(numberOfObjects += entries.Length), 0, 4);
            base.Position = pos;
        }

        public void RemoveAt(int position) {
            int len;
            Seek(position + 1, SeekOrigin.Begin);
            TruckInfo[] buffer = new TruckInfo[10];
            while ((len = Read(buffer, 0, 10)) > 0) {
                for (int i = 0; i < len; i++) {
                    buffer[i].nrCrt--;
                }
                Seek(-len - 1, SeekOrigin.Current);
                Write(buffer, 0, len);
                Seek(1, SeekOrigin.Current);
            }
            SetLength(Length - 1);

            long pos = base.Position;
            base.Seek(0, SeekOrigin.Begin);
            base.Write(BitConverter.GetBytes(--numberOfObjects), 0, 4);
            base.Position = pos;
        }

        public TruckInfo this[int pos] {
            get {
                if (pos >= Length) throw new IndexOutOfRangeException();

                TruckInfo[] ret = new TruckInfo[1];
                ret[0] = new TruckInfo();

                Seek(pos, SeekOrigin.Begin);
                Read(ret, 0, 1);

                return ret[0];
            }
            set {
                TruckInfo[] set = new TruckInfo[1];
                set[0] = value;

                if (pos > Length) throw new ArgumentOutOfRangeException("You can only write to the first position after the end of file.");

                Seek(pos, SeekOrigin.Begin);
                Write(set, 0, 1);

                if (pos == Length) {
                    long poss = base.Position;
                    base.Seek(0, SeekOrigin.Begin);
                    base.Write(BitConverter.GetBytes(++numberOfObjects), 0, 4);
                    base.Position = poss;
                }
            }
        }
    }
}
