using APBD_cw3.DTOs.Requests;
using APBD_cw3.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Wyklad5.Services
{
    public interface IStudentDbService
    {
        public IActionResult EnrollStudent(EnrollStudentRequest request);
        public IActionResult PromoteStudents(int semester, string studies);
        public IActionResult getStudents();
    }
}
