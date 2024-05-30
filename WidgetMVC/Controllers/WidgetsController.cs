using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WidgetAPI.Models;

namespace WidgetMVC.Controllers
{
    public class WidgetsController : Controller
    {
        // GET: Widgets
        public async Task<ActionResult> Index()
        {
            IEnumerable<Widget> widgets = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44356/");
                var response = await client.GetAsync("api/widgets");
                var contents = await response.Content.ReadAsStringAsync();
                widgets = JsonConvert.DeserializeObject<List<Widget>>(contents);
            }
            return View(widgets);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(Widget widget)
        {
            using (var client = new HttpClient())
            {

                var widgetJson = JsonConvert.SerializeObject(widget);
                var content = new StringContent(widgetJson, Encoding.UTF8, "application/json");
                var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:44356/api/widgets")
                {
                    Content = content
                };
                var response = await client.SendAsync(request);


                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }

                var responseContent = await response.Content.ReadAsStringAsync();

                var contentObject = JsonConvert.DeserializeObject<dynamic>(responseContent);
                var message = contentObject.Message.ToString();
                ModelState.AddModelError(string.Empty, message);

                return View(widget);
            }
        }
            
    }
}