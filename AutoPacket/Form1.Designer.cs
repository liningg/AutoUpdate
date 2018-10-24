namespace AutoPacket
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listViewFileList = new System.Windows.Forms.ListView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonClear = new System.Windows.Forms.Button();
            this.buttonScanFile = new System.Windows.Forms.Button();
            this.buttonPublish = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listViewFileList);
            this.groupBox1.Location = new System.Drawing.Point(12, 80);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(606, 360);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "更新文件列表";
            // 
            // listViewFileList
            // 
            this.listViewFileList.FullRowSelect = true;
            this.listViewFileList.Location = new System.Drawing.Point(7, 21);
            this.listViewFileList.Name = "listViewFileList";
            this.listViewFileList.Size = new System.Drawing.Size(593, 333);
            this.listViewFileList.TabIndex = 0;
            this.listViewFileList.UseCompatibleStateImageBehavior = false;
            this.listViewFileList.View = System.Windows.Forms.View.Details;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.buttonClear);
            this.groupBox2.Controls.Add(this.buttonScanFile);
            this.groupBox2.Controls.Add(this.buttonPublish);
            this.groupBox2.Location = new System.Drawing.Point(13, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(606, 72);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "工具栏";
            // 
            // buttonClear
            // 
            this.buttonClear.Location = new System.Drawing.Point(196, 21);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(75, 45);
            this.buttonClear.TabIndex = 2;
            this.buttonClear.Text = "清空";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // buttonScanFile
            // 
            this.buttonScanFile.Location = new System.Drawing.Point(100, 21);
            this.buttonScanFile.Name = "buttonScanFile";
            this.buttonScanFile.Size = new System.Drawing.Size(75, 45);
            this.buttonScanFile.TabIndex = 1;
            this.buttonScanFile.Text = "扫描文件";
            this.buttonScanFile.UseVisualStyleBackColor = true;
            this.buttonScanFile.Click += new System.EventHandler(this.buttonScanFile_Click);
            // 
            // buttonPublish
            // 
            this.buttonPublish.Location = new System.Drawing.Point(7, 20);
            this.buttonPublish.Name = "buttonPublish";
            this.buttonPublish.Size = new System.Drawing.Size(75, 46);
            this.buttonPublish.TabIndex = 0;
            this.buttonPublish.Text = "发布";
            this.buttonPublish.UseVisualStyleBackColor = true;
            this.buttonPublish.Click += new System.EventHandler(this.buttonPublish_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(631, 464);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "文件发布工具";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListView listViewFileList;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button buttonPublish;
        private System.Windows.Forms.Button buttonScanFile;
        private System.Windows.Forms.Button buttonClear;
    }
}

