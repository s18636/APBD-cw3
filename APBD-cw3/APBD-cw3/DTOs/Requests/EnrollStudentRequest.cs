using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using APBD_cw3.DAL_Data_Access_layer_;

namespace APBD_cw3.DTOs.Requests
{
    public class EnrollStudentRequest
    {
        public string IndexNumber { get; set; }

        [Required(ErrorMessage = "Musisz podać imię")]
        [MaxLength(10)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(255)]
        public string LastName { get; set; }
        [Required]
        public DateTime Birthdate { get; set; }

        [Required]
        public string Studies { get; set; }
    }
}
