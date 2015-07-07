using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TruckEntryList
{
    public partial class RaportSelect : Form
    {
        public RaportSelect()
        {
            InitializeComponent();

            if (DateTime.Now.Hour < 1)
                raportDate.Value = DateTime.Now.AddDays(-1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }
    }
}
