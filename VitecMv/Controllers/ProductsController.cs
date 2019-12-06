using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using VitecMv.Models;
using VitecMv.Data;

namespace VitecMv.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;
        private readonly WebClient webClient = new WebClient();
        private readonly string urlApi = "https://vitecmv-api.azurewebsites.net/api/product/";

        public ProductsController(ApplicationDbContext context, ILogger<ProductsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Products
        public IActionResult Index() {
            _logger.LogInformation("\nEn person " + GetIpAddress.GetIPAddress() + " har besøgt produkt siden!\n");

            byte[] myDataBuffer = webClient.DownloadData(urlApi);
            string downloadedString = Encoding.UTF8.GetString(myDataBuffer);

            var products = JsonConvert.DeserializeObject<List<Product>>(downloadedString);

            return View(products);
        }

        // GET: Products/Details/5
        public IActionResult Details(int? id) {
            if (id == null) {
                return NotFound();
            }

            byte[] myDataBuffer = webClient.DownloadData(urlApi + id);
            string downloadedString = Encoding.UTF8.GetString(myDataBuffer);

            var product = JsonConvert.DeserializeObject<Product>(downloadedString);

            return View(product);
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.ID == id);
        }
    }
}
