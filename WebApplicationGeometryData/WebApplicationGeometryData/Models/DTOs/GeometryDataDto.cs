namespace WebApplicationGeometryData.Models.DTOs
{
    public class GeometryDataDto
    {
        public double? X { get; set; }
        public double? Y { get; set; }
        public List<List<double[]>>? Paths { get; set; }
        public List<List<double[]>>? Rings { get; set; }
    }
}
