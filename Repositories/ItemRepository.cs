using System.Collections.Generic;
using TodoAPI.Data;
using TodoAPI.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;

namespace TodoAPI.Repositories
{
    public class ItemRepository : IItemRepository, IDisposable
    {
        private readonly TodoContext _context;
        public ItemRepository(TodoContext context)
        {
            this._context = context;
        }

        public List<Item> GetAll()
        {
            return _context.Items.ToList();
        }

        public Item GetById(long id)
        {
            return _context.Items.Find(id);
        }

        public bool Remove(long id)
        {
            var item = _context.Items.Find(id);
            if(item != null)
            {
                _context.Items.Remove(item);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public Item Save(Item item)
        {
            _context.Items.Add(item);
            _context.SaveChanges();

            return item;
        }

        public Item Update(Item item)
        {
            var entity = _context.Items.Attach(item);
            entity.State = EntityState.Modified;
            _context.SaveChanges();

            return item;
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}