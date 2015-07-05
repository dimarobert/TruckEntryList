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
    public partial class Presenter : Form
    {

        MainForm parent;

        public Presenter(Form owner)
        {
            this.Owner = owner;
            InitializeComponent();
            this.ClientSize = new Size(937, 668);
            this.lstTruckOrder.Size = new Size(331, 642);

            label1.Visible = false;
            lblHour.Text = DateTime.Now.ToString("dd.MM.yyyy\nHH:MM:ss");

            parent = (MainForm)this.Owner;
            parent.PropertyChanged += Parent_PropertyChanged;

            Timer hourTimer = new Timer();
            hourTimer.Tick += HourTimer_Tick;
            hourTimer.Interval = 1000;
            hourTimer.Start();
        }

        private static Screen GetCurrentScreen(Form form)
        {
            return Screen.FromControl(form);
        }

        private void HourTimer_Tick(object sender, EventArgs e)
        {
            lblHour.Text = DateTime.Now.ToString("dd.MM.yyyy\nHH:MM:ss");
        }

        private void Parent_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "EntryData")
            {
                UpdateData();
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0112) // WM_SYSCOMMAND
            {
                // Check your window state here
                if (m.WParam == new IntPtr(0xF030)) // Maximize event - SC_MAXIMIZE from Winuser.h
                {
                    this.WindowState = FormWindowState.Maximized;
                    WindowMaximize();
                }
                if (m.WParam == new IntPtr(0xF120))
                {
                    this.WindowState = FormWindowState.Normal;
                    WindowMaximize();
                }
            }
            base.WndProc(ref m);
        }

        private void WindowMaximize()
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.FormBorderStyle = FormBorderStyle.None;
                label1.Visible = true;
                label1.Location = new Point(10, 10);
            }
            else
            {
                this.FormBorderStyle = FormBorderStyle.Sizable;
                label1.Visible = false;
            }
        }


        private void Presenter_Load(object sender, EventArgs e)
        {
            UpdateDisplay();
        }

        private void Presenter_Resize(object sender, EventArgs e)
        {
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            var availableArea = ClientSize;
            lstTruckOrder.Location = new Point(availableArea.Width - lstTruckOrder.Size.Width, 0);
            lstTruckOrder.Size = new Size(lstTruckOrder.Width, availableArea.Height);

            availableArea.Width = lstTruckOrder.Location.X;

            int hourHeight = (int)(availableArea.Height * 0.2);
            int nrAutoHeight = (int)(availableArea.Height * 0.45);
            int payloadHeight = (int)(availableArea.Height * 0.25);
            int nextTrucksHeight = (int)(availableArea.Height * 0.1);



            lblHour.Location = new Point(0, 0);
            lblHour.Size = new Size(availableArea.Width, hourHeight);
            UpdateLabelFontSize(lblHour);
            lblHour.TextAlign = ContentAlignment.MiddleCenter;
            lblHour.BackColor = Color.Red;

            lblNrAuto.Location = new Point(0, hourHeight);
            lblNrAuto.Size = new Size(availableArea.Width, nrAutoHeight);
            UpdateLabelFontSize(lblNrAuto);
            lblNrAuto.TextAlign = ContentAlignment.MiddleCenter;
            lblNrAuto.BackColor = Color.Blue;

            lblPayload.Location = new Point(0, hourHeight + nrAutoHeight);
            lblPayload.Size = new Size(availableArea.Width, payloadHeight);
            UpdateLabelFontSize(lblPayload);
            lblPayload.TextAlign = ContentAlignment.MiddleCenter;
            lblPayload.BackColor = Color.Yellow;

            lblNextTruck.Location = new Point(0, hourHeight + nrAutoHeight + payloadHeight);
            lblNextTruck.Size = new Size(availableArea.Width, nextTrucksHeight);
            UpdateLabelFontSize(lblNextTruck);
            lblNextTruck.TextAlign = ContentAlignment.MiddleCenter;
            lblNextTruck.BackColor = Color.Green;
        }

        private void UpdateLabelFontSize(Label lbl)
        {
            Bitmap fakeImg = new Bitmap(1, 1);
            Graphics g = Graphics.FromImage(fakeImg);

            var extent = g.MeasureString(lbl.Text, lbl.Font);

            if (extent.Height == 0 || extent.Width == 0)
                return;

            float hRatio = Math.Max(lbl.Size.Height, 0) / extent.Height;
            float wRatio = Math.Max((lbl.Size.Width - 20), 0) / extent.Width;
            float ratio = (hRatio < wRatio) ? hRatio : wRatio;

            if (ratio > 0)
            {
                float newSize = lbl.Font.Size * ratio;
                lbl.Font = new Font(lbl.Font.FontFamily, newSize, lbl.Font.Style);
            }
        }

        private void UpdateData()
        {
            lstTruckOrder.Items.Clear();

            for (int i = 0; i < parent.EntryData.Count; i++)
            {
                AddToList(parent.EntryData[i]);
            }

            lblNrAuto.Text = parent.EntryData[0].nrAuto;
            lblPayload.Text = parent.EntryData[0].payload;
            lblNextTruck.Text = "";
            for (int i = 1; i < parent.EntryData.Count && i < 4; i++)
            {
                lblNextTruck.Text += parent.EntryData[i].nrAuto + (i == 3 ? "" : "    ");
            }
            UpdateDisplay();
        }

        private void AddToList(TruckInfo entry)
        {
            ListViewItem lvi = new ListViewItem();
            lvi.Text = entry.nrCrt.ToString();

            ListViewItem.ListViewSubItem lvsi = new ListViewItem.ListViewSubItem();
            lvsi.Text = entry.nrAuto;
            lvi.SubItems.Add(lvsi);

            lvsi = new ListViewItem.ListViewSubItem();
            lvsi.Text = entry.payload;
            lvi.SubItems.Add(lvsi);

            lvsi = new ListViewItem.ListViewSubItem();
            lvsi.Text = entry.dateRegistered.ToString("HH:MM  dd.MM.yyyy");
            lvi.SubItems.Add(lvsi);


            lstTruckOrder.Items.Add(lvi);
        }

        private void label1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            WindowMaximize();
        }

        private void label1_MouseEnter(object sender, EventArgs e)
        {
            label1.BackColor = Color.Blue;
        }

        private void label1_MouseLeave(object sender, EventArgs e)
        {
            label1.BackColor = Color.Red;
        }
    }
}
