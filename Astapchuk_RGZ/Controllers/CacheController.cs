using Astapchuk_RGZ.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Astapchuk_RGZ.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CacheController : Controller
    {
        private const string connectionString = "Server=192.168.0.106;Username=postgres;Database=cache;Port=5432;Password=postgres;SSLMode=Prefer";

        [HttpGet]
        public async Task<AddDataResult> AddData(string data)
        {
            var result = new AddDataResult();

            using (var connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new NpgsqlCommand("INSERT INTO Cache (value) VALUES (@p1)", connection))
                {
                    command.Parameters.AddWithValue("p1", data);
                    var commandResult = await command.ExecuteNonQueryAsync();
                }

                using (var command = new NpgsqlCommand("SELECT * FROM Cache ORDER BY Id desc LIMIT 1", connection))
                {
                    var reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        result.Id = reader.GetInt32(0);
                        result.Value = reader.GetString(1);
                    }
                    await reader.CloseAsync();
                }
            }

            return await Task.FromResult(result);
        }
    }
}
