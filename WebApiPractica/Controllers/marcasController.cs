using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webApiPractica.Models;
using Microsoft.EntityFrameworkCore;


namespace webApiPractica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class marcasController : ControllerBase
    {
        private readonly equiposContext _equiposContexto;

        public marcasController(equiposContext equiposContexto)

        {
            _equiposContexto = equiposContexto;
        }

        //Retorna el listado de todos los equipos existentes

        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<marcas> listadoEquipo = (from e in _equiposContexto.marcas
                                          select e).ToList();

            if (listadoEquipo.Count() == 0)
            {
                return NotFound();
            }

            return Ok(listadoEquipo);
        }

        //Retorna los registros de una tabla filtrados por su ID

        [HttpGet]
        [Route("GetById/{id}")]
        public IActionResult Get(int id)
        {
            marcas? carrera = (from e in _equiposContexto.marcas
                               where e.id_marcas == id
                               select e).FirstOrDefault();
            if (carrera == null)
            {
                return NotFound();
            }

            return Ok(carrera);
        }

        //Guarda nuevo registro

        [HttpGet]
        [Route("Find/{filtro}")]
        public IActionResult FindyByName(string filtro)
        {
            marcas? carrera = (from e in _equiposContexto.marcas
                               where e.nombre_marca.Contains(filtro)
                               select e).FirstOrDefault();
            if (carrera == null)
            {
                return NotFound();
            }
            return Ok(carrera);
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarMarca([FromBody] marcas marcas)
        {
            try
            {
                _equiposContexto.marcas.Add(marcas);
                _equiposContexto.SaveChanges();
                return Ok(marcas);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarMarca(int id, [FromBody] marcas equipoModificar)
        {
            marcas? equipoActual = (from e in _equiposContexto.marcas
                                    where e.id_marcas == id
                                    select e).FirstOrDefault();
            if (equipoActual == null)
            {
                return NotFound();
            }

            equipoActual.nombre_marca = equipoModificar.nombre_marca;
            equipoActual.estados = equipoModificar.estados;

            _equiposContexto.Entry(equipoActual).State = EntityState.Modified;
            _equiposContexto.SaveChanges();

            return NotFound();
        }

        [HttpDelete]
        [Route("eliminar/{id}")]
        public ActionResult EliminarMarca(int id)
        {
            marcas? equipo = (from e in _equiposContexto.marcas
                              where e.id_marcas == id
                              select e).FirstOrDefault();
            if (equipo == null)
                return NotFound();

            _equiposContexto.marcas.Attach(equipo);
            _equiposContexto.marcas.Remove(equipo);
            _equiposContexto.SaveChanges();

            return Ok(equipo);
        }
    }
}