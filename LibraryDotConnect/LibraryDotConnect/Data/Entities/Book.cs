using System.Collections.Generic;

namespace LibraryDotConnect.Data.Entities
{
	public class Book
	{
		public int Id { get; set; }
		public short Year { get; set; }
		public string Title { get; set; }

		public List<Author> Authors { get; set; }
	}
}
