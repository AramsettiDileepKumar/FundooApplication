using BusinessLogicLayer.InterfaceBL.Labels;
using CommonLayer.Model.Label;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Service;

namespace FundooApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabelController : ControllerBase
    {
        public readonly INotesLabelBL notesLabelBL;
        public LabelController(INotesLabelBL notesLabelBL)
        {
            this.notesLabelBL = notesLabelBL;
        }
        [HttpPost()]
        public async Task<IActionResult> CreateLabel(LabelModel label)
        {
            return Ok(await notesLabelBL.CreateLabel(label));
        }
        [HttpPut("{LabelId}")]
        public async Task<IActionResult> UpdateLabel(int LabelId,LabelNameModel name)
        {
            return Ok(await notesLabelBL.UpdateLabel(LabelId,name));
        }
        [HttpDelete("{labelId}")]
        public async Task<IActionResult> DeleteLabel(int labelId)
        {
            return Ok(await notesLabelBL.DeleteLabel(labelId));
        }
        [HttpGet("{LabelId}")]
        public async Task<IActionResult> GetLabel(int LabelId)
        {
            return Ok(await notesLabelBL.getLabel(LabelId));
        }

    }
}
