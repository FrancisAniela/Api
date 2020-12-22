using System;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Core.Models
{
    public partial class Articulo
    {
        
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string Imagen { get; set; }
        public Decimal Precio1 { get; set; }
        public Decimal? Precio2 { get; set; }
        public int IdEstado { get; set; }
        public string Codigo { get; set; }

        public virtual EstadoArticulo Estado { get; set; }

    }
}
