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
        FormWindowState windowState;
        Size lastNormalSize;
        Point lastNormalLocation;

        public Presenter(Form owner)
        {
            InitializeComponent();

            label1.Visible = false;
            lblHour.Text = DateTime.Now.ToString("dd.MM.yyyy\nHH:mm:ss");
            lstTruckOrder.Items.Clear();
            lblNrAuto.Text = "";
            lblPayload.Text = "";
            lblNextTruck.Text = "";

            parent = (MainForm)owner;
            parent.PropertyChanged += Parent_PropertyChanged;
            UpdateData();

            Timer hourTimer = new Timer();
            hourTimer.Tick += HourTimer_Tick;
            hourTimer.Interval = 1000;
            hourTimer.Start();

            windowState = WindowState;
        }

        public void UpdatePresenterSettings()
        {
            UpdateDisplay();
        }

        private static Screen GetCurrentScreen(Form form)
        {
            return Screen.FromControl(form);
        }

        private void HourTimer_Tick(object sender, EventArgs e)
        {
            lblHour.Text = DateTime.Now.ToString("dd.MM.yyyy\nHH:mm:ss");
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
                    this.windowState = FormWindowState.Maximized;
                    WindowMaximize();
                }
                if (m.WParam == new IntPtr(0xF120))
                {
                    this.windowState = FormWindowState.Normal;
                    WindowMaximize();
                }
                if(m.WParam == new IntPtr(0xF020))
                {
                    this.windowState = FormWindowState.Minimized;
                    WindowMaximize();
                }
            }
            base.WndProc(ref m);
        }

        private void WindowMaximize()
        {
            if (this.windowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
                this.FormBorderStyle = FormBorderStyle.None;

                lastNormalSize = this.Size;
                lastNormalLocation = this.Location;

                this.Location = new Point(0, 0);
                Screen currentScreen = GetCurrentScreen(this);
                this.Size = new Size(currentScreen.WorkingArea.Width, currentScreen.WorkingArea.Height);

                label1.Visible = true;
                label1.Location = new Point(5, 5);
            }
            else if(this.windowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Normal;
                this.FormBorderStyle = FormBorderStyle.Sizable;

                this.Location = lastNormalLocation;
                this.Size = lastNormalSize;

                label1.Visible = false;
            }
            else if(this.windowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Minimized;
            }
        }
        
        private void Presenter_Load(object sender, EventArgs e)
        {
            lastNormalSize = this.Size;
            lastNormalLocation = this.Location;
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

            int hourHeight = (int)(availableArea.Height * PresenterSettings.hourZone);
            int nrAutoHeight = (int)(availableArea.Height * PresenterSettings.nrAutoZone);
            int payloadHeight = (int)(availableArea.Height * PresenterSettings.payloadZone);
            int nextTrucksHeight = (int)(availableArea.Height * PresenterSettings.nextTrucksZone);



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
            lblPayload.BackColor = Color.YellowGreen;

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
            if (parent.EntryData.Count > 0)
            {
                lblNrAuto.Text = parent.EntryData[0].nrAuto;
                lblPayload.Text = parent.EntryData[0].payload;
            } else
            {
                lblNrAuto.Text = "";
                lblPayload.Text = "";
            }
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
            lvsi.Text = entry.dateRegistered.ToString("HH:mm  dd.MM.yyyy");
            lvi.SubItems.Add(lvsi);


            lstTruckOrder.Items.Add(lvi);
        }

        private void label1_Click(object sender, EventArgs e)
        {
            this.windowState = FormWindowState.Normal;
            WindowMaximize();
        }

        private void label1_MouseEnter(object sender, EventArgs e)
        {
            label1.BackColor = Color.Blue;
            label1.ForeColor = Color.Red;
        }

        private void label1_MouseLeave(object sender, EventArgs e)
        {
            label1.BackColor = Color.Red;
            label1.ForeColor = Color.MediumBlue;
        }

        private void lstTruckOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in lstTruckOrder.SelectedItems)
            {
                lvi.Selected = false;
            }
            lstTruckOrder.SelectedItems.Clear();
        }
    }
}
