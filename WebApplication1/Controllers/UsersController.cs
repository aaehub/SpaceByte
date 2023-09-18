using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class UsersController : Controller
    {
        private readonly WebApplication1Context _context;

        public UsersController(WebApplication1Context context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
              return _context.User != null ? 
                          View(await _context.User.ToListAsync()) :
                          Problem("Entity set 'WebApplication1Context.User'  is null.");
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserID,Username,Email,Password,Gender,Role,DateCreated")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserID,Username,Email,Password,Gender,Role,DateCreated")] User user)
        {
            if (id != user.UserID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserID))
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
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.User == null)
            {
                return Problem("Entity set 'WebApplication1Context.User'  is null.");
            }
            var user = await _context.User.FindAsync(id);
            if (user != null)
            {
                _context.User.Remove(user);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
          return (_context.User?.Any(e => e.UserID == id)).GetValueOrDefault();
        }







            // GET: User/Login
            public ActionResult Login()
            {
                return View();
            }

        // POST: User/Login
        [HttpPost, ActionName("login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> login(string na, string pa, bool auto)
        {

            var builder = WebApplication.CreateBuilder();
            string conStr = builder.Configuration.GetConnectionString("WebApplication1Context");

            //getting data from database (sql)
            SqlConnection conn1 = new SqlConnection(conStr);


            string sql;
            sql = "SELECT * FROM [user] where username ='" + na + "' and  password ='" + pa + "' ";
            SqlCommand comm = new SqlCommand(sql, conn1);
            conn1.Open();
            SqlDataReader reader = comm.ExecuteReader();

            if (reader.Read())
            {
                string id = Convert.ToString((int)reader["UserID"]);
                string na1 = (string)reader["username"];

                string ro = (string)reader["role"];

                string email = (string)reader["email"];



                //session data
              
                reader.Close();
                conn1.Close();


                return RedirectToAction("customerhome", "home");
            }
            else
            {
                ViewData["Message"] = "wrong user name or password";



            }

            return View();

        }


        public IActionResult logout()
        {
            HttpContext.Session.Remove("Id");
            HttpContext.Session.Remove("username");
            HttpContext.Session.Remove("role");

            HttpContext.Response.Cookies.Delete("username");
            HttpContext.Response.Cookies.Delete("role");

            return RedirectToAction("customerhome", "home");

        }












        // GET: User/Register
        public ActionResult Register()
            {
                return View();
            }

        // POST: User/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> register([Bind("Username,Email,Password,Gender,role,Date")] User myusers)
        {



            var builder = WebApplication.CreateBuilder();
            string conStr = builder.Configuration.GetConnectionString("WebApplication1Context");
            SqlConnection conn = new SqlConnection(conStr); string sql;
            conn.Open();

            Boolean flage = false;
            sql = "select * from [user] where email = '" + myusers.Email + "'";
            SqlCommand comm = new SqlCommand(sql, conn);
            SqlDataReader reader = comm.ExecuteReader();
            if (reader.Read())
            {
                flage = true;
            }
            reader.Close();

            if (flage == true)
            {

                ViewData["message"] = "Email already exists";
                return View();
            }
            else
            {

                myusers.Role = "User";

                myusers.DateCreated = DateTime.Now;




                _context.Add(myusers);



                await _context.SaveChangesAsync();
                //   HttpContext.Session.SetString("Id", Convert.ToString(myusers.Id));

                return RedirectToAction("login", "users");
            }
            conn.Close();
            return View();





        }





    }
}
