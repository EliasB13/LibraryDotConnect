using Devart.Data.SQLite;
using LibraryDotConnect.Data;
using LibraryDotConnect.Data.Entities;
using LibraryDotConnect.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryDotConnect.ConsoleMenu
{
	public class InsertMenu : Menu
	{
		private readonly Book book = new Book();
		private readonly BooksRepository booksRepository = new BooksRepository();

		public InsertMenu(string title) : base(title, new List<MenuItem>())
		{
		}

		public override void LaunchMenu()
		{
			Console.Clear();

			if (EnterBookTitle() == MenuExitFlag.ReturnToPreviousMenu)
				return;
			if (EnterBookYear() == MenuExitFlag.ReturnToPreviousMenu)
				return;
			if (EnterAuthor() == MenuExitFlag.ReturnToPreviousMenu)
				return;

			booksRepository.Create(book);
			PrintSuccess();
		}

		public MenuExitFlag EnterBookTitle()
		{
			Console.Clear();
			Console.WriteLine(Title + "\n");
			Console.WriteLine("Enter book title:");
			Console.WriteLine("\nType 'back' to return");

			string input = Console.ReadLine();
			if (input == "back")
				return MenuExitFlag.ReturnToPreviousMenu;

			book.Title = input;

			return MenuExitFlag.None;
		}

		public MenuExitFlag EnterBookYear()
		{
			Console.Clear();
			Console.WriteLine(Title + "\n");
			Console.WriteLine("Enter book year:");
			Console.WriteLine("\nType 'back' to return");

			string input = Console.ReadLine();
			if (input == "back")
				return MenuExitFlag.ReturnToPreviousMenu;
			else if (!short.TryParse(input, out short inputYear))
			{
				Console.Clear();

				PrintError("Wrong input year!");
				EnterBookYear();
			}
			else 
				book.Year = inputYear;

			return MenuExitFlag.None;
		}

		public MenuExitFlag EnterAuthor()
		{
			Console.Clear();
			Console.WriteLine(Title + "\n");
			Console.WriteLine("Enter number of author:");
			Console.WriteLine();

			var authors = booksRepository.GetAuthors().ToList();

			foreach (Author author in authors)
			{
				Console.WriteLine(authors.IndexOf(author) + ". " + author.FirstName + " " + author.LastName);
			}

			Console.WriteLine();
			Console.WriteLine("\nType 'back' to return");

			string input = Console.ReadLine();
			if (input == "back")
				return MenuExitFlag.ReturnToPreviousMenu;
			else if (!short.TryParse(input, out short authorIndex) || authorIndex < 0 || authorIndex >= authors.Count)
			{
				Console.Clear();

				PrintError("Wrong author number!");
				EnterAuthor();
			}
			else
			{
				book.Authors = new List<Author>();
				book.Authors.Add(authors[authorIndex]);
			}

			return MenuExitFlag.None;
		}
	}
}
