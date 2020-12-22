using System.Collections.Generic;

namespace WebApi.Core.Models
{
    public partial class EstadoArticulo
    {
        public EstadoArticulo()
        {
            Articulos = new HashSet<Articulo>();
        }
        public int Id { get; set; }
        public string Descripcion { get; set; }


        public ICollection<Articulo> Articulos { get; set; }
    }
}
