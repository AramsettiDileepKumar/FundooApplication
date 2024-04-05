using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer.Model.RequestDTO
{
    public class UpdateNotesRequest
    {
        public string Title { get; set; }

        public string? Description { get; set; } = string.Empty;

        public string Colour { get; set; } = string.Empty;
    }
}
