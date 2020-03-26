using System.Collections;
using System.Collections.Generic;
using cw3.DAL;
using cw3.Models;

namespace cw3.DAL
{
    public class MockDbService : IDbService
    {
        private static IEnumerable<Student> _students;
        
        static MockDbService()
        {
            _students = new List<Student>
            {
                new Student {IdStudent = 1, FirstName = "Jan", LastName = "Kowalski"},
                new Student {IdStudent = 1, FirstName = "Anna", LastName = "Malewski"},
                new Student {IdStudent = 1, FirstName = "Andrzej", LastName = "Andrzejewicz"}
            };
        }
        public IEnumerable<Student> GetStudents()
        {
            return _students;
        }
    }
}