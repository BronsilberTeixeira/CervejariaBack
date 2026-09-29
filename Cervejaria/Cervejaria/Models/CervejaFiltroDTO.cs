namespace Cervejaria.Models
{
    public class CervejaFiltroDTO
    {
        public string? Nome { get; set; }
        public string? Tipo { get; set; }
        public double? PrecoMinimo { get; set; }
        public double? PrecoMaximo { get; set; }

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public string? OrdenarPor { get; set; }
    }
}
