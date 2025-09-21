namespace backend.Models
{
    public class Puja
    {
        public int Id { get; set; }
        public int SubastaId { get; set; }
        public string Usuario { get; set; }
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
    }
}
