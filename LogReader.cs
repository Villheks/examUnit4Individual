using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace LogSummarizer
{
    class Program
    {
        static void Main(string[] args)
        {
            // Check if the correct argument is provided
            if (args.Length != 1 || args[0] != "-A")
            {
                Console.WriteLine("Usage: dotnet run -A");
                return;
            }

            // Define the log file name
            string logFileName = "simon_game_log.txt";

            // Check if the log file exists
            if (!File.Exists(logFileName))
            {
                Console.WriteLine("Error: Log file not found.");
                return;
            }

            // Read the log file and summarize its content
            try
            {
                Dictionary<string, int> buttonPressCounts = new Dictionary<string, int>();
                int roundsFinished = 0;
                int buttonsPressed = 0;

                using (StreamReader reader = new StreamReader(logFileName))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.Contains("Rounds Finished"))
                        {
                            // Extract rounds finished and buttons pressed using regex
                            Match match = Regex.Match(line, @"Rounds Finished: (\d+), Buttons Pressed: (\d+)");
                            if (match.Success)
                            {
                                roundsFinished += int.Parse(match.Groups[1].Value);
                                buttonsPressed += int.Parse(match.Groups[2].Value);
                            }
                        }
                        else if (line.Contains("Button"))
                        {
                            // Extract button color using regex
                            Match match = Regex.Match(line, @"Button (\w+) Pressed");
                            if (match.Success)
                            {
                                string buttonColor = match.Groups[1].Value;
                                if (buttonPressCounts.ContainsKey(buttonColor))
                                {
                                    buttonPressCounts[buttonColor]++;
                                }
                                else
                                {
                                    buttonPressCounts[buttonColor] = 1;
                                }
                            }
                        }
                    }
                }

                // Display summary
                Console.WriteLine($"Total Rounds Finished: {roundsFinished}");
                Console.WriteLine($"Total Buttons Pressed: {buttonsPressed}");
                Console.WriteLine("Button Press Counts:");
                foreach (var kvp in buttonPressCounts)
                {
                    Console.WriteLine($"{kvp.Key}: {kvp.Value}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error summarizing log file: {ex.Message}");
            }
        }
    }
}
