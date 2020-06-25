using APBD_cw3.DTOs.Requests;
using APBD_cw3.DTOs.Responses;
using APBD_cw3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;



namespace Wyklad5.Services
{
    public class SqlServerStudentDbService : IStudentDbService
    {

        public SqlServerStudentDbService()
        {

        }

        public IActionResult EnrollStudent(EnrollStudentRequest request)
        {
            
            

            using (var con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s18636;Integrated Security=True"))
            using (var com = new SqlCommand())
            {
   
                com.Connection = con;

                con.Open();
                var transaction = con.BeginTransaction();
                try
                {
                    com.CommandText = "SELECT IdStudy FROM studies WHERE name = @studies";
                    com.Parameters.AddWithValue("studies", request.Studies);

                    com.Transaction = transaction;
                    var Reader = com.ExecuteReader();
                    if (!Reader.Read())
                    {
                        Reader.Close();
                        transaction.Rollback();
                        return new BadRequestResult();
                    }
                    var idStudies = (int)Reader["IdStudy"];
                    com.CommandText = "SELECT IdEnrollment FROM Enrollment WHERE IdStudy = @idStudy AND Semester = 1";
                    com.Parameters.AddWithValue("idStudy", idStudies);
                    
                    Reader.Close();
                    Reader = com.ExecuteReader();


                    var idEnrollment = -1;
                    if (!Reader.Read())
                    {
                        com.CommandText = "SELECT MAX(IdEnrollment) as IdEnrollment FROM Enrollment";
                        Reader.Close();
                        idEnrollment = Convert.ToInt32(com.ExecuteScalar()) + 1;
                        com.CommandText =  "INSERT INTO Enrollment (IdEnrollment, Semester, IdStudy, StartDate) VALUES (@IdEnrollment , 1, @idStudy, @date)";
                        com.Parameters.AddWithValue("date", DateTime.Now);
                        com.Parameters.AddWithValue("IdEnrollment", idEnrollment);
                        Reader.Close();
                        com.ExecuteNonQuery();
                        
                    }
                    
                    if (idEnrollment == -1)
                        idEnrollment = (int)Reader["IdEnrollment"];

                    com.CommandText = "SELECT 1 FROM Student WHERE IndexNumber = @indexNumber";
                    com.Parameters.AddWithValue("indexNumber", request.IndexNumber);
                    Reader.Close();
                    Reader = com.ExecuteReader();
                    if (Reader.Read())
                    {
                        Reader.Close();
                        transaction.Rollback();
                        return new BadRequestResult();
                    }

                    com.CommandText = "INSERT INTO Student(FirstName, LastName, IndexNumber, BirthDate, IdEnrollment) VALUES (@firstName, @lastName, @indexNumber, @birthDate, @idEnrollment)";
                    com.Parameters.AddWithValue("idEnrollment", idEnrollment);
                    com.Parameters.AddWithValue("birthDate", request.Birthdate.Date);
                    com.Parameters.AddWithValue("indexNumber", request.IndexNumber);
                    com.Parameters.AddWithValue("firstName", request.FirstName);
                    com.Parameters.AddWithValue("lastName", request.LastName);
                    Reader.Close();
                    com.ExecuteNonQuery();
                    transaction.Commit();

                    return new OkObjectResult(new EnrollStudentResponse()
                    {
                        LastName = request.LastName,
                        Semester = 1,
                        StartDate = DateTime.Now
                    }) ;
                }
                catch (SqlException exc)
                {
                    Console.WriteLine(exc.ToString());
                    transaction.Rollback();
                    return new BadRequestResult();
                }
            }
        }

        public IActionResult getStudents()
        {
            var students = new List<Student>();
            
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
                        st.BirthDate = DateTime.Parse(dr["BirthDate"].ToString());

                        students.Add(st);
                    }

                }
            }

            return new OkObjectResult(students);
        }

        public IActionResult PromoteStudents(int semester, string studies)
        {
            using (var con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s18636;Integrated Security=True"))
            using (var com = new SqlCommand())
            {
                com.Connection = con;

                con.Open();
                try {
                    com.CommandText = "SELECT idEnrollmenr FROM enrollment e INNER JOIN studies s ON e.idStudies = s.idStudies WHERE s.name = @studies AND e.semester = @semester";
                    com.Parameters.AddWithValue("studies", studies);
                    com.Parameters.AddWithValue("semester", semester);

                    var Reader = com.ExecuteReader();
                    if (!Reader.Read()) 
                    {
                        Reader.Close();
                        return new BadRequestResult();
                    }

                    com.CommandText = "EXEC promoteStudent(@semester, @studies)";
                    com.Parameters.AddWithValue("semester", semester);
                    com.Parameters.AddWithValue("studies", semester);

                    com.ExecuteNonQuery();

                    return new AcceptedResult();

                }
                catch (SqlException exc)
                {
                    //transaction.Rollback();
                    return new BadRequestResult();
                }
            }
            }
    }
}
