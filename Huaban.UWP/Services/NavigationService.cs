﻿using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.ServiceLocation;
using System.Threading.Tasks;

namespace Huaban.UWP.Services
{
	using Huaban.UWP.Controls;
	using Base;
	public class NavigationService
	{
		public event EventHandler<BackRequestedEventArgs> BackEvent;
		private HBFrame HBFrame { get; set; }
		private Frame Frame { get; set; }
		private Context Context { get; set; }
		public NavigationService(Context context)
		{
			Context = context;
		}

		private void NavigationService_BackRequested(object sender, BackRequestedEventArgs e)
		{

			BackEvent?.Invoke(sender, e);
			bool handled = e.Handled;

			if (!e.Handled)
				this.BackRequested(ref handled);

			e.Handled = handled;
		}

		private bool b;
		private void BackRequested(ref bool handled)
		{
			if (this.CanGoBack)
			{
				handled = true;
				this.GoBack();
			}
			else if (!this.CanGoBack && !handled)
			{
				if (b)
				{
					App.Current.Exit();
				}
				else
				{
					b = true;
					handled = true;
					Task.Run(async () =>
					{
						Context.ShowTip("再按一次退出");
						await Task.Delay(1500);
						b = false;
					});
				}
			}
		}

		public void SetFrame(Frame MainFrame, HBFrame detailFrame)
		{
			Frame = MainFrame;
			HBFrame = detailFrame;
			SystemNavigationManager.GetForCurrentView().BackRequested += NavigationService_BackRequested;
		}
		public void MenuNavigateTo(string pageName, object parameter = null)
		{
			var type = GetPageType(pageName);
			Frame.Navigate(type, parameter);
			DisplayBackButton();
		}
		public void MenuNavigateTo(Type sourcePageType, object parameter = null)
		{
			Frame.Navigate(sourcePageType, parameter);
			DisplayBackButton();
		}
		public void NavigateTo(string pageName, object parameter = null)
		{
			HBFrame.Navigate(GetPageType(pageName), parameter);
			DisplayBackButton();
		}
		public void NavigateTo(Type sourcePageType, object parameter = null)
		{
			HBFrame.Navigate(sourcePageType, parameter);
			DisplayBackButton();
		}
		public void GoBack()
		{
			try
			{
				if (HBFrame.CanGoBack)
					HBFrame.GoBack();
				else if (Frame.CanGoBack)
					Frame.GoBack();

				DisplayBackButton();
			}
			catch (Exception ex)
			{
				string a = ex.Message;
			}

		}
		public bool CanGoBack
		{
			get
			{
				return HBFrame.CanGoBack || Frame.CanGoBack;
			}
		}
		private void DisplayBackButton()
		{
			if (CanGoBack)
				SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
			else
				SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
		}
		private Type GetPageType(string pageName)
		{
			var type = this.GetType();
			var assemblyQualifiedAppType = type.AssemblyQualifiedName;

			var pageNameWithParameter = assemblyQualifiedAppType.Replace(type.FullName, type.Namespace.Substring(0, type.Namespace.LastIndexOf('.')) + ".Views.{0}");

			var viewFullName = string.Format(pageNameWithParameter, pageName);
			var viewType = Type.GetType(viewFullName);

			if (viewType == null)
			{
				throw new ArgumentException(
					"Invalid page type",
					"pageToken");
			}

			return viewType;
		}

		private object GetViewModel(Type sourcePageType)
		{
			string pageName = sourcePageType.Name;
			var type = this.GetType();
			var assemblyQualifiedAppType = type.AssemblyQualifiedName;

			var pageNameWithParameter = assemblyQualifiedAppType.Replace(this.GetType().FullName, type.Namespace.Substring(0, type.Namespace.LastIndexOf('.')) + ".ViewModels.{0}ViewModel");

			var viewFullName = string.Format(pageNameWithParameter, pageName);
			var viewType = Type.GetType(viewFullName);

			if (viewType == null)
			{
				throw new ArgumentException(
					"Invalid viewmodel type",
					"pageToken");
			}
			return Services.ServiceLocator.Resolve(viewType);
		}
	}
}