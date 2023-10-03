﻿using System;
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

            string ss = HttpContext.Session.GetString("role");
            if (ss == "admin")
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

            string ss = HttpContext.Session.GetString("role");
            if (ss == "admin")
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
            string ss = HttpContext.Session.GetString("role");
            if (ss == "admin")
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



            comment.Date = DateTime.Now;
                _context.Add(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            


            return View(comment);
        }







        // GET: Comments/Delete/5
        public async Task<IActionResult> Delete(int? id)
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






        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int CommentID)
        {
            if (_context.Comment == null)
            {
                return Problem("Entity set 'WebApplication1Context.Comment'  is null.");
            }
            var comment = await _context.Comment.FindAsync(CommentID);
            if (comment != null)
            {
                _context.Comment.Remove(comment);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommentExists(int CommentID)
        {
          return (_context.Comment?.Any(e => e.CommentID == CommentID)).GetValueOrDefault();
        }
    }
}
