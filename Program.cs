using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TextFilter
{
    class Program
    {
        private static bool fileNameEntered;

        static void Main()
        {
            string[] files = Directory.GetFiles(@"D:\personal", "*.*",SearchOption.AllDirectories);
            var a= Directory.GetDirectories(@"C:\Users\akumar2\Documents");
            var data = JsonReader.ReadJson("app.json");

            data = GetFiles(data);

            GetTextFromFolder(data);

            //try
            //{

            //    try
            //    {
            //        foreach (var item in data)
            //        {
            //            {
            //                try
            //                {
            //                    GetTextFromFile(item.Source, item.Destination, item.Keyword);
            //                }
            //                catch (Exception ex)
            //                {
            //                    LogError(ex);
            //                }
            //            }
            //            //);
            //        }
            //    }
            //    catch (AggregateException ex)
            //    {
            //        foreach (var innerEx in ex.InnerExceptions)
            //        {
            //            LogError(innerEx);
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    LogError(ex);
            //}
        }

        static void GetTextFromFolder(List<SourceDestination> data)
        {
            try
            {

                try
                {
                    // Parallel.ForEach(data, item =>
                    foreach (var item in data)
                    {
                        {
                            try
                            {
                                GetTextFromFile(item.Source, item.Destination,item.Keyword);
                            }
                            catch (Exception ex)
                            {
                                LogError(ex);
                            }
                        }
                        //);
                    }
                }
                catch (AggregateException ex)
                {
                    foreach (var innerEx in ex.InnerExceptions)
                    {
                        LogError(innerEx);
                    }
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

            static void GetTextFromFile(string SourceFile, string DestinationFile, string[] Keyword)
        {
            try
            {
                   fileNameEntered = false;
                   DestinationFile = GetDestinationFile(DestinationFile, SourceFile);
            
                    string[] Lines = File.ReadAllLines(SourceFile);
              
                foreach (string line in Lines)
                    {
                        // if (!string.IsNullOrWhiteSpace(Keyword))
                        if (Keyword.Length > 0)
                        {
                            //    if (line.ToLower().Contains(Keyword.ToLower()))
                            if (IsKeywordMatching(line, Keyword))
                            {
                                Console.WriteLine($"Filename: {SourceFile}");

                                Console.WriteLine(line);

                            if (!fileNameEntered)
                            {
                                File.AppendAllText(DestinationFile, "filename: " + SourceFile + Environment.NewLine);
                                fileNameEntered = true;
                            }
                            File.AppendAllText(DestinationFile, line + Environment.NewLine);
                            }
                        }
                        else
                        {
                            Console.WriteLine(line);
                            File.AppendAllText(DestinationFile, line + Environment.NewLine);
                        }
                    }
              //  }
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }
        static string GetDestinationFile(string destFilename, string sourceFile)
        {
            if (string.IsNullOrWhiteSpace(destFilename))
            {
                return Path.GetFileName(sourceFile);
            }
            string value = null;
            var ext = Path.GetExtension(destFilename);

            if (!string.IsNullOrWhiteSpace(ext))
            {
                return destFilename;
            }

            else
            {
                value = destFilename + "\\" + Path.GetFileName(sourceFile);
            }

            return value;
        }

        static void LogError(Exception ex)
        {
            string errorMessage = $"{DateTime.Now} An error occurred: {ex.Message}";
            Console.WriteLine(errorMessage);
            File.AppendAllText("error.txt", errorMessage + Environment.NewLine);
        }

        static List<SourceDestination> GetFiles(List<SourceDestination> path)
        {
            List<SourceDestination> Files = new List<SourceDestination>();
            foreach (var item in path)
            {
                if (item.Source.Contains("*"))
                {
                    var index = item.Source.LastIndexOf('\\');
                    var folderPath = item.Source.Remove(index);
                    //    var files = Directory.GetFiles(folderPath);
                    var files = Directory.GetFiles(folderPath, "*.txt", SearchOption.AllDirectories);
                    var like = item.Source.Substring(index);
                    like = like.Replace('*', ' ').Replace('\\', ' ').Trim();
                    foreach (var item3 in files)
                    {
                        if (Path.GetFileName(item3).Contains(like))
                        {
                            Files.Add(new SourceDestination() { Source = item3, Destination = item.Destination, Keyword = item.Keyword });

                        }
                    }

                }

                if (Directory.Exists(item.Source))
                {
                    // var files = Directory.GetFiles(item.Source);
                    var files = Directory.GetFiles(item.Source, "*.txt", SearchOption.AllDirectories);
                    foreach (var item2 in files)
                    {
                        Files.Add(new SourceDestination() { Source = item2, Destination = item.Destination, Keyword = item.Keyword });
                    }

                }

                else if (!string.IsNullOrWhiteSpace(Path.GetExtension(item.Source)))
                {
                    Files.Add(item);
                }
            }

            return Files;
        }

        static bool IsKeywordMatching(string line, string[] keywords)
        {
            foreach (var item in keywords)
            {
                if (!line.ToLower().Contains(item.ToLower()))
                {
                    return false;
                }
            }
            return true;
        }
    }    
}
