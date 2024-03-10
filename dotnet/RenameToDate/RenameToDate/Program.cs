using System;
using System.IO;
using System.Linq;

namespace RenameToDate
{
    class Program
    {
        private static readonly string[] ImageExtensions = new[] { "*.bmp", "*.gif", "*.jpg", "*.jpeg", "*.png", "*.tif", "*.tiff" };

        static void Main(string[] args)
        {
            if (args == null || args.Length == 0) args = new[] { "." };

            var directories = args
                .Select(Path.GetFullPath)
                .SelectMany(path => Enumerable.Concat(
                    new[] { path },
                    Directory.GetDirectories(path, "*", new EnumerationOptions { 
                        AttributesToSkip = FileAttributes.Hidden,
                        RecurseSubdirectories = true,
                    })
                ))
                .Distinct();

            foreach (var directory in directories)
            {
                Console.WriteLine(directory);
                var imageGroups = ImageExtensions
                    .SelectMany(extension => Directory.GetFiles(directory, extension, SearchOption.TopDirectoryOnly))
                    .Select(path => new FileInfo(path))
                    .Where(file => !file.Attributes.HasFlag(FileAttributes.Hidden))
                    .Select(file => new ImageInfo(file.FullName))
                    .Where(image => image.DateTaken.HasValue)
                    .GroupBy(image => image.DateTaken)
                    .OrderBy(group => group.Key);

                foreach (var imageGroup in imageGroups)
                {
                    var count = 0;
                    foreach (var image in imageGroup)
                    {
                        string sourcePath = image.SourcePath;
                        string targetFileName = image.DateTaken.Value.ToString("yyyy-MM-dd HH-mm-ss") + " " + count.ToString("000") + image.Extension;
                        string targetPath = Path.Combine(directory, targetFileName);
                        Console.WriteLine(image.FileName + " => " + targetFileName);
                        File.Move(sourcePath, targetPath);
                        count++;
                    }
                }
            }
        }
    }
}
