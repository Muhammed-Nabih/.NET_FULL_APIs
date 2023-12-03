using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestRESTAPI.Data;
using TestRESTAPI.Data.Models;

namespace TestRESTAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _db;
        public CategoriesController(AppDbContext db)
        {
            _db = db;   
        }


        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var names = await _db.Categories.ToListAsync();
            return Ok(names);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategories(int id)
        {
            var names = await _db.Categories.SingleOrDefaultAsync(x => x.Id == id);
            if (names == null)
            {
                return NotFound($"Category Id {id} Not Exist");
            }

            return Ok(names);
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody]Category category)
        {
            Category c = new()
            {
                Name = category.Name
            };
            await _db.Categories.AddAsync(c);
            _db.SaveChanges();
            return Ok(c);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCategory(Category category)
        {
            var c = await _db.Categories.SingleOrDefaultAsync(x => x.Id == category.Id);
            if (c == null)
            {
                return NotFound($"Category Id {category.Id} Not Exist");
            }
            c.Name = category.Name;
            _db.SaveChanges();
            return Ok(c);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateCategoryPatch([FromBody] JsonPatchDocument<Category> category , [FromRoute] int id)
        {
            var c = await _db.Categories.SingleOrDefaultAsync(x => x.Id == id);
            if (c == null)
            {
                return NotFound($"Category Id {id} Not Exist");
            }
            category.ApplyTo(c);
            await _db.SaveChangesAsync();
            return Ok(c);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveCategory(int id)
        {
            var c = await _db.Categories.SingleOrDefaultAsync (x => x.Id == id);
            if (c == null)
            {
                return NotFound($"Category Id {id} Not Exist");
            }
            _db.Categories.Remove(c);
            _db.SaveChanges();
            return Ok($"The {id} Removed Successfully");
        }
    }
}
