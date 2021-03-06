﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Huaban.UWP.Commands
{
	public class ToBoardDetailCommand : ICommand
	{
		public event EventHandler CanExecuteChanged;

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			App.AppContext.NavigationService.NavigateTo("BoardDetailPage", parameter);
		}
	}
}
