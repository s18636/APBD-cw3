using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR.Protocol;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APBD_cw3.middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            httpContext.Request.EnableBuffering();

            if (httpContext.Request != null)
            {
                string method = httpContext.Request.Method.ToString();
                string path = httpContext.Request.Path;
                string bodyString = "";
                string querystring = httpContext.Request?.QueryString.ToString();



                using (StreamReader reader
                 = new StreamReader(httpContext.Request.Body, Encoding.UTF8, true, 1024, true))
                {
                    bodyString = await reader.ReadToEndAsync();
                }

                using (FileStream stream = new FileStream("Logs/requestsLog.txt", FileMode.Append))
                {
                    string stringData = "\nRequest:\nMethod: " + method +
                        "\npath: " + path +
                        "\nbody: " + bodyString +
                        "\nquery string: " + querystring;

                    var data = new UTF8Encoding().GetBytes(stringData);
                    stream.Write(data);
                    stream.Close();
                }



                await _next(httpContext);
            }

        }
    }
}
