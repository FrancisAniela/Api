﻿using System;
using WebApi.Core.Services.Estados;

namespace WebApi.Core.Services.Articulos
{
    public class ArticuloDto
    {
        public int Id { get; set; }
        public String Descripcion { get; set; }
        public String Nombre { get; set; }
        public Decimal Precio1 { get; set; }
        public Decimal? Precio2 { get; set; }
        public String Codigo { get; set; }
        public String Imagen { get; set; }
        public EstadoArticuloDto EstadoArticulo { get; set; }
    }
}
