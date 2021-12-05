using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using StoredProcedure.Data;
using StoredProcedure.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoredProcedure.Controllers
{
    public class SearchWithDynamicController : Controller
    {
        public StoredProcedureDBContext _context;
        public IConfiguration _config { get; }
        public SearchWithDynamicController
            (
            StoredProcedureDBContext context, IConfiguration config
            )
        {
            _context = context;
            _config = config;
        }

        [HttpGet]
        public IActionResult SearchWithDynamic()
        {
            string connectionStr = _config.GetConnectionString("DefaultConnection");
            using (SqlConnection con = new SqlConnection(connectionStr))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "dbo.SearchEmployees";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                con.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                List<Employee> model = new List<Employee>();
                while (sdr.Read())
                {
                    var details = new Employee();
                    details.FirstName = sdr["FirstName"].ToString();
                    details.LastName = sdr["LastName"].ToString();
                    details.Gender = sdr["Gender"].ToString();
                    details.Salary = Convert.ToInt32(sdr["Salary"]);
                    model.Add(details);
                }
                return View(model);
            }


        }

        [HttpPost]
        public IActionResult SearchWithDynamic(string firstName, string lastName, string gender, int salary)
        {
            string connectionStr = _config.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(connectionStr))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                StringBuilder sbCommand = new
                    StringBuilder("Select * from Employees where 1 = 1");
                cmd.CommandText = "SearchEmployees";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                if (firstName != null)
                {
                    sbCommand.Append(" AND FirstName=@FirstName");
                    SqlParameter param_fn = new SqlParameter("@FirstName", firstName);
                    cmd.Parameters.Add(param_fn);
                }
                if (lastName != null)
                {
                    sbCommand.Append(" AND LastName=@LastName");
                    SqlParameter param_ln = new SqlParameter("@LastName", lastName);
                    cmd.Parameters.Add(param_ln);
                }
                if (gender != null)
                {
                    sbCommand.Append(" AND Gender=@Gender");
                    SqlParameter param_g = new SqlParameter("@Gender", gender);
                    cmd.Parameters.Add(param_g);
                }
                if (salary != 0)
                {
                    sbCommand.Append(" AND Salary=@Salary");
                    SqlParameter param_s = new SqlParameter("@Salary", salary);
                    cmd.Parameters.Add(param_s);
                }
                con.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                List<Employee> model = new List<Employee>();
                while (sdr.Read())
                {
                    var details = new Employee();
                    details.FirstName = sdr["FirstName"].ToString();
                    details.LastName = sdr["LastName"].ToString();
                    details.Gender = sdr["Gender"].ToString();
                    details.Salary = Convert.ToInt32(sdr["Salary"]);
                    model.Add(details);
                }
                return View(model);
            }
        }

    }
    }
