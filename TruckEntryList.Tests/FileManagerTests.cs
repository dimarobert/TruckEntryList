using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace TruckEntryList.Tests {
    [TestClass]
    public class FileManagerTests {

        [TestMethod]
        public void InitializeFixedObjectFileStreamTest() {
            Stream s = new MemoryStream();
            FixedObjectFileStream instance = new FixedObjectFileStream(s);

            Assert.IsTrue(File.Exists(FixedObjectFileStream.TempStreamFile), "FixedObjectFileStream temporary file creation failed");
            instance.Dispose();
            Assert.IsFalse(File.Exists(FixedObjectFileStream.TempStreamFile), "FixedObjectFileStream temporary file destruction failed");
        }

        [TestMethod]
        public void FixedObjectFileStreamAddTest() {

            var rcf = RandomCharFunc();
            var rcfN = RandomCharFunc(true);
            var rdf = RandomDayFunc();
            var rnf = RandomNumberFunc(10, 100);

            int rnd = rnf();

            TruckInfo[] tis = new TruckInfo[rnd];
            for (int i = 0; i < tis.Length; i++) {
                TruckInfo ti = tis[i];
                ti = new TruckInfo();
                ti.nrCrt = RandomNumberFunc(0)();
                ti.nrAuto = rcf().ToString() + rcf() + "-" + rcfN().ToString() + rcfN() + rcfN().ToString() + "-" + rcf().ToString() + rcf() + rcf().ToString();
                ti.payload = "" + rcf().ToString() + rcf() + rcf().ToString() + rcf() + rcf().ToString() + rcf() + rcf().ToString() + rcf() + rcf().ToString();
                ti.dateEntry = rdf();
                ti.dateRegistered = rdf();
                tis[i] = ti;
            }

            Stream ms = new MemoryStream();
            FixedObjectFileStream instance = new FixedObjectFileStream(ms);
            instance.Add(tis);
            instance.FillTestStream(ms);

            ms.Seek(0, SeekOrigin.Begin);
            int count;
            byte[] b = new byte[4];
            ms.Read(b, 0, 4);
            count = BitConverter.ToInt32(b, 0);
            Assert.AreEqual(count, tis.Length);

            foreach (TruckInfo ti in tis) {
                TruckInfo result = new TruckInfo(ms);

                Assert.IsTrue(ti == result);
            }
            
        }

        Func<DateTime> RandomDayFunc() {
            DateTime start = new DateTime(1995, 1, 1);
            Random gen = new Random((int)(new TimeSpan(DateTime.UtcNow.Ticks) - new TimeSpan(new DateTime(1970, 1, 1).Ticks)).TotalSeconds);
            int range = ((TimeSpan)(DateTime.Today - start)).Days;
            return () => start.AddDays(gen.Next(range));
        }
        Func<int> RandomNumberFunc(int min = int.MinValue, int max = int.MaxValue) {
            Random gen = new Random((int)(new TimeSpan(DateTime.UtcNow.Ticks) - new TimeSpan(new DateTime(1970, 1, 1).Ticks)).TotalSeconds);
            return () => gen.Next(min, max);
        }
        Func<char> RandomCharFunc(bool onlyNumbers = false) {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random gen = new Random((int)(new TimeSpan(DateTime.UtcNow.Ticks) - new TimeSpan(new DateTime(1970, 1, 1).Ticks)).TotalSeconds);
            return () => onlyNumbers ? chars[gen.Next(chars.IndexOf('0'), chars.Length)] : chars[gen.Next(chars.Length)];
        }

    }
}
