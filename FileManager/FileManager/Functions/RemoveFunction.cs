using NLog;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace FileManager.Include
{
    internal static class RemoveFunction
    {
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        private static void RemoveSlash(TextBox textBox)
        {
            string path = textBox.Text;
            if (path.LastIndexOf("\\") == path.Length - 1)
                textBox.Text = path.Substring(0, path.Length - 1);
        }

        public static void BackForRemove(TextBox textBox)
        {
            try
            {
                RemoveSlash(textBox);
                string path = textBox.Text;
                path = path.Substring(0, path.LastIndexOf("\\"));
                textBox.Text = path;
                RemoveSlash(textBox);
                _logger.Info($"Go back button click success. Path: {path}");
            }
            catch (Exception ex)
            {
                _logger.Error($"Go back button click error. Error message: {ex.Message}");
            }
        }

        public static void Remove(string path, ListView listView, TextBox textBox)
        {
            try
            {
                var result = MessageBox.Show("Внимание: программа удалит папку в которой вы находитесь и все ее содержимое!", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Stop);
                if (result == MessageBoxResult.Yes)
                {
                    Directory.Delete(path, true);
                    listView.Items.Remove(listView.SelectedItem);
                    BackForRemove(textBox);
                    _logger.Info("Delete folder success");
                }
                else
                    _logger.Info("Folder not deleted");
            }
            catch (Exception ex)
            {
                _logger.Error($"Error remove. Error message: {ex.Message}");
            }
        }
    }
}
