using Microsoft.AspNetCore.Mvc;
using backend.Models;
using Microsoft.AspNetCore.SignalR;
using backend.Hubs;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubastasController : ControllerBase
    {
        private static List<Subasta> subastas = new List<Subasta>
        {
            new Subasta { Id = 1, Titulo = "Laptop", Descripcion = "Laptop gamer", PrecioInicial = 2500, FechaCierre = DateTime.Now.AddDays(2) },
            new Subasta { Id = 2, Titulo = "Smartphone", Descripcion = "Último modelo", PrecioInicial = 1200, FechaCierre = DateTime.Now.AddDays(1) }
        };

        private static List<Puja> pujas = new List<Puja>();
        private readonly IHubContext<PujasHub> _hubContext;

        public SubastasController(IHubContext<PujasHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpGet]
        public IActionResult GetSubastas()
        {
            var resultado = subastas.Select(s =>
            {
                var maxPuja = pujas.Where(p => p.SubastaId == s.Id).Select(p => p.Monto).DefaultIfEmpty(s.PrecioInicial).Max();
                return new {
                    s.Id,
                    s.Titulo,
                    s.Descripcion,
                    PrecioActual = maxPuja,
                    s.FechaCierre
                };
            }).ToList();

            return Ok(resultado);
        }

        [HttpGet("{id}/pujas")]
        public IActionResult GetPujas(int id)
        {
            var historial = pujas
                .Where(p => p.SubastaId == id)
                .OrderByDescending(p => p.Monto)
                .ToList();

            return Ok(historial);
        }

        [HttpPost]
        public IActionResult CrearSubasta([FromBody] Subasta subasta)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            subasta.Id = subastas.Count + 1;
            subastas.Add(subasta);
            return CreatedAtAction(nameof(GetSubastas), new { id = subasta.Id }, subasta);
        }

        [HttpPost("{id}/pujas")]
        public async Task<IActionResult> CrearPuja(int id, [FromBody] Puja nuevaPuja)
        {
            var subasta = subastas.FirstOrDefault(s => s.Id == id);
            if (subasta == null)
                return NotFound("Subasta no encontrada");

            if (nuevaPuja.Monto <= 0)
                return BadRequest("El monto debe ser mayor que 0");

            var montoMax = pujas.Where(p => p.SubastaId == id).Select(p => p.Monto).DefaultIfEmpty(subasta.PrecioInicial).Max();
            if (nuevaPuja.Monto <= montoMax)
                return BadRequest($"La puja debe ser mayor que la puja actual ({montoMax})");

            nuevaPuja.Id = pujas.Count + 1;
            nuevaPuja.SubastaId = id;
            nuevaPuja.Fecha = DateTime.Now;

            pujas.Add(nuevaPuja);

            // Emitir la nueva puja a todos los clientes conectados
            await _hubContext.Clients.All.SendAsync("RecibirPuja", nuevaPuja);

            return CreatedAtAction(nameof(GetPujas), new { id = id }, nuevaPuja);
        }
    }
}
