using System.ComponentModel.DataAnnotations;

namespace WebApi.ApiModels.Requests
{
    public class ArticuloRequest
    {
        [Required(ErrorMessage = "{0} is required")]
        public int Id { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        public string Codigo { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        public decimal Precio1 { get; set; }
        public decimal? Precio2 { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        public string Imagen { get; set; }

    }
}
