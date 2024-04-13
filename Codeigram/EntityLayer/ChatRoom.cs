using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
	public class ChatRoom
	{
		public int Id { get; set; }

		public  string UserToken { get; set; }

		public string ToToken { get; set; }

		public string LastMessage { get; set; }

		public DateTime LastTimeMessage { get; set; }

		public int BlockUserToken { get; set; }
		public int BlocToken { get; set; }

		public int Status { get; set; }

		public DateTime  Created_at { get; set; }


	}
}
