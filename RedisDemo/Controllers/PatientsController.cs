using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using RedisDemo.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace RedisDemo.Controllers
{
    public class PatientsController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly IDistributedCache cache;

        public PatientsController(ApplicationDbContext db, IDistributedCache cache)
        {
            this.db = db;
            this.cache = cache;
        }
        public async Task<IActionResult> Index()
        {
            var cachekey = "patients";
            var patients = await cache.GetStringAsync(cachekey);
            if (patients == null)
            {
                var model = await db.Patients.ToListAsync();
                if (model.Any())
                {
                    var jsonData = JsonSerializer.Serialize(model);
                    var cacheEntryOption = new DistributedCacheEntryOptions();
                    //cacheEntryOption.SlidingExpiration = TimeSpan.FromMinutes(1);
                    cacheEntryOption.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2);
                    await cache.SetStringAsync(cachekey, jsonData, cacheEntryOption);
                }
                return View(model);
            }
            var modelFromCache = JsonSerializer.Deserialize<List<Patient>>(patients);
            return View(modelFromCache);
        }
    }
}
