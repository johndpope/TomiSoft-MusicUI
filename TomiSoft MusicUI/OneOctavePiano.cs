using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TomiSoft.MusicUI {
	/// <summary>
	/// This user control represents a one octave piano
	/// </summary>
	public partial class OneOctavePiano : UserControl {
		private Dictionary<PictureBox, int> WhiteKeys;
		private Dictionary<PictureBox, int> BlackKeys;
		private Dictionary<int, bool> Selected;

		/// <summary>
		/// Gets or sets that a piano key must be pressed until the next click
		/// </summary>
		public bool HoldSelection {
			get;
			set;
		}

		/// <summary>
		/// Gets all the currently pressed notes
		/// </summary>
		public IEnumerable<int> PressedNotes {
			get {
				return from c in this.Selected
					   where c.Value
					   select c.Key;
			}
		}

		/// <summary>
		/// This event occures when a piano key is pressed or hold down (if HoldSelection is true)
		/// </summary>
		public event EventHandler<PianoEventArgs> PianoKeyDown;

		/// <summary>
		/// This event occures when a piano key is released
		/// </summary>
		public event EventHandler<PianoEventArgs> PianoKeyUp;

		/// <summary>
		/// Initializes a new instance of the OneOctavePiano user control
		/// </summary>
		public OneOctavePiano() {
			InitializeComponent();

			this.WhiteKeys = new Dictionary<PictureBox, int>() {
				{ pbWhite0, 0},
				{ pbWhite1, 2},
				{ pbWhite2, 4},
				{ pbWhite3, 5},
				{ pbWhite4, 7},
				{ pbWhite5, 9},
				{ pbWhite6, 11}
			};

			this.BlackKeys = new Dictionary<PictureBox, int>() {
				{ pbBlack0, 1},
				{ pbBlack1, 3},
				{ pbBlack2, 6},
				{ pbBlack3, 8},
				{ pbBlack4, 10}
			};

			this.Selected = new Dictionary<int, bool>();

			this.SubscribeEvents();
		}

		/// <summary>
		/// This method adds events to all piano keys
		/// </summary>
		private void SubscribeEvents() {
			var AllKeys = from c in this.WhiteKeys.Union(this.BlackKeys)
						  select new {
							  pictureBox = c.Key,
							  noteNumber = c.Value,
							  isWhite = this.WhiteKeys.ContainsKey(c.Key)
						  };

			foreach (var item in AllKeys) {
				this.Selected.Add(item.noteNumber, false);

				item.pictureBox.MouseDown += (o, e) => {
					if (this.HoldSelection) {
						if (!this.Selected[item.noteNumber]) {
							this.Selected[item.noteNumber] = true;
							this.PressKey(item.pictureBox, item.noteNumber, item.isWhite);
						}
						else {
							this.Selected[item.noteNumber] = false;
							this.ReleaseKey(item.pictureBox, item.noteNumber, item.isWhite);
						}
					}
					else {
						this.PressKey(item.pictureBox, item.noteNumber, item.isWhite);
					}

				};

				item.pictureBox.MouseUp += (o, e) => {
					if (!this.HoldSelection) {
						this.ReleaseKey(item.pictureBox, item.noteNumber, item.isWhite);
					}
				};
			}
		}

		private void PressKey(PictureBox p, int NoteNumber, bool IsWhite) {
			Bitmap Image = (IsWhite) ? Properties.Resources.piano_selected_white : Properties.Resources.piano_selected_black;

			p.BackgroundImage = Image;
			if (this.PianoKeyDown != null) {
				this.PianoKeyDown(this, new PianoEventArgs(NoteNumber));
			}
		}

		private void ReleaseKey(PictureBox p, int NoteNumber, bool IsWhite) {
			Bitmap Image = (IsWhite) ? Properties.Resources.piano_default_white : Properties.Resources.piano_default_black;

			p.BackgroundImage = Image;
			if (this.PianoKeyUp != null) {
				this.PianoKeyUp(this, new PianoEventArgs(NoteNumber));
			}
		}

		public void ReleaseAllKeys() {
			var Selected = from c in this.WhiteKeys.Union(this.BlackKeys)
						   where this.Selected[c.Value]
						   select new {
							   pictureBox = c.Key,
							   noteNumber = c.Value,
							   isWhite = this.WhiteKeys.ContainsValue(c.Value)
						   };

			foreach (var item in Selected) {
				this.Selected[item.noteNumber] = false;
				this.ReleaseKey(item.pictureBox, item.noteNumber, item.isWhite);
			}
		}
	}
}
