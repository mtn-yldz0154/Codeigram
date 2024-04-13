using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
	public class Post
	{
		public int Id { get; set; }
		public string UserName { get; set; }
		public string UserId { get; set; }
		public int CountLike { get; set; }
		public int CountDislike { get; set; }
		public int CountSeen { get; set; }
		public int CountComment { get; set; }
		public DateTime Created_at { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public string ImageUrl { get; set; }
		public string Pp { get; set; }
		public List<Comments> Comments  { get; set; }
	}
}
