using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace WebApplicationGeometryData.Models
{
    public class GeometryData
    {
        [Key]
        public int Id { get; set; }

        public string Type { get; set; }  // Point, LineString, Polygon

        [Column(TypeName = "geometry")] // مهم جداً عند التعامل مع SQL Server
        public Geometry Geom { get; set; }


    }
}
