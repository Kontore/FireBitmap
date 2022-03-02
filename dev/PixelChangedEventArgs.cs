using System;
using System.Drawing;

namespace Kontore.FireBitmap {
	/// <summary>
	/// An <see cref="EventArgs"/> implementation for an event where a pixel is changed.
	/// </summary>
	public class PixelChangedEventArgs : EventArgs {
		/// <summary>
		/// The position of the changed pixel on the x axis.
		/// </summary>
		public int X { get; set; }
		/// <summary>
		/// The position of the changed pixel on the y axis.
		/// </summary>
		public int Y { get; set; }
		/// <summary>
		/// The former color of the changed pixel
		/// </summary>
		public Color FormerColor { get; set; }
		/// <summary>
		/// The new color of the changed pixel.
		/// </summary>
		public Color NewColor { get; set; }

		public PixelChangedEventArgs() { }

		public PixelChangedEventArgs(int x, int y, Color formerColor, Color newColor) {
			X = x;
			Y = y;
			FormerColor = formerColor;
			NewColor = newColor;
		}
	}
}
