using NLog;
using System;
using System.IO;
using System.Text;

namespace FileManager.Include
{
    internal static class SaveFunction
    {
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private static string projectPath = AppDomain.CurrentDomain.BaseDirectory;

        public static void SaveDialogWindowInformation(params string[] paths)
        {
            string path = Path.Combine(projectPath, "load.txt");
            StringBuilder sb = new StringBuilder();
            foreach (string p in paths)
            {
                sb.AppendLine(p);
            }
            File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
            _logger.Info("Successfully saved paths of all windows");
        }
    }
}
