namespace TruckEntryList
{
    partial class Settings
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
            this.label1 = new System.Windows.Forms.Label();
            this.numHour = new System.Windows.Forms.NumericUpDown();
            this.numNrAuto = new System.Windows.Forms.NumericUpDown();
            this.numPayload = new System.Windows.Forms.NumericUpDown();
            this.numNextTrucks = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cmdSave = new System.Windows.Forms.Button();
            this.lblValidation = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numHour)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNrAuto)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPayload)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNextTrucks)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(44, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Ora Curenta:";
            // 
            // numHour
            // 
            this.numHour.DecimalPlaces = 2;
            this.numHour.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numHour.Location = new System.Drawing.Point(117, 12);
            this.numHour.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numHour.Name = "numHour";
            this.numHour.Size = new System.Drawing.Size(49, 20);
            this.numHour.TabIndex = 1;
            this.numHour.ValueChanged += new System.EventHandler(this.numHour_ValueChanged);
            // 
            // numNrAuto
            // 
            this.numNrAuto.DecimalPlaces = 2;
            this.numNrAuto.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numNrAuto.Location = new System.Drawing.Point(117, 38);
            this.numNrAuto.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numNrAuto.Name = "numNrAuto";
            this.numNrAuto.Size = new System.Drawing.Size(49, 20);
            this.numNrAuto.TabIndex = 1;
            this.numNrAuto.ValueChanged += new System.EventHandler(this.numHour_ValueChanged);
            // 
            // numPayload
            // 
            this.numPayload.DecimalPlaces = 2;
            this.numPayload.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numPayload.Location = new System.Drawing.Point(117, 64);
            this.numPayload.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numPayload.Name = "numPayload";
            this.numPayload.Size = new System.Drawing.Size(49, 20);
            this.numPayload.TabIndex = 1;
            this.numPayload.ValueChanged += new System.EventHandler(this.numHour_ValueChanged);
            // 
            // numNextTrucks
            // 
            this.numNextTrucks.DecimalPlaces = 2;
            this.numNextTrucks.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numNextTrucks.Location = new System.Drawing.Point(117, 90);
            this.numNextTrucks.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numNextTrucks.Name = "numNextTrucks";
            this.numNextTrucks.Size = new System.Drawing.Size(49, 20);
            this.numNextTrucks.TabIndex = 1;
            this.numNextTrucks.ValueChanged += new System.EventHandler(this.numHour_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(62, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Nr. Auto:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(74, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Marfa:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 92);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(99, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Urmatoarele masini:";
            // 
            // cmdSave
            // 
            this.cmdSave.Location = new System.Drawing.Point(91, 116);
            this.cmdSave.Name = "cmdSave";
            this.cmdSave.Size = new System.Drawing.Size(75, 23);
            this.cmdSave.TabIndex = 2;
            this.cmdSave.Text = "Salveaza";
            this.cmdSave.UseVisualStyleBackColor = true;
            this.cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
            // 
            // lblValidation
            // 
            this.lblValidation.AutoSize = true;
            this.lblValidation.Location = new System.Drawing.Point(48, 121);
            this.lblValidation.Name = "lblValidation";
            this.lblValidation.Size = new System.Drawing.Size(0, 13);
            this.lblValidation.TabIndex = 0;
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(173, 147);
            this.Controls.Add(this.cmdSave);
            this.Controls.Add(this.numNextTrucks);
            this.Controls.Add(this.numPayload);
            this.Controls.Add(this.numNrAuto);
            this.Controls.Add(this.numHour);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblValidation);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Settings";
            this.Text = "Settings";
            ((System.ComponentModel.ISupportInitialize)(this.numHour)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNrAuto)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPayload)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNextTrucks)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numHour;
        private System.Windows.Forms.NumericUpDown numNrAuto;
        private System.Windows.Forms.NumericUpDown numPayload;
        private System.Windows.Forms.NumericUpDown numNextTrucks;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button cmdSave;
        private System.Windows.Forms.Label lblValidation;
    }
}