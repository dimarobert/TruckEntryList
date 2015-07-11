using System;
using System.IO;
using System.Text;

namespace TruckEntryList
{
    public class TruckInfo
    {
        public const int sizeInBytes = 122;

        public TruckInfo() { }

        public TruckInfo(Stream s) { ReadObject(s); }

        public int nrCrt { get; set; }
        private string _nrAuto;
        public string nrAuto
        {
            get { return _nrAuto; }
            set
            {
                if (value.Length > 10) throw new ArgumentException("Maximum length of this property is 10.");
                var s = value.Split(new char[] { '-', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (s.Length == 3)
                {
                    _nrAuto = String.Join("-", s);
                }
                else throw new ArgumentException();
            }
        }

        private string _payload;
        public string payload
        {
            get { return _payload; }
            set
            {
                if (value.Length > 30)
                    throw new ArgumentException("Maximum length of this property is 30.");
                _payload = value;
            }
        }
        public DateTime dateRegistered { get; set; }
        public DateTime dateEntry { get; set; }

        public override string ToString()
        {
            return nrCrt + "|" + nrAuto + "|" + payload + "|" + dateRegistered;
        }

        public static bool TryParse(string s, out TruckInfo result)
        {
            result = null;
            var itemProps = s.Split('|');

            if (itemProps.Length == 4 || itemProps.Length == 5)
            {
                TruckInfo ti = new TruckInfo();

                int nrcrt;
                if (int.TryParse(itemProps[0], out nrcrt))
                {
                    ti.nrCrt = nrcrt;
                }
                else
                {
                    return false;
                }
                ti.nrAuto = itemProps[1];
                ti.payload = itemProps[2];
                DateTime time;
                if (DateTime.TryParse(itemProps[3], out time))
                {
                    ti.dateRegistered = time;
                }
                else
                {
                    return false;
                }

                if (itemProps.Length == 5)
                {
                    if (DateTime.TryParse(itemProps[4], out time))
                    {
                        ti.dateEntry = time;
                    }
                    else
                    {
                        return false;
                    }
                }

                result = ti;
                return true;
            }
            return false;
        }

        public static bool TryParse(Stream s, out TruckInfo result)
        {
            result = new TruckInfo();
            try
            {
                result.ReadObject(s);
                return true;
            } catch(Exception ex)
            {
                return false;
            }
        }

        public void WriteObject(Stream stream)
        {
            byte[] stringBuffer = new byte[60];
            byte[] intBuffer = new byte[4];

            Buffer.BlockCopy(BitConverter.GetBytes(nrCrt), 0, intBuffer, 0, sizeof(int));
            stream.Write(intBuffer, 0, sizeof(int));

            Buffer.BlockCopy(Encoding.Unicode.GetBytes(_nrAuto), 0, stringBuffer, 0, Math.Min(20, Encoding.Unicode.GetByteCount(_nrAuto)));
            stream.Write(stringBuffer, 0, 20);

            Array.Clear(stringBuffer, 0, 20);
            Buffer.BlockCopy(Encoding.Unicode.GetBytes(payload), 0, stringBuffer, 0, Math.Min(60, Encoding.Unicode.GetByteCount(payload)));
            stream.Write(stringBuffer, 0, 60);

            Array.Clear(stringBuffer, 0, 60);
            string date = dateRegistered.ToString("HH:mm:ss dd.MM.yyyy");
            Buffer.BlockCopy(Encoding.ASCII.GetBytes(date), 0, stringBuffer, 0, 19);
            stream.Write(stringBuffer, 0, 19);

            Array.Clear(stringBuffer, 0, 20);
            date = dateEntry.ToString("HH:mm:ss dd.MM.yyyy");
            Buffer.BlockCopy(Encoding.ASCII.GetBytes(date), 0, stringBuffer, 0, 19);
            stream.Write(stringBuffer, 0, 19);
        }

        public void ReadObject(Stream stream)
        {
            byte[] stringBuffer = new byte[60];
            byte[] intBuffer = new byte[4];

            stream.Read(intBuffer, 0, 4);
            nrCrt = BitConverter.ToInt32(intBuffer, 0);

            stream.Read(stringBuffer, 0, 20);
            _nrAuto = Encoding.Unicode.GetString(stringBuffer).Replace("\0", "");

            Array.Clear(stringBuffer, 0, 20);
            stream.Read(stringBuffer, 0, 60);
            payload = Encoding.Unicode.GetString(stringBuffer).Replace("\0", "");

            Array.Clear(stringBuffer, 0, 60);
            stream.Read(stringBuffer, 0, 19);
            DateTime dt;
            if (DateTime.TryParse(Encoding.ASCII.GetString(stringBuffer), out dt))
                dateRegistered = dt;
            else throw new FormatException("Could not decode the Registration Date.");

            Array.Clear(stringBuffer, 0, 19);
            stream.Read(stringBuffer, 0, 19);
            if (DateTime.TryParse(Encoding.ASCII.GetString(stringBuffer), out dt))
                dateEntry = dt;
            else throw new FormatException("Could not decode the Entry Date.");
        }
    }
}