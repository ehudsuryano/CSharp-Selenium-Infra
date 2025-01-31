using System.IO;

namespace Infrastructure.Extensions
{
    /// <summary>
    /// Provides utility methods for directory operations, such as copying directories and their contents.
    /// </summary>
    public static class Utilities
    {
        /// <summary>
        /// Copies the contents of one directory to another directory.
        /// </summary>
        /// <param name="source">The path of the source directory.</param>
        /// <param name="destination">The path of the destination directory.</param>
        public static void CopyDirectory(string source, string destination)
        {
            // Get the subdirectories for the specified directory.
            var dir = new DirectoryInfo(source);

            // Check if the source directory exists.
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException($"Source directory does not exist: {source}");
            }

            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destination))
            {
                Directory.CreateDirectory(destination);
            }

            // Get the files in the directory and copy them to the new location.
            foreach (FileInfo file in dir.GetFiles())
            {
                string tempPath = Path.Combine(destination, file.Name);
                file.CopyTo(tempPath, false);
            }

            // Recursively copy subdirectories.
            foreach (DirectoryInfo subDir in dir.GetDirectories())
            {
                string tempPath = Path.Combine(destination, subDir.Name);
                CopyDirectory(subDir.FullName, tempPath);
            }
        }
        public static void CopyFile(string source, string destination)
        {
            var file = new FileInfo(source);
            // Check if the source file exists.
            if (!file.Exists)
            {
                throw new DirectoryNotFoundException($"Source directory does not exist: {source}");
            }

            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destination))
            {
                Directory.CreateDirectory(destination);
            }

            string tempPath = Path.Combine(destination, file.Name);
            file.CopyTo(tempPath, true);
        }

        public static DirectoryInfo TryGetSolutionDirectoryInfo(string currentPath = null)
        {
            var directory = new DirectoryInfo(
                currentPath ?? Directory.GetCurrentDirectory());
            while (directory != null && !directory.GetFiles("*.sln").Any())
            {
                directory = directory.Parent;
            }
            return directory;
        }
    }
}
