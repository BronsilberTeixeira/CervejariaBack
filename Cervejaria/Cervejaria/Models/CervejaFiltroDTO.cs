namespace Cervejaria.Models
{
    public class CervejaFiltroDTO
    {
        public string? Nome { get; set; }
        public string? tipo { get; set; }
        public double? precoMinimo { get; set; }
        public double? precoMaximo { get; set; }

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public string? OrdenarPor { get; set; }
    }
}
