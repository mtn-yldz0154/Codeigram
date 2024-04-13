using EntityLayer;
using Microsoft.AspNetCore.Identity;

namespace BitirmeProjesiUI.Identity
{
	public class Follows
	{

			public string UserId { get; set; }
			public string UserFoto { get; set; }
			public List<Follow> Ffollows { get; set; }
			public int FfollowsCount { get; set; }
			public List<Follow> Ffollow { get; set; }
			public int FfollowCount { get; set; }
	}
}
