using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
	public class ChatMessages
	{
		public int Id { get; set; }

		public int RoomId { get; set; }

		public string SenderToken { get; set; }

		public string Messages { get; set; }

		public int Seen { get; set; }

		public int DeleteUserToken { get; set; }
		public int DeleteToken { get; set; }

		public int Status { get; set; }

		public DateTime Created_at { get; set; }
	}
}
