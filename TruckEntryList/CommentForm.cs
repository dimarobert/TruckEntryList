using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TruckEntryList {
    public partial class CommentForm : Form {
        public CommentForm() {
            InitializeComponent();
        }
        public string Comment { get; set; } = "";
        private void cmdOk_Click(object sender, EventArgs e) {
            this.DialogResult = DialogResult.OK;
            Comment = txtComment.Text;
            this.Close();
        }

        private void CommentForm_Load(object sender, EventArgs e) {
            txtComment.Text = Comment;
        }
    }
}
