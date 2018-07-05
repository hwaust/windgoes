using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WindGoes.Data;

namespace WindGoes.Forms.Data
{
    public partial class DataImportForm : Form
    {
        public DataImportForm()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (IsValidated())
            {
                return;
            }
        }

        private bool IsValidated()
        {
            if (txtConnection.TextLength == 0)
            {
                 
            }
            return true;
        }
    }
}
