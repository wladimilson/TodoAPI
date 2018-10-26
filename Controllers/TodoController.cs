using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoAPI.Models;
using TodoAPI.Repositories;

namespace TodoAPI.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly IItemRepository _repository;

        public TodoController(IItemRepository repository){
            this._repository = repository;
        }

        // GET api/todo
        /// <summary>
        /// Lista os itens da To-do list.
        /// </summary>
        /// <returns>Os itens da To-do list</returns>
        /// <response code="200">Returna os itens da To-do list cadastrados</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public ActionResult<List<Item>> Get()
        {
            return _repository.GetAll();
        }

        // GET api/todo/5
        /// <summary>
        /// Obtém um item da To-do list.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Um item da To-do list</returns>
        /// <response code="201">Returna o item encontrado</response>
        /// <response code="404">Item não encontrado</response>
        [HttpGet("{id}")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public ActionResult<Item> Get(long id)
        {
            var item = _repository.GetById(id);
            if(item != null)
                return item;
            
            return NotFound();
        }

        // POST api/todo
        /// <summary>
        /// Cria um item na To-do list.
        /// </summary>
        /// <remarks>
        /// Exemplo:
        ///
        ///     POST /Todo
        ///     {
        ///        "id": 1,
        ///        "name": "Item1",
        ///        "iscomplete": true
        ///     }
        ///
        /// </remarks>
        /// <param name="value"></param>
        /// <returns>Um novo item criado</returns>
        /// <response code="201">Retorna o novo item criado</response>
        /// <response code="400">Se o item não for criado</response>        
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public ActionResult<Item> Post([FromBody] Item value)
        {
            Console.WriteLine(value?.Name);

            var item = _repository.Save(value);
            if(item != null)
                return item;
            
            return BadRequest();
        }

        // PUT api/todo/1
        /// <summary>
        /// Altera item da To-do list.
        /// </summary>
        /// <remarks>
        /// Exemplo:
        ///
        ///     PUT /Todo/1
        ///     {
        ///        "id": 1,
        ///        "name": "Item1",
        ///        "iscomplete": true
        ///     }
        ///
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns>O item alterado</returns>
        /// <response code="201">Retorna o item alterado</response>
        /// <response code="400">Se o item não for alterado</response>    
        [HttpPut("{id}")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public ActionResult<Item> Put(int id, [FromBody] Item value)
        {
            var item = _repository.Update(value);
            if(item != null)
                return item;
            
            return BadRequest();
        }

        // DELETE api/todo/5
        /// <summary>
        /// Remove item da To-do list.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Uma mensagem de sucesso</returns>
        /// <response code="200">Mensagem que o item foi excluído</response>
        /// <response code="400">Se o item não for excluído</response>    
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult Delete(long id)
        {
            if(_repository.Remove(id))
                return Ok(new { Description = "Item removed" });

            return BadRequest();
        }
    }
}
