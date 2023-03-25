using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webApiPractica.Models;
using Microsoft.EntityFrameworkCore;


namespace webApiPractica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class facultadesController : ControllerBase
    {
        private readonly equiposContext _equiposContexto;

        public facultadesController(equiposContext equiposContexto)

        {
            _equiposContexto = equiposContexto;
        }

        //Retorna el listado de todos los equipos existentes

        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<facultades> listadoEquipo = (from e in _equiposContexto.facultades
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
            facultades? equipo = (from e in _equiposContexto.facultades
                                  where e.facultad_id == id
                                  select e).FirstOrDefault();
            if (equipo == null)
            {
                return NotFound();
            }

            return Ok(equipo);
        }

        //Guarda nuevo registro

        [HttpGet]
        [Route("Find/{filtro}")]
        public IActionResult FindyByNombre(string filtro)
        {
            facultades? equipo = (from e in _equiposContexto.facultades
                                  where e.nombre_facultad.Contains(filtro)
                                  select e).FirstOrDefault();
            if (equipo == null)
            {
                return NotFound();
            }
            return Ok(equipo);
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarFacultad([FromBody] facultades facultades)
        {
            try
            {
                _equiposContexto.facultades.Add(facultades);
                _equiposContexto.SaveChanges();
                return Ok(facultades);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarFacultades(int id, [FromBody] facultades facultades)
        {
            facultades? equipoActual = (from e in _equiposContexto.facultades
                                        where e.facultad_id == id
                                        select e).FirstOrDefault();
            if (equipoActual == null)
            {
                return NotFound();
            }

            equipoActual.nombre_facultad = facultades.nombre_facultad;


            _equiposContexto.Entry(equipoActual).State = EntityState.Modified;
            _equiposContexto.SaveChanges();

            return NotFound();
        }

        [HttpDelete]
        [Route("eliminar/{id}")]
        public ActionResult EliminarFacultad(int id)
        {
            facultades? equipo = (from e in _equiposContexto.facultades
                                  where e.facultad_id == id
                                  select e).FirstOrDefault();
            if (equipo == null)
                return NotFound();

            _equiposContexto.facultades.Attach(equipo);
            _equiposContexto.facultades.Remove(equipo);
            _equiposContexto.SaveChanges();

            return Ok(equipo);
        }

    }
}