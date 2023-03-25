using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webApiPractica.Models;
using Microsoft.EntityFrameworkCore;


namespace webApiPractica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class tipoEquiposController : ControllerBase
    {
        private readonly equiposContext _equiposContexto;

        public tipoEquiposController(equiposContext equiposContexto)

        {
            _equiposContexto = equiposContexto;
        }

        //Retorna el listado de todos los equipos existentes

        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<tipo_equipo> listadoEquipo = (from e in _equiposContexto.tipo_Equipos
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
            tipo_equipo? carrera = (from e in _equiposContexto.tipo_Equipos
                                    where e.id_tipo_equipo == id
                                    select e).FirstOrDefault();
            if (carrera == null)
            {
                return NotFound();
            }

            return Ok(carrera);
        }

        [HttpGet]
        [Route("Find/{filtro}")]
        public IActionResult FindyByDescription(string filtro)
        {
            tipo_equipo? carrera = (from e in _equiposContexto.tipo_Equipos
                                    where e.descripcion.Contains(filtro)
                                    select e).FirstOrDefault();
            if (carrera == null)
            {
                return NotFound();
            }
            return Ok(carrera);
        }

        //Guarda nuevo registro

        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarTipoEquipo([FromBody] tipo_equipo tipo_Equipo)
        {
            try
            {
                _equiposContexto.tipo_Equipos.Add(tipo_Equipo);
                _equiposContexto.SaveChanges();
                return Ok(tipo_Equipo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarTipoEquipo(int id, [FromBody] tipo_equipo equipoModificar)
        {
            tipo_equipo? equipoActual = (from e in _equiposContexto.tipo_Equipos
                                         where e.id_tipo_equipo == id
                                         select e).FirstOrDefault();
            if (equipoActual == null)
            {
                return NotFound();
            }

            equipoActual.descripcion = equipoModificar.descripcion;
            equipoActual.estado = equipoModificar.estado;

            _equiposContexto.Entry(equipoActual).State = EntityState.Modified;
            _equiposContexto.SaveChanges();

            return NotFound();
        }

        [HttpDelete]
        [Route("eliminar/{id}")]
        public ActionResult EliminarReserva(int id)
        {
            tipo_equipo? equipo = (from e in _equiposContexto.tipo_Equipos
                                   where e.id_tipo_equipo == id
                                   select e).FirstOrDefault();
            if (equipo == null)
                return NotFound();

            _equiposContexto.tipo_Equipos.Attach(equipo);
            _equiposContexto.tipo_Equipos.Remove(equipo);
            _equiposContexto.SaveChanges();

            return Ok(equipo);
        }
    }

}