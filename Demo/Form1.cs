using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Demo {
	public partial class Form1 : Form {
		public Form1() {
			InitializeComponent();

			string[] Notes = {
								 "C",
								 "B",
								 "A#",
								 "A",
								 "G#",
								 "G",
								 "F#",
								 "F",
								 "E",
								 "D#",
								 "D",
								 "C#",
								 "C"
							 };

			foreach (var item in Notes) {
				timeline1.RowLabels.Add(item);
			}

			Random r = new Random();
			for (int i = 0; i < 20; i++) {
				string Key = Notes[r.Next(0, Notes.Length-1)];
				int Start = r.Next(20);
				int End = r.Next(0, 5);

				timeline1.AddEvent(Key, Start, End);
			}
		}
	}
}
