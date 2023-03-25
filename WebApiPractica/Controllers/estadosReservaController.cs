using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webApiPractica.Models;
using Microsoft.EntityFrameworkCore;


namespace webApiPractica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class estadosReservaController : ControllerBase
    {
        private readonly equiposContext _equiposContexto;

        public estadosReservaController(equiposContext equiposContexto)

        {
            _equiposContexto = equiposContexto;
        }

        //Retorna el listado de todos los equipos existentes

        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<estados_reserva> listadoEquipo = (from e in _equiposContexto.estados_Reservas
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
            estados_reserva? equipo = (from e in _equiposContexto.estados_Reservas
                                       where e.estado_res_id == id
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
        public IActionResult FindyByEstado(string filtro)
        {
            estados_reserva? equipo = (from e in _equiposContexto.estados_Reservas
                                       where e.estado.Contains(filtro)
                                       select e).FirstOrDefault();
            if (equipo == null)
            {
                return NotFound();
            }
            return Ok(equipo);
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarEstadoReserva([FromBody] estados_reserva estados_Reserva)
        {
            try
            {
                _equiposContexto.estados_Reservas.Add(estados_Reserva);
                _equiposContexto.SaveChanges();
                return Ok(estados_Reserva);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarEstadoReserva(int id, [FromBody] estados_reserva equipoModificar)
        {
            estados_reserva? equipoActual = (from e in _equiposContexto.estados_Reservas
                                             where e.estado_res_id == id
                                             select e).FirstOrDefault();
            if (equipoActual == null)
            {
                return NotFound();
            }

            equipoActual.estado = equipoModificar.estado;


            _equiposContexto.Entry(equipoActual).State = EntityState.Modified;
            _equiposContexto.SaveChanges();

            return NotFound();
        }

        [HttpDelete]
        [Route("eliminar/{id}")]
        public ActionResult EliminarEstadoReserva(int id)
        {
            estados_reserva? equipo = (from e in _equiposContexto.estados_Reservas
                                       where e.estado_res_id == id
                                       select e).FirstOrDefault();
            if (equipo == null)
                return NotFound();

            _equiposContexto.estados_Reservas.Attach(equipo);
            _equiposContexto.estados_Reservas.Remove(equipo);
            _equiposContexto.SaveChanges();

            return Ok(equipo);
        }

    }

}