using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcsessLayer.Abstract
{
	public interface IRepository<T>
	{
		void Create(T entity);
		void Delete(int id);
		void Update(T entity);
		T GetById(int id);
		List<T> GetAll();



	}
}
