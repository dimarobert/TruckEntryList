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
            TruckInfo ti = new TruckInfo();
            ti.nrCrt = 5976868;
            ti.nrAuto = "b-984-cpq";
            ti.payload = "shit";
            ti.dateEntry = RandomDayFunc()();
            ti.dateRegistered = new DateTime(1876, 7, 22, 23, 41, 58);

            Stream ms = new MemoryStream();
            FixedObjectFileStream instance = new FixedObjectFileStream(ms);
            instance.Add(ti);
            instance.FillTestStream(ms);

            ms.Seek(4, SeekOrigin.Begin);
            TruckInfo result = new TruckInfo(ms);

            Assert.IsTrue(ti == result);
        }

        Func<DateTime> RandomDayFunc() {
            DateTime start = new DateTime(1995, 1, 1);
            Random gen = new Random((int)(new TimeSpan(DateTime.UtcNow.Ticks) - new TimeSpan(new DateTime(1970, 1, 1).Ticks)).TotalSeconds);
            int range = ((TimeSpan)(DateTime.Today - start)).Days;
            return () => start.AddDays(gen.Next(range));
        }
    }
}
