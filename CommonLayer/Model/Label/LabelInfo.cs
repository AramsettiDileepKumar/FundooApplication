using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer.Model.Label
{
     public class LabelInfo
    {
        public string LabelName { get; set; } = string.Empty;
        public string Title { get; set; }= string.Empty;
        public string? Description { get; set; }
        public string Colour { get; set; } = string.Empty;
    }
}
