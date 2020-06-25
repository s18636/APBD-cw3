using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APBD_cw3.DAL_Data_Access_layer_;
using APBD_cw3.DTOs.Requests;
using Microsoft.AspNetCore.Mvc;
using Wyklad5.Services;

namespace APBD_cw3.Controllers
{
    [ApiController]
    [Route("api/enrollments")]

    public class EnrollmentsController : ControllerBase
    {

        private readonly IStudentDbService _dbService;

        public EnrollmentsController(IStudentDbService service)
        {
            _dbService = service;
        }

        [HttpPost]

        public IActionResult EnrollStudent(EnrollStudentRequest request) 
        {

            return _dbService.EnrollStudent(request);
        }

        [HttpPost("promotions")]
        public IActionResult promoteStudent(string studiesName, int semester) 
        {
            return _dbService.PromoteStudents(semester, studiesName);
        }

    }
}