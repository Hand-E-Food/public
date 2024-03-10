// See https://aka.ms/new-console-template for more information
using System;
using System.CodeDom;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows;

internal class Program
{
    [STAThread]
    private static void Main(string[] args)
    {
        var dataObject = Clipboard.GetDataObject();
        foreach (var format in dataObject.GetFormats())
        {
            object data = dataObject.GetData(format);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(format + " : " + data.GetType().FullName);
            Write(data);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            Console.WriteLine();
        }
    }

    private static byte[] ToByteArray(object obj)
    {
        if (obj == null) throw new ArgumentNullException(nameof(obj));
        using MemoryStream stream = new MemoryStream();
        new BinaryFormatter().Serialize(stream, obj);
        return stream.ToArray();
    }

    private static void Write(object data)
    {
        byte[] bytes;
        if (data is string @string)
            bytes = Encoding.Default.GetBytes(@string);
        else if (data is MemoryStream memoryStream)
            bytes = memoryStream.ToArray();
        else
            bytes = ToByteArray(data);

        foreach (byte b in bytes)
        {
            if (b >= 32 && b <= 127)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write((char)b);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(b.ToString("x2"));
            }
        }
    }
}