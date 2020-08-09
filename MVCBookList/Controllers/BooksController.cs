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

        [BindProperty]
        public Book Book { get; set; }

        public BooksController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Book = new Book();
            if (id == null)
            {
                //Create Book request
                return View(Book);
            }
            //Update Book request
            Book = _context.Book.FirstOrDefault(u => u.Id == id);
            if(Book == null)
            {
                return NotFound();
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert()
        {
            if (ModelState.IsValid)
            {
                if(Book.Id == 0)
                {
                    //Create Book
                    _context.Book.Add(Book);
                }
                else
                {
                    //Update Book
                    _context.Book.Update(Book);
                }
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(Book);
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
