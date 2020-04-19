using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using cw3.DAL;
using cw3.Models;
using Microsoft.AspNetCore.Mvc;



namespace Cwicz_3.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private readonly IDbService _dbService;

        public StudentsController(IDbService dbService)
        {
            _dbService = dbService;

        }
        [HttpDelete]
        public IActionResult DeleteStudent(int id)
        {
            return Ok("Usuwanie ukończone");
        }

        [HttpPut]
        public IActionResult PutStudent(int id)
        {
            return Ok("Aktualizacja dokończona");
        }


        [HttpGet]
        public IActionResult GetStudents()
        {

            List<Student> listaStudentow = new List<Student>();

            using (var con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s19461;Integrated Security=True"))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "select FirstName, LastName, birthdate, Studies.name, Enrollment.semester from Student " +
                    "inner join Enrollment on Enrollment.IdEnrollment = Student.IdEnrollment " +
                    "inner join Studies on Studies.IdStudy = Enrollment.IdStudy; ";

                con.Open();
                var dr = com.ExecuteReader();
                while (dr.Read())
                {
                    var st = new Student();
                    st.FirstName = dr["FirstName"].ToString();
                    st.LastName = dr["LastName"].ToString();
                    st.BirthDate = dr["birthdate"].ToString();
                    st.StudiesName = dr["name"].ToString();
                    st.Semester = dr["semester"].ToString();

                    listaStudentow.Add(st);

                }


            }
            return Ok(listaStudentow);
        }
        [HttpGet("{id}")]
        public IActionResult GetEnrollment(int id)
        {

            List<Enrollment> enList = new List<Enrollment>();
            using (var con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s19461;Integrated Security=True"))
            using (var com = new SqlCommand())
            {
                com.Connection = con;

                com.CommandText = "select Enrollment.IdEnrollment, Semester, IdStudy,StartDate from Student " +
                    "inner join Enrollment on Enrollment.IdEnrollment = Student.IdEnrollment" +
                    $" where IndexNumber=@id;";

                com.Parameters.AddWithValue("id", id);
                con.Open();
                SqlDataReader dr = com.ExecuteReader();

                while (dr.Read())
                {
                    var en = new Enrollment();
                    en.IdEnrollment = dr.GetInt32(0);
                    en.Semester = dr.GetInt32(1);
                    en.IdStudy = dr.GetInt32(2);
                    en.StartDate = dr["StartDate"].ToString();
                    enList.Add(en);

                }

                return Ok(enList);
            }


        }

    }
}