namespace WindGoes.Forms
{
    partial class MultiDataSearchForm
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
            this.lblKey = new System.Windows.Forms.Label();
            this.txtKey = new System.Windows.Forms.TextBox();
            this.lvwResult = new System.Windows.Forms.ListView();
            this.lblResult = new System.Windows.Forms.Label();
            this.lblRecordNum = new System.Windows.Forms.Label();
            this.lblNum = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblKey
            // 
            this.lblKey.AutoSize = true;
            this.lblKey.Location = new System.Drawing.Point(12, 9);
            this.lblKey.Name = "lblKey";
            this.lblKey.Size = new System.Drawing.Size(269, 12);
            this.lblKey.TabIndex = 0;
            this.lblKey.Text = "请输入要搜索的内容(按Enter选择并返回结果）：";
            // 
            // txtKey
            // 
            this.txtKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtKey.Location = new System.Drawing.Point(14, 33);
            this.txtKey.Name = "txtKey";
            this.txtKey.Size = new System.Drawing.Size(470, 21);
            this.txtKey.TabIndex = 1;
            this.txtKey.TextChanged += new System.EventHandler(this.txtKey_TextChanged);
            this.txtKey.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtKey_KeyDown);
            // 
            // lvwResult
            // 
            this.lvwResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvwResult.CheckBoxes = true;
            this.lvwResult.FullRowSelect = true;
            this.lvwResult.GridLines = true;
            this.lvwResult.HideSelection = false;
            this.lvwResult.HoverSelection = true;
            this.lvwResult.Location = new System.Drawing.Point(14, 87);
            this.lvwResult.Name = "lvwResult";
            this.lvwResult.Size = new System.Drawing.Size(470, 193);
            this.lvwResult.TabIndex = 2;
            this.lvwResult.UseCompatibleStateImageBehavior = false;
            this.lvwResult.View = System.Windows.Forms.View.Details;
            this.lvwResult.DoubleClick += new System.EventHandler(this.lvwResult_DoubleClick);
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.Location = new System.Drawing.Point(12, 66);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(203, 12);
            this.lblResult.TabIndex = 3;
            this.lblResult.Text = "查询结果列表(按上下键进行选择）：";
            // 
            // lblRecordNum
            // 
            this.lblRecordNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRecordNum.AutoSize = true;
            this.lblRecordNum.Location = new System.Drawing.Point(332, 66);
            this.lblRecordNum.Name = "lblRecordNum";
            this.lblRecordNum.Size = new System.Drawing.Size(113, 12);
            this.lblRecordNum.TabIndex = 4;
            this.lblRecordNum.Text = "查询到的记录条数：";
            // 
            // lblNum
            // 
            this.lblNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblNum.AutoSize = true;
            this.lblNum.Location = new System.Drawing.Point(461, 66);
            this.lblNum.Name = "lblNum";
            this.lblNum.Size = new System.Drawing.Size(23, 12);
            this.lblNum.TabIndex = 5;
            this.lblNum.Text = "0条";
            // 
            // MultiDataSearchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(494, 292);
            this.Controls.Add(this.lblNum);
            this.Controls.Add(this.lblRecordNum);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.lvwResult);
            this.Controls.Add(this.txtKey);
            this.Controls.Add(this.lblKey);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MultiDataSearchForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DataSearch";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DataSearchForm_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblKey;
        private System.Windows.Forms.TextBox txtKey;
        private System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.Label lblRecordNum;
        private System.Windows.Forms.Label lblNum;
        private System.Windows.Forms.ListView lvwResult;
    }
}