using LibraryDotConnect.ConsoleMenu;
using System.Collections.Generic;
using LibraryDotConnect.Data;

namespace LibraryDotConnect
{
	class Program
	{
		static void Main(string[] args)
		{
			var selectMenu = new SelectMenu("Select");
			var insertMenu = new InsertMenu("Insert");
			var deleteMenu = new DeleteMenu("Delete");
			var updateMenu = new UpdateMenu("Update");

			var mainMenuItems = new List<MenuItem>
			{
				new MenuItem("SELECT", selectMenu.LaunchMenu),
				new MenuItem("INSERT", insertMenu.LaunchMenu),
				new MenuItem("UPDATE", updateMenu.LaunchMenu),
				new MenuItem("DELETE", deleteMenu.LaunchMenu),
			};

			var mainMenu = new Menu("MainMenu", mainMenuItems);

			while(true)
			{
				mainMenu.LaunchMenu();
			}
		}
	}
}
