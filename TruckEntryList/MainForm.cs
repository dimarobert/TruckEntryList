using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace TruckEntryList {
    public partial class MainForm : Form, INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        private const string dataFile = "./dataFile.dat";
        private const string skipFile = "./skipFile.dat";
        private StreamWriter logFile;
        private const string completedFile = "./completedFile.dat";

        private enum SavingStates {
            Saving,
            SavingRed,
            Saved,
            SavedGreen
        }

        private SavingStates SavingState;
        private Presenter presenter;

        private List<TruckInfo> entryData;
        public List<TruckInfo> EntryData {
            get { return entryData; }
            set {
                entryData = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("EntryData"));
            }
        }


        private List<TruckInfo> skipData;
        public List<TruckInfo> SkipData {
            get { return skipData; }
            set {
                skipData = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SkipData"));
            }
        }

        private int autoIncrementNrCrt;
        public int AutoIncrementNrCrt {
            get { return autoIncrementNrCrt; }
            set {
                autoIncrementNrCrt = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("AutoIncrementNrCrt"));
            }
        }

        public MainForm() {
            InitializeComponent();

            logFile = new StreamWriter(File.Open("./logFile.txt", FileMode.Append));

            EntryData = new List<TruckInfo>();
            SkipData = new List<TruckInfo>();

            AutoIncrementNrCrt = 1;

        }

        private void MainForm_Load(object sender, EventArgs e) {

            lstTruckOrder.Items.Clear();
            lstSkip.Items.Clear();

            presenter = new Presenter(this);
            presenter.Show();

            LoadImages();

            LoadData();

            lblNrCrt.DataBindings.Add("Text", this, "AutoIncrementNrCrt");

            Timer regUpdater = new Timer();
            regUpdater.Tick += RegUpdater_Tick;
            regUpdater.Interval = 1000;
            regUpdater.Start();

            Timer raportTimer = new Timer();
            DateTime tick = DateTime.Now;
            tick = tick.AddHours(-tick.Hour).AddMinutes(-tick.Minute).AddSeconds(-tick.Second);
            tick = tick.AddDays(1);
            raportTimer.Interval = (int)(tick - DateTime.Now).TotalMilliseconds;
            raportTimer.Tick += RaportTimer_Tick;
            raportTimer.Start();
        }

        private void LoadImages() {
            cmdNext.Image = LoadSvgImage("./imgs/next.svg", cmdNext.Width);
            cmdAdd.Image = LoadSvgImage("./imgs/add.svg", cmdAdd.Width);
            cmdShowPresenter.Image = LoadSvgImage("./imgs/presenter.svg", cmdShowPresenter.Width);
            cmdRaport.Image = LoadSvgImage("./imgs/raport.svg", cmdRaport.Width);
            cmdSettings.Image = LoadSvgImage("./imgs/settings.svg", cmdSettings.Width);
            cmdSkip.Image = LoadSvgImage("./imgs/skip.svg", cmdSkip.Width);
        }

        /// <summary>
        /// If height is not specified it will get the width value.
        /// </summary>
        private Bitmap LoadSvgImage(string imgFile, int width, int height = -1) {
            if (!File.Exists(imgFile))
                return null;

            if (width < 0)
                throw new ArgumentOutOfRangeException("width must be positive!");

            if (height == -1)
                height = width;

            Bitmap img;

            var svgDoc = Svg.SvgDocument.Open(imgFile);
            svgDoc.Width = width;
            svgDoc.Height = height;
            img = svgDoc.Draw();
            return img;
        }

        private void RaportTimer_Tick(object sender, EventArgs e) {
            DateTime tick = DateTime.Parse(DateTime.Now.ToString("00:00:00 dd.MM.yyyy"));
            tick.AddDays(1);
            ((Timer)sender).Interval = (int)(tick - DateTime.Now).TotalMilliseconds;

            string file = Path.GetFullPath(PresenterSettings.raportFolder);
            if (!file.EndsWith(Path.DirectorySeparatorChar.ToString())) file += Path.DirectorySeparatorChar.ToString();
            file += "Raport " + DateTime.Now.AddDays(-1).ToString("yyyy.MM.dd") + ".xlsx";

            CreateRaport(DateTime.Now.AddDays(-1), file);
        }

        private void CreateRaport(DateTime rDate, string file) {
            if (!File.Exists(completedFile)) {
                MessageBox.Show("Fisierul in care se stocheaza istoricul intrarilor nu a fost gasit.\nDaca este prima folosire a aplicatiei si nu s-a folosit niciodata functia Urmatorul, totul este in regula.\nIn caz contrar anuntati administratorul.", "Nu s-a putut crea raportul", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            string dataRaport = rDate.ToString("dd.MM.yyyy");

            FileInfo fileInfo = new FileInfo(file);
            fileInfo.Directory.Create();

            Microsoft.Office.Interop.Excel.Application excelApp = null;
            _Workbook excelWb = null;
            try {
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

                excelWs.Cells[3, 6] = "Comentarii";
                AddBorder(excelWs.Cells[3, 6], XlBorderWeight.xlThick);

                int i = 1;

                using (FixedObjectFileStream stream = new FixedObjectFileStream(completedFile, FileMode.Open, FileAccess.ReadWrite)) {
                    for (int j = 0; j < stream.Length; j++) {
                        TruckInfo ti = stream[j];

                        if (ti.dateRegistered.ToString("dd.MM.yyyy") != dataRaport)
                            continue;

                        excelWs.Cells[3 + i, 1] = i;
                        excelWs.Cells[3 + i, 2] = ti.nrAuto;
                        excelWs.Cells[3 + i, 3] = ti.payload;
                        excelWs.Cells[3 + i, 4] = ti.dateRegistered;
                        excelWs.Cells[3 + i, 5] = ti.dateEntry;
                        excelWs.Cells[3 + i, 6] = ti.comments;
                        i++;
                    }
                }

                if (i != 1)
                    AddBorder(excelWs.get_Range("A4", "E" + (i + 2)), XlBorderWeight.xlThin, true, true);
                else {
                    // Nu avem nimic de raportat. ABORT
                    MessageBox.Show("Raportul pe data " + dataRaport + " este gol. Raportul nu a fost salvat.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    excelWb.Close(false, Type.Missing, Type.Missing);
                    excelApp.Quit();
                    return;
                }

                Range r = excelWs.Range[excelWs.Cells[1, 1], excelWs.Cells[1, 6]];
                r.EntireColumn.AutoFit();
                r.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                foreach (Range rr in r.Columns) {
                    rr.ColumnWidth = rr.ColumnWidth + 2;
                }

                r = excelWs.get_Range("A3", "E" + (i + 2));
                r.HorizontalAlignment = XlHAlign.xlHAlignCenter;



                excelWb.SaveAs(file, XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing,
                                false, false, XlSaveAsAccessMode.xlNoChange, XlSaveConflictResolution.xlLocalSessionChanges, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                excelWb.Close(false, Type.Missing, Type.Missing);
                excelApp.Quit();

            } catch (Exception ex) {
                MessageBox.Show("Salvarea raportului a esuat!\nMessage: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                if (excelWb != null)
                    excelWb.Close(false, Type.Missing, Type.Missing);
                if (excelApp != null)
                    excelApp.Quit();
            }
        }

        private void AddBorder(Range cell, XlBorderWeight weight, bool allBorders = false, bool notTop = false) {

            Borders border = cell.Borders;
            if (!allBorders) {
                border.LineStyle = XlLineStyle.xlContinuous;
                border.Weight = weight;
            } else {
                if (!notTop) {
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

        private void RegUpdater_Tick(object sender, EventArgs e) {
            dateRegister.Value = DateTime.Now;
        }

        private void LoadData() {
            EntryData.Clear();
            SkipData.Clear();

            LoadList(EntryData, dataFile, (ti) => {
                AddToList(ti);
                AutoIncrementNrCrt = ti.nrCrt + 1;
            });
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("EntryData"));

            LoadList(SkipData, skipFile, (ti) => {
                AddToSkipList(ti);
            });
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SkipData"));
        }

        private void LoadList(IList<TruckInfo> list, string filePath, Action<TruckInfo> callback) {
            if (File.Exists(filePath)) {
                using (FixedObjectFileStream stream = new FixedObjectFileStream(filePath, FileMode.Open, FileAccess.ReadWrite)) {
                    for (int i = 0; i < stream.Length; i++) {
                        TruckInfo ti = stream[i];
                        list.Add(ti);
                        callback?.Invoke(ti);
                    }
                }
            }
        }

        private void AddEntryToFile(TruckInfo entry, bool first = false) {
            using (FixedObjectFileStream fos = new FixedObjectFileStream(dataFile, FileMode.OpenOrCreate, FileAccess.ReadWrite, true)) {
                if (first) {
                    fos.Insert(0, entry);
                    for (int i = 1; i < fos.NumberOfObjects; i++) {
                        var e = fos[i];
                        e.nrCrt++;
                        fos[i] = e;
                    }
                } else
                    fos.Add(entry);
            }
        }

        private void AddSkipToFile(TruckInfo entry) {
            using (FixedObjectFileStream fos = new FixedObjectFileStream(skipFile, FileMode.OpenOrCreate, FileAccess.ReadWrite, true)) {
                entry.nrCrt = fos.NumberOfObjects + 1;
                fos.Add(entry);
            }
        }

        private void AddEntryToCompleted(TruckInfo entry) {

            if (!File.Exists(completedFile)) {
                Stream s = File.Create(completedFile);
                s.Write(BitConverter.GetBytes(0), 0, 4);
                s.Close();
            }

            using (FixedObjectFileStream stream = new FixedObjectFileStream(completedFile, FileMode.OpenOrCreate, FileAccess.ReadWrite, true)) {
                entry.nrCrt = stream.NumberOfObjects + 1;
                stream.Add(entry);
            }
        }

        private void AddToList(TruckInfo entry) {
            ListViewItem lvi = new ListViewItem();
            lvi.Text = entry.nrCrt.ToString();

            ListViewItem.ListViewSubItem lvsi = new ListViewItem.ListViewSubItem();
            lvsi.Text = entry.nrAuto;
            lvi.SubItems.Add(lvsi);

            lvsi = new ListViewItem.ListViewSubItem();
            lvsi.Text = entry.payload;
            lvi.SubItems.Add(lvsi);

            lvsi = new ListViewItem.ListViewSubItem();
            lvsi.Text = entry.dateRegistered.ToString("HH:mm:ss dd.MM.yyyy");
            lvi.SubItems.Add(lvsi);


            lstTruckOrder.Items.Add(lvi);
        }

        private void AddToSkipList(TruckInfo entry) {
            ListViewItem lvi = new ListViewItem();
            lvi.Text = entry.nrCrt.ToString();

            ListViewItem.ListViewSubItem lvsi = new ListViewItem.ListViewSubItem();
            lvsi.Text = entry.nrAuto;
            lvi.SubItems.Add(lvsi);

            lvsi = new ListViewItem.ListViewSubItem();
            lvsi.Text = entry.comments;
            lvi.SubItems.Add(lvsi);

            lstSkip.Items.Add(lvi);
        }

        private void RestoreToList(TruckInfo entry) {
            ListViewItem lvi = new ListViewItem();
            lvi.Text = 1.ToString();

            ListViewItem.ListViewSubItem lvsi = new ListViewItem.ListViewSubItem();
            lvsi.Text = entry.nrAuto;
            lvi.SubItems.Add(lvsi);

            lvsi = new ListViewItem.ListViewSubItem();
            lvsi.Text = entry.payload;
            lvi.SubItems.Add(lvsi);

            lvsi = new ListViewItem.ListViewSubItem();
            lvsi.Text = entry.dateRegistered.ToString("HH:mm:ss dd.MM.yyyy");
            lvi.SubItems.Add(lvsi);

            lstTruckOrder.Items.Insert(0, lvi);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("EntryData"));
        }

        private void PrintError(int lineCount) {
            logFile.WriteLine(DateTime.Now.ToString() + ": Error reading data on line " + lineCount);
        }

        private void cmdAdd_Click(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(txtNrAuto.Text)) {
                MessageBox.Show("Numarul masinii nu poate fii gol!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (string.IsNullOrEmpty(txtPayload.Text)) {
                MessageBox.Show("Va rugam alegeti marfa transportata!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            TruckInfo ti = new TruckInfo();

            ti.nrCrt = AutoIncrementNrCrt;
            try {
                ti.nrAuto = txtNrAuto.Text;
            } catch (ArgumentException ex) {
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
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs("EntryData"));
            }
        }

        private void lstTruckOrder_MouseClick(object sender, MouseEventArgs e) {
            if (lstTruckOrder.SelectedItems.Count == 1 && e.Button == MouseButtons.Right) {
                mnuTruckList.Show(lstTruckOrder.PointToScreen(e.Location));
            }
        }

        private void lstSkip_MouseClick(object sender, MouseEventArgs e) {
            if (lstSkip.SelectedItems.Count == 1 && e.Button == MouseButtons.Right) {
                mnuSkipList.Show(lstSkip.PointToScreen(e.Location));
            }
        }

        private void cmdRemTruck_Click(object sender, EventArgs e) {
            if (lstTruckOrder.SelectedItems.Count == 1) {
                using (FixedObjectFileStream stream = new FixedObjectFileStream(dataFile, FileMode.Open, FileAccess.ReadWrite)) {
                    stream.RemoveAt(lstTruckOrder.SelectedIndices[0]);

                    if (EntryData != null)
                        foreach (TruckInfo ti in EntryData.Skip(lstTruckOrder.SelectedIndices[0] + 1))
                            lstTruckOrder.Items[--ti.nrCrt].SubItems[0].Text = ti.nrCrt.ToString();
                }

                if (EntryData != null) {
                    EntryData.RemoveAt(lstTruckOrder.SelectedIndices[0]);
                    AutoIncrementNrCrt = EntryData.Count == 0 ? 1 : EntryData.Last().nrCrt + 1;
                }
                if (lstTruckOrder != null)
                    lstTruckOrder.Items.Remove(lstTruckOrder.SelectedItems[0]);

                if (PropertyChanged != null) {
                    PropertyChanged(this, new PropertyChangedEventArgs("EntryData"));
                }
            }
        }

        private void cmdNext_Click(object sender, EventArgs e) {
            if (EntryData.Count == 0) {
                MessageBox.Show("Nu exista nici o masina in asteptare!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            EntryData[0].dateEntry = DateTime.Now;
            AddEntryToCompleted(EntryData[0]);

            using (FixedObjectFileStream stream = new FixedObjectFileStream(dataFile, FileMode.Open, FileAccess.ReadWrite)) {
                stream.RemoveAt(0);
            }

            if (EntryData != null) {
                EntryData.RemoveAt(0);
                AutoIncrementNrCrt = EntryData.Count == 0 ? 1 : EntryData.Last().nrCrt + 1;
                lstTruckOrder.Items.RemoveAt(0);

                foreach (TruckInfo ti in EntryData)
                    lstTruckOrder.Items[--ti.nrCrt - 1].SubItems[0].Text = ti.nrCrt.ToString();
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("EntryData"));
        }

        private void cmdSkip_Click(object sender, EventArgs e) {
            if (EntryData.Count == 0) {
                MessageBox.Show("Nu exista nici o masina in asteptare!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            var entry = EntryData[0];
            CommentForm cForm = new CommentForm();
            cForm.Comment = entry.comments;
            cForm.ShowDialog();
            if (cForm.DialogResult == DialogResult.OK) {
                entry.comments = cForm.Comment;
            }

            AddSkipToFile(entry);
            AddToSkipList(entry);
            SkipData.Add(entry);

            using (FixedObjectFileStream stream = new FixedObjectFileStream(dataFile, FileMode.Open, FileAccess.ReadWrite)) {
                stream.RemoveAt(0);
            }

            if (EntryData != null) {
                EntryData.RemoveAt(0);
                AutoIncrementNrCrt = EntryData.Count == 0 ? 1 : EntryData.Last().nrCrt + 1;
                lstTruckOrder.Items.RemoveAt(0);

                foreach (TruckInfo ti in EntryData)
                    lstTruckOrder.Items[--ti.nrCrt - 1].SubItems[0].Text = ti.nrCrt.ToString();
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("EntryData"));
        }

        private void cmdShowPresenter_Click(object sender, EventArgs e) {
            if (presenter != null) {
                if (!presenter.Visible) {
                    presenter.Dispose();
                    presenter = new Presenter(this);
                    presenter.UpdatePresenterSettings();
                    presenter.Show();
                }
            } else {
                presenter = new Presenter(this);
                presenter.Show();
            }
        }

        private void cmdSettings_Click(object sender, EventArgs e) {
            Settings settings = new Settings();
            settings.ShowDialog();

            if (presenter != null)
                presenter.UpdatePresenterSettings();
        }

        private void cmdRaport_Click(object sender, EventArgs e) {
            RaportSelect rs = new RaportSelect();
            if (rs.ShowDialog() == DialogResult.OK) {
                SavingState = SavingStates.Saving;

                string file = Path.GetFullPath(PresenterSettings.raportFolder);
                if (!file.EndsWith(Path.DirectorySeparatorChar.ToString())) file += Path.DirectorySeparatorChar.ToString();
                file += "Raport " + rs.raportDate.Value.ToString("yyyy.MM.dd") + ".xlsx";
                if (File.Exists(file)) {
                    OpenFileDialog ofd = new OpenFileDialog();
                    ofd.InitialDirectory = file.Substring(0, file.IndexOf(Path.GetFileName(file)));
                    ofd.Multiselect = false;
                    ofd.FileName = file.Substring(file.IndexOf(Path.GetFileName(file)));
                    ofd.Filter = "Microsoft Excel 2010 File|*.xlsx";
                    if (ofd.ShowDialog() == DialogResult.OK)
                        file = ofd.FileName;
                }

                BackgroundWorker bw = new BackgroundWorker();
                bw.DoWork += Bw_DoWork;
                bw.RunWorkerAsync(400);

                CreateRaport(rs.raportDate.Value, file);
                SavingState = SavingStates.Saved;
            }
            rs.Dispose();
        }

        private Bitmap GetRaportImage(Color color) {
            Bitmap img;
            var svgDoc = Svg.SvgDocument.Open("./imgs/raport.svg");
            svgDoc.Children[0].Fill = new Svg.SvgColourServer(color);
            svgDoc.Width = svgDoc.Height = cmdRaport.Width;
            img = svgDoc.Draw();

            return img;
        }

        private void Bw_DoWork(object sender, DoWorkEventArgs e) {
            int sleepTime;
            int? nst = e.Argument as int?;
            if (nst == null)
                sleepTime = 200;
            else sleepTime = nst.Value;
            while (true) {
                if (SavingState == SavingStates.Saving) {
                    cmdRaport.Image = GetRaportImage(Color.Red);
                    SavingState = SavingStates.SavingRed;
                    System.Threading.Thread.Sleep(sleepTime);
                } else if (SavingState == SavingStates.SavingRed) {
                    cmdRaport.Image = GetRaportImage(Color.Black);
                    SavingState = SavingStates.Saving;
                    System.Threading.Thread.Sleep(sleepTime);
                } else if (SavingState == SavingStates.Saved) {
                    cmdRaport.Image = GetRaportImage(Color.Green);
                    SavingState = SavingStates.SavedGreen;
                    System.Threading.Thread.Sleep(sleepTime + 300);
                } else if (SavingState == SavingStates.SavedGreen) {
                    cmdRaport.Image = GetRaportImage(Color.Black);
                    break;
                }
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
            if (MessageBox.Show("Sunteti sigur ca vreti sa parasiti aplicatia?", "Confirmare", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                e.Cancel = true;
        }

        private void cmdReturnToList_Click(object sender, EventArgs e) {
            if (lstSkip.SelectedItems.Count == 1) {
                var entry = SkipData[lstSkip.SelectedIndices[0]];
                entry.nrCrt = 1;

                RestoreToList(entry);
                AddEntryToFile(entry, true);
                EntryData.Insert(0, entry);
                EntryData[0].nrCrt = 0;

                foreach (TruckInfo ti in EntryData)
                    lstTruckOrder.Items[ti.nrCrt].SubItems[0].Text = (++ti.nrCrt).ToString();

                using (FixedObjectFileStream stream = new FixedObjectFileStream(skipFile, FileMode.Open, FileAccess.ReadWrite)) {
                    stream.RemoveAt(lstSkip.SelectedIndices[0]);

                    if (SkipData != null)
                        foreach (TruckInfo ti in SkipData.Skip(lstSkip.SelectedIndices[0] + 1))
                            lstSkip.Items[--ti.nrCrt].SubItems[0].Text = ti.nrCrt.ToString();
                }

                if (SkipData != null) {
                    SkipData.RemoveAt(lstSkip.SelectedIndices[0]);
                }
                if (lstSkip != null)
                    lstSkip.Items.Remove(lstSkip.SelectedItems[0]);

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("EntryData"));
            }
        }
    }
}
