using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCBookList.Models;

namespace MVCBookList.Controllers
{
    public class BooksController : Controller
    {

        private readonly ApplicationDbContext _context;

        public BooksController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        #region APICalls
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Json(new { data = await _context.Book.ToListAsync() });

        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int Id)
        {
            var bookFromDb = await _context.Book.FirstOrDefaultAsync(u => u.Id == Id);
            if (bookFromDb == null)
            {
                return Json(new { success = false, message = "An error occurred." });
            }
            _context.Book.Remove(bookFromDb);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Book successfully Deleted." });
        }
        #endregion
    }
}
