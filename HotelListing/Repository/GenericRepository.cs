﻿using System.Linq.Expressions;
using HotelListing.Data;
using HotelListing.IRepository;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _db;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _db = context.Set<T>();
        }


        public async Task Add(T entity)
        {
            await _context.AddAsync(entity);
        }

        public void Update(T entity)
        {
            _db.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public async Task Delete(int id)
        {
            var entity = await _db.FindAsync(id);
            _context.Remove(entity);

        }

        public async Task<T> Get(Expression<Func<T, bool>> expression = null, List<string> includes = null)
        {
            IQueryable<T> query = _db;
            if (includes != null)
            {
                foreach (var property in includes)
                {
                    query.Include(property);
                }
            }

            return await query.AsNoTracking().FirstOrDefaultAsync(expression);
        }

        public async Task<T> GetAll(Expression<Func<T, bool>> expression = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, bool OrderByDescending = false, List<string> includes = null)
        {
            IQueryable<T> query = _db;
            if (includes != null)
            {
                foreach (var property in includes)
                {
                    query.Include(property);
                }
            }

            if (orderBy != null)
            {
                if (OrderByDescending = false)
                {
                    query = query.OrderBy(expression);
                }
                else
                {
                    query = query.OrderByDescending(expression);

                }
            }


            return await query.AsNoTracking().FirstOrDefaultAsync(expression);
        }

    }
}
