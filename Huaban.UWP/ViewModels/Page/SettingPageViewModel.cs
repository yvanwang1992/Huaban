﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml;
namespace Huaban.UWP.ViewModels
{
	using Base;
	using Controls;
	using Commands;
	using Models;
	public class SettingPageViewModel : HBViewModel
	{
		public SettingPageViewModel(Context context)
			: base(context)
		{
			Title = "设置";
			Setting = Setting.Current;

			Setting.PropertyChanged += (s, e) =>
			{
				NotifyPropertyChanged(e.PropertyName);
			};
		}

		#region Properties
		private string _CacheSize;
		public string CacheSize
		{
			get { return _CacheSize; }
			set { SetValue(ref _CacheSize, value); }
		}

		public Setting Setting { private set; get; }

		#endregion

		#region Commands

		private DelegateCommand _ReViewCommand;
		public DelegateCommand ReViewCommand
		{
			get
			{
				return _ReViewCommand ?? (_ReViewCommand = new DelegateCommand(
					async o =>
					{
						await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-windows-store://review/?ProductId=9NBLGGH5FWXP"));
					},
					o => !IsLoading)
				);
			}
		}

		private DelegateCommand _ClearCacheCommand;
		public DelegateCommand ClearCacheCommand
		{
			get
			{
				return _ClearCacheCommand ?? (_ClearCacheCommand = new DelegateCommand(
					o =>
					{
						//await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-windows-store://review/?ProductId=9NBLGGH5X991"));
					},
					o => !IsLoading)
				);
			}
		}

		#endregion

		#region Methods
		public async override void OnNavigatedTo(HBNavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			if (e.NavigationMode == NavigationMode.New)
			{
				CacheSize = GetFormatSize(await StorageHelper.GetCacheFolderSize());
			}
		}
		private string GetFormatSize(double size)
		{
			if (size < 1024)
			{
				return size + "byte";
			}
			else if (size < 1024 * 1024)
			{
				return Math.Round(size / 1024, 2) + "KB";
			}
			else if (size < 1024 * 1024 * 1024)
			{
				return Math.Round(size / 1024 / 1024, 2) + "MB";
			}
			else
			{
				return Math.Round(size / 1024 / 1024 / 2014, 2) + "GB";
			}
		}

		#endregion
	}
}
