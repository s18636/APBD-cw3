using APBD_cw3.DTOs.Requests;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Wyklad5.Services;

namespace APBD_cw3.Handlers
{
    public class AuthHandler
    {
        private IStudentDbService _dbService;
        public AuthHandler(IStudentDbService dbService)
        {
            _dbService = dbService;
        }
        public IActionResult HandleAuthenticateAsync(LoginRequest loginRequest)
        {
            if (!_dbService.CheckCredential(loginRequest.login, loginRequest.password)) 
            {
                return new BadRequestResult();
            }

            return new AcceptedResult();
        }
    }
}
