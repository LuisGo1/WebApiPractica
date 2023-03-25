using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webApiPractica.Models;
using Microsoft.EntityFrameworkCore;

namespace webApiPractica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class reservasController : ControllerBase
    {
        private readonly equiposContext _equiposContexto;

        public reservasController(equiposContext equiposContexto)

        {
            _equiposContexto = equiposContexto;
        }

        //Retorna el listado de todos los equipos existentes

        [HttpGet]
        [Route("GetAll")]

        public IActionResult get()
        {
            var reserva = (from r in _equiposContexto.reservas
                           join e in _equiposContexto.equipos on r.equipo_id equals e.id_equipos
                           join u in _equiposContexto.usuarios on r.usuario_id equals u.usuario_id
                           join er in _equiposContexto.estados_Reservas on r.estado_reserva_id equals er.estado_res_id
                           select new
                           {
                               r.reserva_id,
                               r.equipo_id,
                               e.nombre,
                               r.usuario_id,
                               nombre_usuario = u.nombre,
                               r.fecha_salida,
                               r.hora_salida,
                               r.tiempo_reserva,
                               r.estado_reserva_id,
                               estado_reserva = er.estado,
                               r.fecha_retorno,
                               r.hora_retorno,

                           }).ToList();
            if (reserva.Count == 0)
            {
                return NotFound();
            }
            return Ok(reserva);
        }

        //Retorna los registros de una tabla filtrados por su ID

        [HttpGet]
        [Route("GetById/{id}")]
        public IActionResult Get(int id)
        {
            reservas? carrera = (from e in _equiposContexto.reservas
                                 where e.reserva_id == id
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
        public IActionResult GuardarReserva([FromBody] reservas reservas)
        {
            try
            {
                _equiposContexto.reservas.Add(reservas);
                _equiposContexto.SaveChanges();
                return Ok(reservas);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarReserva(int id, [FromBody] reservas equipoModificar)
        {
            reservas? equipoActual = (from e in _equiposContexto.reservas
                                      where e.reserva_id == id
                                      select e).FirstOrDefault();
            if (equipoActual == null)
            {
                return NotFound();
            }

            equipoActual.equipo_id = equipoModificar.equipo_id;
            equipoActual.usuario_id = equipoModificar.usuario_id;
            equipoActual.fecha_salida = equipoModificar.fecha_salida;
            equipoActual.hora_salida = equipoModificar.hora_salida;
            equipoActual.tiempo_reserva = equipoModificar.tiempo_reserva;
            equipoActual.estado_reserva_id = equipoModificar.estado_reserva_id;
            equipoActual.fecha_retorno = equipoModificar.fecha_retorno;

            _equiposContexto.Entry(equipoActual).State = EntityState.Modified;
            _equiposContexto.SaveChanges();

            return NotFound();
        }

        [HttpDelete]
        [Route("eliminar/{id}")]
        public ActionResult EliminarReserva(int id)
        {
            reservas? equipo = (from e in _equiposContexto.reservas
                                where e.reserva_id == id
                                select e).FirstOrDefault();
            if (equipo == null)
                return NotFound();

            _equiposContexto.reservas.Attach(equipo);
            _equiposContexto.reservas.Remove(equipo);
            _equiposContexto.SaveChanges();

            return Ok(equipo);
        }
    }
}