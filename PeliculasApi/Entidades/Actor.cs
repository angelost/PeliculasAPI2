using System.ComponentModel.DataAnnotations;

namespace PeliculasApi.Entidades
{
    public class Actor: IId
    {
        // Propiedades
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Foto { get; set; }

        // Propiedades Navegacion
        public List<PeliculasActores> PeliculasActores { get; set; }
    }
}
