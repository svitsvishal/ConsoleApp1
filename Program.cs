using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Data;
using System.Reflection.Metadata;
using System.Text;

namespace WebinarApi
{

    public class Registrant
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Company { get; set; }
        public string JobTitle { get; set; }
        public string PhoneNumber { get; set; }
        public string Country { get; set; }
        public string StateProvince { get; set; }
        public string Promocode { get; set; }
        public string Sector { get; set; }
        public string CampaignCode { get; set; }
        public string RegistrationSource { get; set; }
        public string RegistrationDate { get; set; }
        public string IpAddress { get; set; }
    }
    public class LatestSchedule
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string TimeZone { get; set; }
    }

    public class Urls
    {
        public string Audience { get; set; }
        public string Presenter { get; set; }
        public string AutoJoin { get; set; }
        public string Integration { get; set; }
        public string Reporting { get; set; }
    }

    public class Webinar
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string WebinarKey { get; set; }
        public string Language { get; set; }
        public string Source { get; set; }
        public LatestSchedule LatestSchedule { get; set; }
        public Urls Urls { get; set; }
        public bool ScreenSharingEnabled { get; set; }
        public bool BilingualEnabled { get; set; }
        public bool SubtitleAutoGenerationEnabled { get; set; }
        public bool GreenRoomEnabled { get; set; }
    }

    public class WebinarResponse
    {
        public List<Webinar> Webinars { get; set; }
    }
    public class Program
    {
        public static async Task Main(string[] args)
        {
           await Getregistrants();
         //   await GetAllUserList();
            //string userList = await GetAllUserList();
            //WebinarResponse response = JsonConvert.DeserializeObject<WebinarResponse>(userList);

            //foreach (var webinar in response.Webinars)
            //{
            //    Console.WriteLine($"Title: {webinar.Title}");
            //    Console.WriteLine($"ID: {webinar.Id}");
            //    Console.WriteLine($"Language: {webinar.Language}");
            //    Console.WriteLine($"Start: {webinar.LatestSchedule.Start}");
            //    Console.WriteLine($"End: {webinar.LatestSchedule.End}");
            //    Console.WriteLine($"Audience URL: {webinar.Urls.Audience}");
            //    Console.WriteLine("---------------");
            //}
            ////  Console.WriteLine(userList);
            //var jsonResponse = JObject.Parse(userList);
            //var hasMore = jsonResponse["hasMore"]?.Value<bool>() ?? false;

            Console.ReadKey();

        }

        public static async Task GetAllUserList()
        {
            bool hasMore = true;
            int pageIndex = 1;
            string filePath = $@"H:\1014\DATAREQ\316\Webinars_final.csv";
            while (hasMore)
            {


                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.webinar.net/v2/webinars?pageSize=100&pageIndex={pageIndex}");
                request.Headers.Add("Authorization", "Basic OGY2OTk3ZmUtZTM0Yi00MDcxLWJmNzAtMWZhNjk0ZWI3ZDM1OmFHcjVlUVl3QkFq");
                request.Headers.Add("Cookie", "AWSALB=gunnEdqZaRAJrZNJLqjDO3DXb5OKNHNKaHLYMKT614punJDHNB9mPJMNj9iGg82wFXsYsBq3NbknLuonD8TSFswxWHqtKzlVG4r98JqHGAI80zUXdSA/joDWWYJQ; AWSALBCORS=gunnEdqZaRAJrZNJLqjDO3DXb5OKNHNKaHLYMKT614punJDHNB9mPJMNj9iGg82wFXsYsBq3NbknLuonD8TSFswxWHqtKzlVG4r98JqHGAI80zUXdSA/joDWWYJQ");
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                // Console.WriteLine(await response.Content.ReadAsStringAsync());

                var content = await response.Content.ReadAsStringAsync();
                var jsonObject = JsonConvert.DeserializeObject(content);

                var jsonResponse = JObject.Parse(content);

                // Process the data you want (e.g., log it)
                Console.WriteLine(jsonResponse);
                StringBuilder csvContent = new StringBuilder();
                // string userList = await GetAllUserList();
                WebinarResponse response1 = JsonConvert.DeserializeObject<WebinarResponse>(content);
                csvContent.AppendLine("ID,Title,Language,Start Time,End Time,Audience URL");
                foreach (var webinar in response1.Webinars)
                {
                    Console.WriteLine($"Title: {webinar.Title}");
                    Console.WriteLine($"ID: {webinar.Id}");
                    Console.WriteLine($"Language: {webinar.Language}");
                    Console.WriteLine($"Start: {webinar.LatestSchedule.Start}");
                    Console.WriteLine($"End: {webinar.LatestSchedule.End}");
                    Console.WriteLine($"Audience URL: {webinar.Urls.Audience}");
                    Console.WriteLine("---------------");
                 
                }
                //  Console.WriteLine(userList);
                // var jsonResponse = JObject.Parse(userList);
                //  var hasMore = jsonResponse["hasMore"]?.Value<bool>() ?? false;


                // Create a StringBuilder for CSV data


                // CSV header
                bool fileExists = File.Exists(filePath);

                using (StreamWriter writer = new StreamWriter(filePath, append: true, Encoding.UTF8))
                {
                    // Write header only if the file is new
                    if (!fileExists)
                    {
                        writer.WriteLine("ID,Title,Language,Start Time,End Time,Audience URL");
                    }

                    int i = 0;
                    while (i < response1.Webinars.Count)
                    {
                        var webinar = response1.Webinars[i];
                        writer.WriteLine($"{webinar.Id},{webinar.Title},{webinar.Language},{webinar.LatestSchedule.Start},{webinar.LatestSchedule.End},{webinar.Urls.Audience}");
                        i++; // Increment the index
                    }
                }

                //  Loop through webinars and add data
                //foreach (var webinar in response1.Webinars)
                //{
                //    csvContent.AppendLine($"{webinar.Id},{webinar.Title},{webinar.Language},{webinar.LatestSchedule.Start},{webinar.LatestSchedule.End},{webinar.Urls.Audience}");
                //}

                //// Write to CSV file
                //File.WriteAllText(filePath, csvContent.ToString());

                Console.WriteLine($"CSV file saved at {filePath}");
                // Check the "hasMore" tag in the response
                hasMore = jsonResponse["hasMore"]?.Value<bool>() ?? false;
                Console.WriteLine(jsonResponse["pageIndex"].ToString());
                pageIndex = pageIndex + 1;
            }



          //  return await response.Content.ReadAsStringAsync();


        }

        public static void ConvetBS()
        {
        //    var settings = new JsonSerializerSettings
        //    {
        //        NullValueHandling = NullValueHandling.Ignore,
        //        MissingMemberHandling = MissingMemberHandling.Ignore
        //    };


        //    var client = new RestClient("https://api.webinar.net/v2/webinars/6061bdeb37ea2e68c0f21676");
        //    client.Timeout = -1;
        //    var request = new RestRequest(Method.GET);
        //    string svcCredentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes("8f6997fe-e34b-4071-bf70-1fa694eb7d35" + ":" + "aGr5eQYwBAj"));
        //    request.AddHeader("Authorization", "Basic " + svcCredentials);
        //    request.AddHeader("Authorization", "Basic aGr5eQYwBAj");
        //    //request.AddHeader("Accept", "application/vnd.Creative Force.v2.1+json");
        //    request.AddHeader("api_key", "8f6997fe-e34b-4071-bf70-1fa694eb7d35");
        //    request.AddHeader("x-api-language", "en_GB");
        //    IRestResponse response = client.Execute(request);
        //    Console.WriteLine(response.Content);
        }

        public static async Task Getregistrants()
        {
            string filePath = $@"H:\1014\DATAREQ\316\Webinars_final.csv"; // Change to your actual file path

            using (StreamReader reader = new StreamReader(filePath))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] values = line.Split(','); // Assuming CSV values are comma-separated

                    Console.WriteLine($"Column 1: {values[0]}, Column 2: {values[1]}");
                    if (values[0] !="ID")
                    { await FetchAndSaveRegistrantsToCsv( values[0].Trim(), values[1].Trim()); }
                   
                }
            }

            //var client = new HttpClient();
            //var request = new HttpRequestMessage(HttpMethod.Get, "https://api.webinar.net/v2/webinars/67dd689a275ab472115a1c49/registrants/");
            //request.Headers.Add("Authorization", "Basic OGY2OTk3ZmUtZTM0Yi00MDcxLWJmNzAtMWZhNjk0ZWI3ZDM1OmFHcjVlUVl3QkFq");
            //request.Headers.Add("Cookie", "AWSALB=KqgZFtJHRA35H3JONovPlRoM7nY3hiAdZtR5NdtoHsMyUgpQxIiPNV7zSXEGODMHcQBolTJJzgaBEgB7OhL/pzslOiAsyDtIL6UtqrIhGv0JvfhZbWCj6LZVV1xg; AWSALBCORS=KqgZFtJHRA35H3JONovPlRoM7nY3hiAdZtR5NdtoHsMyUgpQxIiPNV7zSXEGODMHcQBolTJJzgaBEgB7OhL/pzslOiAsyDtIL6UtqrIhGv0JvfhZbWCj6LZVV1xg");
            //var response = await client.SendAsync(request);
            //response.EnsureSuccessStatusCode();
            //Console.WriteLine(await response.Content.ReadAsStringAsync());

        }

        public static async Task FetchAndSaveRegistrantsToCsv(string webinarID,string webinarTitle)
        {
           // webinarID = "6703f4b8984dc17509cea4b7";
            var client = new HttpClient();
            int pageindex = 1;
         
            bool hasMore = true;
            List<Registrant> registrantsList = new List<Registrant>();

            while (hasMore)
            {
                string url = $"https://api.webinar.net/v2/webinars/{webinarID}/registrants?pageIndex={pageindex}";
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Add("Authorization", "Basic OGY2OTk3ZmUtZTM0Yi00MDcxLWJmNzAtMWZhNjk0ZWI3ZDM1OmFHcjVlUVl3QkFq");

                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();

                var jsonResponse = JObject.Parse(responseContent);

                // Deserialize registrants list
                var registrants = jsonResponse["registrants"]?.ToObject<List<Registrant>>();
                if (registrants != null)
                {
                    registrantsList.AddRange(registrants);
                }

                // Check if more data is available
                hasMore = jsonResponse["hasMore"]?.Value<bool>() ?? false;
                pageindex++;
               
                // Optional: Handle pagination if API provides a next page URL
                // url = jsonResponse["nextPageUrl"]?.Value<string>() ?? url;
            }

            // Save to CSV (append mode)
            SaveToCsv(registrantsList, webinarID, webinarTitle);

        }

        public static void SaveToCsv(List<Registrant> registrants ,string webinarID ,string webinarTitle)
        {
           //string filePath = "$@\"H:\\1014\\DATAREQ\\316\\registrants.csv";
            string filePath = $@"H:\1014\DATAREQ\316\registrants_final.csv";
            bool fileExists = File.Exists(filePath);

            using (var writer = new StreamWriter(filePath, append: true))
            {
                // Write header row only if the file does not exist
                if (!fileExists)
                {
                    writer.WriteLine("Id,FirstName,LastName,EmailAddress,Company,JobTitle,PhoneNumber,Country,StateProvince,Promocode,Sector,CampaignCode,RegistrationSource,RegistrationDate,IpAddress,webinarID,webinarTitle");
                }

                // Write data rows (appended)
                foreach (var reg in registrants)
                {
                    Console.WriteLine($" {reg.EmailAddress} , {webinarTitle} ");
                    writer.WriteLine($"{reg.Id},{reg.FirstName},{reg.LastName},{reg.EmailAddress},{reg.Company},{reg.JobTitle},{reg.PhoneNumber},{reg.Country},{reg.StateProvince},{reg.Promocode},{reg.Sector},{reg.CampaignCode},{reg.RegistrationSource},{reg.RegistrationDate},{reg.IpAddress},{webinarID},{webinarTitle}");
                }
            }

            Console.WriteLine($"Data appended to CSV file: {filePath}");
        }
    }
    }
