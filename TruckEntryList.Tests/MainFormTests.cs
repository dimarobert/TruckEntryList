using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TruckEntryList;

namespace TruckEntryList.Tests {
    [TestClass]
    public class MainFormTests {

        private static MainForm GetMainFormInstance() {
            return new MainForm();
        }

        [TestMethod]
        public void LoadImagesTest() {
            MainForm instance = GetMainFormInstance();

            Assert.IsTrue(true, "Not Implemented");

        }

    }
}
