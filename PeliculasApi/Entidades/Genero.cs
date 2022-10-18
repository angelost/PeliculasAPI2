using System.ComponentModel.DataAnnotations;

namespace PeliculasApi.Entidades
{
    public class Genero: IId
    {
        // Propiedades
        public int Id { get; set; }

        [Required]
        [StringLength(40)]
        public string Nombre { get; set; }

        // Propiedades Navegacion
        public List<PeliculasGeneros> PeliculasGeneros { get; set; }

    }
}
