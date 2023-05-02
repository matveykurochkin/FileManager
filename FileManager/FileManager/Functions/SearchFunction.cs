using NLog;
using System;
using System.IO;
using System.Windows.Controls;

namespace FileManager.Include
{
    internal static class SearchFunction
    {
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public static void Search(string path, TextBox textBox, ListView listView)
        {
            if (textBox.Text != "")
            {
                try
                {
                    listView.Items.Clear();

                    foreach (var searchFile in Directory.GetFiles(path, textBox.Text, SearchOption.AllDirectories))
                    {
                        FileInfo fileInfo = new FileInfo(searchFile);
                        listView.Items.Add(fileInfo);
                    }

                    foreach (var searchDirectory in Directory.GetDirectories(path, textBox.Text, SearchOption.AllDirectories))
                    {
                        DirectoryInfo directoryInfo = new DirectoryInfo(searchDirectory);
                        listView.Items.Add(directoryInfo);
                    }

                    _logger.Info($"Search complited");
                }
                catch (Exception ex)
                {
                    _logger.Error($"Search not complited. Error message: {ex.Message}");
                }
            }
        }
    }
}
