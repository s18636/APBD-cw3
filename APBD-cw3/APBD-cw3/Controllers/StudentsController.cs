using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using APBD_cw3.DAL_Data_Access_layer_;
using APBD_cw3.Models;
using Microsoft.AspNetCore.Mvc;
using Wyklad5.Services;

namespace APBD_cw3.Controllers
{

    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {

        private readonly IStudentDbService _dbService;

        public StudentsController(IStudentDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet]
        public IActionResult GetStudents()
        {

            return Ok(_dbService.getStudents());
        }


    }
}