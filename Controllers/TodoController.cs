using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TodoAPI.Models;
using TodoAPI.Repositories;

namespace TodoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly IItemRepository _repository;

        public TodoController(IItemRepository repository){
            this._repository = repository;
        }
        // GET api/todo
        [HttpGet]
        public ActionResult<List<Item>> Get()
        {
            return _repository.GetAll();
        }

        // GET api/todo/5
        [HttpGet("{id}")]
        public ActionResult<Item> Get(long id)
        {
            var item = _repository.GetById(id);
            if(item != null)
                return item;
            
            return NotFound();
        }

        // POST api/todo
        [HttpPost]
        public ActionResult<Item> Post([FromBody] Item value)
        {
            Console.WriteLine(value?.Name);

            var item = _repository.Save(value);
            if(item != null)
                return item;
            
            return UnprocessableEntity();
        }

        // PUT api/todo/5
        [HttpPut("{id}")]
        public ActionResult<Item> Put(int id, [FromBody] Item value)
        {
            var item = _repository.Update(value);
            if(item != null)
                return item;
            
            return UnprocessableEntity();
        }

        // DELETE api/todo/5
        [HttpDelete("{id}")]
        public ActionResult Delete(long id)
        {
            if(_repository.Remove(id))
                return Ok(new { Description = "Item removed" });

            return UnprocessableEntity();
        }
    }
}
