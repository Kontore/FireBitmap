using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Kontore.FireBitmap {
	/// <summary>
	/// A wrapper for the <see cref="Bitmap"/>.
	/// Allows fast setting and getting of bits, but is slower than <see cref="FireBitmapFaster"/>.
	/// </summary>
	public class FireBitmap : FireBitmapBase {
		protected PixelChangedEventArgs PixelChangedEventArgs { get; }
		public EventHandler<PixelChangedEventArgs> PixelChanged;

		protected void OnPixelChanged(PixelChangedEventArgs e) {
			PixelChanged?.Invoke(this, e);
		}

		/// <summary>
		/// "Binds" the <see cref="FireBitmapBase.Bits"/> property and the pixels of the <see cref="Bitmap"/>.
		/// </summary>
		/// <param name="width">The width of the bitmap. Must be greater or equal to 0.</param>
		/// <param name="height">The height of the bitmap. Must be greater or equal to 0.</param>
		/// <param name="pixelFormat">The pixel format of the bitmap.</param>
		public FireBitmap(int width, int height, PixelFormat pixelFormat = PixelFormat.Format32bppArgb) 
			: base(width, height, pixelFormat) {
			PixelChangedEventArgs = new PixelChangedEventArgs();
		}

		public override void SetPixel(int x, int y, Color color) {
			if (color == null) throw new ArgumentNullException(nameof(color));
			if (x < 0 || x >= Width) throw new ArgumentOutOfRangeException(nameof(x), "Must be within the width - 1 of the bitmap. (" + 0 + ":" + (Width - 1) + ")");
			if (y < 0 || y >= Height) throw new ArgumentOutOfRangeException(nameof(y), "Must be within the height - 1 of the bitmap. (" + 0 + ":" + (Height - 1) + ")");
			
			int index = x + (y * Bitmap.Width);
			int argb = color.ToArgb();

			Bits[index] = argb;

			PixelChangedEventArgs.X = x;
			PixelChangedEventArgs.Y = y;
			PixelChangedEventArgs.FormerColor = Color.FromArgb(Bits[index]);
			PixelChangedEventArgs.NewColor = color;

			OnPixelChanged(PixelChangedEventArgs);
		}

		public override Color GetPixel(int x, int y) {
			if (x < 0 || x >= Width) throw new ArgumentOutOfRangeException(nameof(x), "Must be within the width - 1 of the bitmap. (" + 0 + ":" + (Width - 1) + ")");
			if (y < 0 || y >= Height) throw new ArgumentOutOfRangeException(nameof(y), "Must be within the height - 1 of the bitmap. (" + 0 + ":" + (Height - 1) + ")");

			return base.GetPixel(x, y);
		}

		public override IFireBitmap DeepCopy() {
			var copy = FromBitmap(Bitmap, (w, h, pf) => new FireBitmap(w, h, pf));
			typeof(FireBitmap).GetProperty("Disposed").SetValue(copy, Disposed);
			return copy;
		}
	}
}
