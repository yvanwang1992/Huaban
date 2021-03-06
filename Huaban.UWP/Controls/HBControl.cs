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
	using Huaban.UWP.Base;
	using Windows.Foundation;

	public class HBControl : UserControl
	{
		public HBControl()
		{
			ViewModel = (ViewModelBase)ControlHelper.GetViewModel(this.GetType());
			this.Loaded += (s, e) =>
			{
				if (!ViewModel.IsInited)
					ViewModel.Inited();
			};
		}
		public HBFrame Frame { get; internal set; }
		public ViewModelBase ViewModel
		{
			private set
			{
				this.DataContext = value;
			}
			get
			{
				return this.DataContext as ViewModelBase;
			}
		}

		public virtual void OnNavigatingFrom(HBNavigatingCancelEventArgs e)
		{
			ViewModel.OnNavigatingFrom(e);
		}
		public virtual void OnNavigatedFrom(HBNavigationEventArgs e)
		{
			ViewModel.OnNavigatedFrom(e);
		}
		public virtual void OnNavigatedTo(HBNavigationEventArgs e)
		{
			ViewModel.OnNavigatedTo(e);
		}
		protected override Size ArrangeOverride(Size finalSize)
		{
			ViewModel?.ArrangeOverride(finalSize);
			return base.ArrangeOverride(finalSize);
		}
	}
}
