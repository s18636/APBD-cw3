using APBD_cw3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APBD_cw3.DAL_Data_Access_layer_
{
    public interface IDbService
    {

        public IEnumerable<Student> getStudents();
    }
}
