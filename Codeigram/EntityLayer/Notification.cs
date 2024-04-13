using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
	public class Notification
	{
		public int Id { get; set; }

		public int NotificationType { get; set; }

		public string PP { get; set; }

		public string Username { get; set; }

		public string Message { get; set; }

		public int Seen { get; set; }

		public int Status { get; set; }

		public string BuyerToken { get; set; }

		public DateTime Created_at { get; set; }

	}
}
