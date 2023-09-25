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
    public class CommentsController : Controller
    {
        private readonly WebApplication1Context _context;

        public CommentsController(WebApplication1Context context)
        {
            _context = context;
        }

        // GET: Comments
        public async Task<IActionResult> Index()
        {

            string ss = HttpContext.Session.GetString("role"); if (ss == "admin")
            {
                return _context.Comment != null ? 
                          View(await _context.Comment.ToListAsync()) :
                          Problem("Entity set 'WebApplication1Context.Comment'  is null.");

            }
            else
            {
              
                return RedirectToAction("logout", "users");
            }
        }

        // GET: Comments/Details/5
        public async Task<IActionResult> Details(int? id)


        {

            string ss = HttpContext.Session.GetString("role"); if (ss == "admin")
            {

                if (id == null || _context.Comment == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment
                .FirstOrDefaultAsync(m => m.CommentID == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
            }
            else
            {
                return RedirectToAction("logout", "users");
            }
        }

        // GET: Comments/Create
        public IActionResult Create()
        {

            string ss = HttpContext.Session.GetString("role"); if (ss == "admin")
            {
                return View();
            }
            else
            {
                return RedirectToAction("logout", "users");
            }
        }

        // POST: Comments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CommentID,Date,Comment_Text,ArticleID,UserID")] Comment comment)
        {

            string ss = HttpContext.Session.GetString("role"); if (ss == "admin")
            {
                if (ModelState.IsValid)
            {
                _context.Add(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(comment);
            }
            else
            {
                return RedirectToAction("logout", "users");
            }
        }

        // GET: Comments/Edit/5
        public async Task<IActionResult> Edit(int? id)



        {

            string ss = HttpContext.Session.GetString("role"); if (ss == "admin")
            {

                if (id == null || _context.Comment == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            return View(comment);
            }
            else
            {
                return RedirectToAction("logout", "users");
            }
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CommentID,Date,Comment_Text,ArticleID,UserID")] Comment comment)
        {

            string ss = HttpContext.Session.GetString("role"); if (ss == "admin")
            {
                if (id != comment.CommentID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment.CommentID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(comment);
            }
            else
            {
                return RedirectToAction("logout", "users");
            }
        }

        // GET: Comments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            string ss = HttpContext.Session.GetString("role"); if (ss == "admin")
            {
                if (id == null || _context.Comment == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment
                .FirstOrDefaultAsync(m => m.CommentID == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
            }
            else
            {
                return RedirectToAction("logout", "users");
            }
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            if (_context.Comment == null)
            {
                return Problem("Entity set 'WebApplication1Context.Comment'  is null.");
            }
            var comment = await _context.Comment.FindAsync(id);
            if (comment != null)
            {
                _context.Comment.Remove(comment);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommentExists(int id)
        {
          return (_context.Comment?.Any(e => e.CommentID == id)).GetValueOrDefault();
        }
    }
}
