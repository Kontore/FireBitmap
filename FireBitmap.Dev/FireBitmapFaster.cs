using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Kontore.FireBitmap {
	/// <summary>
	/// A wrapper for the <see cref="Bitmap"/>.
	/// Allows faster setting and getting of bits than the <see cref="FireBitmap"/>, but has less features.
	/// </summary>
	public class FireBitmapFaster : FireBitmapBase {
		/// <summary>
		/// "Binds" the <see cref="FireBitmapBase.Bits"/> property and the pixels of the <see cref="Bitmap"/>.
		/// </summary>
		/// <param name="width">The width of the bitmap. Must be greater or equal to 0.</param>
		/// <param name="height">The height of the bitmap. Must be greater or equal to 0.</param>
		/// <param name="pixelFormat">The pixel format of the bitmap.</param>
		public FireBitmapFaster(int width, int height, PixelFormat pixelFormat = PixelFormat.Format32bppArgb)
			: base(width, height, pixelFormat) { }

		public override IFireBitmap DeepCopy() {
			var copy = FromBitmap(Bitmap, (w, h, pf) => new FireBitmapFaster(w, h, pf));
			typeof(FireBitmapFaster).GetProperty("Disposed").SetValue(copy, Disposed);
			return copy;
		}
	}
}
