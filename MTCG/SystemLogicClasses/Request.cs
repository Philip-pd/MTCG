using System;
using System.Collections.Generic;
using System.Text;

namespace MTCG.SystemLogicClasses
{
    public class Request //Public so we can access GetRequest
    {
        public string Type { get; set; }
        public string URL { get; set; }
        public string Host { get; set; }
        public String[] parametres { get; set; }
        private Request(string type, string url, string host, String[] parametres) //Private cause GetRequest generates
        {
            this.Type = type;
            this.URL = url;
            this.Host = host;
            this.parametres = parametres;
        }
        public static Request GetRequest(String request) //Returns Request with all parameters worked out
        {
            if (String.IsNullOrEmpty(request))
                return null;
            String[] tokens = request.Split(' ', '\n');
            string type = tokens[0];
            string url = tokens[1];
            string host = tokens[4];
            string source = tokens[6];
            String[] parametres = null; //make cases for insomnia and curl cause they handle header differently

            if (type == "GET") //Get Parametres are in adress bar But you will neeed the 1 header with the token
            {
                parametres = new String[tokens.Length - 7]; //position of Accept */* +1 manuell 
                Array.Copy(tokens, 7, parametres, 0, tokens.Length - 7);
                Console.WriteLine(String.Format("{0} {1} @ {2}", type, url, host));
                return new Request(type, url, host, parametres);
            }
            parametres = new String[tokens.Length - 7]; //position of Accept */* +1 manuell
            Array.Copy(tokens, 7, parametres, 0, tokens.Length - 7);
            Console.WriteLine(String.Format("{0} {1} @ {2}", type, url, host));
            return new Request(type, url, host, parametres);

        }
    }
}
