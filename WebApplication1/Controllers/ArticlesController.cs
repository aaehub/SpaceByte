using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using WebApplication1.Models; // Replace with the namespace of your models
using Microsoft.EntityFrameworkCore; // Required for Entity Framework
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualBasic;
using System.Net.Mime;
using Newtonsoft.Json;
using Microsoft.NET.StringTools;

namespace WebApplication1.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly WebApplication1Context _context;

        public ArticlesController(WebApplication1Context context)
        {
            _context = context;
        }

        // GET: Articles
        public async Task<IActionResult> Index()
        {

            string ss = HttpContext.Session.GetString("role"); if (ss == "admin")
            {


                return _context.Article != null ?
                          View(await _context.Article.ToListAsync()) :
                          Problem("Entity set 'WebApplication1Context.Article'  is null.");

            }
            else
            {
                return RedirectToAction("login", "users");
            }

            }

        // GET: Articles/Details/5
     

      
        public List<comments_details> GetComment(int articleId)
        {
          
            
            List<comments_details> comments = new List<comments_details>();

            // Your database connection and query logic here
            var builder = WebApplication.CreateBuilder();
            string conStr = builder.Configuration.GetConnectionString("WebApplication1Context");
            SqlConnection conn = new SqlConnection(conStr);
            string sql = "SELECT Comment.*, [User].Username AS Username FROM [User] JOIN Comment ON [User].UserID = Comment.UserID WHERE Comment.ArticleID = @ArticleID";
            SqlCommand comm = new SqlCommand(sql, conn);
            comm.Parameters.AddWithValue("@ArticleID", articleId);

            conn.Open();

            SqlDataReader reader = comm.ExecuteReader();

            while (reader.Read())
            {
                comments.Add(new comments_details
                {
                    CommentID = (int)reader["CommentID"],
                    Date = (DateTime)reader["Date"],
                    Comment_Text = (string)reader["Comment_Text"],
                    ArticleID = (int)reader["ArticleID"],
                    UserID = (int)reader["UserID"],
                    username = (string)reader["username"]
                });
            }
            reader.Close();
            conn.Close();

            return comments;
        }






        public async Task<IActionResult> Details(int? id)
        {


            if (id == null || _context.Article == null)
            {
                return NotFound();
            }

            var article = await _context.Article
                .FirstOrDefaultAsync(m => m.ArticleID == id);
            if (article == null)
            {
                return NotFound();
            }




            List<comments_details> comments = new List<comments_details>();


            var builder = WebApplication.CreateBuilder();
            string conStr = builder.Configuration.GetConnectionString("WebApplication1Context");
            SqlConnection conn = new SqlConnection(conStr);
            string sql;
            sql = "SELECT Comment.*, [User].Username AS Username FROM [User] JOIN Comment ON [User].UserID = Comment.UserID WHERE Comment.ArticleID = " + article.ArticleID+" ";
            SqlCommand comm = new SqlCommand(sql, conn);

            conn.Open();

            SqlDataReader reader = comm.ExecuteReader();


             while (reader.Read())
            {
                comments.Add(new comments_details
                {
                    CommentID = (int)reader["CommentID"],
                    Date = (DateTime)reader["Date"],
                    Comment_Text = (string)reader["Comment_Text"],
                    ArticleID = (int)reader["ArticleID"],
                    UserID = (int)reader["UserID"],
                    username = (string)reader["username"] });
            }
            reader.Close();
            conn.Close();



            List<Content> contents = new List<Content>();

            var builder2 = WebApplication.CreateBuilder();
            string conStr2 = builder.Configuration.GetConnectionString("WebApplication1Context");
            SqlConnection conn2 = new SqlConnection(conStr2);
            
           // string sql2 = "SELECT p.OrderNumber, p.Content AS Content, 'Paragraph' AS ContentType FROM Paragraph p WHERE p.ArticleID =" + article.ArticleID + " AND p.Content IS NOT NULL UNION ALL SELECT a.OrderNumber, a.VidURL AS Content, 'APIMedia' AS ContentType FROM APIMedia a WHERE a.ArticleID = " + article.ArticleID + "  AND a.VidURL IS NOT NULL UNION ALL SELECT v.OrderNumber, v.VideoURL AS Content, 'Video' AS ContentType FROM Video v WHERE v.ArticleID = " + article.ArticleID + "  AND v.VideoURL IS NOT NULL UNION ALL SELECT i.OrderNumber, i.ImageURL AS Content, 'Image' AS ContentType FROM Image i WHERE i.ArticleID = " + article.ArticleID + "  AND i.ImageURL IS NOT NULL ORDER BY OrderNumber;";
            string sql3 = "SELECT [OrderNumber], [Content] AS Content ,[ContentType] as ContentType FROM [dbo].[Content]  WHERE [ArticleID] = "+article.ArticleID+" AND [Content] IS NOT NULL ORDER BY [OrderNumber];";

            SqlCommand comm2 = new SqlCommand(sql3, conn2);
            conn2.Open();

            SqlDataReader reader2 = comm2.ExecuteReader();

            while (reader2.Read())
            {

                Content content = new Content();


               
                content.content = (string)reader2["Content"];
                content.ContentType = (string)reader2["ContentType"];
                content.OrderNumber = (int)reader2["OrderNumber"];


                contents.Add(content);


            }
            reader2.Close();
            conn2.Close();



            ViewData["contents"] = contents;


            ViewData["comments"] = comments;


            return View(article);



        }




        public ActionResult Create()
        {

            string ss = HttpContext.Session.GetString("role"); if (ss == "admin")
            {

                return View();
            }
            else
            {
                return RedirectToAction("login", "users");
            }

            }

       

        [HttpPost]
        public ActionResult Create(Article model)
        {

            string ss = HttpContext.Session.GetString("role"); if (ss == "admin")
            {

                // Create a new Article entity
                var article = new Article
                {
                    Title = model.Title,
                    Description = model.Description,
                    Category = model.Category,
                    AuthorID = model.AuthorID,
                    Content = model.Content,
                    PublicationDate = DateTime.Now,
                    ContentList = new List<Content>()
                };

                // Add each content item to the article
                foreach (var content in model.ContentList)
                {
                    var newContent = new Content
                    {
                       ArticleID = article.ArticleID,
                       Article= article,
                        OrderNumber = content.OrderNumber,
                        content = content.content,
                        ContentType = content.ContentType
                    };

                    article.ContentList.Add(newContent);
                }


            _context.Article.Add(article);
            _context.SaveChanges();



            return RedirectToAction("Details", new { id = article.ArticleID });

            }
            else
            {
                return RedirectToAction("login", "users");
            }
        }

        // GET: Articles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            string ss = HttpContext.Session.GetString("role"); if (ss == "admin")
            {

                if (id == null || _context.Article == null)
            {
                return NotFound();
            }

            var article = await _context.Article.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }
            return View(article);
            }
            else
            {
                return RedirectToAction("login", "users");
            }

        }

        // POST: Articles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ArticleID,Title,Description,Content,Category,AuthorID,PublicationDate")] Article article)
        {

            string ss = HttpContext.Session.GetString("role"); if (ss == "admin")
            {

                if (id != article.ArticleID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(article);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticleExists(article.ArticleID))
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
            return View(article);

            }
            else
            {
                return RedirectToAction("login", "users");
            }

        }

        // GET: Articles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {

            string ss = HttpContext.Session.GetString("role"); if (ss == "admin")
            {
                if (id == null || _context.Article == null)
            {
                return NotFound();
            }

            var article = await _context.Article
                .FirstOrDefaultAsync(m => m.ArticleID == id);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);

            }
            else
            {
                return RedirectToAction("login", "users");
            }

        }



        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)


        {


            string ss = HttpContext.Session.GetString("role"); if (ss == "admin")
            {
                if (_context.Article == null)
            {
                return Problem("Entity set 'WebApplication1Context.Article'  is null.");
            }
            var article = await _context.Article.FindAsync(id);
            if (article != null)
            {
                _context.Article.Remove(article);

                var builder = WebApplication.CreateBuilder();
                string conStr = builder.Configuration.GetConnectionString("WebApplication1Context");
                SqlConnection conn1 = new SqlConnection(conStr);

                string sql, sql2;
                sql = "DELETE FROM Comment WHERE ArticleID = " + id + ";";
                sql2 = "DELETE FROM Content WHERE ArticleID = " + id + ";";

                SqlCommand comm = new SqlCommand(sql, conn1);
                conn1.Open();
                comm.ExecuteNonQuery();

                SqlCommand comm2 = new SqlCommand(sql2, conn1);
                comm2.ExecuteNonQuery();

                _context.Article.Remove(article);

                conn1.Close();


            }



            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction("login", "users");
            }
        }

        private bool ArticleExists(int id)
        {
          return (_context.Article?.Any(e => e.ArticleID == id)).GetValueOrDefault();
        }


        














        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateComment(string commenttext, int articleid, int accountid)
        {





            var builder = WebApplication.CreateBuilder();
            string conStr = builder.Configuration.GetConnectionString("WebApplication1Context");



            SqlConnection conn = new SqlConnection(conStr);


            string sql;



            //string ss = HttpContext.Session.GetString("Id");
            //int b = Convert.ToInt32(ss);

            sql = " INSERT INTO Comment VALUES(  GETDATE() " + ", '" + commenttext + "' ," + articleid + ",  " + 1+ ")";

            SqlCommand comm = new SqlCommand(sql, conn);
            conn.Open();




            comm.ExecuteNonQuery();
            comm.Dispose();

            conn.Close();




            return RedirectToAction("Details", "articles", new { id = articleid });




        }






    }







}
