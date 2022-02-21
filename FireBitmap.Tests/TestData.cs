using System.IO;
using System.Collections.Generic;
using System.Drawing;
using NUnit.Framework;
using System;
using System.Linq;

namespace Kontore.FireBitmap.Tests {
	public class TestData {
		/// <summary>
		/// The current project folder in a local environment.
		/// <c>".."</c> actually refers to the <c>bin</c> folder, since that's where the executables are saved.
		/// </summary>
		public const string LOCAL_PROJECT_FOLDER = @"..\..\..";

		/// <summary>
		/// The GitHub workspace environment variable.
		/// If it's null, the <see cref="LOCAL_PROJECT_FOLDER"/> will be used.
		/// </summary>
		public static string GITHUB_WORKSPACE { get; } = Environment.GetEnvironmentVariable("GITHUB_WORKSPACE");

		/// <summary>
		/// The current project folder in the GitHub Actions environment.
		/// If <see cref="GITHUB_WORKSPACE"/> null, the <see cref="LOCAL_PROJECT_FOLDER"/> will be used.
		/// </summary>
		public static string GITHUB_ACTIONS_PROJECT_FOLDER { get; } = Path.Combine(GITHUB_WORKSPACE ?? "", "FireBitmap.Tests");
		
		/// <summary>
		/// The bitmap objects created from the images placed in the <c>PROJECT_FOLDER\TestBitmaps</c> folder.
		/// </summary>
		public static List<TestCaseData> TestBitmaps { get; } = new List<TestCaseData>();

		/// <summary>
		/// The supported image extensions.
		/// </summary>
		private static string[] ImageExtensions { get; } = new[] { ".png", ".jpg", ".jpeg", ".jpe", ".jif", ".jfif", ".gif", ".bmp", ".tiff", ".tif" };

		/// <summary>
		/// Fills all of the <see cref="TestData"/> fields and properties with values.
		/// </summary>
		static TestData() {
			string testBitmapsFolder = GITHUB_WORKSPACE != null
				? Path.Combine(GITHUB_ACTIONS_PROJECT_FOLDER, "TestBitmaps")
				: Path.Combine(LOCAL_PROJECT_FOLDER, "TestBitmaps");

			var bitmapPaths = Directory.GetFiles(testBitmapsFolder, "*.*")
				.Where(file => ImageExtensions.Any(ex => file.EndsWith(ex, StringComparison.OrdinalIgnoreCase)));

			foreach (var bitmapPath in bitmapPaths) {
				var testCase = new TestCaseData(new Bitmap(Image.FromFile(bitmapPath)));
				testCase.SetArgDisplayNames(new FileInfo(bitmapPath).Length / 1024 + "KB");
				TestBitmaps.Add(testCase);
			}
		}
	}
}
