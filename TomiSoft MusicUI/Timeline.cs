using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace TomiSoft.MusicUI {
	public partial class Timeline : UserControl {
		private ObservableCollection<string> rowLabels = new ObservableCollection<string>();
		private List<PictureBox> PictureBoxes = new List<PictureBox>();
		private int rowHeight = 15;
		private int labelColumnWidth = 100;
		private int colWidth = 25;
		private StringAlignment labelAlign = StringAlignment.Near;
		private Color labelBackground = Color.LightGray;
		private Color gridColor = Color.Blue;
		private Color eventColor = Color.Gray;
		private List<PictureBox> SelectedItems = new List<PictureBox>();
		private Point MouseClickPos;
		private bool MoveEnabled = false;

		/// <summary>
		/// Gets the collection that stores the labels for the rows
		/// </summary>
		[EditorBrowsable]
		public ObservableCollection<string> RowLabels {
			get {
				return this.rowLabels;
			}
		}

		/// <summary>
		/// Gets or sets the grid color.
		/// </summary>
		[EditorBrowsable]
		public Color GridColor {
			get {
				return this.gridColor;
			}
			set {
				this.gridColor = value;
				this.Invalidate();
			}
		}

		/// <summary>
		/// Gets or sets the event color.
		/// </summary>
		[EditorBrowsable]
		public Color EventColor {
			get {
				return this.eventColor;
			}
			set {
				this.eventColor = value;
				this.Invalidate();
			}
		}

		/// <summary>
		/// Gets or sets the row height
		/// </summary>
		[EditorBrowsable]
		public int RowHeight {
			get { return rowHeight; }
			set { 
				rowHeight = value;
				this.Invalidate();
			}
		}

		/// <summary>
		/// Gets or sets that column's width that contains the labels
		/// </summary>
		[EditorBrowsable]
		public int LabelColumnWidth {
			get { return labelColumnWidth; }
			set {
				labelColumnWidth = value;
				this.Invalidate();
			}
		}

		/// <summary>
		/// Gets or sets the column width
		/// </summary>
		[EditorBrowsable]
		public int ColumnWidth {
			get { return colWidth; }
			set {
				colWidth = value;
				this.Invalidate();
			}
		}

		/// <summary>
		/// Gets or sets the text alignment of the labels
		/// </summary>
		[EditorBrowsable]
		public StringAlignment LabelAlign {
			get { return labelAlign; }
			set {
				labelAlign = value;
				this.Invalidate();
			}
		}

		/// <summary>
		/// Gets or sets the background color of the label column
		/// </summary>
		[EditorBrowsable]
		public Color LabelBackColor {
			get { return labelBackground; }
			set { 
				labelBackground = value;
				this.Invalidate();
			}
		}
		
		/// <summary>
		/// Initializes a new instance of the Timeline class
		/// </summary>
		public Timeline() {
			InitializeComponent();

			this.rowLabels.CollectionChanged += (o, e) => this.Invalidate();

			this.AutoScroll = true;
			this.AutoScrollMinSize = new Size(3000, 1000);
			this.ResizeRedraw = true;
		}

		protected override void OnPaint(PaintEventArgs e) {
			e.Graphics.TranslateTransform(this.AutoScrollPosition.X, this.AutoScrollPosition.Y);
			e.Graphics.DrawLine(Pens.Black, 0, 0, 3000, 1000);
			base.OnPaint(e);

			Graphics g = e.Graphics;
			g.Clear(this.BackColor);

			this.DrawGrid(g);
		}

		/// <summary>
		/// Draws the grid with the labels
		/// </summary>
		/// <param name="g">The target graphics</param>
		protected void DrawGrid(Graphics g) {
			int GridHeight = this.rowLabels.Count() * this.rowHeight;

			//Adjust label column background
			g.FillRectangle(
				new SolidBrush(this.labelBackground),
				0, 0,
				this.labelColumnWidth,
				GridHeight
			);

			//Draw the top line
			g.DrawLine(
				new Pen(this.gridColor),
				new Point(0, 0),
				new Point(this.Width + this.AutoScrollMinSize.Width, 0)
			);

			//Label text formatting properties
			StringFormat labelTextFormat = new StringFormat();
			labelTextFormat.Alignment = this.labelAlign;
			labelTextFormat.LineAlignment = StringAlignment.Center;

			//Draw the horizontal lines and the corresponding labels
			int LastHeight = 0;
			for (int i = 0; i < this.rowLabels.Count; i++, LastHeight += this.rowHeight) {
				g.DrawLine(
					new Pen(this.gridColor),
					new Point(0, LastHeight + this.rowHeight),
					new Point(this.Width + this.AutoScrollMinSize.Width, LastHeight + this.rowHeight)
				);

				g.DrawString(
					this.rowLabels[i],
					this.Font,
					new SolidBrush(this.ForeColor),
					new RectangleF(0, this.rowHeight * i, this.labelColumnWidth, this.rowHeight),
					labelTextFormat
				);
			}

			//Draw the vertical lines
			for (int Col = this.labelColumnWidth; Col < this.Width + this.AutoScrollMinSize.Width; Col += this.colWidth) {
				g.DrawLine(
					new Pen(this.gridColor),
					new Point(Col, 0),
					new Point(Col, GridHeight)
				);
			}
		}

		public void AddEvent(string Label, int StartTime, int EndTime) {
			PictureBox pb = new PictureBox();
			pb.Parent = this;
			
			pb.BackColor = this.eventColor;
			pb.Height = this.rowHeight - 8;
			pb.Top = this.rowLabels.IndexOf(Label) * this.rowHeight + this.rowHeight / 2 - pb.Height / 2;
			pb.Left = this.labelColumnWidth + this.colWidth * StartTime;
			pb.Width = EndTime * this.colWidth;
			pb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

			pb.MouseDown += (o, e) => this.InitSelectedItemsMove(pb, e.Location);
			pb.MouseMove += (o, e) => this.MoveSelectedItems(e.Location);
			pb.MouseUp += (o, e) => this.FinishMove();

			this.Controls.Add(pb);
			this.PictureBoxes.Add(pb);
		}

		private void FinishMove() {
			if (!ModifierKeys.HasFlag(Keys.Control)) {
				foreach (var item in this.SelectedItems) {
					item.BackColor = Color.Gray;
				}
			}

			this.SelectedItems.Clear();
			this.MoveEnabled = false;
		}

		private void InitSelectedItemsMove(PictureBox p, Point MousePos) {
			this.MouseClickPos = MousePos;
			this.MoveEnabled = true;

			if (!this.SelectedItems.Contains(p)) {
				p.BackColor = Color.Red;
				this.SelectedItems.Add(p);
			}
		}

		private void MoveSelectedItems(Point MousePos) {
			if (!this.MoveEnabled)
				return;

			foreach (var item in this.SelectedItems) {
				int DeltaX = MousePos.X - this.MouseClickPos.X;
				if (item.Left + DeltaX >= this.labelColumnWidth)
					item.Left += DeltaX;
				else
					item.Left = this.labelColumnWidth;
			}
		}
	}
}
