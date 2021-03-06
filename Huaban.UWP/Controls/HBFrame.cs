﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Automation;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml;

namespace Huaban.UWP.Controls
{
	public delegate void HBNavigatedEventHandler(object sender, HBNavigationEventArgs args);
	public delegate void HBNavigatingCancelEventHandlerr(object sender, HBNavigationEventArgs args);
	public delegate void HBNavigationFailedEventHandlerr(object sender, HBNavigationEventArgs args);
	public delegate void HBNavigationStoppedEventHandler(object sender, HBNavigationEventArgs args);
	public class HBFrame : ContentControl
	{
		public HBFrame()
		{
			this.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch;
			this.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Stretch;
			this.VerticalContentAlignment = Windows.UI.Xaml.VerticalAlignment.Stretch;
			this.HorizontalContentAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch;
		}
		public event HBNavigatedEventHandler Navigated;
		public event NavigatingCancelEventHandler Navigating;
		public event NavigationFailedEventHandler NavigationFailed;
		public event NavigationStoppedEventHandler NavigationStopped;
		public bool CanGoBack
		{
			get
			{
				return PageIndex >= 0;
			}
		}
		public Type CurrentSourcePageType { get; }
		public List<HBControl> PageStack { get; private set; } = new List<HBControl>();
		private int PageIndex { get; set; } = -1;
		private HBControl CurrentPage { set; get; }

		private Grid FrameGrid
		{
			get
			{
				if (this.Content == null)
				{
					var grid = new Grid();
					grid.Background = this.Background;
					Content = grid;
				}


				return (Grid)Content;
			}
		}

		public bool Navigate(Type sourcePageType, object parameter = null)
		{
			this.Visibility = Visibility.Visible;
			var page = CreatePage(sourcePageType);
			var args = new HBNavigationEventArgs()
			{
				SourcePageType = sourcePageType,
				Content = page,
				NavigationMode = NavigationMode.New,
				Parameter = parameter
			};


			page.OnNavigatedTo(args);

			Navigated?.Invoke(this, args);
			page.RenderTransform = new CompositeTransform();
			page.ManipulationMode = ManipulationModes.TranslateX;
			page.ManipulationDelta += HBPage_ManipulationDelta;
			page.ManipulationCompleted += Page_ManipulationCompleted;
			return true;
		}

		private void Page_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
		{
			var page = sender as HBControl;
			if (page == null)
				return;

			CompositeTransform transform = page.RenderTransform as CompositeTransform;
			if (transform.TranslateX > page.ActualWidth * 0.5)
			{
				if (CanGoBack)
					GoBack();
				else
					transform.TranslateX = 0;
			}
			else {
				transform.TranslateX = 0;
			}
		}

		protected void HBPage_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
		{
			CompositeTransform transform = ((HBControl)sender).RenderTransform as CompositeTransform;

			//移动操作
			transform.TranslateX += e.Delta.Translation.X * 1.3;
			//transform.TranslateY += e.Delta.Translation.Y;

			e.Handled = true;
		}
		public void GoBack()
		{
			if (!CanGoBack)
				return;
			var page = PageStack[PageIndex];
			var args = new HBNavigatingCancelEventArgs();
			page.OnNavigatingFrom(args);

			page.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
			PageStack.Remove(page);
			FrameGrid.Children.Remove(page);

			page.OnNavigatedFrom(new HBNavigationEventArgs());
			GC.Collect();
			GC.SuppressFinalize(page);
			GC.ReRegisterForFinalize(page);
			page.Content = null;
			page = null;
			PageIndex--;
			if (PageIndex >= 0)
			{
				page = PageStack[PageIndex];
				//page.OnNavigatedTo(new HBNavigationEventArgs() { NavigationMode = NavigationMode.Back });
			}
			else {
				this.Visibility = Visibility.Collapsed;
			}
		}

		private HBControl CreatePage(Type type)
		{
			if (PageIndex <= PageStack.Count - 1)
			{
				for (int i = PageStack.Count - 1; i > PageIndex; i--)
				{
					var _page = PageStack[i];
					PageStack.Remove(_page);
					FrameGrid.Children.Remove(_page);
				}
			}
			var page = Activator.CreateInstance(type) as HBControl;

			FrameGrid.Children.Add(page);
			PageStack.Add(page);
			PageIndex++;
			return page;
		}

	}
}
