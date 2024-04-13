using BuisnessLayer.Abstract;
using DataAcsessLayer.Abstract;
using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLayer.Concrete
{
	public class FollowManager : IFollowService
	{
		private IFollowRepository _followRepository;

		public FollowManager(IFollowRepository followRepository)
		{
			_followRepository= followRepository;
		}
		public void addFollow(Follow follow)
		{
			_followRepository.Create(follow);
		}
	}
}
