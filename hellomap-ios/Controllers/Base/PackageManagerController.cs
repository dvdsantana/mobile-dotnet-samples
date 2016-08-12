﻿
using System;
using System.Collections.Generic;
using System.IO;
using Carto.DataSources;
using Carto.PackageManager;
using UIKit;

namespace CartoMobileSample
{
	public class PackageManagerController : MapBaseController
	{
		public override string Name { get { return "Package Manager"; } }

		public override string Description
		{
			get
			{
				return "A sample demonstrating how to use offline package manager of the Carto Mobile SDK. " +
						 "The sample downloads the latest package list from Carto online service, " +
						 "displays this list and allows user to manage offline packages";
			}
		}

		public static PackageManagerTileDataSource DataSource;

		CartoPackageManager packageManager;

		string currentFolder = ""; // Current 'folder' of the package, for example "Asia/"
		string language = "en"; // Language for the package names. Most major languages are supported

		readonly List<Package> packageList = new List<Package>();

		PackageListener PackageUpdateListener = new PackageListener();

		public List<Package> Packages { get { return packageManager.GetPackages(language, currentFolder); } }

		public PackageManagerController(string folder = null)
		{
			if (folder != null)
			{
				currentFolder = folder;
			}	
		}

		PackageManagerListView ContentView;

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			// Create PackageManager instance for dealing with offline packages
			string folder = Utils.GetDocumentDirectory("mappackages");

			if (!Directory.Exists(folder))
			{
				Directory.CreateDirectory(folder);
				Console.WriteLine("Directory: Does not exist... Creating");
			}
			else
			{
				Console.WriteLine("Directory: Exists");
			}

			packageManager = new CartoPackageManager("nutiteq.osm", folder);

			packageManager.PackageManagerListener = PackageUpdateListener;

			packageManager.StartPackageListDownload();

			ContentView = new PackageManagerListView();
			View = ContentView;
			ContentView.AddRows(packageList);
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			// Always Attach handlers OnResume to avoid memory leaks and objects with multple handlers
			PackageUpdateListener.OnPackageListUpdate += UpdatePackages;
			PackageUpdateListener.OnPackageListFail += UpdatePackages;

			PackageUpdateListener.OnPackageCancel += UpdatePackage;
			PackageUpdateListener.OnPackageUpdate += UpdatePackage;
			PackageUpdateListener.OnPackageStatusChange += UpdatePackage;
			PackageUpdateListener.OnPackageFail += UpdatePackage;

			packageManager.Start();
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);

			// Always detach handlers OnPause to avoid memory leaks and objects with multple handlers
			PackageUpdateListener.OnPackageListUpdate -= UpdatePackages;
			PackageUpdateListener.OnPackageListFail -= UpdatePackages;

			PackageUpdateListener.OnPackageCancel -= UpdatePackage;
			PackageUpdateListener.OnPackageUpdate -= UpdatePackage;
			PackageUpdateListener.OnPackageStatusChange -= UpdatePackage;
			PackageUpdateListener.OnPackageFail -= UpdatePackage;

			packageManager.Stop(true);
		}

		#region Package update

		void UpdatePackages(object sender, EventArgs e)
		{
			UpdatePackages();
		}

		void UpdatePackages()
		{
			InvokeOnMainThread(delegate
			{
				packageList.Clear();
				packageList.AddRange(Packages);
				ContentView.ListSource.Items = packageList;
				ContentView.ReloadData();
			});
		}

		void UpdatePackage(object sender, PackageEventArgs e)
		{
			UpdatePackage(e.Id);
		}

		void UpdatePackage(object sender, PackageStatusEventArgs e)
		{
			UpdatePackage(e.Id);
		}

		void UpdatePackage(object sender, PackageFailedEventArgs e)
		{
			//Alert("Error: " + e.ErrorType);
			UpdatePackage(e.Id);
		}

		void UpdatePackage(string id)
		{
			InvokeOnMainThread(delegate
			{
				// Try to find the package that needs to be updated
				for (int i = 0; i < packageList.Count; i++)
				{
					Package pkg = packageList[i];

					if (id.Equals(pkg.Id))
					{
						PackageStatus status = packageManager.GetLocalPackageStatus(id, -1);
						pkg.UpdateStatus(status);

						packageList[i] = pkg;
						ContentView.ReloadData();
						break;
					}
				}
			});
		}

		#endregion

		#region Row Internal Button Click handling

		public void OnAdapterActionButtonClick(object sender, EventArgs e)
		{
			PackageManagerButton button = (PackageManagerButton)sender;
			System.Console.WriteLine("Clicked: " + button.PackageId + " - " + button.PackageName + " - " + button.Type);

			if (button.Type == PackageManagerButtonType.CancelPackageTasks)
			{
				packageManager.CancelPackageTasks(button.PackageId);
			}
			else if (button.Type == PackageManagerButtonType.SetPackagePriority)
			{
				packageManager.SetPackagePriority(button.PackageId, button.PriorityIndex);
			}
			else if (button.Type == PackageManagerButtonType.StartPackageDownload)
			{
				packageManager.StartPackageDownload(button.PackageId);

			}
			else if (button.Type == PackageManagerButtonType.StartRemovePackage)
			{
				packageManager.StartPackageRemove(button.PackageId);
			}
			else if (button.Type == PackageManagerButtonType.UpdatePackages)
			{
				currentFolder = currentFolder + button.PackageName + "/";
				UpdatePackages();
			}
		}

		#endregion
	}
}