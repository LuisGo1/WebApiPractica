using System.ComponentModel.DataAnnotations;

namespace webApiPractica.Models
{
    public class estados_equipos
    {
        [Key]
        public int id_estados_equipos { get; set; }
        public string? estado { get; set; }
        public string? descripcion { get; set; }

    }
}