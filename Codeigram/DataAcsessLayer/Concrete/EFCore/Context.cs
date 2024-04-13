using EntityLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcsessLayer.Concrete.EFCore
{
	public class Context:DbContext
	{
	
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer("Server=LAPTOP-LM2N83TK;Database=CodeigramDB;uid=sa;pwd=123;TrustServerCertificate=True;");
		}


		public DbSet<Follow> Follows { get; set; }
		public DbSet<ChatMessages> ChatMessages { get; set; }
		public DbSet<ChatRoom> ChatRooms { get; set; }
		public DbSet<Notification> Notifications { get; set; }
		public DbSet<PostLike> PostLikes { get; set; }
		public DbSet<Post> Posts { get; set; }
		public DbSet<Yorum> Yorums { get; set; }
	}
}
