namespace Tag_Renamer
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
            this.lstItems = new System.Windows.Forms.ListView();
            this.colInput = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colOutput = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblFormat = new System.Windows.Forms.Label();
            this.txtFormat = new System.Windows.Forms.TextBox();
            this.btnRun = new System.Windows.Forms.Button();
            this.lblTag = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lstItems
            // 
            this.lstItems.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colInput,
            this.colOutput});
            this.lstItems.Location = new System.Drawing.Point(12, 97);
            this.lstItems.Name = "lstItems";
            this.lstItems.Size = new System.Drawing.Size(868, 442);
            this.lstItems.TabIndex = 0;
            this.lstItems.UseCompatibleStateImageBehavior = false;
            this.lstItems.View = System.Windows.Forms.View.Details;
            // 
            // colInput
            // 
            this.colInput.Text = "入力";
            this.colInput.Width = 433;
            // 
            // colOutput
            // 
            this.colOutput.Text = "出力";
            this.colOutput.Width = 431;
            // 
            // lblFormat
            // 
            this.lblFormat.AutoSize = true;
            this.lblFormat.Location = new System.Drawing.Point(10, 17);
            this.lblFormat.Name = "lblFormat";
            this.lblFormat.Size = new System.Drawing.Size(61, 12);
            this.lblFormat.TabIndex = 1;
            this.lblFormat.Text = "フォーマット：";
            // 
            // txtFormat
            // 
            this.txtFormat.Location = new System.Drawing.Point(77, 14);
            this.txtFormat.Name = "txtFormat";
            this.txtFormat.Size = new System.Drawing.Size(641, 19);
            this.txtFormat.TabIndex = 2;
            this.txtFormat.Text = "*T!D2* - *p* - *t*";
            this.txtFormat.TextChanged += new System.EventHandler(this.txtFormat_TextChanged);
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(724, 12);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(75, 23);
            this.btnRun.TabIndex = 3;
            this.btnRun.Text = "実行";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // lblTag
            // 
            this.lblTag.Location = new System.Drawing.Point(12, 40);
            this.lblTag.Name = "lblTag";
            this.lblTag.Size = new System.Drawing.Size(868, 54);
            this.lblTag.TabIndex = 4;
            this.lblTag.Text = "タグ";
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(805, 12);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 5;
            this.btnClear.Text = "クリア";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(892, 551);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.lblTag);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.txtFormat);
            this.Controls.Add(this.lblFormat);
            this.Controls.Add(this.lstItems);
            this.Name = "MainForm";
            this.Text = "タグリネーム";
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainForm_DragEnter);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lstItems;
        private System.Windows.Forms.ColumnHeader colInput;
        private System.Windows.Forms.ColumnHeader colOutput;
        private System.Windows.Forms.Label lblFormat;
        private System.Windows.Forms.TextBox txtFormat;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Label lblTag;
        private System.Windows.Forms.Button btnClear;
    }
}

