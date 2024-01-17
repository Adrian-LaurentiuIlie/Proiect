using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Proiect.Data;
using LibraryModel.Models;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace Proiect.Controllers
{
    public class HotelsController : Controller
    {
        private readonly LibraryContext _context;
        private string _baseUrl = "http://localhost:5283/api/Hotels";

        public HotelsController(LibraryContext context)
        {
            _context = context;
        }

        // GET: Hotels
        public async Task<ActionResult> Index()
        {
            var client = new HttpClient();
            var response = await client.GetAsync(_baseUrl);
            if (response.IsSuccessStatusCode)
            {
                var hotel = JsonConvert.DeserializeObject<List<Hotel>>(await response.Content. ReadAsStringAsync());
                return View(hotel);
            }
            return NotFound();
        }

        // GET: Inventory/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }
            var client = new HttpClient();
            var response = await client.GetAsync($"{_baseUrl}/{id.Value}");
            if (response.IsSuccessStatusCode)
            {
                var hotel = JsonConvert.DeserializeObject<Hotel>(
                await response.Content.ReadAsStringAsync());
                return View(hotel);
            }
            return NotFound();
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("HotelID,Name,Adress")] Hotel hotel)
        {
            if (!ModelState.IsValid) return View(hotel);
            try
            {
                var client = new HttpClient();
                string json = JsonConvert.SerializeObject(hotel);
                var response = await client.PostAsync(_baseUrl,
                new StringContent(json, Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Unable to create record: {ex.Message}");
            }
            return View(hotel);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }
            var client = new HttpClient();
            var response = await client.GetAsync($"{_baseUrl}/{id.Value}");
            if (response.IsSuccessStatusCode)
            {
                var hotel = JsonConvert.DeserializeObject<Hotel>(
                await response.Content.ReadAsStringAsync());
                return View(hotel);
            }
            return new NotFoundResult();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind("HotelID,Name,Adress,")] Hotel hotel)
        {
            if (!ModelState.IsValid) return View(hotel);
            var client = new HttpClient();
            string json = JsonConvert.SerializeObject(hotel);
            var response = await client.PutAsync($"{_baseUrl}/{hotel.HotelID}",
            new StringContent(json, Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View(hotel);
        }
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }
            var client = new HttpClient();
            var response = await client.GetAsync($"{_baseUrl}/{id.Value}");
            if (response.IsSuccessStatusCode)
            {
                var hotel = JsonConvert.DeserializeObject<Hotel>(await response.Content.ReadAsStringAsync());
                return View(hotel);
            }
            return new NotFoundResult();
        }

        // POST: Customers/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete([Bind("HotelID")] Hotel hotel)
        {
            try
            {
                var client = new HttpClient();
                HttpRequestMessage request =
                new HttpRequestMessage(HttpMethod.Delete, $"{_baseUrl}/{hotel.HotelID}")
                {
                    Content = new StringContent(JsonConvert.SerializeObject(hotel), Encoding.UTF8, "application/json")
                };
                var response = await client.SendAsync(request);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Unable to delete record: {ex.Message}");
            }
            return View(hotel);
        }

        private bool HotelExists(int id)
        {
          return (_context.Hotels?.Any(e => e.HotelID == id)).GetValueOrDefault();
        }

    }
}
