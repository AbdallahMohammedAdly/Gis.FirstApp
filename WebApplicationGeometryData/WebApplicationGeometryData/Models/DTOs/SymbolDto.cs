namespace WebApplicationGeometryData.Models.DTOs
{
    public class SymbolDto
    {
        public string Type { get; set; }
        public int[] Color { get; set; }
        public int Angle { get; set; }
        public int XOffset { get; set; }
        public int YOffset { get; set; }
        public int Size { get; set; }
        public string Style { get; set; }
        public OutlineDto Outline { get; set; }
    }
}
