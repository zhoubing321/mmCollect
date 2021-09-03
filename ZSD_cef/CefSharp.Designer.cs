namespace ZSD_cef
{
    partial class CefSharp
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CefSharp));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.textBox_Url = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_back = new System.Windows.Forms.Button();
            this.btn_go = new System.Windows.Forms.Button();
            this.btn_refresh = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.statusStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 496);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1043, 25);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolLabel
            // 
            this.toolLabel.Name = "toolLabel";
            this.toolLabel.Size = new System.Drawing.Size(21, 20);
            this.toolLabel.Text = "…";
            // 
            // textBox_Url
            // 
            this.textBox_Url.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_Url.Location = new System.Drawing.Point(139, 6);
            this.textBox_Url.Margin = new System.Windows.Forms.Padding(4);
            this.textBox_Url.Multiline = true;
            this.textBox_Url.Name = "textBox_Url";
            this.textBox_Url.Size = new System.Drawing.Size(612, 30);
            this.textBox_Url.TabIndex = 2;
            this.textBox_Url.WordWrap = false;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Location = new System.Drawing.Point(4, 48);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1043, 442);
            this.panel1.TabIndex = 4;
            // 
            // btn_back
            // 
            this.btn_back.BackColor = System.Drawing.Color.White;
            this.btn_back.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_back.Location = new System.Drawing.Point(4, 2);
            this.btn_back.Margin = new System.Windows.Forms.Padding(4);
            this.btn_back.Name = "btn_back";
            this.btn_back.Size = new System.Drawing.Size(40, 38);
            this.btn_back.TabIndex = 3;
            this.btn_back.Text = "<";
            this.btn_back.UseVisualStyleBackColor = false;
            this.btn_back.Click += new System.EventHandler(this.btn_back_Click);
            // 
            // btn_go
            // 
            this.btn_go.BackColor = System.Drawing.Color.White;
            this.btn_go.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_go.Location = new System.Drawing.Point(91, 2);
            this.btn_go.Margin = new System.Windows.Forms.Padding(4);
            this.btn_go.Name = "btn_go";
            this.btn_go.Size = new System.Drawing.Size(40, 38);
            this.btn_go.TabIndex = 3;
            this.btn_go.Text = ">";
            this.btn_go.UseVisualStyleBackColor = false;
            this.btn_go.Click += new System.EventHandler(this.btn_go_Click);
            // 
            // btn_refresh
            // 
            this.btn_refresh.BackColor = System.Drawing.Color.White;
            this.btn_refresh.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_refresh.Location = new System.Drawing.Point(47, 2);
            this.btn_refresh.Margin = new System.Windows.Forms.Padding(4);
            this.btn_refresh.Name = "btn_refresh";
            this.btn_refresh.Size = new System.Drawing.Size(40, 38);
            this.btn_refresh.TabIndex = 3;
            this.btn_refresh.Text = "◎";
            this.btn_refresh.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_refresh.UseVisualStyleBackColor = false;
            this.btn_refresh.Click += new System.EventHandler(this.btn_refresh_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabControl1.ItemSize = new System.Drawing.Size(250, 30);
            this.tabControl1.Location = new System.Drawing.Point(760, 2);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(283, 35);
            this.tabControl1.TabIndex = 5;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 34);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage1.Size = new System.Drawing.Size(275, 0);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "百度一下";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // CefSharp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1043, 521);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btn_go);
            this.Controls.Add(this.btn_refresh);
            this.Controls.Add(this.btn_back);
            this.Controls.Add(this.textBox_Url);
            this.Controls.Add(this.statusStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "CefSharp";
            this.Text = "CefSharp浏览器【v20201210】";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CefSharp_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolLabel;
        private System.Windows.Forms.TextBox textBox_Url;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_back;
        private System.Windows.Forms.Button btn_go;
        private System.Windows.Forms.Button btn_refresh;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
    }
}

