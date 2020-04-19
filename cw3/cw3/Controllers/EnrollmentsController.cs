using cw3.Models;
using cw3.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cw3.Controllers
{
    [Route("api/enrollments")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private IStudentsDbService _dbService;

        public EnrollmentsController(IStudentsDbService dbService)
        {
            _dbService = dbService;

        }


        [HttpPost]
        public IActionResult PutStudentOnSemester([FromBody]Student student)
        {
            Console.WriteLine(student.StudiesName);
            if (student.FirstName == null || student.LastName == null || student.StudiesName == null ||
                student.IndexNumber == null || student.BirthDate == null)
            {
                return BadRequest();
            }
            Enrollment czyIstnieje = _dbService.Rejestracja(student.StudiesName, student);
            Console.WriteLine(czyIstnieje);
            if (czyIstnieje == null) return BadRequest();

            ObjectResult ob = new ObjectResult(czyIstnieje);
            ob.StatusCode = 201;
            return ob;
        }

        [HttpPost("promotions")]
        public IActionResult upgradeStudent([FromBody] Studies studie)
        {
            if (studie.Studiess == null || studie.Semester == null) return BadRequest();

            ObjectResult ob = new ObjectResult(new SqlServerDbService().PromoteStudents(studie.Semester, studie.Studiess));
            ob.StatusCode = 201;
            return ob;
        }

    }
}