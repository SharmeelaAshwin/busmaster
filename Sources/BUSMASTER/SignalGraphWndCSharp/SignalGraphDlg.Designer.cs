namespace SignalGraphWndCSharp
{
    using GraphLib;
    partial class SignalGraphDlg
    {
        PlotterDisplayEx GraphCtrl = null;
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
            this.GraphCtrl = new GraphLib.PlotterDisplayEx();
            this.label1 = new System.Windows.Forms.Label();
            this.GraphTypeCombo = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.BackGroundColorCombo = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // GraphCtrl
            // 
            this.GraphCtrl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GraphCtrl.BackColor = System.Drawing.Color.White;
            this.GraphCtrl.BackgroundColorBot = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.GraphCtrl.BackgroundColorTop = System.Drawing.Color.Navy;
            this.GraphCtrl.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.GraphCtrl.DashedGridColor = System.Drawing.Color.Blue;
            this.GraphCtrl.DoubleBuffering = true;
            this.GraphCtrl.Location = new System.Drawing.Point(0, 0);
            this.GraphCtrl.Name = "GraphCtrl";
            this.GraphCtrl.PlaySpeed = 0.5F;
            this.GraphCtrl.Size = new System.Drawing.Size(727, 403);
            this.GraphCtrl.SolidGridColor = System.Drawing.Color.Navy;
            this.GraphCtrl.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 430);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Graph Type";
            // 
            // GraphTypeCombo
            // 
            this.GraphTypeCombo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.GraphTypeCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.GraphTypeCombo.FormattingEnabled = true;
            this.GraphTypeCombo.Location = new System.Drawing.Point(81, 427);
            this.GraphTypeCombo.Name = "GraphTypeCombo";
            this.GraphTypeCombo.Size = new System.Drawing.Size(173, 21);
            this.GraphTypeCombo.TabIndex = 2;
            this.GraphTypeCombo.SelectedIndexChanged += new System.EventHandler(this.GraphTypeCombo_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 486);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Background Color";
            // 
            // BackGroundColorCombo
            // 
            this.BackGroundColorCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.BackGroundColorCombo.FormattingEnabled = true;
            this.BackGroundColorCombo.Location = new System.Drawing.Point(110, 483);
            this.BackGroundColorCombo.Name = "BackGroundColorCombo";
            this.BackGroundColorCombo.Size = new System.Drawing.Size(144, 21);
            this.BackGroundColorCombo.TabIndex = 4;
            this.BackGroundColorCombo.SelectedIndexChanged += new System.EventHandler(this.BackGroundColorCombo_SelectedIndexChanged);
            // 
            // SignalGraphDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(728, 532);
            this.Controls.Add(this.BackGroundColorCombo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.GraphTypeCombo);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.GraphCtrl);
            this.Name = "SignalGraphDlg";
            this.Text = "Signal Graph";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox GraphTypeCombo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox BackGroundColorCombo;


    }
}

