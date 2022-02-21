using System.IO;
using System.Collections.Generic;
using System.Drawing;
using NUnit.Framework;
using System;

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
		public static string GITHUB_WORKSPACE = Environment.GetEnvironmentVariable("GITHUB_WORKSPACE");

		/// <summary>
		/// The current project folder in the GitHub Actions environment.
		/// If <see cref="GITHUB_WORKSPACE"/> null, the <see cref="LOCAL_PROJECT_FOLDER"/> will be used.
		/// </summary>
		public static string GITHUB_ACTIONS_PROJECT_FOLDER = Path.Combine(GITHUB_WORKSPACE ?? "", "FireBitmap.Tests");
		
		/// <summary>
		/// The bitmap objects created from the images placed in the <c>PROJECT_FOLDER\TestBitmaps</c> folder.
		/// </summary>
		public static List<TestCaseData> TestBitmaps { get; } = new List<TestCaseData>();

		/// <summary>
		/// Fills all of the <see cref="TestData"/> fields and properties with values.
		/// </summary>
		static TestData() {
			if (GITHUB_WORKSPACE != null) {
				var bitmapPaths = Directory.GetFiles(Path.Combine(GITHUB_ACTIONS_PROJECT_FOLDER, "TestBitmaps"), "*.png");

				foreach (var bitmapPath in bitmapPaths) {
					var testCase = new TestCaseData(new Bitmap(Image.FromFile(bitmapPath)));
					testCase.SetArgDisplayNames(new FileInfo(bitmapPath).Length / 1024 + "KB");
					TestBitmaps.Add(testCase);
				}
			} else {
				var bitmapPaths = Directory.GetFiles(Path.Combine(LOCAL_PROJECT_FOLDER, "TestBitmaps"), "*.png");

				foreach (var bitmapPath in bitmapPaths) {
					var testCase = new TestCaseData(new Bitmap(Image.FromFile(bitmapPath)));
					testCase.SetArgDisplayNames(new FileInfo(bitmapPath).Length / 1024 + "KB");
					TestBitmaps.Add(testCase);
				}
			}
		}
	}
}
