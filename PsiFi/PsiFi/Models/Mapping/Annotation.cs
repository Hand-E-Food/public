namespace PsiFi.Models.Mapping
{
    class Annotation
    {
        public Appearance Appearance { get; set; }

        public static readonly Annotation Target = new Annotation
        {
            Appearance = new Appearance('X', System.ConsoleColor.Red),
        };
    }
}
