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
        public IActionResult GetStudent(int id)
        {
            //_dbService.getStudents().Find(id);

            return NotFound("Nie znaleziono studenta");
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