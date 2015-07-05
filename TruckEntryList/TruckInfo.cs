using System;

namespace TruckEntryList
{
    public class TruckInfo
    {
        public int nrCrt { get; set; }
        private string _nrAuto;
        public string nrAuto
        {
            get { return _nrAuto; }
            set
            {
                var s = value.Split(new char[] { '-', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (s.Length == 3)
                {
                    _nrAuto = String.Join("-", s);
                }
                else throw new ArgumentException();
            }
        }
        public string payload { get; set; }
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

            if (itemProps.Length == 4)
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
                result = ti;
                return true;
            }
            return false;
        }
    }
}