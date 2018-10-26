using System.Collections.Generic;
using TodoAPI.Models;

namespace TodoAPI.Repositories
{
    public interface IItemRepository
    {
        List<Item> GetAll();
        Item GetById(long id);
        Item Save(Item item);
        Item Update(Item item);
        bool Remove(long id);
    }
}