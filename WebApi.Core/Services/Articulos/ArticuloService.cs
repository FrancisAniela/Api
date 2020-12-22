using AutoMapper;
using System;
using System.Collections.Generic;
using WebApi.Core.Extensions;
using WebApi.Core.Models;
using WebApi.Core.Repositories;
using System.Linq;
using WebApi.Core.Enums;
using System.Linq.Expressions;

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


        public void Crear(ArticuloDto articuloDto)
        {
            if (!this.Validar(articuloDto))
                return;

            Articulo found = _mapper.Map<Articulo>(articuloDto);

            found.FechaCreacion = DateTime.UtcNow;
            found.IdEstado = (int)EstadoArticuloEnum.Activo;

            _articuloRepository.Add(found);
            _articuloRepository.Commit();

        }

        public List<ArticuloDto> Articulos() 
        {
            List<Expression<Func<Articulo, Object>>> includes = new List<Expression<Func<Articulo, Object>>>();
            includes.Add(x => x.Estado);

            List<Articulo> articulo = _articuloRepository.GetMany(x=> x.Id == 1 , includes).ToList();

            return _mapper.Map<List<ArticuloDto>>(articulo);
        
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
