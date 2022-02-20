using System;
using System.Drawing;

namespace Kontore.FireBitmap.Extensions {
	public static partial class FastBitmapX {
		/// <summary>
		/// Gets all the pixels in this bitmap.
		/// </summary>
		/// <param name="fbm">The current fire bitmap.</param>
		/// <returns>A 2D <see cref="Color"/> array. The first dimension is the x axis, and the second is the y axis.</returns>
		public static Color[,] GetPixels2D(this IFireBitmap fbm) {
			if (fbm == null) throw new ArgumentNullException(nameof(fbm));

			var pixels = new Color[fbm.Width, fbm.Height];

			for (int x = 0; x < fbm.Width; x++) {
				for (int y = 0; y < fbm.Height; y++) {
					pixels[x, y] = fbm.GetPixel(x, y);
				}
			}

			return pixels;
		}

		/// <summary>
		/// Gets all the pixels in this bitmap.
		/// </summary>
		/// <param name="fbm">The current fire bitmap.</param>
		/// <returns>A <see cref="Color"/> array that contains all the pixels. From top left to bottom right.</returns>
		public static Color[] GetPixels(this IFireBitmap fbm) {
			if (fbm == null) throw new ArgumentNullException(nameof(fbm));
			
			var pixels = new Color[fbm.Width * fbm.Height];

			for (int i = 0; i < pixels.Length; i++) {
				int y = i / fbm.Width;
				int x = i - (y * fbm.Width);

				pixels[i] = fbm.GetPixel(x, y);
			}
			
			return pixels;
		}
	}
}
