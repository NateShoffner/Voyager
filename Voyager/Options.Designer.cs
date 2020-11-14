namespace Voyager
{
    partial class Options
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Options));
            this.cancelbtn = new System.Windows.Forms.Button();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.mainbtn = new System.Windows.Forms.ToolStripButton();
            this.appearancebtn = new System.Windows.Forms.ToolStripButton();
            this.contentbtn = new System.Windows.Forms.ToolStripButton();
            this.networkbtn = new System.Windows.Forms.ToolStripButton();
            this.advancedbtn = new System.Windows.Forms.ToolStripButton();
            this.okbtn = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cancelbtn
            // 
            this.cancelbtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelbtn.Location = new System.Drawing.Point(322, 305);
            this.cancelbtn.Name = "cancelbtn";
            this.cancelbtn.Size = new System.Drawing.Size(75, 23);
            this.cancelbtn.TabIndex = 1;
            this.cancelbtn.Text = "Cancel";
            this.cancelbtn.UseVisualStyleBackColor = true;
            this.cancelbtn.Click += new System.EventHandler(this.cancelbtn_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.CanOverflow = false;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mainbtn,
            this.appearancebtn,
            this.contentbtn,
            this.networkbtn,
            this.advancedbtn});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(489, 75);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // mainbtn
            // 
            this.mainbtn.Image = ((System.Drawing.Image)(resources.GetObject("mainbtn.Image")));
            this.mainbtn.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.mainbtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mainbtn.Margin = new System.Windows.Forms.Padding(20, 1, 20, 2);
            this.mainbtn.Name = "mainbtn";
            this.mainbtn.Size = new System.Drawing.Size(52, 72);
            this.mainbtn.Text = "Main";
            this.mainbtn.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.mainbtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // appearancebtn
            // 
            this.appearancebtn.Image = ((System.Drawing.Image)(resources.GetObject("appearancebtn.Image")));
            this.appearancebtn.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.appearancebtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.appearancebtn.Margin = new System.Windows.Forms.Padding(20, 1, 20, 2);
            this.appearancebtn.Name = "appearancebtn";
            this.appearancebtn.Size = new System.Drawing.Size(74, 72);
            this.appearancebtn.Text = "Appearance";
            this.appearancebtn.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.appearancebtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // contentbtn
            // 
            this.contentbtn.Image = ((System.Drawing.Image)(resources.GetObject("contentbtn.Image")));
            this.contentbtn.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.contentbtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.contentbtn.Margin = new System.Windows.Forms.Padding(20, 1, 20, 2);
            this.contentbtn.Name = "contentbtn";
            this.contentbtn.Size = new System.Drawing.Size(54, 72);
            this.contentbtn.Text = "Content";
            this.contentbtn.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.contentbtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // networkbtn
            // 
            this.networkbtn.Image = ((System.Drawing.Image)(resources.GetObject("networkbtn.Image")));
            this.networkbtn.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.networkbtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.networkbtn.Margin = new System.Windows.Forms.Padding(20, 1, 20, 2);
            this.networkbtn.Name = "networkbtn";
            this.networkbtn.Size = new System.Drawing.Size(56, 72);
            this.networkbtn.Text = "Network";
            this.networkbtn.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.networkbtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // advancedbtn
            // 
            this.advancedbtn.Image = ((System.Drawing.Image)(resources.GetObject("advancedbtn.Image")));
            this.advancedbtn.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.advancedbtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.advancedbtn.Margin = new System.Windows.Forms.Padding(20, 1, 20, 2);
            this.advancedbtn.Name = "advancedbtn";
            this.advancedbtn.Size = new System.Drawing.Size(64, 72);
            this.advancedbtn.Text = "Advanced";
            this.advancedbtn.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.advancedbtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // okbtn
            // 
            this.okbtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okbtn.Location = new System.Drawing.Point(403, 305);
            this.okbtn.Name = "okbtn";
            this.okbtn.Size = new System.Drawing.Size(75, 23);
            this.okbtn.TabIndex = 3;
            this.okbtn.Text = "Ok";
            this.okbtn.UseVisualStyleBackColor = true;
            this.okbtn.Click += new System.EventHandler(this.okbtn_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.button1);
            this.panel1.Location = new System.Drawing.Point(12, 89);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(465, 210);
            this.panel1.TabIndex = 4;
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(273, 99);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(102, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Internet Options";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // Options
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(489, 340);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.okbtn);
            this.Controls.Add(this.cancelbtn);
            this.Controls.Add(this.toolStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Options";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cancelbtn;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton contentbtn;
        private System.Windows.Forms.ToolStripButton mainbtn;
        private System.Windows.Forms.ToolStripButton appearancebtn;
        private System.Windows.Forms.ToolStripButton advancedbtn;
        private System.Windows.Forms.Button okbtn;
        private System.Windows.Forms.ToolStripButton networkbtn;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.Button button1;
    }
}