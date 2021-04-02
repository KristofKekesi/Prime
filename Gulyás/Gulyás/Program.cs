using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace prime
{
    class Program
    {
        // LOG System
        private void Log(string log)
        {
            string logFile = "log.txt";
            Console.WriteLine(logFile + log);
        }

        static void Main(string[] args)
        {
            // preset values
            int lastPrime = 0;
            string folderName = "";
            string fileName = "";
            string forSaving;


            // variable for the next number to check
            int check = 0;
            if (File.Exists("settings.json"))
            {
                JObject settingsJson = JObject.Parse(File.ReadAllText("settings.json"));

                check = (int)settingsJson.SelectToken("lastChecked");
            }

            // starting timer
            DateTime sinceLastSave = DateTime.UtcNow;


            // main loop 
            while (true)
            {
                // if number is 1 or below it is not a prime number
                if (check > 1)
                {
                    bool passed = true; // if still true at the end it is prime
                    for (int index = 2; index <= check; index++)
                    {
                        if (index != 1 && index != check) // a prime is just divisible by 1 or itself
                        {
                            if (check % index == 0)
                            {
                                passed = false; // not a prime
                                break;
                            }
                        }
                    }

                    // saving prime number
                    if (passed)
                    {

                        // folder name
                        // folder number made by the millionth place value
                        if (check.ToString().Length > 6)
                        {
                            folderName = check.ToString().Remove(check.ToString().Length - 6);
                        } else
                        {
                            folderName = "0";
                        }
                        

                        // making folder
                        if (!Directory.Exists(folderName))
                        {
                            Directory.CreateDirectory(folderName);
                        }


                        // file name
                        // folder number made by the thousand place value
                        string checkString;
                        if (check.ToString().Length > 6)
                        {
                            checkString = check.ToString()[^6..].TrimStart(new Char[] { '0' });
                        }
                        else {
                            checkString = check.ToString();
                        }

                        if (checkString.Length > 3)
                        {
                            fileName = checkString.Remove(checkString.Length - 3) + "000-" + (int.Parse(checkString.Remove(checkString.Length - 3)) + 1 ).ToString() + "000.json";
                        } 
                        else
                        {
                            fileName = "0-1000.json";
                        }


                        // making file
                        if (!File.Exists(folderName + "\\" + fileName))
                        {
                            File.WriteAllText(folderName + "\\" + fileName, "[\n]");
                        }

                        // future REFACTOR
                        // prime to save with additional info
                        forSaving = "   {\"value\": " + check.ToString() + ", \"isPrime\": true}";


                        // saving file to the local API
                        var txtLines = File.ReadAllLines(folderName + "\\" + fileName).ToList();
                        txtLines.Insert(txtLines.Count - 1, forSaving);
                        File.WriteAllLines(folderName + "\\" + fileName, txtLines);

                        lastPrime = check;
                    }


                    // future REFACTOR
                    // saving settings
                    if (passed || check % 100 == 0)
                    {
                        // making file for settings
                        if (!File.Exists("settings.json"))
                        {
                            File.WriteAllText("settings.json", "");
                        }


                        // settings
                        StringBuilder final = new StringBuilder();
                        using (JsonWriter writer = new JsonTextWriter(new StringWriter(final)))
                        {
                            writer.Formatting = Formatting.Indented;

                            writer.WriteStartObject();
                            writer.WritePropertyName("lastChecked");
                            writer.WriteValue(check.ToString());
                            writer.WritePropertyName("lastPrime");
                            writer.WriteValue(lastPrime.ToString());
                            writer.WriteEndObject();
                        }


                        // saving data to the settings
                        File.WriteAllText("settings.json", final.ToString());


                        // read settings
                        if (File.Exists(".exit"))
                        {
                            File.Delete(".exit");
                            break;
                        }
                    }


                        // saving to APIs with Git Commit
                        if ((DateTime.UtcNow - sinceLastSave).TotalHours > 0.5) // in every 0.5 hours
                    {
                        Process currentProcess;
                        
                        currentProcess = Process.Start("git", "add --all");
                        currentProcess.WaitForExit();

                        currentProcess = Process.Start("git", "commit -m \"max: " + folderName + "\\" + fileName + "\"");
                        currentProcess.WaitForExit();

                        currentProcess = Process.Start("git", "push");
                        currentProcess.WaitForExit();

                        sinceLastSave = DateTime.UtcNow;
                    }

                }
                check++; // looping through
            }
        }
    }
}
