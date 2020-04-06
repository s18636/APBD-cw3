using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using APBD_cw3.DAL_Data_Access_layer_;
using APBD_cw3.Models;
using Microsoft.AspNetCore.Mvc;

namespace APBD_cw3.Controllers
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

        [HttpGet]
        public IActionResult GetStudents()
        {

            var students = new List<Student>();
            //return Ok(_dbService.getStudents());
            using (var client = new SqlConnection("Data Source=db-mssql;Initial Catalog=s18636;Integrated Security=True"))
            {
                using (var con = new SqlCommand())
                {
                    con.Connection = client;
                    con.CommandText = "SELECT IndexNumber, FirstName, LastName, BirthDate, Name, Semester" +
                        "  FROM Student s, enrollment e, Studies " +
                        "  WHERE e.idEnrollment = s.idEnrollment AND e.idStudy = Studies.idStudy";

                    client.Open();

                    var dr = con.ExecuteReader();
                    while (dr.Read()) 
                    {
                        var st = new Student();
                        st.IndexNumber = dr["IndexNumber"].ToString();
                        st.FirstName = dr["FirstName"].ToString();
                        st.LastName = dr["LastName"].ToString();
                        st.BirthDate = dr["BirthDate"].ToString();
                        st.StudiesName = dr["Name"].ToString();
                        st.Semester = Int32.Parse(dr["semester"].ToString());

                        students.Add(st);
                    }

                }
            }

            return Ok(students);
        }


        [HttpGet("{id}")]
        public IActionResult GetStudent(string id)
        {
            var enrollments = new List<Enrollment>();

            using (var con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s18636;Integrated Security=True")) 
            { 
                using (var com = new SqlCommand())
                {
                    com.Connection = con;
                    com.CommandText = "SELECT * " +
                        "FROM Enrollment e, Student s " +
                        "WHERE s.idEnrollment = e.idEnrollment AND s.indexnumber = @id ";
                    com.Parameters.AddWithValue("id", id);

                    con.Open();

                    var dr = com.ExecuteReader();
                    while (dr.Read())
                    {
                        var en = new Enrollment();
                        en.idEnrollment = Int32.Parse(dr["idEnrollment"].ToString());
                        en.semester = Int32.Parse(dr["semester"].ToString());
                        en.idStudy = Int32.Parse(dr["idStudy"].ToString());
                        en.startDate = dr["startDate"].ToString();

                        enrollments.Add(en);
                    }

                }
            }


            return Ok(enrollments);
        }

        
        [HttpPost]
        public IActionResult createStudent(Student student) {
            return Ok(student);
        }

        [HttpPut("{id}")]
        public IActionResult updateStudent(int id){
            return Ok("Aktualizacja dokończona");
        }

        [HttpDelete("{id}")]
        public IActionResult deleteStudent(int id) {
            return Ok("usuwanie zakończone");
        }


    }
}