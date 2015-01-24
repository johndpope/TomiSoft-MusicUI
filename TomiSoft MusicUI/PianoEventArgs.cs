using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TomiSoft.MusicUI {
	/// <summary>
	/// This class gives information about the pressed or released piano key
	/// </summary>
	public class PianoEventArgs : EventArgs {
		private int key;

		/// <summary>
		/// Gets the pressed or released key's number.
		/// </summary>
		public int Key {
			get {
				return this.key;
			}
		}

		/// <summary>
		/// Initializes a new instance of the PianoEventArgs class.
		/// </summary>
		/// <param name="Key">The number of the key that was pressed or released</param>
		public PianoEventArgs(int Key) {
			this.key = Key;
		}
	}
}
