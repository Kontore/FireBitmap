using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Kontore.FireBitmap {
	/// <summary>
	/// An interface for fast bitmap implementations.
	/// Provides the minimum contract, see <see cref="FireBitmapBase"/> for a more fleshed out base class.
	/// </summary>
	public interface IFireBitmap : IDisposable {
		/// <summary>
		/// The bitmap that is in use.
		/// </summary>
		Bitmap Bitmap { get; }
		/// <summary>
		/// The individual bits of the <see cref="Bitmap"/> going from the top left to the bottom right.
		/// The integer represents a 32-bit ARGB value of a color.
		/// The color can be obtained with the <see cref="Color.FromArgb(int)"/> method,
		/// but it is recommended to use the
		/// <see cref="GetPixel(int, int)"/> and <see cref="SetPixel(int, int, Color)"/> methods.
		/// </summary>
		int[] Bits { get; }
		/// <summary>
		/// Whether the object has been disposed.
		/// </summary>
		bool Disposed { get; }
		/// <summary>
		/// The <see cref="GCHandle"/> for the bits object.
		/// This is what effectively "binds" the <see cref="Bits"/> property and the actual pixels of the <see cref="Bitmap"/>.
		/// You need to set this up in the constructor.
		/// </summary>
		GCHandle BitsHandle { get; }
		/// <summary>
		/// The width of the bitmap in pixels. Equivalent to <c>this.Bitmap.Width</c>.
		/// </summary>
		int Width { get; }
		/// <summary>
		/// The height of the bitmap in pixels. Equivalent to <c>this.Bitmap.Height</c>.
		/// </summary>
		int Height { get; }
		/// <summary>
		/// Sets a pixel at the specified coordinates to the specified color.
		/// </summary>
		/// <param name="x">The zero-based position of the pixel on the x axis. Must be within the width - 1 of the bitmap.</param>
		/// <param name="y">The zero-based position of the pixel on the y axis. Must be within the height - 1 of the bitmap.</param>
		/// <param name="color">The new color.</param>
		void SetPixel(int x, int y, Color color);
		/// <summary>
		/// Gets a pixel at the specified coordinates.
		/// </summary>
		/// <param name="x">The zero-based position of the pixel on the x axis. Must be within the width - 1 of the bitmap.</param>
		/// <param name="y">The zero-based position of the pixel on the y axis. Must be within the height - 1 of the bitmap.</param>
		/// <returns>The color of the pixel.</returns>
		Color GetPixel(int x, int y);
		/// <summary>
		/// Perform a deep copy of this object.
		/// </summary>
		/// <returns>A deep copy of this object.</returns>
		IFireBitmap DeepCopy();
	}
}
