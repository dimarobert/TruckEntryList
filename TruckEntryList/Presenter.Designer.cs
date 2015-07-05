namespace TruckEntryList
{
    partial class Presenter
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
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "999",
            "CL-999-DDD",
            "Rapita",
            "24:99:99 99.99.9999"}, -1);
            this.lstTruckOrder = new System.Windows.Forms.ListView();
            this.nrcrt = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.nrAuto = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.payload = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.dateReg = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblNrAuto = new System.Windows.Forms.Label();
            this.lblPayload = new System.Windows.Forms.Label();
            this.lblHour = new System.Windows.Forms.Label();
            this.lblNextTruck = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lstTruckOrder
            // 
            this.lstTruckOrder.BackColor = System.Drawing.Color.DimGray;
            this.lstTruckOrder.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.nrcrt,
            this.nrAuto,
            this.payload,
            this.dateReg});
            this.lstTruckOrder.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lstTruckOrder.ForeColor = System.Drawing.Color.White;
            this.lstTruckOrder.FullRowSelect = true;
            this.lstTruckOrder.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
            this.lstTruckOrder.Location = new System.Drawing.Point(876, 8);
            this.lstTruckOrder.MultiSelect = false;
            this.lstTruckOrder.Name = "lstTruckOrder";
            this.lstTruckOrder.Size = new System.Drawing.Size(315, 522);
            this.lstTruckOrder.TabIndex = 7;
            this.lstTruckOrder.UseCompatibleStateImageBehavior = false;
            this.lstTruckOrder.View = System.Windows.Forms.View.Details;
            this.lstTruckOrder.SelectedIndexChanged += new System.EventHandler(this.lstTruckOrder_SelectedIndexChanged);
            // 
            // nrcrt
            // 
            this.nrcrt.Text = "#";
            this.nrcrt.Width = 34;
            // 
            // nrAuto
            // 
            this.nrAuto.Text = "Nr. Auto";
            this.nrAuto.Width = 89;
            // 
            // payload
            // 
            this.payload.Text = "Marfa";
            this.payload.Width = 69;
            // 
            // dateReg
            // 
            this.dateReg.Text = "Ora Inscriere";
            this.dateReg.Width = 118;
            // 
            // lblNrAuto
            // 
            this.lblNrAuto.ForeColor = System.Drawing.Color.White;
            this.lblNrAuto.Location = new System.Drawing.Point(278, 172);
            this.lblNrAuto.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblNrAuto.Name = "lblNrAuto";
            this.lblNrAuto.Size = new System.Drawing.Size(75, 19);
            this.lblNrAuto.TabIndex = 8;
            this.lblNrAuto.Text = "A-99-AAA";
            this.lblNrAuto.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPayload
            // 
            this.lblPayload.ForeColor = System.Drawing.Color.White;
            this.lblPayload.Location = new System.Drawing.Point(172, 267);
            this.lblPayload.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPayload.Name = "lblPayload";
            this.lblPayload.Size = new System.Drawing.Size(75, 19);
            this.lblPayload.TabIndex = 9;
            this.lblPayload.Text = "label2";
            this.lblPayload.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblHour
            // 
            this.lblHour.ForeColor = System.Drawing.Color.White;
            this.lblHour.Location = new System.Drawing.Point(285, 20);
            this.lblHour.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblHour.Name = "lblHour";
            this.lblHour.Size = new System.Drawing.Size(75, 19);
            this.lblHour.TabIndex = 10;
            this.lblHour.Text = "label1";
            this.lblHour.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblNextTruck
            // 
            this.lblNextTruck.ForeColor = System.Drawing.Color.White;
            this.lblNextTruck.Location = new System.Drawing.Point(110, 499);
            this.lblNextTruck.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblNextTruck.Name = "lblNextTruck";
            this.lblNextTruck.Size = new System.Drawing.Size(75, 19);
            this.lblNextTruck.TabIndex = 11;
            this.lblNextTruck.Text = "label1";
            this.lblNextTruck.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Red;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(8, 8);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(22, 24);
            this.label1.TabIndex = 12;
            this.label1.Text = "X";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.Click += new System.EventHandler(this.label1_Click);
            this.label1.MouseEnter += new System.EventHandler(this.label1_MouseEnter);
            this.label1.MouseLeave += new System.EventHandler(this.label1_MouseLeave);
            // 
            // Presenter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1203, 637);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lstTruckOrder);
            this.Controls.Add(this.lblNextTruck);
            this.Controls.Add(this.lblHour);
            this.Controls.Add(this.lblPayload);
            this.Controls.Add(this.lblNrAuto);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Presenter";
            this.Text = "Presenter";
            this.Load += new System.EventHandler(this.Presenter_Load);
            this.Resize += new System.EventHandler(this.Presenter_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lstTruckOrder;
        private System.Windows.Forms.ColumnHeader nrcrt;
        private System.Windows.Forms.ColumnHeader nrAuto;
        private System.Windows.Forms.ColumnHeader payload;
        private System.Windows.Forms.ColumnHeader dateReg;
        private System.Windows.Forms.Label lblNrAuto;
        private System.Windows.Forms.Label lblPayload;
        private System.Windows.Forms.Label lblHour;
        private System.Windows.Forms.Label lblNextTruck;
        private System.Windows.Forms.Label label1;
    }
}