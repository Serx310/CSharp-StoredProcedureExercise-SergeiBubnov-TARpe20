using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using StoredProcedure.Data;
using StoredProcedure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoredProcedure.Controllers
{
    public class DynamicSQLInSpController : Controller
    {
        public StoredProcedureDBContext _context;
        public IConfiguration _config { get; }
        public DynamicSQLInSpController
            (
            StoredProcedureDBContext context, IConfiguration config
            )
        {
            _context = context;
            _config = config;
        }



        /// <summary>
        /// DynamicSQLInStoredProcedure
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult DynamicSQLInSp()
        {
            string connectionStr = _config.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(connectionStr))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "dbo.spSearchEmployeesGoodDynamicSQL";
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


        /// <summary>
        /// DynamicSQLInStoredProcedure
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult DynamicSQLInSp(string firstName, string lastName, string gender, int salary)
        {
            string connectionStr = _config.GetConnectionString("DefaultConnection");

            using (SqlConnection con = new SqlConnection(connectionStr))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "dbo.spSearchEmployeesGoodDynamicSQL";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                if (firstName != null)
                {
                    SqlParameter param = new SqlParameter("@FirstName", firstName);
                    cmd.Parameters.Add(param);
                }
                if (lastName != null)
                {
                    SqlParameter param = new SqlParameter("@LastName", lastName);
                    cmd.Parameters.Add(param);
                }
                if (gender != null)
                {
                    SqlParameter param = new SqlParameter("@Gender", gender);
                    cmd.Parameters.Add(param);
                }
                if (salary != 0)
                {
                    SqlParameter param = new SqlParameter("@Salary", salary);
                    cmd.Parameters.Add(param);
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
