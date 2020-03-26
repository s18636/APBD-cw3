using APBD_cw3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APBD_cw3.DAL_Data_Access_layer_
{

    public class MockDbService : IDbService
    {

        private static IEnumerable<Student> _students;

        static MockDbService() {

            _students = new List<Student> 
            {
                new Student{FirstName = "Jakub", LastName = "Szczepan" },
                new Student{FirstName = "ktos", LastName = "tam" },
                new Student{FirstName = "ktos", LastName = "inny" }
            };
        }

        public IEnumerable<Student> getStudents()
        {
            return _students;
        }
    }
}
