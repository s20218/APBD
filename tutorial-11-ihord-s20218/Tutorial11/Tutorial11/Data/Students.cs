using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tutorial11.Models;

namespace Tutorial11.Data
{
    public class Students
    {
        public static List<Student> ListOfStudents { get; set; } = new List<Student>();

        static Students()
        {
            ListOfStudents.Add(new Student
            {
                FirstName = "John",
                LastName = "Smith",
                Birthdate = new DateTime(1999, 3, 14).Date.ToString("dd/MM/yyyy"),
                Studies = "Computer Science",
                IdStudent = 1
            });
            ListOfStudents.Add(new Student
            {
                FirstName = "Anne",
                LastName = "Smith",
                Birthdate = new DateTime(2000, 7, 25).ToString("dd/MM/yyyy"),
                Studies = "New Media Art",
                IdStudent = 2
            });
        }
    }
}
