using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {


    
        


        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public async Task<IActionResult> customerhome()
        {

            

                List<Article> li = new List<Article>();


            var builder = WebApplication.CreateBuilder();
            string conStr = builder.Configuration.GetConnectionString("WebApplication1Context");


            SqlConnection conn = new SqlConnection(conStr);


            string sql;
            sql = "select * from article order by ArticleID";
            SqlCommand comm = new SqlCommand(sql, conn);

            conn.Open();



            SqlDataReader reader = comm.ExecuteReader();

            while (reader.Read())
            {
                li.Add(new Article
                {

                    ArticleID = (int)reader["ArticleID"],
                    Title = (string)reader["Title"],
                    Description = (string)reader["Description"],
                    Category = (string)reader["Category"],
                    PublicationDate = (DateTime)reader["PublicationDate"],


                });
            }
            reader.Close();
            conn.Close();



            return View(li);

        }




        public async Task<IActionResult> adminhome()
        {
            string ss = HttpContext.Session.GetString("role");
            if (ss == "admin")
            {



                return View();
            }
            else {

                return RedirectToAction("login", "users");

            }


        }




    }
}