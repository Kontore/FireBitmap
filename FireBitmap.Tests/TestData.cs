using System.IO;
using System.Collections.Generic;
using System.Drawing;
using NUnit.Framework;

namespace Kontore.FireBitmap.Tests {
	public class TestData {
		/// <summary>
		/// The current project folder.
		/// <c>".."</c> actually refers to the <c>bin</c> folder, since that's where the executables are saved.
		/// </summary>
		public const string PROJECT_FOLDER = @"..\..\..";
		/// <summary>
		/// The bitmap objects created from the images placed in the <c>PROJECT_FOLDER\TestBitmaps</c> folder.
		/// </summary>
		public static List<TestCaseData> TestBitmaps { get; } = new List<TestCaseData>();

		/// <summary>
		/// Fills all of the <see cref="TestData"/> fields and properties with values.
		/// </summary>
		static TestData() {
			var bitmapPaths = Directory.GetFiles($@"{PROJECT_FOLDER}\TestBitmaps", "*.png");

			foreach (var bitmapPath in bitmapPaths) {
				var testCase = new TestCaseData(new Bitmap(Image.FromFile(bitmapPath)));
				testCase.SetArgDisplayNames(new FileInfo(bitmapPath).Length / 1024 + "KB");
				TestBitmaps.Add(testCase);
			}
		}
	}
}
