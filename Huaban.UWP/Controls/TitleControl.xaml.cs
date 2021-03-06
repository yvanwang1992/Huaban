﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Huaban.UWP.Controls
{
	public sealed partial class TitleControl : UserControl
	{
		public TitleControl()
		{

			this.InitializeComponent();
		}
		public new UIElement Content
		{
			get { return (UIElement)GetValue(ContentProperty); }
			set { SetValue(ContentProperty, value); }
		}

		public static new readonly DependencyProperty ContentProperty =
			DependencyProperty.Register("Content", typeof(UIElement), typeof(TitleControl), new PropertyMetadata(null, OnContentChanged));

		private static void OnContentChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			var control = (TitleControl)sender;
			control.contentControl.Content = e.NewValue;
		}
	}
}
