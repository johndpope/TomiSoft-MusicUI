namespace Demo {
	partial class Form1 {
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
			this.timeline1 = new TomiSoft.MusicUI.Timeline();
			this.SuspendLayout();
			// 
			// timeline1
			// 
			this.timeline1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.timeline1.BackColor = System.Drawing.SystemColors.Control;
			this.timeline1.ColumnWidth = 25;
			this.timeline1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.timeline1.ForeColor = System.Drawing.SystemColors.ControlText;
			this.timeline1.LabelAlign = System.Drawing.StringAlignment.Near;
			this.timeline1.LabelBackColor = System.Drawing.Color.LightGray;
			this.timeline1.LabelColumnWidth = 100;
			this.timeline1.Location = new System.Drawing.Point(12, 12);
			this.timeline1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.timeline1.Name = "timeline1";
			this.timeline1.RowHeight = 17;
			this.timeline1.Size = new System.Drawing.Size(450, 238);
			this.timeline1.TabIndex = 0;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(474, 262);
			this.Controls.Add(this.timeline1);
			this.Name = "Form1";
			this.Text = "TomiSoft MusicUI Demo";
			this.ResumeLayout(false);

		}

		#endregion

		private TomiSoft.MusicUI.Timeline timeline1;



	}
}

