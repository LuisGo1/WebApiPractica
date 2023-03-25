using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webApiPractica.Models;


namespace webApiPractica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class estado_equiposController : ControllerBase
    {
        private readonly equiposContext _equiposContexto;

        public estado_equiposController(equiposContext equiposContexto)

        {
            _equiposContexto = equiposContexto;
        }

        //Retorna el listado de todos los equipos existentes

        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<estados_equipos> listadoEquipo = (from e in _equiposContexto.estados_Equipos
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
            estados_equipos? equipos = (from e in _equiposContexto.estados_Equipos
                                        where e.id_estados_equipos == id
                                        select e).FirstOrDefault();
            if (equipos == null)
            {
                return NotFound();
            }

            return Ok(equipos);
        }

        //Guarda nuevo registro

        [HttpGet]
        [Route("Find/{filtro}")]
        public IActionResult FindyByDescripcion(string filtro)
        {
            estados_equipos? equipos = (from e in _equiposContexto.estados_Equipos
                                        where e.descripcion.Contains(filtro)
                                        select e).FirstOrDefault();
            if (equipos == null)
            {
                return NotFound();
            }
            return Ok(equipos);
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarEstadoEquipo([FromBody] estados_equipos estados_Equipos)
        {
            try
            {
                _equiposContexto.estados_Equipos.Add(estados_Equipos);
                _equiposContexto.SaveChanges();
                return Ok(estados_Equipos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarEstadoEquipo(int id, [FromBody] estados_equipos equipoModificar)
        {
            estados_equipos? equipoActual = (from e in _equiposContexto.estados_Equipos
                                             where e.id_estados_equipos == id
                                             select e).FirstOrDefault();
            if (equipoActual == null)
            {
                return NotFound();
            }

            equipoActual.estado = equipoModificar.estado;
            equipoActual.descripcion = equipoModificar.descripcion;

            _equiposContexto.Entry(equipoActual).State = EntityState.Modified;
            _equiposContexto.SaveChanges();

            return NotFound();
        }

        [HttpDelete]
        [Route("eliminar/{id}")]
        public ActionResult EliminarEstadoEquipo(int id)
        {
            estados_equipos? equipo = (from e in _equiposContexto.estados_Equipos
                                       where e.id_estados_equipos == id
                                       select e).FirstOrDefault();
            if (equipo == null)
                return NotFound();

            _equiposContexto.estados_Equipos.Attach(equipo);
            _equiposContexto.estados_Equipos.Remove(equipo);
            _equiposContexto.SaveChanges();

            return Ok(equipo);
        }

    }
}