namespace Kombinatorik {
	partial class CombinatorScreen {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.ele_pb_display = new System.Windows.Forms.PictureBox();
			this.label1 = new System.Windows.Forms.Label();
			this.ele_nud_gridx = new System.Windows.Forms.NumericUpDown();
			this.ele_nud_gridy = new System.Windows.Forms.NumericUpDown();
			this.ele_l_result = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.ele_btn_start = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.ele_pb_display)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ele_nud_gridx)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ele_nud_gridy)).BeginInit();
			this.SuspendLayout();
			// 
			// ele_pb_display
			// 
			this.ele_pb_display.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ele_pb_display.Location = new System.Drawing.Point(12, 12);
			this.ele_pb_display.Name = "ele_pb_display";
			this.ele_pb_display.Size = new System.Drawing.Size(350, 254);
			this.ele_pb_display.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.ele_pb_display.TabIndex = 0;
			this.ele_pb_display.TabStop = false;
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(13, 274);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(64, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Dimensions:";
			// 
			// ele_nud_gridx
			// 
			this.ele_nud_gridx.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.ele_nud_gridx.Location = new System.Drawing.Point(83, 272);
			this.ele_nud_gridx.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.ele_nud_gridx.Name = "ele_nud_gridx";
			this.ele_nud_gridx.Size = new System.Drawing.Size(42, 20);
			this.ele_nud_gridx.TabIndex = 2;
			this.ele_nud_gridx.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
			// 
			// ele_nud_gridy
			// 
			this.ele_nud_gridy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.ele_nud_gridy.Location = new System.Drawing.Point(131, 272);
			this.ele_nud_gridy.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.ele_nud_gridy.Name = "ele_nud_gridy";
			this.ele_nud_gridy.Size = new System.Drawing.Size(42, 20);
			this.ele_nud_gridy.TabIndex = 3;
			this.ele_nud_gridy.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
			// 
			// ele_l_result
			// 
			this.ele_l_result.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ele_l_result.Location = new System.Drawing.Point(303, 274);
			this.ele_l_result.Name = "ele_l_result";
			this.ele_l_result.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.ele_l_result.Size = new System.Drawing.Size(59, 13);
			this.ele_l_result.TabIndex = 4;
			this.ele_l_result.Text = "0000000";
			this.ele_l_result.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(224, 274);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(73, 13);
			this.label3.TabIndex = 5;
			this.label3.Text = "Combinations:";
			// 
			// ele_btn_start
			// 
			this.ele_btn_start.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.ele_btn_start.Location = new System.Drawing.Point(179, 269);
			this.ele_btn_start.Name = "ele_btn_start";
			this.ele_btn_start.Size = new System.Drawing.Size(39, 23);
			this.ele_btn_start.TabIndex = 6;
			this.ele_btn_start.Text = "Start";
			this.ele_btn_start.UseVisualStyleBackColor = true;
			this.ele_btn_start.Click += new System.EventHandler(this.ele_btn_start_Click);
			// 
			// CombinatorScreen
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(374, 299);
			this.Controls.Add(this.ele_btn_start);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.ele_l_result);
			this.Controls.Add(this.ele_nud_gridy);
			this.Controls.Add(this.ele_nud_gridx);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.ele_pb_display);
			this.Name = "CombinatorScreen";
			this.Text = "Form1";
			((System.ComponentModel.ISupportInitialize)(this.ele_pb_display)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ele_nud_gridx)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ele_nud_gridy)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox ele_pb_display;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown ele_nud_gridx;
		private System.Windows.Forms.NumericUpDown ele_nud_gridy;
		private System.Windows.Forms.Label ele_l_result;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button ele_btn_start;
	}
}

