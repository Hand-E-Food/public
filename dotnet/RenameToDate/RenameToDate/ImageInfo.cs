using System;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Drawing.Imaging;
using System.Globalization;

namespace RenameToDate
{
    public class ImageInfo
    {
        private static readonly DateTime MinimumDateTime = new DateTime(1980, 1, 1);
        private static readonly int[] DateTakenPropertyIds = { 0x9003, 0x132, 0x9004 };

        public DateTime? DateTaken { get; }
        public string Directory { get; }
        public string Extension { get; }
        public string FileName { get; }
        public string SourcePath { get; }

        public ImageInfo(string path)
        {
            Directory = Path.GetDirectoryName(path);
            Extension = Path.GetExtension(path).ToLower();
            FileName = Path.GetFileName(path);
            SourcePath = path;

            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (var image = Image.FromStream(stream, false, false))
            {
                var fileInfo = new FileInfo(path);

                DateTaken = DateTakenPropertyIds
                    .Where(image.PropertyIdList.Contains)
                    .Select(image.GetPropertyItem)
                    .Select(ConvertToDateTime)
                    .Concat(new[] { fileInfo.CreationTime, fileInfo.LastWriteTime })
                    .Where(dateTime => dateTime >= MinimumDateTime)
                    .Min();
            }
        }

        private static DateTime ConvertToDateTime(PropertyItem propertyItem)
        {
            var dateTimeString = Encoding.UTF8.GetString(propertyItem.Value).Remove(19);
            return DateTime.ParseExact(dateTimeString, "yyyy:MM:dd HH:mm:ss", CultureInfo.InvariantCulture);
        }
    }
}
