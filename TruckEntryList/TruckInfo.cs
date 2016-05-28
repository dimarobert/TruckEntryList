using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace TruckEntryList {
    public class TruckInfo {
        public const int sizeInBytes = 415;

        public TruckInfo() {
            _payload = "";
            _nrAuto = "";
        }

        public TruckInfo(Stream s) { ReadObject(s); }

        public int nrCrt { get; set; }
        private string _nrAuto;
        public string nrAuto {
            get { return _nrAuto; }
            set {
                if (value.Length > 10) throw new ArgumentException("Maximum length of this property is 10.");
                var s = value.Split(new char[] { '-', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (s.Length == 3) {
                    _nrAuto = String.Join("-", s).ToUpper();
                } else throw new ArgumentException();
            }
        }

        private string _payload;
        public string payload {
            get { return _payload; }
            set {
                if (value.Length > 30)
                    throw new ArgumentException("Maximum length of this property is 30.");
                _payload = value;
            }
        }
        public DateTime dateRegistered { get; set; }
        public DateTime dateEntry { get; set; }

        public string comments { get; set; }
        public DateTime dateSkip { get; set; }
        public DateTime dateReturn { get; set; }

        [Obsolete("Use WriteObject")]
        public override string ToString() {
            return nrCrt + "|" + nrAuto + "|" + payload + "|" + dateRegistered;
        }

        [Obsolete("Use ReadObject")]
        public static bool TryParse(string s, out TruckInfo result) {
            result = null;
            var itemProps = s.Split('|');

            if (itemProps.Length == 4 || itemProps.Length == 5) {
                TruckInfo ti = new TruckInfo();

                int nrcrt;
                if (int.TryParse(itemProps[0], out nrcrt)) {
                    ti.nrCrt = nrcrt;
                } else {
                    return false;
                }
                ti.nrAuto = itemProps[1];
                ti.payload = itemProps[2];
                DateTime time;
                if (DateTime.TryParseExact(itemProps[3], "HH:mm:ss dd.MM.yyyy", new CultureInfo("ro-RO"), DateTimeStyles.None, out time)) {
                    ti.dateRegistered = time;
                } else {
                    return false;
                }

                if (itemProps.Length == 5) {
                    if (DateTime.TryParseExact(itemProps[4], "HH:mm:ss dd.MM.yyyy", new CultureInfo("ro-RO"), DateTimeStyles.None, out time)) {
                        ti.dateEntry = time;
                    } else {
                        return false;
                    }
                }

                result = ti;
                return true;
            }
            return false;
        }

        public static bool TryParse(Stream s, out TruckInfo result) {
            result = new TruckInfo();
            try {
                result.ReadObject(s);
                return true;
            } catch (Exception ex) {
                return false;
            }
        }

        public void WriteObject(Stream stream) {
            byte[] stringBuffer = new byte[255];

            stream.Write(BitConverter.GetBytes(nrCrt), 0, sizeof(int));

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

            Array.Clear(stringBuffer, 0, 255);
            Buffer.BlockCopy(Encoding.Unicode.GetBytes(comments ?? ""), 0, stringBuffer, 0, Math.Min(255, Encoding.Unicode.GetByteCount((comments ?? ""))));
            stream.Write(stringBuffer, 0, 255);

            Array.Clear(stringBuffer, 0, 255);
            date = dateSkip.ToString("HH:mm:ss dd.MM.yyyy");
            Buffer.BlockCopy(Encoding.ASCII.GetBytes(date), 0, stringBuffer, 0, 19);
            stream.Write(stringBuffer, 0, 19);

            Array.Clear(stringBuffer, 0, 255);
            date = dateReturn.ToString("HH:mm:ss dd.MM.yyyy");
            Buffer.BlockCopy(Encoding.ASCII.GetBytes(date), 0, stringBuffer, 0, 19);
            stream.Write(stringBuffer, 0, 19);
        }

        public void ReadObject(Stream stream) {
            byte[] stringBuffer = new byte[255];

            stream.Read(stringBuffer, 0, 4);
            nrCrt = BitConverter.ToInt32(stringBuffer, 0);

            Array.Clear(stringBuffer, 0, 20);
            stream.Read(stringBuffer, 0, 20);
            _nrAuto = string.Join("", Encoding.Unicode.GetString(stringBuffer).TakeWhile(c => c != '\0'));

            Array.Clear(stringBuffer, 0, 20);
            stream.Read(stringBuffer, 0, 60);
            payload = string.Join("", Encoding.Unicode.GetString(stringBuffer).TakeWhile(c => c != '\0'));

            Array.Clear(stringBuffer, 0, 60);
            stream.Read(stringBuffer, 0, 19);
            DateTime dt;
            if (DateTime.TryParseExact(string.Join("", Encoding.ASCII.GetString(stringBuffer).TakeWhile(c => c != '\0')), "HH:mm:ss dd.MM.yyyy", new CultureInfo("ro-RO"), DateTimeStyles.None, out dt))
                dateRegistered = dt;
            else throw new FormatException("Could not decode the Registration Date.");

            Array.Clear(stringBuffer, 0, 19);
            stream.Read(stringBuffer, 0, 19);
            if (DateTime.TryParseExact(string.Join("", Encoding.ASCII.GetString(stringBuffer).TakeWhile(c => c != '\0')), "HH:mm:ss dd.MM.yyyy", new CultureInfo("ro-RO"), DateTimeStyles.None, out dt))
                dateEntry = dt;
            else throw new FormatException("Could not decode the Entry Date.");

            Array.Clear(stringBuffer, 0, 255);
            stream.Read(stringBuffer, 0, 255);
            comments = string.Join("", Encoding.Unicode.GetString(stringBuffer).TakeWhile(c => c != '\0'));

            Array.Clear(stringBuffer, 0, 255);
            stream.Read(stringBuffer, 0, 19);
            if (DateTime.TryParseExact(string.Join("", Encoding.ASCII.GetString(stringBuffer).TakeWhile(c => c != '\0')), "HH:mm:ss dd.MM.yyyy", new CultureInfo("ro-RO"), DateTimeStyles.None, out dt))
                dateSkip = dt;
            else throw new FormatException("Could not decode the Skip Date.");

            Array.Clear(stringBuffer, 0, 20);
            stream.Read(stringBuffer, 0, 19);
            if (DateTime.TryParseExact(string.Join("", Encoding.ASCII.GetString(stringBuffer).TakeWhile(c => c != '\0')), "HH:mm:ss dd.MM.yyyy", new CultureInfo("ro-RO"), DateTimeStyles.None, out dt))
                dateReturn = dt;
            else throw new FormatException("Could not decode the Return Date.");
        }

        public static bool operator ==(TruckInfo one, TruckInfo other) {
            return one.nrCrt == other.nrCrt && one.nrAuto == other.nrAuto && one.payload == other.payload && one.dateEntry == other.dateEntry && one.dateRegistered == other.dateRegistered && one.comments == other.comments && one.dateSkip == other.dateSkip;
        }

        public static bool operator !=(TruckInfo one, TruckInfo other) {
            return !(one == other);
        }
    }
}