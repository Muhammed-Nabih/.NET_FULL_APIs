﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestRESTAPI.Data;
using TestRESTAPI.Data.Models;
using TestRESTAPI.Models;

namespace TestRESTAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        public ItemsController(AppDbContext db)
        {
            _db = db;
        }
        private readonly AppDbContext _db;


        [HttpGet]
        public async Task<IActionResult> AllItems()
        {
            var items = await _db.Items.ToListAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> OneItem(int id)
        {
            var item = await _db.Items.SingleOrDefaultAsync(x => x.Id == id);
            if(item == null)
            {
                return NotFound($"The Item Code {id} Not Found");
            }
            return Ok(item);
        }

        [HttpGet("ItemsWithCategory/{idCategory}")]
        public async Task<IActionResult> AllItemsWithCategory(int idCategory)
        {
            var item =  await _db.Items.Where(x => x.CategoryId == idCategory).ToListAsync();
            if (item == null)
            {
                return NotFound($"The Item Id {idCategory} Has No Items ");
            }
            return Ok(item);
        }



        [HttpPost]
        public async Task<IActionResult> AddItem([FromForm]mdlItem mdl)
        {
            using var stream = new MemoryStream();
            await mdl.Image.CopyToAsync(stream);
            var item = new Item
            {
                Name = mdl.Name,
                price = mdl.price,
                Notes = mdl.Notes,
                Image = stream.ToArray(),
                CategoryId = mdl.CategoryId
            };
            await _db.Items.AddAsync(item);
            await _db.SaveChangesAsync();
            return Ok(item);
        }




        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var item = await _db.Items.SingleOrDefaultAsync(x => x.Id == id);
            if (item == null)
            {
                return BadRequest($"Item Id {id} Not Found");
            }
            _db.Items.Remove(item);
            await _db.SaveChangesAsync();
            return Ok(item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(int id , [FromForm]mdlItem mdl)
        {
            var item = await _db.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound($"Item Id {id} Not Found");
            }
            var isCategory = await _db.Categories.AnyAsync(x=> x.Id == mdl.CategoryId );
            if (isCategory)
            {
                return NotFound($"Category Id {id} Not Found");
            }
            if(mdl.Image  != null)
            {
                using var stream = new MemoryStream();
                await mdl.Image.CopyToAsync(stream);
                item.Image = stream.ToArray();
            }
            item.Name = mdl.Name;
            item.price = mdl.price;
            item.Notes = mdl.Notes;
            item.CategoryId = mdl.CategoryId;
            _db.SaveChanges();    
            return Ok(item);
        }

    }
}
