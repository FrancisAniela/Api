using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using WebApi.ApiModels.Requests;
using WebApi.Core.Services.Articulos;

namespace WebApi.Controllers
{

    [Route("api/v1/[controller]")]
    [ApiController]
    public class ArticuloController : Controller
    {
        private IMapper _mapper;
        private IArticuloService _articuloService;


        public ArticuloController(IMapper mapper , IArticuloService articuloService) 
        {
            _mapper = mapper;
            _articuloService = articuloService;
        }


        [HttpPost]
        public IActionResult CrearActualizar(List<ArticuloRequest> articuloRequest)
        {


            _articuloService.CrearActualizar(_mapper.Map<List<ArticuloDto>>(articuloRequest));

            return NoContent();
        }

        [HttpGet]
        public IActionResult ObtenerArticuloPorFecha(DateTime fecha)
        {
            try
            {

                List<ArticuloDto> articuloDto = _articuloService.Articulos(fecha);
                return Ok(articuloDto);
            }
            catch (Exception ex)
            {
                return Ok(Response);
            }

        }

        [HttpPost("Precio")]
        public IActionResult ObtenerArticuloPorCodigo(List<string> Codigos)
        {
            List<PrecioCodigoDto> articuloDto = _articuloService.ArticuloPorPrecio(Codigos);

            return Ok(articuloDto);
        }
    }
}
