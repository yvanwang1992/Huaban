﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Huaban.UWP.Commands
{
	using Models;
	public class PinImageLoadCommand : ICommand
	{
		public event EventHandler CanExecuteChanged;

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			var pin = parameter as Pin;
			if (pin != null)
				pin.IsLoaded = true;
			
		}
	}
}
