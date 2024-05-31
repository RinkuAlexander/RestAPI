using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WidgetAPI.Models
{
    public class Widget
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Manufacturer Manufacturer { get; set; }
    }
}