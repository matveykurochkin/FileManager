using NLog;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Controls;

namespace FileManager.Include
{
    internal static class LoadFunction
    {
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private static string[] Drives = Environment.GetLogicalDrives();
        private static string projectPath = AppDomain.CurrentDomain.BaseDirectory;

        public static void LoadInfoDirectory(string path, Label label)
        {
            try
            {
                int filesCount = Directory.GetFiles(path).Length;
                int dirsCount = Directory.GetDirectories(path).Length;

                DirectoryInfo directoryInfo = new DirectoryInfo(path);

                long directorySize = 0;
                FileInfo[] files = directoryInfo.GetFiles();

                foreach (FileInfo file in files)
                    directorySize += file.Length;

                if (filesCount == 0)
                    label.Content = $"{filesCount} file(s), {dirsCount} dir(s)";
                else if (directorySize / 1024 < 1)
                    label.Content = $"{(directorySize % 1024).ToString("#,#", new CultureInfo("ru-RU"))} b in {filesCount} file(s), {dirsCount} dir(s)";
                else
                    label.Content = $"{(directorySize / 1024).ToString("#,#", new CultureInfo("ru-RU"))} k in {filesCount} file(s), {dirsCount} dir(s)";

                _logger.Info("Load Info Directory view successfully");
            }
            catch (Exception ex)
            {
                _logger.Error($"Load Info Directory view not successfully. Error message: {ex.Message}");
            }
        }

        public static void LoadFilesAndDirectories(bool isFile, string filePath, string selectedItemName, ListView listView)
        {
            DirectoryInfo fileList;
            string tempFilePath = "";
            FileAttributes fileAttr;
            try
            {
                if (isFile)
                {
                    tempFilePath = filePath + "\\" + selectedItemName;
                    FileInfo fileDetails = new FileInfo(tempFilePath);
                    fileAttr = File.GetAttributes(tempFilePath);
                    Process.Start(tempFilePath);
                }
                else
                {
                    fileList = new DirectoryInfo(filePath);
                    FileInfo[] fileInfo = fileList.GetFiles();
                    DirectoryInfo[] directoryInfo = fileList.GetDirectories();

                    listView.Items.Clear();

                    for (int i = 0; i < directoryInfo.Length; i++)
                        listView.Items.Add(directoryInfo[i]);


                    for (int i = 0; i < fileInfo.Length; i++)
                        listView.Items.Add(fileInfo[i]);
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Files and Directories not uploaded. Error message: {ex.Message}");
            }
            listView.Items.Refresh();
        }

        public static string[] LoadDialogWindowInformation()
        {
            string path = $"{projectPath}\\load.txt";
            string userName = Environment.UserName;
            if (!File.Exists(path))
            {
                using (FileStream fileStream = File.Create(path))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes($"C:\\Users\\{userName}\nC:\\Users\\{userName}");
                    fileStream.Write(info, 0, info.Length);
                }
            }
            string[] lines = File.ReadAllLines(path);
            _logger.Info($"Successfully load paths of all windows");
            return lines;
        }
    }
}
