using System.Collections.Generic;

namespace WebApi.Core.Services.Articulos
{
    public interface IArticuloService 
    {
        public void Crear(ArticuloDto articuloDto);
        public List<ArticuloDto> Articulos();
    }
}
