using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WidgetAPI.Models;

namespace WidgetAPI.Controllers
{
        public class WidgetsController : ApiController
    {
        public static List<Widget> _widgets = new List<Widget>{
        new Widget{ Id=1, Name="Widget 1", Manufacturer=new Manufacturer{Name="ACME widgets", Address="123 easy street", Founded=new System.DateTime(2001,01,01)} },
        new Widget{ Id=2, Name="Widget 2", Manufacturer=new Manufacturer{Name="Widgets Emporiu", Address="789 Bay street", Founded=new System.DateTime(2011,05,02) } },
        new Widget{ Id=3, Name="Widget 3", Manufacturer= new Manufacturer{Name="ACME widgets", Address="123 easy street", Founded=new System.DateTime(2001,01,01)}}
        };

        [HttpGet]
        public IHttpActionResult GetWidgetById([FromUri] int id)
        {
            if (!_widgets.Any(x => x.Id == id))
            {
                return NotFound();
            }

            return Ok(_widgets.Where(x => x.Id == id));
        }

        [HttpGet]
        public IHttpActionResult GetAllWidgets(bool includeManufacturer = true)
        {
            return Ok(includeManufacturer ? _widgets : _widgets.Select(x => new Widget { Id = x.Id, Name = x.Name }));
        }

        [HttpPost]
        public IHttpActionResult PostWidget([FromBody] Widget widget)
        {
            if (string.IsNullOrEmpty(widget.Name))
            {
                return BadRequest("The widget name can not be null or empty.");
            }

            widget.Id = _widgets.Max(x => x.Id) + 1;
            _widgets.Add(widget);

            return Created(new Uri(Request.RequestUri + "/" + widget.Id.ToString()), widget);
        }

        [HttpPut]
        public IHttpActionResult PutWidget([FromBody] Widget widget)
        {
            if (string.IsNullOrEmpty(widget.Name))
            {
                return BadRequest("The widget name can not be null or empty.");
            }

            var widgetToUpdate = _widgets.FirstOrDefault(x => x.Id == widget.Id);
            if (widgetToUpdate == null)
            {
                return PostWidget(widget);
            }

            widgetToUpdate.Name = widget.Name;
            widgetToUpdate.Manufacturer = widget.Manufacturer;

            return StatusCode(HttpStatusCode.NoContent);
        }
        
        [HttpDelete]
        public IHttpActionResult DeleteWidget([FromUri] int id)
        {
            var widgetToDelete = _widgets.FirstOrDefault(x => x.Id == id);
            if (widgetToDelete == null)
            {
                return NotFound();
            }

            _widgets.Remove(widgetToDelete);

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
