using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryDotConnect.ConsoleMenu
{
	public class Menu
	{
		public List<MenuItem> MenuItems { get; set; } = new List<MenuItem>();

		public string Title { get; set; }

		public Menu(string title, IEnumerable<MenuItem> menuItems)
		{
			Title = title;

			MenuItems.AddRange(menuItems);
		}

		public virtual void PrintBooks()
		{
			Console.Clear();
			Console.WriteLine(Title + "\n");

			foreach (MenuItem item in MenuItems)
			{
				Console.WriteLine(MenuItems.IndexOf(item) + ". " + item.Text);
			}

			Console.WriteLine("\nType 'off' to exit");
		}


		public static void PrintError(string error)
		{
			Console.Clear();
			Console.WriteLine($"ERROR | {error}");
			Console.WriteLine("\nPress any key to continue...");
			Console.ReadLine();
		}

		public static void PrintSuccess()
		{
			Console.Clear();
			Console.WriteLine("Success!");
			Console.WriteLine("\nPress any key to continue...");
			Console.ReadLine();
		}

		public virtual void LaunchMenu()
		{
			PrintBooks();

			string choice = Console.ReadLine();

			if (choice == "off")
				Environment.Exit(1);
				
			if (!int.TryParse(choice, out int choiceIndex) || choiceIndex < 0 || choiceIndex >= MenuItems.Count)
			{
				Console.Clear();

				PrintError("Wrong input number!");
				LaunchMenu();
			} 
			else
			{
				var menuItemSelected = MenuItems[choiceIndex];
				menuItemSelected.Action();
			}
		}
	}
}
