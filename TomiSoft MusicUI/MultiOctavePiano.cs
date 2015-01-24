using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TomiSoft.MusicUI {
	public partial class MultiOctavePiano : UserControl {
		private int octaves;

		public IEnumerable<int> PressedNotes {
			get {
				foreach (var item in this.Pianos) {
					foreach (var a in item.Key.PressedNotes) {
						yield return a + item.Value * 12;
					}
				}
			}
		}
		
		public int Octaves {
			get { return this.octaves; }
			set {
				if (value < 1)
					throw new ArgumentException("The value must be greater than 0");

				this.octaves = value;

				//Clean up existing pianos
				foreach (var item in this.Pianos) {
					item.Key.Dispose();
				}
				this.Pianos.Clear();

				//Creating new pianos
				OneOctavePiano p = null;
				for (int i = 0; i < this.octaves; i++) {
					p = new OneOctavePiano();
					p.Top = 0;
					p.Left = i * p.Width;

					//Magic! Do not touch!!!
					int j = i;

					p.PianoKeyDown += (o, e) => {
						if (this.PianoKeyDown != null) {
							this.PianoKeyDown(this, new PianoEventArgs(e.Key + (j + this.LowestOctave) * 12));
						}
					};

					p.PianoKeyUp += (o, e) => {
						if (this.PianoKeyUp != null) {
							this.PianoKeyUp(this, new PianoEventArgs(e.Key + (j + this.LowestOctave) * 12));
						}
					};

					this.Controls.Add(p);

					this.Pianos.Add(p, i + this.LowestOctave);
				}

				this.Height = p.Height;
				this.Width = p.Left + p.Width;
			}
		}

		private int lowestOctave;

		public int LowestOctave {
			get {
				return this.lowestOctave;
			}

			set {
				this.lowestOctave = value;

				for (int i = 0; i < this.Pianos.Keys.Count; i++) {
					this.Pianos[this.Pianos.Keys.ElementAt(i)] = i + lowestOctave;
				}
			}
		}

		private bool holdSelection;
		
		/// <summary>
		/// Gets or sets that a piano key must be pressed until the next click
		/// </summary>
		public bool HoldSelection {
			get {
				return this.holdSelection;
			}
			set {
				this.holdSelection = value;
				foreach (var item in this.Pianos.Keys) {
					item.HoldSelection = value;
				}
			}
		}

		private Dictionary<OneOctavePiano, int> Pianos;

		/// <summary>
		/// This event occures when a piano key is pressed or hold down (if HoldSelection is true)
		/// </summary>
		public event EventHandler<PianoEventArgs> PianoKeyDown;

		/// <summary>
		/// This event occures when a piano key is released
		/// </summary>
		public event EventHandler<PianoEventArgs> PianoKeyUp;

		/// <summary>
		/// Initializes a new instance of the MultiOctavePiano user control with 1 octave and 0 lowest octave value
		/// </summary>
		public MultiOctavePiano() : this(1, 0) {

		}

		/// <summary>
		/// Initializes a new instance of the MultiOctavePiano user control with given number of octaves and with the
		/// given lowest octave value
		/// </summary>
		/// <param name="Octaves">The number of octaves</param>
		/// <param name="LowestOctave">The number of the lowest octave</param>
		public MultiOctavePiano(int Octaves, int LowestOctave) {
			InitializeComponent();

			this.Pianos = new Dictionary<OneOctavePiano, int>();
			this.LowestOctave = LowestOctave;
			this.Octaves = Octaves;
		}

		/// <summary>
		/// Releases all pressed notes
		/// </summary>
		public void ReleaseAllKeys() {
			foreach (var item in this.Pianos) {
				item.Key.ReleaseAllKeys();
			}
		}
	}
}
