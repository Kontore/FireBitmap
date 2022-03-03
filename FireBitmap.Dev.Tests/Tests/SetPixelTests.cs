using System;
using System.Diagnostics;
using System.Drawing;
using NUnit.Framework;
using Kontore.Extensions.MiscellaneousXN;

namespace Kontore.FireBitmap.Tests {
	[TestFixtureSource(typeof(TestData), nameof(TestData.TestBitmaps))]
	public class SetPixelTests {
		private static Stopwatch Stopwatch { get; } = new Stopwatch();
		/// <summary>
		/// The source bitmap which will be used in the tests to verify that the <see cref="FireBitmap"/> works.
		/// </summary>
		private static Bitmap SourceBitmap { get; set; }

		public SetPixelTests(TestCaseData data) {
			SourceBitmap = (Bitmap)data.Arguments[0];
		}

		/// <summary>
		/// Tests the <see cref="FireBitmap.SetPixel(int, int, Color)"/> method
		/// by copying all pixels from the source bitmap to a new bitmap.
		/// Asserts whether they are equal using the <see cref="Bitmap.GetPixel(int, int)"/> method.
		/// </summary>
		[Test]
		public void SetPixel() {
			var fastBitmap = new FireBitmapFaster(SourceBitmap.Width, SourceBitmap.Height, SourceBitmap.PixelFormat);

			for (int y = 0; y < SourceBitmap.Height; y++) {
				for (int x = 0; x < SourceBitmap.Width; x++) {
					var sourcePixel = SourceBitmap.GetPixel(x, y);

					fastBitmap.SetPixel(x, y, sourcePixel);

					var newPixel = fastBitmap.Bitmap.GetPixel(x, y);

					Assert.AreEqual(sourcePixel, newPixel);
				}
			}
		}

		/// <summary>
		/// TODO:
		/// This functions incorrectly. It seems to be influenced by other tests. Fix later.
		/// As of 3/2/22, if this test would work, it would pass.
		/// 
		/// Tests whether the <see cref="FireBitmap.SetPixel(int, int, Color)"/> method is faster than the
		/// <see cref="Bitmap.SetPixel(int, int, Color)"/> method.
		/// </summary>
		[Test]
		public void IsSetPixelFaster() {
			var fastBitmap = new FireBitmapFaster(SourceBitmap.Width, SourceBitmap.Height, SourceBitmap.PixelFormat);
			var normalBitmap = new Bitmap(SourceBitmap.Width, SourceBitmap.Height);
			long fastTicks = 0;
			long normalTicks = 0;

			for (int y = 0; y < SourceBitmap.Height; y++) {
				for (int x = 0; x < SourceBitmap.Width; x++) {
					var sourcePixel = SourceBitmap.GetPixel(x, y);
					
					fastTicks += Stopwatch.Time(() => fastBitmap.SetPixel(x, y, sourcePixel)).ElapsedTicks;
					normalTicks += Stopwatch.Time(() => normalBitmap.SetPixel(x, y, sourcePixel)).ElapsedTicks;

					Assert.AreEqual(sourcePixel, fastBitmap.GetPixel(x, y));
				}
			}

			Console.WriteLine($"FastBitmap ticks: {fastTicks} | FastBitmap milliseconds: {(float)fastTicks / 10000:0.00}\n" +
				$"Bitmap ticks: {normalTicks} | Bitmap milliseconds: {(float)normalTicks / 10000:0.00}\n" +
				$"FastBitmap is {(float)normalTicks / fastTicks:0.0}x faster than Bitmap");

			Assert.Less(fastTicks, normalTicks);
		}
	}
}