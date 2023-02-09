using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace k190169_Q3
{
    public static class Function1
    {

        [FunctionName("Function1")]
        
        
        
        public static void Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            generateJsonFiles();
        }
        class categoryInfo
        {

            public string scriptName { get; set; }
            public String price { get; set; }

            public void setscriptName(String s)
            {
                scriptName = s;
            }
            public void setprice(String p)
            {
                price = p;
            }
        }

        public static void generateJsonFiles()
        {
            String filePath = "Summary16Oct2022.html";
            //filePath = Console.ReadLine();

            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();

            // There are various options, set as needed
            htmlDoc.OptionFixNestedTags = true;

            // filePath is a path to a file containing the html
            htmlDoc.Load(filePath);

            // Use:  htmlDoc.LoadHtml(xmlString);  to load from a string (was htmlDoc.LoadXML(xmlString)
            List<String> Scripts_Title = new List<String>();
            List<String> Categories = new List<String>();
            List<List<String>> final = new List<List<String>>();
            //List<String> temp1 = new List<String>();
            List<List<String>> prices = new List<List<String>>();

            if (htmlDoc.DocumentNode != null)
            {
                HtmlAgilityPack.HtmlNode bodyNode = htmlDoc.DocumentNode.SelectSingleNode("//body");

                if (bodyNode != null)
                {
                    var snode = htmlDoc.DocumentNode.SelectNodes("//div[@class = 'table-responsive']");
                    var nodes = htmlDoc.DocumentNode.SelectNodes("//th[@colspan =8]");
                    var innerNodes = htmlDoc.DocumentNode.SelectNodes("//td[@class = 'dataportal']");


                    var tempNode = htmlDoc.DocumentNode.SelectNodes("//div[@class = 'table-responsive']//td");
                    List<String> allth = new List<String>();
                    foreach (var node in tempNode)
                    {
                        allth.Add(node.InnerText.ToString().Trim());
                    }


                    foreach (var node in nodes)
                    {
                        String temp = node.InnerText.ToString().Trim();
                        temp = temp.Replace(".", "");
                        temp = temp.Replace("/", "");
                        temp = temp.Replace("-", "");

                        Categories.Add(temp);
                    }
                    foreach (var node in innerNodes)
                    {
                        Scripts_Title.Add(node.InnerText.Trim());

                    }
                    foreach (var node in snode)
                    {

                        List<String> temp = new List<String>();
                        List<String> temp1 = new List<String>();
                        for (int i = 0; i < Scripts_Title.Count; i++)
                        {
                            if (node.InnerText.Contains(Scripts_Title[i]))
                            {
                                temp.Add(Scripts_Title[i]);
                                temp1.Add(allth[allth.IndexOf(Scripts_Title[i].Trim()) + 1]);
                            }
                        }
                        final.Add(temp);
                        prices.Add(temp1);
                    }
                }
            }

            String Xml = "";
            List<String> scripts = new List<String>();
            List<String> temp_prices = new List<String>();
            for (int i = 0; i < 1; i++)
            {
                Xml = "Q2XmlFiles\\" + Categories[i] + ".xml";
                using (XmlReader reader = XmlReader.Create(Xml))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name.ToString())
                            {
                                case "Script":
                                    scripts.Add(reader.ReadString());
                                    break;
                                case "Price":
                                    temp_prices.Add(reader.ReadString());
                                    break;
                            }
                        }
                    }
                }

                for (int j = 0; j < scripts.Count; j++)
                {
                    categoryInfo c = new categoryInfo();
                    c.setprice(temp_prices[j]);
                    c.setscriptName(scripts[j]);
                    var jsonObject = JsonConvert.SerializeObject(c);
                    Console.WriteLine(jsonObject);
                    String path = AppDomain.CurrentDomain.BaseDirectory + "\\Output";
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Output\\" + Categories[i] + "\\" + c.scriptName + ".json";
                    String jsonToWrite = JsonConvert.SerializeObject(c, Newtonsoft.Json.Formatting.Indented);
                    System.IO.File.WriteAllText(filePath,jsonToWrite);

                }


            }
        }




    }
}
