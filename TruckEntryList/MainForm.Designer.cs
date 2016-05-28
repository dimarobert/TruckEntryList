namespace TruckEntryList
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "1",
            "CL-999-DDD",
            "Rapita",
            "24:99:99 99.99.9999"}, -1);
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem(new string[] {
            "1",
            "CL-999-DDD",
            "Obs"}, -1);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.txtNrAuto = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblNrCrt = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.dateRegister = new System.Windows.Forms.DateTimePicker();
            this.cmdNext = new System.Windows.Forms.Button();
            this.lstTruckOrder = new System.Windows.Forms.ListView();
            this.nrcrt = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.nrAuto = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.payload = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.dateReg = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.txtPayload = new System.Windows.Forms.ComboBox();
            this.mnuTruckList = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmdRemTruck = new System.Windows.Forms.ToolStripMenuItem();
            this.cmdShowPresenter = new System.Windows.Forms.Button();
            this.cmdSettings = new System.Windows.Forms.Button();
            this.cmdRaport = new System.Windows.Forms.Button();
            this.cmdAdd = new System.Windows.Forms.Button();
            this.cmdSkip = new System.Windows.Forms.Button();
            this.lstSkip = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.mnuSkipList = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmdReturnToList = new System.Windows.Forms.ToolStripMenuItem();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.mnuTruckList.SuspendLayout();
            this.mnuSkipList.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtNrAuto
            // 
            this.txtNrAuto.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtNrAuto.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.txtNrAuto.Location = new System.Drawing.Point(122, 51);
            this.txtNrAuto.Name = "txtNrAuto";
            this.txtNrAuto.Size = new System.Drawing.Size(100, 21);
            this.txtNrAuto.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(12, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 17);
            this.label1.TabIndex = 100;
            this.label1.Text = "Numar Curent:";
            // 
            // lblNrCrt
            // 
            this.lblNrCrt.AutoSize = true;
            this.lblNrCrt.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblNrCrt.Location = new System.Drawing.Point(118, 19);
            this.lblNrCrt.Name = "lblNrCrt";
            this.lblNrCrt.Size = new System.Drawing.Size(38, 22);
            this.lblNrCrt.TabIndex = 100;
            this.lblNrCrt.Text = "NR";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label3.Location = new System.Drawing.Point(48, 54);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 17);
            this.label3.TabIndex = 100;
            this.label3.Text = "Nr. Auto:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label4.Location = new System.Drawing.Point(64, 81);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 17);
            this.label4.TabIndex = 100;
            this.label4.Text = "Marfa:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label5.Location = new System.Drawing.Point(18, 107);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(94, 17);
            this.label5.TabIndex = 100;
            this.label5.Text = "Ora Inscriere:";
            // 
            // dateRegister
            // 
            this.dateRegister.Enabled = false;
            this.dateRegister.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.dateRegister.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dateRegister.Location = new System.Drawing.Point(122, 105);
            this.dateRegister.Name = "dateRegister";
            this.dateRegister.ShowUpDown = true;
            this.dateRegister.Size = new System.Drawing.Size(100, 21);
            this.dateRegister.TabIndex = 3;
            // 
            // cmdNext
            // 
            this.cmdNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.cmdNext.ForeColor = System.Drawing.Color.Blue;
            this.cmdNext.Location = new System.Drawing.Point(152, 172);
            this.cmdNext.Name = "cmdNext";
            this.cmdNext.Size = new System.Drawing.Size(70, 70);
            this.cmdNext.TabIndex = 5;
            this.cmdNext.UseVisualStyleBackColor = true;
            this.cmdNext.Click += new System.EventHandler(this.cmdNext_Click);
            // 
            // lstTruckOrder
            // 
            this.lstTruckOrder.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.nrcrt,
            this.nrAuto,
            this.payload,
            this.dateReg});
            this.lstTruckOrder.FullRowSelect = true;
            this.lstTruckOrder.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
            this.lstTruckOrder.Location = new System.Drawing.Point(228, 12);
            this.lstTruckOrder.MultiSelect = false;
            this.lstTruckOrder.Name = "lstTruckOrder";
            this.lstTruckOrder.Size = new System.Drawing.Size(319, 230);
            this.lstTruckOrder.TabIndex = 6;
            this.lstTruckOrder.UseCompatibleStateImageBehavior = false;
            this.lstTruckOrder.View = System.Windows.Forms.View.Details;
            this.lstTruckOrder.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lstTruckOrder_MouseClick);
            // 
            // nrcrt
            // 
            this.nrcrt.Text = "Nr.Crt.";
            this.nrcrt.Width = 42;
            // 
            // nrAuto
            // 
            this.nrAuto.Text = "Nr. Auto";
            this.nrAuto.Width = 77;
            // 
            // payload
            // 
            this.payload.Text = "Marfa";
            // 
            // dateReg
            // 
            this.dateReg.Text = "Ora Inscriere";
            this.dateReg.Width = 116;
            // 
            // txtPayload
            // 
            this.txtPayload.FormattingEnabled = true;
            this.txtPayload.Items.AddRange(new object[] {
            "Rapita",
            "Floarea Soarelui",
            "Soia",
            "Biodiesel",
            "Srot",
            "Metanol",
            "Glicerina"});
            this.txtPayload.Location = new System.Drawing.Point(122, 78);
            this.txtPayload.Name = "txtPayload";
            this.txtPayload.Size = new System.Drawing.Size(100, 21);
            this.txtPayload.TabIndex = 2;
            // 
            // mnuTruckList
            // 
            this.mnuTruckList.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.mnuTruckList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmdRemTruck});
            this.mnuTruckList.Name = "mnuTruckList";
            this.mnuTruckList.Size = new System.Drawing.Size(108, 26);
            // 
            // cmdRemTruck
            // 
            this.cmdRemTruck.Name = "cmdRemTruck";
            this.cmdRemTruck.Size = new System.Drawing.Size(107, 22);
            this.cmdRemTruck.Text = "Sterge";
            this.cmdRemTruck.Click += new System.EventHandler(this.cmdRemTruck_Click);
            // 
            // cmdShowPresenter
            // 
            this.cmdShowPresenter.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.cmdShowPresenter.ForeColor = System.Drawing.Color.Green;
            this.cmdShowPresenter.Location = new System.Drawing.Point(86, 202);
            this.cmdShowPresenter.Name = "cmdShowPresenter";
            this.cmdShowPresenter.Size = new System.Drawing.Size(40, 40);
            this.cmdShowPresenter.TabIndex = 5;
            this.cmdShowPresenter.UseVisualStyleBackColor = true;
            this.cmdShowPresenter.Click += new System.EventHandler(this.cmdShowPresenter_Click);
            // 
            // cmdSettings
            // 
            this.cmdSettings.ForeColor = System.Drawing.Color.Red;
            this.cmdSettings.Location = new System.Drawing.Point(14, 212);
            this.cmdSettings.Name = "cmdSettings";
            this.cmdSettings.Size = new System.Drawing.Size(30, 30);
            this.cmdSettings.TabIndex = 101;
            this.cmdSettings.UseVisualStyleBackColor = true;
            this.cmdSettings.Click += new System.EventHandler(this.cmdSettings_Click);
            // 
            // cmdRaport
            // 
            this.cmdRaport.Location = new System.Drawing.Point(50, 212);
            this.cmdRaport.Name = "cmdRaport";
            this.cmdRaport.Size = new System.Drawing.Size(30, 30);
            this.cmdRaport.TabIndex = 102;
            this.cmdRaport.UseVisualStyleBackColor = true;
            this.cmdRaport.Click += new System.EventHandler(this.cmdRaport_Click);
            // 
            // cmdAdd
            // 
            this.cmdAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.cmdAdd.Location = new System.Drawing.Point(187, 132);
            this.cmdAdd.Name = "cmdAdd";
            this.cmdAdd.Size = new System.Drawing.Size(35, 35);
            this.cmdAdd.TabIndex = 4;
            this.cmdAdd.UseVisualStyleBackColor = true;
            this.cmdAdd.Click += new System.EventHandler(this.cmdAdd_Click);
            // 
            // cmdSkip
            // 
            this.cmdSkip.Location = new System.Drawing.Point(86, 156);
            this.cmdSkip.Name = "cmdSkip";
            this.cmdSkip.Size = new System.Drawing.Size(40, 40);
            this.cmdSkip.TabIndex = 103;
            this.cmdSkip.UseVisualStyleBackColor = true;
            this.cmdSkip.Click += new System.EventHandler(this.cmdSkip_Click);
            // 
            // lstSkip
            // 
            this.lstSkip.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.lstSkip.FullRowSelect = true;
            this.lstSkip.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem2});
            this.lstSkip.Location = new System.Drawing.Point(553, 12);
            this.lstSkip.MultiSelect = false;
            this.lstSkip.Name = "lstSkip";
            this.lstSkip.Size = new System.Drawing.Size(208, 230);
            this.lstSkip.TabIndex = 104;
            this.lstSkip.UseCompatibleStateImageBehavior = false;
            this.lstSkip.View = System.Windows.Forms.View.Details;
            this.lstSkip.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lstSkip_MouseClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Nr.Crt.";
            this.columnHeader1.Width = 42;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Nr. Auto";
            this.columnHeader2.Width = 77;
            // 
            // mnuSkipList
            // 
            this.mnuSkipList.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.mnuSkipList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmdReturnToList});
            this.mnuSkipList.Name = "mnuTruckList";
            this.mnuSkipList.Size = new System.Drawing.Size(171, 26);
            // 
            // cmdReturnToList
            // 
            this.cmdReturnToList.Name = "cmdReturnToList";
            this.cmdReturnToList.Size = new System.Drawing.Size(170, 22);
            this.cmdReturnToList.Text = "Adauga la inceput";
            this.cmdReturnToList.Click += new System.EventHandler(this.cmdReturnToList_Click);
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Observatii";
            this.columnHeader3.Width = 80;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(769, 254);
            this.Controls.Add(this.lstSkip);
            this.Controls.Add(this.cmdSkip);
            this.Controls.Add(this.cmdRaport);
            this.Controls.Add(this.cmdSettings);
            this.Controls.Add(this.txtPayload);
            this.Controls.Add(this.lstTruckOrder);
            this.Controls.Add(this.dateRegister);
            this.Controls.Add(this.lblNrCrt);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtNrAuto);
            this.Controls.Add(this.cmdShowPresenter);
            this.Controls.Add(this.cmdNext);
            this.Controls.Add(this.cmdAdd);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Truck Entry List";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.mnuTruckList.ResumeLayout(false);
            this.mnuSkipList.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdAdd;
        private System.Windows.Forms.TextBox txtNrAuto;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblNrCrt;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dateRegister;
        private System.Windows.Forms.Button cmdNext;
        private System.Windows.Forms.ListView lstTruckOrder;
        private System.Windows.Forms.ColumnHeader nrcrt;
        private System.Windows.Forms.ColumnHeader nrAuto;
        private System.Windows.Forms.ColumnHeader payload;
        private System.Windows.Forms.ColumnHeader dateReg;
        private System.Windows.Forms.ComboBox txtPayload;
        private System.Windows.Forms.ContextMenuStrip mnuTruckList;
        private System.Windows.Forms.ToolStripMenuItem cmdRemTruck;
        private System.Windows.Forms.Button cmdShowPresenter;
        private System.Windows.Forms.Button cmdSettings;
        private System.Windows.Forms.Button cmdRaport;
        private System.Windows.Forms.Button cmdSkip;
        private System.Windows.Forms.ListView lstSkip;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ToolStripMenuItem cmdReturnToList;
        private System.Windows.Forms.ContextMenuStrip mnuSkipList;
        private System.Windows.Forms.ColumnHeader columnHeader3;
    }
}

