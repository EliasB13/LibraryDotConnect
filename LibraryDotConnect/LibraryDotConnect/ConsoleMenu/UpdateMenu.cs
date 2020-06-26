using Devart.Data.SQLite;
using LibraryDotConnect.Data;
using LibraryDotConnect.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryDotConnect.ConsoleMenu
{
	public class UpdateMenu : Menu
	{
		private static readonly BooksRepository booksRepository = new BooksRepository();
		private List<Book> Books { get; set; }
		private Book BookToUpdate = new Book();

		public UpdateMenu(string title) : base(title, new List<MenuItem>())
		{
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
				BookToUpdate = Books[choiceIndex];
				PrintUpdateMenu();
			}
		}

		public override void PrintBooks()
		{
			Console.Clear();
			Console.WriteLine(Title + "\n");
			Console.WriteLine("Select book to update: \n");

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

		public void PrintUpdateMenu()
		{
			Console.Clear();

			EnterBookTitle();
			EnterBookYear();
			EnterAuthor();

			booksRepository.Update(BookToUpdate);
			PrintSuccess();
		}

		public void EnterBookTitle()
		{
			Console.Clear();
			Console.WriteLine(Title + "\n");
			Console.WriteLine("Title: " + BookToUpdate.Title + "\n");
			Console.WriteLine("Enter new book title:");
			Console.WriteLine("\nType 'back' to return");

			string input = Console.ReadLine();
			if (input == "back")
				return;

			BookToUpdate.Title = input;
		}

		public void EnterBookYear()
		{
			Console.Clear();
			Console.WriteLine(Title + "\n");
			Console.WriteLine("Year: " + BookToUpdate.Year + "\n");
			Console.WriteLine("Enter new book year:");
			Console.WriteLine("\nType 'back' to return");

			string input = Console.ReadLine();
			if (input == "back")
				return;
			else if (!short.TryParse(input, out short inputYear))
			{
				Console.Clear();

				PrintError("Wrong input year!");
				EnterBookYear();
			}
			else
				BookToUpdate.Year = inputYear;
		}

		public void EnterAuthor()
		{
			Console.Clear();
			Console.WriteLine(Title + "\n");
			Console.WriteLine("Enter new number of author:");
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
				return;
			else if (!short.TryParse(input, out short authorIndex) || authorIndex < 0 || authorIndex >= authors.Count)
			{
				Console.Clear();

				PrintError("Wrong author number!");
				EnterAuthor();
			}
			else
			{
				BookToUpdate.Authors = new List<Author>();
				BookToUpdate.Authors.Add(authors[authorIndex]);
			}
		}
	}
}
