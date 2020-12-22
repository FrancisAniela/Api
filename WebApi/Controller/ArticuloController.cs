using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebApi.ApiModels.Requests;
using WebApi.Core.Services.Articulos;

namespace WebApi.Controller
{

    [Route("api/v1/[controller]")]
    [ApiController]
    public class ArticuloController : ControllerBase
    {
        private IMapper _mapper;
        private IArticuloService _articuloService;


        public ArticuloController(IMapper mapper , IArticuloService articuloService) 
        {
            _mapper = mapper;
            _articuloService = articuloService;
        }


        [HttpPost]
        public IActionResult Create(ArticuloRequest articuloRequest)
        {
            _articuloService.Crear(_mapper.Map<ArticuloDto>(articuloRequest));

            return Ok();
        }

        [HttpGet]
        public IActionResult Obtener()
        {
           List<ArticuloDto> articuloDto = _articuloService.Articulos();

            return Ok(articuloDto);
        }
        
    }
}
