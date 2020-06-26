using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryDotConnect.ConsoleMenu
{
	public class MenuItem
	{
		public string Text { get; set; }

		public Action Action { get; set; }

		public MenuItem(string text, Action action)
		{
			Text = text;
			Action = action;
		}
	}
}
