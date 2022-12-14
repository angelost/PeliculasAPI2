using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations;

namespace PeliculasApi.Entidades
{
    public class SalaDeCine : IId
    {
        public int Id { get; set; }
        [Required]
        [StringLength(120)]
        public string Nombre { get; set; }
        public Point Ubicacion { get; set; }
        
        // Propiedades Navegacion
        public List<PeliculasSalasDeCine> PeliculasSalasDeCines { get; set; }
    }
}
