using APBD_cw3.DTOs.Requests;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Wyklad5.Services
{
    public interface IStudentDbService
    {
        public IActionResult EnrollStudent(EnrollStudentRequest request);
        public IActionResult PromoteStudents(int semester, string studies);
    }
}
