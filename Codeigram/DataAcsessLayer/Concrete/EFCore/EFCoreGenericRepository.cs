using DataAcsessLayer.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcsessLayer.Concrete.EFCore
{
	public class EFCoreGenericRepository<TEntity, TContext> : IRepository<TEntity>
	where TEntity : class
	where TContext : DbContext, new()

	{
		public void Create(TEntity entity)
		{
			using (var context = new TContext())
			{
				context.Set<TEntity>().Add(entity);
				context.SaveChanges();
			}
		}

		public void Delete(int id)
		{
			using (var context = new TContext())
			{
				var entity = context.Set<TEntity>().Find(id);
				context.Set<TEntity>().Remove(entity);
				context.SaveChanges();
			}
		}

		public List<TEntity> GetAll()
		{
			using (var context = new TContext())
			{
				return context.Set<TEntity>().ToList();
			}
		}

		public TEntity GetById(int id)
		{
			using (var context = new TContext())
			{
				return context.Set<TEntity>().Find(id);
			}
		}

		public virtual void Update(TEntity entity)
		{
			using (var context = new TContext())
			{
				context.Set<TEntity>().Update(entity);
				context.SaveChanges();
			}
		}
	}

}
