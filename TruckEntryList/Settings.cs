using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
            txtRaportFolder.Text = PresenterSettings.raportFolder;
            ValidateData();
        }

        private void cmdSave_Click(object sender, EventArgs e)
        {
            PresenterSettings.hourZone = (float)numHour.Value;
            PresenterSettings.nrAutoZone = (float)numNrAuto.Value;
            PresenterSettings.payloadZone = (float)numPayload.Value;
            PresenterSettings.nextTrucksZone = (float)numNextTrucks.Value;
            PresenterSettings.raportFolder = txtRaportFolder.Text;
            this.Close();
        }

        private void inputs_ValueChanged(object sender, EventArgs e)
        {
            ValidateData();
        }

        private void ValidateData()
        {
            bool rExists = false;
            try
            {
                DirectoryInfo di = new DirectoryInfo(Path.GetFullPath(txtRaportFolder.Text));
                rExists = di.Exists;
            }
            catch (Exception ex) { }

            if (numHour.Value + numNrAuto.Value + numPayload.Value + numNextTrucks.Value == 1 && rExists)
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

        private void cmdBrowseFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = Path.GetFullPath("./");
            if (fbd.ShowDialog() == DialogResult.OK)
                txtRaportFolder.Text = fbd.SelectedPath;
        }
    }
}
