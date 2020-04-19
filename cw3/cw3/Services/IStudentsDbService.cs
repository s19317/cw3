using cw3.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace cw3.Services
{
    public interface IStudentsDbService
    {
        public Enrollment Rejestracja(string studiesName, Student student);
        public void DodStudenta(Student student, SqlCommand com, int IdEnrollment);
        public Enrollment PromoteStudents(int semester, string studies);
        public bool CzyInstnieje(string index);
    }
}