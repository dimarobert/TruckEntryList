using Microsoft.Office.Interop.Excel;
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
    public partial class MainForm : Form, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private const string dataFile = "./dataFile.dat";
        private StreamWriter logFile;
        private const string completedFile = "./completedFile.dat";

        private Presenter presenter;

        private List<TruckInfo> entryData;
        public List<TruckInfo> EntryData
        {
            get { return entryData; }
            set
            {
                entryData = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("EntryData"));
                }
            }
        }

        private int autoIncrementNrCrt;
        public int AutoIncrementNrCrt
        {
            get { return autoIncrementNrCrt; }
            set
            {
                autoIncrementNrCrt = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("AutoIncrementNrCrt"));
            }
        }

        public MainForm()
        {
            InitializeComponent();

            logFile = new StreamWriter(File.Open("./logFile.txt", FileMode.Append));

            EntryData = new List<TruckInfo>();

            AutoIncrementNrCrt = 1;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            CreateRaport();
            lstTruckOrder.Items.Clear();

            presenter = new Presenter(this);
            presenter.Show();

            LoadData();

            lblNrCrt.DataBindings.Add("Text", this, "AutoIncrementNrCrt");

            Timer regUpdater = new Timer();
            regUpdater.Tick += RegUpdater_Tick;
            regUpdater.Interval = 1000;
            regUpdater.Start();

            Timer raportTimer = new Timer();
            raportTimer.Interval = (int)(new TimeSpan(24, 0, 0) - new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)).TotalMilliseconds;
            raportTimer.Tick += RaportTimer_Tick;
            raportTimer.Start();
        }

        private void RaportTimer_Tick(object sender, EventArgs e)
        {
            ((Timer)sender).Interval = (int)new TimeSpan(24, 0, 0).TotalMilliseconds;

            CreateRaport();
        }

        private void CreateRaport()
        {
            string dataRaport = DateTime.Now.Hour == 0 ? (DateTime.Now.Day - 1).ToString("00") + "" : DateTime.Now.Day.ToString("00") + "";
            dataRaport += "." + DateTime.Now.Month.ToString("00") + "." + DateTime.Now.Year;

            string fileName = Path.GetFullPath(@".\Rapoarte\Raport " + dataRaport + ".xlsx");
            FileInfo file = new FileInfo(fileName);
            file.Directory.Create();

            Microsoft.Office.Interop.Excel.Application excelApp = null;
            _Workbook excelWb = null;
            try
            {
                excelApp = new Microsoft.Office.Interop.Excel.Application();
                excelApp.Visible = false;
                excelApp.UserControl = false;
                excelWb = excelApp.Workbooks.Add("");
                _Worksheet excelWs = excelWb.ActiveSheet;

                excelWs.Range[excelWs.Cells[1, 1], excelWs.Cells[1, 5]].Merge();



                excelWs.Cells[1, 1] = "Raport pentru ziua: " + dataRaport;


                excelWs.Cells[3, 1] = "Nr. Crt.";
                AddBorder(excelWs.Cells[3, 1], XlBorderWeight.xlThick);

                excelWs.Cells[3, 2] = "Nr. Auto";
                AddBorder(excelWs.Cells[3, 2], XlBorderWeight.xlThick);

                excelWs.Cells[3, 3] = "Marfa";
                AddBorder(excelWs.Cells[3, 3], XlBorderWeight.xlThick);

                excelWs.Cells[3, 4] = "Data Inregistrare";
                AddBorder(excelWs.Cells[3, 4], XlBorderWeight.xlThick);

                excelWs.Cells[3, 5] = "Data Intrare";
                AddBorder(excelWs.Cells[3, 5], XlBorderWeight.xlThick);


                string[] lines = File.ReadAllLines(completedFile);
                TruckInfo ti;
                int i = 1;
                foreach(string line in lines)
                {
                    if (TruckInfo.TryParse(line, out ti))
                    {
                        if (!(ti.dateRegistered.Day == DateTime.Now.Day || ti.dateRegistered.Day == DateTime.Now.Day - 1))
                            continue;

                        excelWs.Cells[3 + i, 1] = i;
                        excelWs.Cells[3 + i, 2] = ti.nrAuto;
                        excelWs.Cells[3 + i, 3] = ti.payload;
                        excelWs.Cells[3 + i, 4] = ti.dateRegistered;
                        excelWs.Cells[3 + i, 5] = ti.dateEntry;
                        i++;
                    }
                }

                excelWs.Range[excelWs.Cells[1, 1], excelWs.Cells[1, 5]].EntireColumn.AutoFit();
                excelWb.SaveAs(fileName, XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing,
                                false, false, XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                excelWb.Close();
                excelApp.Quit();

            }
            catch (Exception ex)
            {
                if (excelWb != null)
                    excelWb.Close();
                if (excelApp != null)
                    excelApp.Quit();
            }
        }

        private void AddBorder(Range cell, XlBorderWeight weight)
        {
            Borders border = cell.Borders;
            border.LineStyle = XlLineStyle.xlContinuous;
            border.Weight = weight;
        }

        private void RegUpdater_Tick(object sender, EventArgs e)
        {
            dateRegister.Value = DateTime.Now;
        }

        private void LoadData()
        {
            EntryData.Clear();
            string line;
            int lineCnt = 0;
            bool errorOnReading = false;
            if (!File.Exists(dataFile))
                return;
            using (StreamReader sr = new StreamReader(dataFile))
            {
                while (sr.Peek() > -1)
                {
                    line = sr.ReadLine();
                    lineCnt++;
                    TruckInfo ti;
                    if (TruckInfo.TryParse(line, out ti))
                    {
                        EntryData.Add(ti);
                        AddToList(ti);
                        AutoIncrementNrCrt = ti.nrCrt + 1;
                    }
                    else
                    {
                        PrintError(lineCnt);
                        errorOnReading = true;
                    }
                }
            }
            if (errorOnReading)
            {
                MessageBox.Show("Au aparut erori in citirea datelor.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("EntryData"));
            }
        }

        private void AddEntryToFile(TruckInfo entry)
        {
            using (StreamWriter sw = new StreamWriter(dataFile, true))
            {
                sw.WriteLine(entry.ToString());
            }
        }

        private void AddEntryToCompleted(TruckInfo entry)
        {
            using (StreamWriter sw = new StreamWriter(completedFile, true))
            {
                sw.WriteLine(entry.ToString() + "|" + DateTime.Now);
            }
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
            lvsi.Text = entry.dateRegistered.ToString("HH:MM:ss dd.MM.yyyy");
            lvi.SubItems.Add(lvsi);


            lstTruckOrder.Items.Add(lvi);
        }

        private void PrintError(int lineCount)
        {
            logFile.WriteLine(DateTime.Now.ToString() + ": Error reading data on line " + lineCount);
        }

        private void cmdAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtNrAuto.Text))
            {
                MessageBox.Show("Numarul masinii nu poate fii gol!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (string.IsNullOrEmpty(txtPayload.Text))
            {
                MessageBox.Show("Va rugam alegeti marfa transportata!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            TruckInfo ti = new TruckInfo();

            ti.nrCrt = AutoIncrementNrCrt;
            try
            {
                ti.nrAuto = txtNrAuto.Text;
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show("Numarul masinii nu este in formatul corect!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            ti.payload = txtPayload.Text;
            ti.dateRegistered = dateRegister.Value;

            AddEntryToFile(ti);
            EntryData.Add(ti);
            AddToList(ti);
            AutoIncrementNrCrt++;

            txtNrAuto.Text = "";
            txtNrAuto.Focus();
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("EntryData"));
            }
        }

        private void lstTruckOrder_MouseClick(object sender, MouseEventArgs e)
        {
            if (lstTruckOrder.SelectedItems.Count == 1 && e.Button == MouseButtons.Right)
            {
                mnuTruckList.Show(lstTruckOrder.PointToScreen(e.Location));
            }
        }

        private void cmdRemTruck_Click(object sender, EventArgs e)
        {
            if (lstTruckOrder.SelectedItems.Count == 1)
            {
                string[] lines = File.ReadAllLines(dataFile);
                using (StreamWriter sw = new StreamWriter(dataFile, false))
                {
                    bool lineFound = false;
                    foreach (string line in lines)
                    {
                        if (lineFound)
                        {
                            TruckInfo ti;
                            TruckInfo.TryParse(line, out ti);
                            ti.nrCrt--;

                            EntryData[ti.nrCrt].nrCrt--;
                            lstTruckOrder.Items[ti.nrCrt].SubItems[0].Text = ti.nrCrt.ToString();

                            sw.WriteLine(ti.ToString());
                        }
                        else
                        {
                            if (!line.StartsWith(lstTruckOrder.SelectedItems[0].Text + "|"))
                                sw.WriteLine(line);
                            else lineFound = true;
                        }
                    }
                }
                EntryData.RemoveAt(lstTruckOrder.SelectedIndices[0]);
                lstTruckOrder.Items.Remove(lstTruckOrder.SelectedItems[0]);
                AutoIncrementNrCrt = EntryData.Last().nrCrt + 1;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("EntryData"));
                }
            }
        }

        private void cmdNext_Click(object sender, EventArgs e)
        {
            if (EntryData.Count == 0)
            {
                MessageBox.Show("Nu exista nici o masina in asteptare!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            AddEntryToCompleted(EntryData[0]);
            string[] lines = File.ReadAllLines(dataFile);
            using (StreamWriter sw = new StreamWriter(dataFile, false))
            {
                lines = lines.Skip(1).ToArray();

                foreach (string line in lines)
                {
                    TruckInfo ti;
                    TruckInfo.TryParse(line, out ti);

                    ti.nrCrt--;
                    EntryData[ti.nrCrt].nrCrt--;
                    lstTruckOrder.Items[ti.nrCrt].Text = ti.nrCrt.ToString();

                    sw.WriteLine(ti.ToString());
                }
            }

            EntryData.RemoveAt(0);
            lstTruckOrder.Items.RemoveAt(0);
            if (EntryData.Count == 0)
                AutoIncrementNrCrt = 1;
            else
                AutoIncrementNrCrt = EntryData.Last().nrCrt + 1;

            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("EntryData"));
            }
        }

        private void cmdShowPresenter_Click(object sender, EventArgs e)
        {
            if (presenter != null)
            {
                if (!presenter.Visible)
                {
                    presenter.Dispose();
                    presenter = new Presenter(this);
                    presenter.UpdatePresenterSettings();
                    presenter.Show();
                }
            }
            else
            {
                presenter = new Presenter(this);
                presenter.Show();
            }
        }

        private void cmdSettings_Click(object sender, EventArgs e)
        {
            Settings settings = new Settings();
            settings.ShowDialog();

            if (presenter != null)
                presenter.UpdatePresenterSettings();
        }
    }
}
