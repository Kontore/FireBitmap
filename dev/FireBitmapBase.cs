using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace Kontore.FireBitmap {
	/// <summary>
	/// An abstract class of the <see cref="IFireBitmap"/> that implements some methods and properties.
	/// <see cref="SetPixel(int, int, Color)"/> and <see cref="GetPixel(int, int)"/> are implemented the most simple way possible,
	/// no exception handling (except for the constructor), no events. It is the fastest implementation though.
	/// </summary>
	public abstract class FireBitmapBase : IFireBitmap {
		public virtual Bitmap Bitmap { get; protected set; }
		public virtual int[] Bits { get; protected set; }
		public virtual bool Disposed { get; protected set; }
		public virtual GCHandle BitsHandle { get; protected set; }
		public virtual int Width { get; protected set; }
		public virtual int Height { get; protected set; }

		public FireBitmapBase(int width, int height, PixelFormat pixelFormat = PixelFormat.Format32bppArgb) {
			if (width < 0) throw new ArgumentOutOfRangeException(nameof(width), "The width value must be greater or equal to 0");
			if (height < 0) throw new ArgumentOutOfRangeException(nameof(height), "The height value must be greater or equal to 0");

			Width = width;
			Height = height;
			Bits = new int[Width * Height];
			BitsHandle = GCHandle.Alloc(Bits, GCHandleType.Pinned);
			Bitmap = new Bitmap(Width, Height, Width * 4, pixelFormat, BitsHandle.AddrOfPinnedObject());
		}

		/// <summary>
		/// Creates an object that inherits the <see cref="IFireBitmap"/> interface
		/// and fills it with the pixels of the specified <see cref="Bitmap"/>.
		/// </summary>
		///	<param name="bitmap">The bitmap which to copy the pixels from.</param>
		/// <param name="creator">
		///		The "constructor" of the object. The parameters: 
		///		<list type="table">
		///			<item>1. <description>The width of the bitmap.</description></item>
		///			<item>2. <description>The height of the bitmap.</description></item>
		///			<item>3. <description>The <see cref="PixelFormat"/> of the bitmap.</description></item>
		///		</list>
		///
		///		All you really need to do is <c>"FromBitmap(myBitmap, (w, h, pf) =&gt; new FastBitmap(w, h, pf)"</c>, as long
		///		as that constructor exists.
		/// </param>
		/// <returns>An object that inherits the <see cref="IFireBitmap"/> interface which has all the pixels of the specified bitmap.</returns>
		public static TFireBitmapBase FromBitmap<TFireBitmapBase>(Bitmap bitmap, Func<int, int, PixelFormat, TFireBitmapBase> creator)
			where TFireBitmapBase : FireBitmapBase {
			if (bitmap == null) throw new ArgumentNullException(nameof(bitmap));

			var fbm = creator(bitmap.Width, bitmap.Height, bitmap.PixelFormat);

			using (var g = Graphics.FromImage(fbm.Bitmap)) {
				g.DrawImage(bitmap, new Rectangle(0, 0, fbm.Width, fbm.Height));
			}

			return fbm;
		}

		public virtual void SetPixel(int x, int y, Color color) {
			int index = x + (y * Bitmap.Width);
			int argb = color.ToArgb();

			Bits[index] = argb;
		}

		public virtual Color GetPixel(int x, int y) {
			int index = x + (y * Bitmap.Width);
			int argb = Bits[index];
			Color result = Color.FromArgb(argb);

			return result;
		}

		/// <summary>
		/// Disposes of the <see cref="Bitmap"/> property and frees the <see cref="BitsHandle"/>.
		/// </summary>
		public virtual void Dispose() {
			if (Disposed) return;
			Disposed = true;
			Bitmap.Dispose();
			BitsHandle.Free();
		}

		public abstract IFireBitmap DeepCopy();
	}
}
