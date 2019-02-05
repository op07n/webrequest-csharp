using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;


/*****

From    https://codesamplez.com/programming/http-request-c-sharp



Using This C# HTTP Request Class:

Implementing this class to an application is quite easy. First you have to create an instance of the class and then call a parameter less function to receive the response data. So, all things to feed it is in the constructor calling. There are three different type of constructor you can call. One with one parameter(web resource url), it simply download the data of the web page. Second one with 2 parameters(url and method of request,get or post), actually, this is the one, you won’t use anytime for this version of the class, as without data post type is meaning less(I have kept it to be it as modular and so that calling constructor can be flexible enough and other parameters also can be set by creating properties if needed). Third one, with 3 parameters(url, method and data).

For url parameter, you must have to use a valid uri. For method parameter, you have to use “GET” or “POST” depending on your request type. Third parameter should be all data url encoded should be like this format:
“variable1=value1&variable2=value2”

Here is a sample code snippet to make a complete request and get the string response:

//create the constructor with post type and few data
MyWebRequest myRequest = new MyWebRequest("http://www.yourdomain.com","POST","a=value1&b=value2");
//show the response string on the console screen.
Console.WriteLine(myRequest.GetResponse());

********/



namespace WpfTestApplication
{
    public class MyWebRequest
    {
        private WebRequest request;
        private Stream dataStream;

        private string status;

        public String Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
            }
        }

        public MyWebRequest(string url)
        {
            // Create a request using a URL that can receive a post.

            request = WebRequest.Create(url);
        }

        public MyWebRequest(string url, string method)
            : this(url)
        {

            if (method.Equals("GET") || method.Equals("POST"))
            {
                // Set the Method property of the request to POST.
                request.Method = method;
            }
            else
            {
                throw new Exception("Invalid Method Type");
            }
        }

        public MyWebRequest(string url, string method, string data)
            : this(url, method)
        {

            // Create POST data and convert it to a byte array.
            string postData = data;
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            // Set the ContentType property of the WebRequest.
            request.ContentType = "application/x-www-form-urlencoded";

            // Set the ContentLength property of the WebRequest.
            request.ContentLength = byteArray.Length;

            // Get the request stream.
            dataStream = request.GetRequestStream();

            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);

            // Close the Stream object.
            dataStream.Close();

        }

        public string GetResponse()
        {
            // Get the original response.
            WebResponse response = request.GetResponse();

            this.Status = ((HttpWebResponse)response).StatusDescription;

            // Get the stream containing all content returned by the requested server.
            dataStream = response.GetResponseStream();

            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);

            // Read the content fully up to the end.
            string responseFromServer = reader.ReadToEnd();

            // Clean up the streams.
            reader.Close();
            dataStream.Close();
            response.Close();

            return responseFromServer;
        }

    }
}
