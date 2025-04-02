using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using WebApplicationGeometryData.Data;
using WebApplicationGeometryData.Models;

public class GeometryBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public GeometryBackgroundService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                // جلب البيانات غير المحفوظة من أي مصدر خارجي أو قائمة مؤقتة
                List<GeometryData> newGeometries = await GetPendingGeometriesAsync();

                if (newGeometries.Count > 0)
                {
                    _context.Geometries.AddRange(newGeometries);
                    await _context.SaveChangesAsync();
                    Console.WriteLine($"✅ تم حفظ {newGeometries.Count} عناصر في قاعدة البيانات.");
                }
            }

            // الانتظار 30 ثانية قبل التحقق مرة أخرى
            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }

    private async Task<List<GeometryData>> GetPendingGeometriesAsync()
    {
        // يمكنك هنا استبدالها بمنطق جلب البيانات الحقيقي (مثل قائمة مؤقتة أو API خارجي)
        await Task.Delay(500); // محاكاة التأخير
        return new List<GeometryData>(); // حالياً قائمة فارغة، يمكنك تعديلها حسب احتياجك
    }
}
