using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using WebApi.Core.Enums;
using WebApi.Core.Extensions;
using WebApi.Core.Models;
using WebApi.Core.Repositories;

namespace WebApi.Core.Services.Articulos
{
    public class ArticuloService : IArticuloService
    {
        IMapper _mapper;
        IWebApiRepository<Articulo> _articuloRepository;
        public ArticuloService(IMapper mapper, IWebApiRepository<Articulo> articuloRepository)
        {
            _mapper = mapper;
            _articuloRepository = articuloRepository;
        }

        public void CrearActualizar(List<ArticuloDto> articuloDto)
        {
            List<Articulo> articulos = _mapper.Map<List<Articulo>>(articuloDto);

            List<Articulo> update = new List<Articulo>();
            List<Articulo> crear = new List<Articulo>();

            
           

            foreach (Articulo articulo in articulos)
            {
                var found = _articuloRepository.FirstOrDefault(x => x.Codigo == articulo.Codigo);


                if (found != null)
                {
                    found.FechaModificacion = DateTime.UtcNow;
                    found.IdEstado = (int)EstadoArticuloEnum.Activo;
                    found.Descripcion = articulo.Descripcion;
                    found.Nombre = articulo.Nombre;
                    found.Precio1 = articulo.Precio1;
                    found.Precio2 = articulo.Precio2;
                    found.Imagen = articulo.Imagen;
                    update.Add(found);
                }
                else
                {
                    articulo.IdEstado = (int)EstadoArticuloEnum.Activo;
                    articulo.FechaCreacion = DateTime.UtcNow;
                    crear.Add(articulo);
                }
            }

            _articuloRepository.AddRange(crear);
            _articuloRepository.UpdateRange(update);


            _articuloRepository.Commit();

        }
        
        public List<ArticuloDto> Articulos(DateTime fecha) 
        {
            List<Expression<Func<Articulo, Object>>> includes = new List<Expression<Func<Articulo, Object>>>();
            includes.Add(x => x.Estado);

            List<Articulo> articulo = _articuloRepository.GetMany(x=> x.FechaCreacion >= fecha || x.FechaModificacion>=fecha  , includes).ToList();

            return _mapper.Map<List<ArticuloDto>>(articulo);
        
        }

      

        public List<PrecioCodigoDto> ArticuloPorPrecio(List<string> codigos) 
        {

            List<Expression<Func<Articulo, Object>>> includes = new List<Expression<Func<Articulo, Object>>>();
            includes.Add(x => x.Estado);

            List<Articulo> articulo = _articuloRepository.GetMany(x => codigos.Contains(x.Codigo)).ToList();

            return _mapper.Map<List<PrecioCodigoDto>>(articulo);

        }

        public bool Validar(ArticuloDto articuloDto) 
        {
            if (articuloDto == null)
                throw new NullReferenceException("Error data");


            if(articuloDto.Nombre.IsNullOrEmpty())
                throw new NullReferenceException("El nombre no puede estar vació");


            if (articuloDto.Descripcion.IsNullOrEmpty())
                throw new NullReferenceException("La descripción no puede estar vació");

            return true;
        }
    }
}
