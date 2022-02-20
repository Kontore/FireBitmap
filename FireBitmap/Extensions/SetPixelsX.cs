using System;
using System.Drawing;

namespace Kontore.FireBitmap.Extensions {
	public static partial class FastBitmapX {
		/// <summary>
		/// Changes the pixels of the bitmap using a 2D array.
		/// The array must go from the top left to the bottom right.
		/// </summary>
		/// <param name="fbm">The current fire bitmap.</param>
		/// <param name="newPixels">The new pixels of the bitmap.</param>
		public static void SetPixels2D(this IFireBitmap fbm, Color[,] newPixels) {
			if (fbm == null) throw new ArgumentNullException(nameof(fbm));
			if (newPixels == null) throw new ArgumentException(nameof(newPixels));

			for (int x = 0; x < fbm.Width; x++) {
				for (int y = 0; y < fbm.Height; y++) {
					fbm.SetPixel(x, y, newPixels[x, y]);
				}
			}
		}

		/// <summary>
		/// Changes the pixels of this fast bitmap to the specified pixels.
		/// The array must go from the top left to the bottom right.
		/// </summary>
		/// <param name="fbm">The current fire bitmap.</param>
		/// <param name="newPixels">The new pixels of the bitmap.</param>
		public static void SetPixels(this IFireBitmap fbm, Color[] newPixels) {
			if (fbm == null) throw new ArgumentNullException(nameof(fbm));
			if (newPixels == null) throw new ArgumentNullException(nameof(newPixels));

			for (int i = 0; i < fbm.Width * fbm.Height; i++) {
				int y = i / fbm.Width;
				int x = i - (y * fbm.Width);

				fbm.SetPixel(x, y, newPixels[i]);
			}
		}

		/// <summary>
		/// Applies the <paramref name="selector"/> function on all bitmap pixels.
		/// </summary>
		/// <param name="fbm">The current fire bitmap.</param>
		/// <param name="selector">The selector to apply to each pixel.</param>
		public static void SetPixels(this IFireBitmap fbm, Func<Color, Color> selector) {
			if (fbm == null) throw new ArgumentNullException(nameof(fbm));
			if (selector == null) throw new ArgumentNullException(nameof(selector));

			for (int x = 0; x < fbm.Width; x++) {
				for (int y = 0; y < fbm.Height; y++) {
					fbm.SetPixel(x, y, selector(fbm.GetPixel(x, y)));
				}
			}
		}

		/// <summary>
		/// Applies the <paramref name="selector"/> function on a pixel specified by the given coordinates.
		/// </summary>
		/// <param name="fbm">The current fire bitmap.</param>
		/// <param name="x">The zero-based position of the pixel on the x axis. Must be within the width - 1 of the bitmap.</param>
		/// <param name="y">The zero-based position of the pixel on the y axis. Must be within the height - 1 of the bitmap.</param>
		/// <param name="selector">The selector to apply to the specified pixel.</param>
		public static void SetPixel(this IFireBitmap fbm, int x, int y, Func<Color, Color> selector) {
			if (fbm == null) throw new ArgumentNullException(nameof(fbm));
			if (selector == null) throw new ArgumentNullException(nameof(selector));
			
			fbm.SetPixel(x, y, selector(fbm.GetPixel(x, y)));
		}
	}
}
