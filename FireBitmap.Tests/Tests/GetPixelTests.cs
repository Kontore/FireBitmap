using System;
using System.Diagnostics;
using System.Drawing;
using NUnit.Framework;
using Kontore.Extensions.MiscellaneousXN;

namespace Kontore.FireBitmap.Tests {
	[TestFixtureSource(typeof(TestData), nameof(TestData.TestBitmaps))]
	public class GetPixelTests {
		private static Stopwatch Stopwatch { get; } = new Stopwatch();
		/// <summary>
		/// The source bitmap which will be used in the tests to verify that the <see cref="FireBitmap"/> works.
		/// </summary>
		private static Bitmap SourceBitmap { get; set; }

		public GetPixelTests(TestCaseData data) {
			SourceBitmap = (Bitmap)data.Arguments[0];
		}
		
		/// <summary>
		/// Tests the <see cref="FireBitmap.GetPixel(int, int)"/> method
		/// by using it to compare the pixels of the source bitmap and a copy of that bitmap
		/// generated with the <see cref="Bitmap.SetPixel(int, int, Color)"/> method.
		/// </summary>
		[Test]
		public void GetPixel() {
			var fastBitmap = new FireBitmapFaster(SourceBitmap.Width, SourceBitmap.Height, SourceBitmap.PixelFormat);

			for (int y = 0; y < SourceBitmap.Height; y++) {
				for (int x = 0; x < SourceBitmap.Width; x++) {
					var sourcePixel = SourceBitmap.GetPixel(x, y);

					fastBitmap.Bitmap.SetPixel(x, y, sourcePixel);

					var newPixel = fastBitmap.GetPixel(x, y);

					Assert.AreEqual(sourcePixel, newPixel);
				}
			}
		}

		/// <summary>
		/// TODO:
		/// This functions incorrectly. It seems to be influenced by other tests. Fix later.
		/// As of 3/2/22, if this test would work, it would pass.
		/// 
		/// Tests whether the<see cref="FireBitmap.GetPixel(int, int)"/> method is faster than the
		/// <see cref = "Bitmap.GetPixel(int, int)"/> method.
		/// </summary>
		//[Test]
		public void IsGetPixelFaster() {
			var fastBitmap = new FireBitmap(SourceBitmap.Width, SourceBitmap.Height, SourceBitmap.PixelFormat);
			long fastTicks = 0;
			long normalTicks = 0;

			for (int y = 0; y < SourceBitmap.Height; y++) {
				for (int x = 0; x < SourceBitmap.Width; x++) {
					Color sourcePixel = default;
					Color newPixel = default;

					normalTicks += Stopwatch.Time(() => sourcePixel = SourceBitmap.GetPixel(x, y)).ElapsedTicks;
					fastBitmap.Bitmap.SetPixel(x, y, sourcePixel);
					fastTicks += Stopwatch.Time(() => newPixel = fastBitmap.GetPixel(x, y)).ElapsedTicks;

					Assert.AreEqual(sourcePixel, newPixel);
				}
			}

			Console.WriteLine($"FastBitmap ticks: {fastTicks} | FastBitmap milliseconds: {(float)fastTicks / 10000:0.00}\n" +
				$"Bitmap ticks: {normalTicks} | Bitmap milliseconds: {(float)normalTicks / 10000:0.00}\n" +
				$"FastBitmap is {(float)normalTicks / fastTicks:0.0}x faster than Bitmap");

			Assert.Less(fastTicks, normalTicks);
		}
	}
}