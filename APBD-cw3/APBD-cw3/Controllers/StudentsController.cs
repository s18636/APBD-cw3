using System;
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
            return Ok(_dbService.getStudents());
        }


        [HttpGet("{id}")]
        public IActionResult GetStudent(int id)
        {
            _dbService.getStudents().Find(id);

            return NotFound("Nie znaleziono studenta");
        }

        
        [HttpPost]
        public IActionResult createStudent(Student student) {
            student.IndexNumber = $"s{new Random().Next(1, 20000)}";
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