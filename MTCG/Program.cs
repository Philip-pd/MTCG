using System;
using MTCG.SystemLogicClasses;


namespace MTCG
{
    class Program
    {
        static void Main(string[] args)//Basically just for testing until we get REST
        {
            Console.WriteLine("Starting server on port 8080"); //starts the server
            HTTPServer server = new HTTPServer(8080);
            server.Start();
        }
    }
}
