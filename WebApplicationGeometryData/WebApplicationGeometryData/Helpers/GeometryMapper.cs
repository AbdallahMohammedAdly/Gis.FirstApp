using NetTopologySuite.Geometries;
using WebApplicationGeometryData.Models.DTOs;

public static class GeometryMapper
{
    public static Geometry ConvertToGeometry(GeometryDataDto dto)
    {
        if (dto.X.HasValue && dto.Y.HasValue)
        {
            return new Point(dto.X.Value, dto.Y.Value) { SRID = 3857 }; // تحويل النقطة
        }
        else if (dto.Paths != null && dto.Paths.Count > 0)
        {
            var coordinates = dto.Paths[0]
                .Select(p => new Coordinate(p[0], p[1]))
                .ToArray();

            return new LineString(coordinates) { SRID = 3857 }; // تحويل الخط
        }
        else if (dto.Rings != null && dto.Rings.Count > 0)
        {
            var rings = dto.Rings
                .Select(ring => new LinearRing(ring.Select(p => new Coordinate(p[0], p[1])).ToArray()))
                .ToArray();

            return new Polygon(rings[0]) { SRID = 3857 }; // تحويل المضلع
        }

        return null;
    }
}
