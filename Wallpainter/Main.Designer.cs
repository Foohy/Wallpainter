namespace Wallpainter
{
    partial class WallpainterMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WallpainterMain));
            this.textboxWindowName = new System.Windows.Forms.TextBox();
            this.buttonAttach = new System.Windows.Forms.Button();
            this.buttonDetach = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textboxWindowName
            // 
            this.textboxWindowName.AccessibleRole = System.Windows.Forms.AccessibleRole.SplitButton;
            this.textboxWindowName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textboxWindowName.Location = new System.Drawing.Point(87, 12);
            this.textboxWindowName.Name = "textboxWindowName";
            this.textboxWindowName.Size = new System.Drawing.Size(299, 20);
            this.textboxWindowName.TabIndex = 0;
            // 
            // buttonAttach
            // 
            this.buttonAttach.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAttach.Location = new System.Drawing.Point(12, 38);
            this.buttonAttach.Name = "buttonAttach";
            this.buttonAttach.Size = new System.Drawing.Size(291, 31);
            this.buttonAttach.TabIndex = 1;
            this.buttonAttach.Text = "ATTACH";
            this.buttonAttach.UseVisualStyleBackColor = true;
            this.buttonAttach.Click += new System.EventHandler(this.buttonAttach_Click);
            // 
            // buttonDetach
            // 
            this.buttonDetach.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDetach.Enabled = false;
            this.buttonDetach.Location = new System.Drawing.Point(309, 38);
            this.buttonDetach.Name = "buttonDetach";
            this.buttonDetach.Size = new System.Drawing.Size(77, 31);
            this.buttonDetach.TabIndex = 2;
            this.buttonDetach.Text = "DETACH";
            this.buttonDetach.UseVisualStyleBackColor = true;
            this.buttonDetach.Click += new System.EventHandler(this.buttonDetach_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Window Title";
            // 
            // WallpainterMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(398, 81);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonDetach);
            this.Controls.Add(this.buttonAttach);
            this.Controls.Add(this.textboxWindowName);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "WallpainterMain";
            this.Text = "Wallpainter";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textboxWindowName;
        private System.Windows.Forms.Button buttonAttach;
        private System.Windows.Forms.Button buttonDetach;
        private System.Windows.Forms.Label label1;
    }
}

