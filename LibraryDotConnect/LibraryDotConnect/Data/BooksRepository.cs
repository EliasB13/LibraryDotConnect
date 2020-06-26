using Devart.Data.SQLite;
using LibraryDotConnect.ConsoleMenu;
using LibraryDotConnect.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryDotConnect.Data
{
	public class BooksRepository
	{
		private readonly string ConnectionString = Properties.Settings.Default.ConnectionString;

		public BooksRepository()
		{
			EnableForeignKeys();
		}

		public IEnumerable<Book> GetBooksWithAuthors()
		{
			var booksList = new List<Book>();

			string queryString =
			  "select book.book_id, book.title, book.year, author.author_id, author.first_name, author.last_name from book " +
			  "left join book_author on book.book_id = book_author.book_id " +
			  "inner join author on book_author.author_id = author.author_id";

			using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
			{
				connection.Open();

				SQLiteCommand command = connection.CreateCommand();
				command.CommandText = queryString;

				using (SQLiteDataReader reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						int bookId = reader.GetInt32(0);

						if (booksList.Any(b => b.Id == bookId))
						{
							var book = booksList.Single(b => b.Id == bookId);
							book.Authors.Add(new Author(reader.GetInt32(3), reader.GetString(4), reader.GetString(5)));
						}
						else
						{
							var bookAuthors = new List<Author>();
							bookAuthors.Add(new Author(reader.GetInt32(3), reader.GetString(4), reader.GetString(5)));

							booksList.Add(new Book()
							{
								Id = bookId,
								Title = reader.GetString(1),
								Year = reader.GetInt16(2),
								Authors = bookAuthors
							});
						}
					}
				}

				connection.Close();

				return booksList;
			}
		}

		public IEnumerable<Author> GetAuthors()
		{
			var authorsList = new List<Author>();

			string queryString = "select * from author";

			using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
			{
				connection.Open();

				SQLiteCommand command = connection.CreateCommand();
				command.CommandText = queryString;

				using (SQLiteDataReader reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						authorsList.Add(
							new Author(
								reader.GetInt32(0),
								reader.GetString(1),
								reader.GetString(2)
							));
					}
				}

				connection.Close();

				return authorsList;
			}
		}

		public void Update(Book book)
		{
			string queryString = "update book set title=@title, year=@year where book_id=@id";
			using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
			{
				using (SQLiteCommand cmd = new SQLiteCommand(queryString, connection))
				{
					cmd.Parameters.Add("@title", SQLiteType.Text).Value = book.Title;
					cmd.Parameters.Add("@year", SQLiteType.Int16).Value = book.Year;
					cmd.Parameters.Add("@id", SQLiteType.Int32).Value = book.Id;

					connection.Open();
					cmd.ExecuteNonQuery();
					connection.Close();
				}
			}
		}

		public void Create(Book book)
		{
			string insertBookQuery = "insert into book (title, year) values (@title, @year); select last_insert_rowid()";
			string insertBookAuthorQuery = "insert into book_author (book_id, author_id) values (@bookId, @authorId)";
			int bookId;


			using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
			{
				connection.Open();
				SQLiteTransaction transaction = connection.BeginTransaction();

				try
				{
					using (SQLiteCommand cmd = new SQLiteCommand(insertBookQuery, connection))
					{
						cmd.Parameters.Add("@title", SQLiteType.Text).Value = book.Title;
						cmd.Parameters.Add("@year", SQLiteType.Int16).Value = book.Year;

						bookId = Convert.ToInt32(cmd.ExecuteScalar());
					}

					using (SQLiteCommand cmd = new SQLiteCommand(insertBookAuthorQuery, connection))
					{
						cmd.Parameters.Add("@bookId", SQLiteType.Int32).Value = bookId;
						cmd.Parameters.Add("@authorId", SQLiteType.Int32).Value = book.Authors.First().Id;

						cmd.ExecuteNonQuery();
					}

					transaction.Commit();
				}
				catch (SQLiteException ex)
				{
					transaction.Rollback();

					Menu.PrintError(ex.Message);
				}

				connection.Close();
			}
		}

		public void Delete(int bookId)
		{
			string queryString = "delete from book where book_id=@id";
			using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
			{
				using (SQLiteCommand cmd = new SQLiteCommand(queryString, connection))
				{
					cmd.Parameters.Add("@id", SQLiteType.Int32).Value = bookId;

					connection.Open();
					cmd.ExecuteNonQuery();
					connection.Close();
				}
			}
		}

		private void EnableForeignKeys()
		{
			string queryString = "PRAGMA foreign_keys = ON";
			using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
			{
				using (SQLiteCommand cmd = new SQLiteCommand(queryString, connection))
				{
					connection.Open();
					cmd.ExecuteNonQuery();
					connection.Close();
				}
			}
		}
	}
}
