using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Data;
using System.Reflection.Metadata;
using System.Text;
using ClosedXML.Excel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
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
        public string Jobarea { get; set; }
        public string Jobfunction { get; set; }
        
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
            // webinarID = "67dc72deab4ca869633d7f0d";
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
          //  SaveToCsv(registrantsList, webinarID, webinarTitle);
            SaveToExcel(registrantsList, webinarID, webinarTitle);

        }

        public static void SaveToCsv(List<Registrant> registrants ,string webinarID ,string webinarTitle)
        {
           //string filePath = "$@\"H:\\1014\\DATAREQ\\316\\registrants.csv";
            string filePath = $@"H:\1014\DATAREQ\316\registrants_final_14Arp01.csv";
            bool fileExists = File.Exists(filePath);

            using (var writer = new StreamWriter(filePath, append: true))
            {
                // Write header row only if the file does not exist
                if (!fileExists)
                {
                    writer.WriteLine("Id,FirstName,LastName,EmailAddress,Company,JobTitle,Jobarea,Jobfunction,PhoneNumber,Country,StateProvince,Promocode,Sector,CampaignCode,RegistrationSource,RegistrationDate,IpAddress,webinarID,webinarTitle");
                }

                // Write data rows (appended)
                foreach (var reg in registrants)
                {
                    Console.WriteLine($" {reg.EmailAddress} , {webinarTitle} ");
                   
                        writer.WriteLine($"{reg.Id},{reg.FirstName},{reg.LastName},{reg.EmailAddress},{reg.Company},{reg.Jobarea},{reg.Jobfunction},{reg.JobTitle},{reg.PhoneNumber},{reg.Country},{reg.StateProvince},{reg.Promocode},{reg.Sector},{reg.CampaignCode},{reg.RegistrationSource},{reg.RegistrationDate},{reg.IpAddress},{webinarID},{webinarTitle}");
                                           
                }
            }

            Console.WriteLine($"Data appended to CSV file: {filePath}");
        }



        public static void SaveToExcel(List<Registrant> registrants, string webinarID, string webinarTitle)
        {
            string filePath = $@"H:\1014\DATAREQ\316\registrants_final_14Arp01_new.xlsx";
            bool fileExists = File.Exists(filePath);

            XSSFWorkbook workbook;
            ISheet sheet;
            int startRow;

            if (fileExists)
            {
                // Open existing workbook
                using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    workbook = new XSSFWorkbook(fs);
                }

                // Get the first sheet or create it if it doesn't exist
                sheet = workbook.GetSheetAt(0) ?? workbook.CreateSheet("Registrants");

                // Find the last row with data
                startRow = sheet.LastRowNum + 1;

                // If the sheet is empty or only has headers, set startRow appropriately
                if (startRow <= 0)
                {
                    AddHeaders(workbook, sheet);
                    startRow = 1; // Start at row 1 (after headers, 0-based index)
                }
            }
            else
            {
                // Create a new workbook
                workbook = new XSSFWorkbook();
                sheet = workbook.CreateSheet("Registrants");

                // Add headers
                AddHeaders(workbook, sheet);

                // Start at row 1 (after headers, 0-based index)
                startRow = 1;
            }

            // Add data rows
            foreach (var reg in registrants)
            {
                Console.WriteLine($" {reg.EmailAddress} , {webinarTitle} ");

                var row = sheet.CreateRow(startRow++);

                row.CreateCell(0).SetCellValue(reg.Id);
                row.CreateCell(1).SetCellValue(reg.FirstName);
                row.CreateCell(2).SetCellValue(reg.LastName);
                row.CreateCell(3).SetCellValue(reg.EmailAddress);
                row.CreateCell(4).SetCellValue(reg.Company);
                row.CreateCell(5).SetCellValue(reg.JobTitle);
                row.CreateCell(6).SetCellValue(reg.Jobarea);
                row.CreateCell(7).SetCellValue(reg.Jobfunction);
                row.CreateCell(8).SetCellValue(reg.PhoneNumber);
                row.CreateCell(9).SetCellValue(reg.Country);
                row.CreateCell(10).SetCellValue(reg.StateProvince);
                row.CreateCell(11).SetCellValue(reg.Promocode);
                row.CreateCell(12).SetCellValue(reg.Sector);
                row.CreateCell(13).SetCellValue(reg.CampaignCode);
                row.CreateCell(14).SetCellValue(reg.RegistrationSource);
                row.CreateCell(15).SetCellValue(reg.RegistrationDate != null ? reg.RegistrationDate.ToString() : "");
                row.CreateCell(16).SetCellValue(reg.IpAddress);
                row.CreateCell(17).SetCellValue(webinarID);
                row.CreateCell(18).SetCellValue(webinarTitle);
            }

            // Auto-size columns
            for (int i = 0; i < 19; i++)
            {
                sheet.AutoSizeColumn(i);
            }

            // Write to file
            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(fileStream);
            }

            Console.WriteLine($"Data appended to Excel file: {filePath}");
        }

        // Helper method to add headers to the sheet
        private static void AddHeaders(XSSFWorkbook workbook, ISheet sheet)
        {
            string[] headers = new string[] {
        "Id", "FirstName", "LastName", "EmailAddress", "Company", "JobTitle",
        "Jobarea", "Jobfunction", "PhoneNumber", "Country", "StateProvince",
        "Promocode", "Sector", "CampaignCode", "RegistrationSource",
        "RegistrationDate", "IpAddress", "webinarID", "webinarTitle"
    };

            // Create a bold font style for headers
            var headerFont = workbook.CreateFont();
            headerFont.IsBold = true;

            var headerStyle = workbook.CreateCellStyle();
            headerStyle.SetFont(headerFont);

            // Create header row
            var headerRow = sheet.CreateRow(0);

            // Add headers
            for (int i = 0; i < headers.Length; i++)
            {
                var cell = headerRow.CreateCell(i);
                cell.SetCellValue(headers[i]);
                cell.CellStyle = headerStyle;
            }
        }
        }
    }
