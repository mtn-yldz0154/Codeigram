using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
	public class Follow
	{
		public int Id { get; set; }

		public string SenderToken { get; set; }
		public string BuyerToken { get; set; }

		public string SenderName { get; set; }
		public string BuyerName { get; set; }

		public int Status { get; set; }

		public DateTime Created_At { get; set; }
	}
}
