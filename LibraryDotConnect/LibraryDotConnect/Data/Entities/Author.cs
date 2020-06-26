using System.Collections.Generic;

namespace LibraryDotConnect.Data.Entities
{
	public class Author
	{
		public Author(int id, string firstName, string lastName)
		{
			Id = id;
			FirstName = firstName;
			LastName = lastName;
		}

		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }

		public List<Book> Books { get; set; }
	}
}
