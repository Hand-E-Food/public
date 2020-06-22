namespace PsiFi.Models.Mapping
{
    class Annotation
    {
        /// <summary>
        /// Annotation indicating where the player is targeting.
        /// </summary>
        public static readonly Annotation Target = new Annotation
        {
            Appearance = new Appearance('X', System.ConsoleColor.Red),
        };
        
        /// <summary>
        /// This annotation's appearance.
        /// </summary>
        public Appearance Appearance { get; set; }
    }
}
