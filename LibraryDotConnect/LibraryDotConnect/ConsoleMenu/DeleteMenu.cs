using Devart.Data.SQLite;
using LibraryDotConnect.Data;
using LibraryDotConnect.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryDotConnect.ConsoleMenu
{
	public class DeleteMenu : Menu
	{
		private static readonly BooksRepository booksRepository = new BooksRepository();
		private List<Book> Books { get; set; }

		public DeleteMenu(string title) : base(title, new List<MenuItem>())
		{
		}

		public override void PrintBooks()
		{
			Console.Clear();
			Console.WriteLine(Title + "\n");

			foreach (Book book in Books)
			{
				Console.Write(string.Format("{0,-25}", Books.IndexOf(book) + ". " + book.Title));
				Console.Write(string.Format("{0,-10}", book.Year));
				Console.Write("Authors: ");

				foreach (Author author in book.Authors)
				{
					Console.Write(author.FirstName + " " + author.LastName + ", ");
				}

				Console.WriteLine("\n");
			}

			Console.WriteLine("\nType 'back' to return");
		}

		public override void LaunchMenu()
		{
			Books = booksRepository.GetBooksWithAuthors().ToList();

			PrintBooks();

			string choice = Console.ReadLine();

			if (choice == "back")
				return;

			if (!int.TryParse(choice, out int choiceIndex) || choiceIndex < 0 || choiceIndex >= Books.Count)
			{
				Console.Clear();

				PrintError("Wrong input number!");
				LaunchMenu();
			}
			else
			{
				try
				{
					booksRepository.Delete(Books[choiceIndex].Id);
					PrintSuccess();
				}
				catch (SQLiteException ex)
				{
					PrintError(ex.Message);
				}
			}
		}
	}
}
