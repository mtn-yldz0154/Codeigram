using DataAcsessLayer.Abstract;
using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcsessLayer.Concrete.EFCore
{
	public class EFCoreFollowRepository:EFCoreGenericRepository<Follow,Context>, IFollowRepository
	{
	}
}
