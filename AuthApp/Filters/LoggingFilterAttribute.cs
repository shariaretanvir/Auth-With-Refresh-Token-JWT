using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AuthApp.Filters
{
    public class LoggingFilterAttribute : Attribute, IAsyncResourceFilter
    {
        public ILogger<LoggingFilterAttribute> logger { get; }
        public LoggingFilterAttribute(ILogger<LoggingFilterAttribute> logger)
        {
            this.logger = logger;
        }


        #region logging request and response part


        //When reading from HttpContext.Request.Body via StreamReader() we need to keep a copy of the data
        //and reset the stream position to the beginning so that other request delegates(middleware components) are also able to read from it.
        //If you don't reset the position you will end up with a HTTP 400 or even HTTP 500 response.

        //Also when dealing with the HTTP response HttpContext.Response.Body we need to swap out the stream with one that is buffered
        //and that supports seeking.We are going to use MemoryStream for this purpose.


        //The POST calls need to be taken special care of: since we need to read from the Request body which is a stream.
        //And Streams can be read only once if not enabled Seek(). Hence we enable buffering on the Request,
        //and then read till the end of the body stream. We finally reset the Request body stream to 0 so that it can be read again by the actual endpoint.
        //The read string is saved on to the Payload attribute. We then send the Request off to further processing by calling the next.Invoke()
        //method with the HttpContext as a parameter.


        //We cannot read the Response straight away like we do on the Request, since it will not be available until the processing is done and
        //the next.Invoke() completes its execution.Hence we keep track of the Response by assigning a MemoryStream onto it.
        //And since this stream needs to be disposed properly to avoid memory issues we wrap it up under a using block.
        //The Response is written by the endpoint handler onto the memory stream instance we pass from here (instead of the actual response body)
        //and so we copy the content back to the Response body once the stream is read.
        #endregion

        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            string requestBody = "";
            var request = context.HttpContext.Request;
            if (request.Method == "POST")
            {
                //This line allows us to set the reader for the request back at the beginning of its stream.
                request.EnableBuffering();

                //We now need to read the request stream.  First, we create a new byte[] with the same length as the request stream...
                var buffer = new byte[(int)request.ContentLength];

                //...Then we copy the entire request stream into the new buffer.
                await request.Body.ReadAsync(buffer, 0, buffer.Length);

                //We convert the byte[] into a string using UTF8 encoding...
                requestBody = Encoding.UTF8.GetString(buffer);

                request.Body.Position = 0;
            }
            logger.LogInformation("Start reading request body........");
            logger.LogInformation($"Path : {request.Path} || Method : {request.Method} || RequstBody : {requestBody}");
            logger.LogInformation("End reading request body........");

            var response = context.HttpContext.Response;

            var originalResponse = response.Body;

            //create a new memory stream....
            using (var tempResponse = new MemoryStream())
            {
                //...and use that for the temporary response body
                response.Body = tempResponse;
                response.Body.Position = 0;

                //Continue down the Middleware pipeline, eventually returning to this class
                await next.Invoke();

                //We need to read the response stream from the beginning...
                response.Body.Seek(0, SeekOrigin.Begin);
                
                //...and copy it into a string
                var responseText = await new StreamReader(response.Body).ReadToEndAsync();

                //We need to reset the reader for the response so that the client can read it.
                response.Body.Seek(0, SeekOrigin.Begin);

                logger.LogInformation("Start reading response body........");

                logger.LogInformation($"Response Code {response.StatusCode} || Response Body : {responseText}");

                logger.LogInformation("Start reading response body........");

                //Copy the contents of the new memory stream (which contains the response) to the original stream, which is then returned to the client.
                await tempResponse.CopyToAsync(originalResponse);
            }
        }
    }
}
