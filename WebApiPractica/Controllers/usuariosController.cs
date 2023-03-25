using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webApiPractica.Models;
using Microsoft.EntityFrameworkCore;


namespace webApiPractica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class usuariosController : ControllerBase
    {
        private readonly equiposContext _equiposContexto;

        public usuariosController(equiposContext equiposContexto)

        {
            _equiposContexto = equiposContexto;
        }

        //Retorna el listado de todos los equipos existentes

        [HttpGet]
        [Route("GetAll")]

        public IActionResult get()
        {
            var usuarios = (from u in _equiposContexto.usuarios
                            join c in _equiposContexto.carreras on u.carrera_id equals c.carrera_id
                            select new
                            {
                                u.usuario_id,
                                u.nombre,
                                u.documento,
                                u.tipo,
                                u.carnet,
                                u.carrera_id,
                                c.nombre_carrera,

                            }).ToList();
            if (usuarios.Count == 0)
            {
                return NotFound();
            }
            return Ok(usuarios);
        }

        //Retorna los registros de una tabla filtrados por su ID

        [HttpGet]
        [Route("GetById/{id}")]
        public IActionResult Get(int id)
        {
            usuarios? carrera = (from e in _equiposContexto.usuarios
                                 where e.usuario_id == id
                                 select e).FirstOrDefault();
            if (carrera == null)
            {
                return NotFound();
            }

            return Ok(carrera);
        }

        [HttpGet]
        [Route("Find/{filtro}")]
        public IActionResult FindyByName(string filtro)
        {
            usuarios? carrera = (from e in _equiposContexto.usuarios
                                 where e.nombre.Contains(filtro)
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
        public IActionResult GuardarUsuario([FromBody] usuarios usuarios)
        {
            try
            {
                _equiposContexto.usuarios.Add(usuarios);
                _equiposContexto.SaveChanges();
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarUsuario(int id, [FromBody] usuarios equipoModificar)
        {
            usuarios? equipoActual = (from e in _equiposContexto.usuarios
                                      where e.usuario_id == id
                                      select e).FirstOrDefault();
            if (equipoActual == null)
            {
                return NotFound();
            }

            equipoActual.nombre = equipoModificar.nombre;
            equipoActual.documento = equipoModificar.documento;
            equipoActual.tipo = equipoModificar.tipo;
            equipoActual.carnet = equipoModificar.carnet;
            equipoActual.carrera_id = equipoModificar.carrera_id;

            _equiposContexto.Entry(equipoActual).State = EntityState.Modified;
            _equiposContexto.SaveChanges();

            return NotFound();
        }

        [HttpDelete]
        [Route("eliminar/{id}")]
        public ActionResult EliminarUsuario(int id)
        {
            usuarios? equipo = (from e in _equiposContexto.usuarios
                                where e.usuario_id == id
                                select e).FirstOrDefault();
            if (equipo == null)
                return NotFound();

            _equiposContexto.usuarios.Attach(equipo);
            _equiposContexto.usuarios.Remove(equipo);
            _equiposContexto.SaveChanges();

            return Ok(equipo);
        }
    }
}