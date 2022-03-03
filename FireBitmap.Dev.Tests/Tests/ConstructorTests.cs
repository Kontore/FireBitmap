using System.Drawing;
using NUnit.Framework;
using System.IO;
using System.Drawing.Imaging;
using Kontore.FireBitmap;

namespace Kontore.FireBitmap.Tests {
	[TestFixtureSource(typeof(TestData), nameof(TestData.TestBitmaps))]
	public class ConstructorTests {
		/// <summary>
		/// The source bitmap which will be used in the tests to verify that the <see cref="FireBitmap"/> works.
		/// </summary>
		private static Bitmap SourceBitmap { get; set; }

		public ConstructorTests(TestCaseData data) {
			SourceBitmap = (Bitmap)data.Arguments[0];
		}

		/// <summary>
		/// Tests whether the <see cref="FireBitmapBase"/> constructor copies the specified bitmap into the <see cref="FireBitmap"/>.
		/// RGB difference tolerance: 5.
		/// </summary>
		[Test]
		public void HasSamePixels() {
			var fastBitmap = FireBitmapBase.FromBitmap(SourceBitmap, (h, w, pf) => new FireBitmap(h, w, pf));

			for (int y = 0; y < SourceBitmap.Height; y++) {
				for (int x = 0; x < SourceBitmap.Width; x++) {
					var sourcePixel = SourceBitmap.GetPixel(x, y);
					var fastPixel = fastBitmap.Bitmap.GetPixel(x, y);

					Assert.AreEqual(sourcePixel.R, fastPixel.R, 5);
					Assert.AreEqual(sourcePixel.B, fastPixel.B, 5);
					Assert.AreEqual(sourcePixel.G, fastPixel.G, 5);
				}
			}
		}

		/// <summary>
		/// Tests whether the <see cref="FireBitmapBase"/> constructor keeps the size of the image when copying.
		/// File size tolerance: 10%.
		/// </summary>
		[Test]
		public void IsSameSize() {
			var fastBitmap = FireBitmapBase.FromBitmap(SourceBitmap, (h, w, pf) => new FireBitmap(h, w, pf));
			long fastByteSize;
			long normalByteSize;

			using (var stream = new MemoryStream()) {
				fastBitmap.Bitmap.Save(stream, ImageFormat.Png);
				fastByteSize = stream.Length;
			}

			using (var stream = new MemoryStream()) {
				SourceBitmap.Save(stream, ImageFormat.Png);
				normalByteSize = stream.Length;
			}

			Assert.AreEqual(normalByteSize, fastByteSize, normalByteSize / 10);
		}
	}
}
