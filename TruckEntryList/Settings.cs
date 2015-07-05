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
    public partial class Settings : Form
    {

        public Settings()
        {
            InitializeComponent();

            numHour.Value = (decimal)PresenterSettings.hourZone;
            numNrAuto.Value = (decimal)PresenterSettings.nrAutoZone;
            numPayload.Value = (decimal)PresenterSettings.payloadZone;
            numNextTrucks.Value = (decimal)PresenterSettings.nextTrucksZone;
            ValidateData();
        }

        private void cmdSave_Click(object sender, EventArgs e)
        {
            PresenterSettings.hourZone = (float)numHour.Value;
            PresenterSettings.nrAutoZone = (float)numNrAuto.Value;
            PresenterSettings.payloadZone = (float)numPayload.Value;
            PresenterSettings.nextTrucksZone = (float)numNextTrucks.Value;
            this.Close();
        }

        private void numHour_ValueChanged(object sender, EventArgs e)
        {
            ValidateData();
        }

        private void ValidateData()
        {
            if (numHour.Value + numNrAuto.Value + numPayload.Value + numNextTrucks.Value == 1)
            {
                lblValidation.ForeColor = Color.Green;
                lblValidation.Text = "OK";
                lblValidation.Location = new Point(cmdSave.Location.X - lblValidation.Size.Width, lblValidation.Location.Y);
                cmdSave.Enabled = true;
            }
            else
            {
                lblValidation.ForeColor = Color.Red;
                lblValidation.Text = "Invalid";
                lblValidation.Location = new Point(cmdSave.Location.X - lblValidation.Size.Width, lblValidation.Location.Y);
                cmdSave.Enabled = false;
            }
        }
    }
}
