using System;
using System.Diagnostics;
using System.Linq;

namespace prime
{
    class Program
    {
        static void Main(string[] args)
        {
            // preset values
            int lastPrime = 0;
            string folderName = "";
            string fileName = "";
            string forSaving;


            // future REFACTOR
            // variable for the next number to check
            int check = 0;

            if (System.IO.File.Exists("settings.json"))
            {
                var settings = System.IO.File.ReadAllLines("settings.json").ToList();
                for (int index = 0; index < settings.Count; index++)
                {
                    if (settings[index].Contains("lastChecked"))
                    {
                        check = int.Parse(settings[index].Split(":")[1].Replace(" ", "").Replace(",", ""));
                        break;
                    }
                }
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
                        if (!System.IO.Directory.Exists(folderName))
                        {
                            System.IO.Directory.CreateDirectory(folderName);
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
                        if (!System.IO.File.Exists(folderName + "\\" + fileName))
                        {
                            System.IO.File.WriteAllText(folderName + "\\" + fileName, "[\n]");
                        }

                        // future REFACTOR
                        // prime to save with additional info
                        forSaving = "   {\"value\": " + check.ToString() + ", \"isPrime\": true}";


                        // saving file to the local API
                        var txtLines = System.IO.File.ReadAllLines(folderName + "\\" + fileName).ToList();
                        txtLines.Insert(txtLines.Count - 1, forSaving);
                        System.IO.File.WriteAllLines(folderName + "\\" + fileName, txtLines);

                        lastPrime = check;
                    }


                    // future REFACTOR
                    // saving settings
                    if (passed || check % 100 == 0)
                    {
                        // making file for settings
                        if (!System.IO.File.Exists("settings.json"))
                        {
                            System.IO.File.WriteAllText("settings.json", "");
                        }


                        // settings
                        forSaving = "[\n    \"lastChecked\": " + check.ToString() + ",\n    \"lastPrime\": " + lastPrime.ToString() + "\n]";


                        // read settings
                        if (System.IO.File.Exists(".exit"))
                        {
                            System.IO.File.Delete(".exit");
                            break;
                        }


                        // saving data to the settings
                        System.IO.File.WriteAllText("settings.json", forSaving);
                    }

                    if (check % 100000 == 0)
                    {
                        Console.ReadLine();
                    }

                    if (check % 1000 == 0)
                    {
                        Console.WriteLine(check);
                    }


                        // future BUGFIX
                        // saving to API with Git Commit
                        if ((DateTime.UtcNow - sinceLastSave).TotalHours > 0.5) // in every 0.5 hours
                    {
                        Process currentProcess;
                        
                        currentProcess = Process.Start("git", "add --all");
                        currentProcess.WaitForExit();

                        currentProcess = Process.Start("git", "commit -m \"up to " + folderName + "\\" + fileName + "000\"");
                        currentProcess.WaitForExit();

                        currentProcess = Process.Start("git", "push");
                        currentProcess.WaitForExit();
                    }

                }
                check++; // looping through
            }
        }
    }
}
