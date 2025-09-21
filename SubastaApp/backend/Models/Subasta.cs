// Models/Subasta.cs
namespace backend.Models
{
    public class Subasta
    {
        public int Id { get; set; }  // opcional si es autoincremental
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public decimal PrecioInicial { get; set; }
        public DateTime FechaCierre { get; set; }
    }
}