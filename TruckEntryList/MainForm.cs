using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
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


            TruckInfo ti = new TruckInfo();
            ti.nrCrt = 10;
            ti.nrAuto = "A-01-ACA";
            ti.payload = "ss";
            ti.dateRegistered = DateTime.Now.AddHours(-5);
            ti.dateEntry = DateTime.Now;

            Stream fs = File.Open("./serializationTest.dat", FileMode.Create);
            ti.WriteObject(fs);
            fs.Seek(0, SeekOrigin.Begin);
            TruckInfo ti2 = new TruckInfo(fs);
            fs.Close();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

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
            DateTime tick = DateTime.Parse(DateTime.Now.ToString("00:00:00 dd.MM.yyyy"));
            tick = tick.AddDays(1);
            raportTimer.Interval = (int)(tick - DateTime.Now).TotalMilliseconds;
            raportTimer.Tick += RaportTimer_Tick;
            raportTimer.Start();
        }

        private void RaportTimer_Tick(object sender, EventArgs e)
        {
            DateTime tick = DateTime.Parse(DateTime.Now.ToString("00:00:00 dd.MM.yyyy"));
            tick.AddDays(1);
            ((Timer)sender).Interval = (int)(tick - DateTime.Now).TotalMilliseconds;

            string file = Path.GetFullPath(PresenterSettings.raportFolder);
            if (!file.EndsWith(Path.DirectorySeparatorChar.ToString())) file += Path.DirectorySeparatorChar.ToString();
            file += "Raport " + DateTime.Now.AddDays(-1).ToString("yyyy.MM.dd") + ".xlsx";

            CreateRaport(DateTime.Now.AddDays(-1), file);
        }

        private void CreateRaport(DateTime rDate, string file)
        {

            string dataRaport = rDate.ToString("dd.MM.yyyy");
            /*if (DateTime.Now.Hour >= 23)
            {
                dataRaport = DateTime.Now.Day.ToString("00") + "." + DateTime.Now.Month.ToString("00") + "." + DateTime.Now.Year;
            }
            else
            {
                dataRaport = (DateTime.Now.Day - 1).ToString("00") + "." + DateTime.Now.Month.ToString("00") + "." + DateTime.Now.Year;
            }*/

            //string file = file;
            FileInfo fileInfo = new FileInfo(file);
            fileInfo.Directory.Create();

            Microsoft.Office.Interop.Excel.Application excelApp = null;
            _Workbook excelWb = null;
            try
            {
                excelApp = new Microsoft.Office.Interop.Excel.Application();
                excelApp.DisplayAlerts = false;
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
                foreach (string line in lines)
                {
                    if (TruckInfo.TryParse(line, out ti))
                    {

                        if (ti.dateRegistered.ToString("dd.MM.yyyy") != dataRaport)
                            continue;

                        excelWs.Cells[3 + i, 1] = i;
                        excelWs.Cells[3 + i, 2] = ti.nrAuto;
                        excelWs.Cells[3 + i, 3] = ti.payload;
                        excelWs.Cells[3 + i, 4] = ti.dateRegistered;
                        excelWs.Cells[3 + i, 5] = ti.dateEntry;
                        i++;
                    }
                }

                if (i != 1)
                    AddBorder(excelWs.get_Range("A4", "E" + (i + 2)), XlBorderWeight.xlThin, true, true);
                else
                {
                    // Nu avem nimic de raportat. ABORT
                    MessageBox.Show("Raportul pe data " + dataRaport + " este gol. Raportul nu a fost salvat.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    excelWb.Close(false, Type.Missing, Type.Missing);
                    excelApp.Quit();
                    return;
                }

                Range r = excelWs.Range[excelWs.Cells[1, 1], excelWs.Cells[1, 5]];
                r.EntireColumn.AutoFit();
                r.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                foreach (Range rr in r.Columns)
                {
                    rr.ColumnWidth = rr.ColumnWidth + 2;
                }

                r = excelWs.get_Range("A3", "E" + (i + 2));
                r.HorizontalAlignment = XlHAlign.xlHAlignCenter;



                excelWb.SaveAs(file, XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing,
                                false, false, XlSaveAsAccessMode.xlNoChange, XlSaveConflictResolution.xlLocalSessionChanges, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                excelWb.Close(false, Type.Missing, Type.Missing);
                excelApp.Quit();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Salvarea raportului a esuat!\nMessage: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                if (excelWb != null)
                    excelWb.Close(false, Type.Missing, Type.Missing);
                if (excelApp != null)
                    excelApp.Quit();
            }
        }

        private void AddBorder(Range cell, XlBorderWeight weight, bool allBorders = false, bool notTop = false)
        {

            Borders border = cell.Borders;
            if (!allBorders)
            {
                border.LineStyle = XlLineStyle.xlContinuous;
                border.Weight = weight;
            }
            else
            {
                if (!notTop)
                {
                    border[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
                    border[XlBordersIndex.xlEdgeTop].Weight = weight;
                }
                border[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
                border[XlBordersIndex.xlEdgeLeft].Weight = weight;
                border[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;
                border[XlBordersIndex.xlEdgeRight].Weight = weight;
                border[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
                border[XlBordersIndex.xlEdgeBottom].Weight = weight;

                border[XlBordersIndex.xlInsideHorizontal].LineStyle = XlLineStyle.xlContinuous;
                border[XlBordersIndex.xlInsideHorizontal].Weight = weight;
                border[XlBordersIndex.xlInsideVertical].LineStyle = XlLineStyle.xlContinuous;
                border[XlBordersIndex.xlInsideVertical].Weight = weight;

            }
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
            if (!File.Exists(completedFile))
                File.Create(completedFile).Close();
            int pos = File.ReadAllLines(completedFile).Length + 1;
            entry.nrCrt = pos;
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

        private void cmdRaport_Click(object sender, EventArgs e)
        {
            RaportSelect rs = new RaportSelect();
            if (rs.ShowDialog() == DialogResult.OK)
            {
                string file = Path.GetFullPath(PresenterSettings.raportFolder);
                if (!file.EndsWith(Path.DirectorySeparatorChar.ToString())) file += Path.DirectorySeparatorChar.ToString();
                file += "Raport " + rs.raportDate.Value.ToString("yyyy.MM.dd") + ".xlsx";
                if (File.Exists(file))
                {
                    OpenFileDialog ofd = new OpenFileDialog();
                    ofd.InitialDirectory = file.Substring(0, file.IndexOf(Path.GetFileName(file)));
                    ofd.Multiselect = false;
                    ofd.FileName = file.Substring(file.IndexOf(Path.GetFileName(file)));
                    ofd.Filter = "Microsoft Excel 2010 File|*.xlsx";
                    if (ofd.ShowDialog() == DialogResult.OK)
                        file = ofd.FileName;
                }
                CreateRaport(rs.raportDate.Value, file);
            }
            rs.Dispose();
            // TODO: Check last raport made and go from there.
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Sunteti sigur ca vreti sa parasiti aplicatia?", "Confirmare", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                e.Cancel = true;
        }
    }
}
