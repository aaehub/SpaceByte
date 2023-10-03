using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ContentsController : Controller
    {
        private readonly WebApplication1Context _context;

        public ContentsController(WebApplication1Context context)
        {
            _context = context;
        }

        // GET: Contents
        public async Task<IActionResult> Index()
        {
            string ss = HttpContext.Session.GetString("role");
            if (ss == "admin")
            {

                var webApplication1Context = _context.Content.Include(c => c.Article);
                return View(await webApplication1Context.ToListAsync());
            }
            else
            {

                return RedirectToAction("logout", "users");

            }
        }

        // GET: Contents/Details/5
        public async Task<IActionResult> Details(int? id)
        {


            string ss = HttpContext.Session.GetString("role");
            if (ss == "admin")
            {

                if (id == null || _context.Content == null)
                {
                    return NotFound();
                }

                var content = await _context.Content
                    .Include(c => c.Article)
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (content == null)
                {
                    return NotFound();
                }

                return View(content);
            }
            else
            {

                return RedirectToAction("logout", "users");

            }
        }

        // GET: Contents/Create
        // GET: Contents/Create
        public IActionResult Create(int? id)
        {
            string ss = HttpContext.Session.GetString("role");
            if (ss == "admin")
            {

                if (id != null)
                {
                    ViewData["ArticleID"] = id;
                }


                return View();
            }
            else
            {

                return RedirectToAction("logout", "users");

            }
        }









        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Content model, IFormFile ArticleFile)
        {
            string ss = HttpContext.Session.GetString("role");
            if (ss == "admin")
            {



                // Create a new Content entity
                var content = new Content
                {
                    Article = model.Article,
                    OrderNumber = model.OrderNumber,
                    content = model.content,
                    ContentType = model.ContentType,
                    ArticleID = model.ArticleID,

                };


                // Save the article file
                if (ArticleFile != null && ArticleFile.Length > 0)
                {

                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + ArticleFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        ArticleFile.CopyTo(fileStream);
                    }

                    // Update the content with the image file path
                    content.content = "/images/" + uniqueFileName;

                    await _context.SaveChangesAsync();
                }




                // Save the content to the database
                _context.Content.Add(content);
                await _context.SaveChangesAsync();




                ViewData["ArticleID"] = new SelectList(_context.Article, "ArticleID", "ArticleID", model.ArticleID);


                return RedirectToAction("Create");
            }
            else
            {

                return RedirectToAction("logout", "users");

            }
        }





        // GET: Contents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            string ss = HttpContext.Session.GetString("role");
            if (ss == "admin")
            {


                if (id == null || _context.Content == null)
                {
                    return NotFound();
                }

                var content = await _context.Content
                    .Include(c => c.Article)
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (content == null)
                {
                    return NotFound();
                }

                return View(content);


            }
            else
            {

                return RedirectToAction("logout", "users");

            }

        }

        // POST: Contents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            string ss = HttpContext.Session.GetString("role");
            if (ss == "admin")
            {

                if (_context.Content == null)
                {
                    return Problem("Entity set 'WebApplication1Context.Content'  is null.");
                }
                var content = await _context.Content.FindAsync(id);
                if (content != null)
                {
                    _context.Content.Remove(content);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));


            }
            else
            {

                return RedirectToAction("logout", "users");

            }
        }

        private bool ContentExists(int id)
        {
            return (_context.Content?.Any(e => e.Id == id)).GetValueOrDefault();
        }



    }
}
