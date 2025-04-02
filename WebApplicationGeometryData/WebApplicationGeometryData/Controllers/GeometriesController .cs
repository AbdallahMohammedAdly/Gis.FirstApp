using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationGeometryData.Data;
using WebApplicationGeometryData.Models;
using WebApplicationGeometryData.Models.DTOs;
using NetTopologySuite.Geometries;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlTypes;

namespace WebApplicationGeometryData.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeometriesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GeometriesController(AppDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> SaveGeometry([FromBody] List<GeometryDto> geometriesDto)
        {
            if (geometriesDto == null || !geometriesDto.Any())
                return BadRequest("No data received");

            var geometries = new List<GeometryData>();

            foreach (var dto in geometriesDto)
            {
                var geometry = ConvertToGeometry(dto.Geometry);
                if (geometry == null)
                    return BadRequest("Invalid geometry data");

                geometries.Add(new GeometryData
                {
                    Type = dto.Geometry.Paths != null ? "LineString" :
                           dto.Geometry.Rings != null ? "Polygon" :
                           "Point",
                    Geom = geometry
                });
            }

            await _context.Geometries.AddRangeAsync(geometries);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Data saved successfully!", count = geometries.Count });
        }


        [HttpGet]
        public async Task<IActionResult> GetGeometries()
        {
            var geometries = await _context.Geometries.ToListAsync();

            var geometriesDto = geometries.Select(g => new
            {
                geometry = g.Geom switch
                {
                    Point p => new
                    {
                        spatialReference = new { latestWkid = 3857, wkid = 102100 },
                        x = p.X,
                        y = p.Y
                    },
                    LineString l => new
                    {
                        spatialReference = new { latestWkid = 3857, wkid = 102100 },
                        paths = new List<List<double[]>>
            {
                l.Coordinates.Select(c => new double[] { c.X, c.Y }).ToList()
            }
                    },
                    Polygon poly => new
                    {
                        spatialReference = new { latestWkid = 3857, wkid = 102100 },
                        rings = new List<List<double[]>>
            {
                poly.Coordinates.Select(c => new double[] { c.X, c.Y }).ToList()
            }
                    },
                    _ => (object?)null  // ✅ تحديد `null` كـ `object?`
                },

                symbol = g.Type switch
                {
                    "Point" => new
                    {
                        type = "esriSMS",
                        color = new int[] { 138, 43, 226, 255 },
                        size = 12,
                        style = "esriSMSSquare",
                        outline = new
                        {
                            type = "esriSLS",
                            color = new int[] { 255, 255, 255, 255 },
                            width = 3,
                            style = "esriSLSSolid"
                        }
                    },
                    "LineString" => new
                    {
                        type = "esriSLS",
                        color = new int[] { 138, 43, 226, 255 },
                        width = 4,
                        style = "esriSLSDash"
                    },
                    "Polygon" => new
                    {
                        type = "esriSFS",
                        color = new int[] { 138, 43, 226, 128 },
                        outline = new
                        {
                            type = "esriSLS",
                            color = new int[] { 255, 255, 255, 255 },
                            width = 1,
                            style = "esriSLSSolid"
                        }
                    },
                    _ => (object?)null  // ✅ تحديد `null` كـ `object?`
                }
            }).ToList();



            return Ok(geometriesDto);
        }

        private static Geometry ConvertToGeometry(GeometryDataDto dto)
        {
            if (dto.X.HasValue && dto.Y.HasValue)
            {
                return new Point(dto.X.Value, dto.Y.Value) { SRID = 4326 }; 
            }
            else if (dto.Paths != null && dto.Paths.Any())
            {
                var coordinates = dto.Paths[0].Select(p => new Coordinate(p[0], p[1])).ToArray();
                return coordinates.Length >= 2 ? new LineString(coordinates) { SRID = 4326 } : null;
            }
            else if (dto.Rings != null && dto.Rings.Any())
            {
                var rings = dto.Rings.Select(ring => new LinearRing(ring.Select(p => new Coordinate(p[0], p[1])).ToArray())).ToArray();
                return rings.Length > 0 && rings[0].NumPoints >= 4 ? new Polygon(rings[0]) { SRID = 4326 } : null;
            }

            return null;
        }



    }
}
