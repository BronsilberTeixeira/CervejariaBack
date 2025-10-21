using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cervejaria.Models
{
    public class Cerveja
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        private string _nome;
        public string nome { get => _nome; set => _nome.ToUpper(); }

        public string tipo { get; set; }

        public string description { get ; set ; }

        public string image { get; set; }
        
        public double preco { get; set; }
    }
}
