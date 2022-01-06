using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

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
        public static Request GetRequest(String request,string token,string body) //Returns Request with all parameters worked out
        {
            if (String.IsNullOrEmpty(request))
                return null;
            String[] tokens = request.Split(' ', '\n');
            string type = tokens[0];
            string url = tokens[1];
            string host = tokens[4];
            string source = tokens[6]; //use this to find out what to do next
            String[] parametres = new string[4]; //make cases for insomnia and curl cause they handle header differently
            String[] bodyparts = BreakUpBody(body); //gets the variables out of the JSON

            if (!String.IsNullOrEmpty(token) &&  bodyparts.Length>=4) // request with body params & token in header
            {
                parametres[0] = "Auth:";
                parametres[1] = token;
                parametres[2] = "body:";
                parametres[3] = bodyparts[4]; //all requests with token only use 1 bodyparts element;

                Console.WriteLine(String.Format("{0} {1} @ {2}", type, url, host));
                return new Request(type, url, host, parametres); //actually creates the request as we now have all params
            }else if(String.IsNullOrEmpty(token) && bodyparts.Length >= 9) //request without token but with body params //basically only signup
            {
                parametres[0] = "Username:";
                parametres[1] = bodyparts[4];
                parametres[2] = "Password:";
                parametres[3] = bodyparts[9];

                Console.WriteLine(String.Format("{0} {1} @ {2}", type, url, host));
                return new Request(type, url, host, parametres); //actually creates the request as we now have all params
            }
            else  //Request with either no body or neither body nor token. Can still write empty string to param 1
            {
                parametres[0] = "Auth:";
                parametres[1] = token;

                Console.WriteLine(String.Format("{0} {1} @ {2}", type, url, host));
                return new Request(type, url, host, parametres); //actually creates the request as we now have all params
            }

        }
        private static String[] BreakUpBody(string body) //splits the json manually since we don't know what class it would be yet
        {
            String[] bodyparts = body.Split(':','\"');

            return bodyparts;
        }
    }

    
}
