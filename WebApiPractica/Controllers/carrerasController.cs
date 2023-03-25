using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webApiPractica.Models;
using Microsoft.EntityFrameworkCore;


namespace webApiPractica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class carrerasController : ControllerBase
    {
        private readonly equiposContext _equiposContexto;

        public carrerasController(equiposContext equiposContexto)

        {
            _equiposContexto = equiposContexto;
        }

        //Retorna el listado de todos los equipos existentes

        [HttpGet]
        [Route("GetAll")]

        public IActionResult get()
        {
            var carreras = (from c in _equiposContexto.carreras
                            join f in _equiposContexto.facultades on c.facultad_id equals f.facultad_id
                            select new
                            {
                                c.carrera_id,
                                c.nombre_carrera,
                                c.facultad_id,
                                f.nombre_facultad,

                            }).ToList();
            if (carreras.Count == 0)
            {
                return NotFound();
            }
            return Ok(carreras);
        }

        //Retorna los registros de una tabla filtrados por su ID

        [HttpGet]
        [Route("GetById/{id}")]
        public IActionResult Get(int id)
        {
            carreras? carrera = (from e in _equiposContexto.carreras
                                 where e.carrera_id == id
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
            carreras? carrera = (from e in _equiposContexto.carreras
                                 where e.nombre_carrera.Contains(filtro)
                                 select e).FirstOrDefault();
            if (carrera == null)
            {
                return NotFound();
            }
            return Ok(carrera);
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarCarrera([FromBody] carreras carreras)
        {
            try
            {
                _equiposContexto.carreras.Add(carreras);
                _equiposContexto.SaveChanges();
                return Ok(carreras);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarCarrera(int id, [FromBody] carreras equipoModificar)
        {
            carreras? equipoActual = (from e in _equiposContexto.carreras
                                      where e.carrera_id == id
                                      select e).FirstOrDefault();
            if (equipoActual == null)
            {
                return NotFound();
            }

            equipoActual.nombre_carrera = equipoModificar.nombre_carrera;
            equipoActual.facultad_id = equipoModificar.facultad_id;

            _equiposContexto.Entry(equipoActual).State = EntityState.Modified;
            _equiposContexto.SaveChanges();

            return NotFound();
        }

        [HttpDelete]
        [Route("eliminar/{id}")]
        public ActionResult EliminarCarrera(int id)
        {
            carreras? equipo = (from e in _equiposContexto.carreras
                                where e.carrera_id == id
                                select e).FirstOrDefault();
            if (equipo == null)
                return NotFound();

            _equiposContexto.carreras.Attach(equipo);
            _equiposContexto.carreras.Remove(equipo);
            _equiposContexto.SaveChanges();

            return Ok(equipo);
        }
    }
}