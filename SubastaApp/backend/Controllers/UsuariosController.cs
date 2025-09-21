using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetUsuarios()
        {
            var usuarios = new List<object>
            {
                new { Id = 1, Nombre = "Víctor", Apellido = "Cruz", Email = "victor@example.com" },
                new { Id = 2, Nombre = "Enzo", Apellido = "Laqui", Email = "enzo@example.com" },
                new { Id = 3, Nombre = "Edison", Apellido = "Meneses", Email = "edison@example.com" }
            };

            return Ok(usuarios);
        }
    }
}
