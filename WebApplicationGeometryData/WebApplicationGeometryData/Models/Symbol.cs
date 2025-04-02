namespace WebApplicationGeometryData.Models
{
    public class Symbol
    {
        public string id { get; set; }
        public string Type { get; set; }
        public int[] Color { get; set; }
        public int Angle { get; set; }
        public int XOffset { get; set; }
        public int YOffset { get; set; }
        public int Size { get; set; }
        public string Style { get; set; }
        public int Width { get; set; }
        public Outline Outline { get; set; }
    }
}
