using System;
using System.Collections.Generic;

namespace WebApi.Core.Services.Articulos
{
    public interface IArticuloService 
    {
        public void CrearActualizar(List<ArticuloDto> articuloaDto);
        public List<ArticuloDto> Articulos(DateTime fecha);
        public List<PrecioCodigoDto> ArticuloPorPrecio(List<string> codigos);
    }
}
