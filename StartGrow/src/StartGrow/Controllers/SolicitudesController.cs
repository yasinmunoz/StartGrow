using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StartGrow.Data;
using StartGrow.Models;
using StartGrow.Models.SolicitudViewModels;
using Microsoft.AspNetCore.Authorization;
namespace StartGrow.Controllers
{
    [Authorize(Roles = "Trabajador")]
    public class SolicitudesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SolicitudesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Solicitudes
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Solicitud.Include(s => s.Proyecto).Include(s => s.Trabajador);
            return View(await applicationDbContext.ToListAsync());
        }


        //GET: Select
        public IActionResult SelectProyectosForSolicitud(string nombreProyecto, string[] tipoSeleccionado, string[] areasSeleccionada, int? capital, DateTime? fecha
    )
        {
            SelectProyectosForSolicitudViewModel selectProyecto = new SelectProyectosForSolicitudViewModel();
            selectProyecto.areas = _context.Areas;
            selectProyecto.Tipos = _context.TiposInversiones;
            selectProyecto.proyectos = _context.Proyecto.Include(m => m.Rating).Include(m => m.ProyectoAreas).
                ThenInclude<Proyecto, ProyectoAreas, Areas>(p => p.Areas).Include(p => p.ProyectoTiposInversiones).
                ThenInclude<Proyecto, ProyectoTiposInversiones, TiposInversiones>(p => p.TiposInversiones).Where(p => p.RatingId == null);

            if (nombreProyecto != null)
            {
                selectProyecto.proyectos = selectProyecto.proyectos.Where(p => p.Nombre.Contains(nombreProyecto));
            }

            if (capital != null)
            {

                selectProyecto.proyectos = selectProyecto.proyectos.Where(p => p.Importe.CompareTo((float)capital) >= 0);
            }

            if (tipoSeleccionado.Length != 0)
            {

                selectProyecto.proyectos = selectProyecto.proyectos.Where(p => p.ProyectoTiposInversiones.Any(ti => tipoSeleccionado.Contains(ti.TiposInversiones.Nombre)));

            }

            if (areasSeleccionada.Length != 0)

            {
                selectProyecto.proyectos = selectProyecto.proyectos.Where(p => p.ProyectoAreas.Any(ti => areasSeleccionada.Contains(ti.Areas.Nombre)));
                /*
                foreach (String i in areasSeleccionada)
                selectProyecto.proyectos = selectProyecto.proyectos.Where(p => p.ProyectoAreas.Any(a => a.Areas.Nombre.Contains(i)));
          */
            }



            if (fecha != null)
            {
                selectProyecto.proyectos = selectProyecto.proyectos.Where(p => p.FechaExpiracion.Date.Equals(fecha));
            }
            selectProyecto.proyectos.ToList();

            return View(selectProyecto);
        }

        //POST: Select 
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SelectProyectosForSolicitud(SelectedProyectosForSolicitudViewModel selectedproyectos)
        {
            if (selectedproyectos.IdsToAdd != null)
            {
                return RedirectToAction("Create", selectedproyectos);
            }


            ModelState.AddModelError(string.Empty, "Debes seleccionar al menos 1 proyecto para publicar");

            SelectProyectosForSolicitudViewModel selectProyecto = new SelectProyectosForSolicitudViewModel();
            selectProyecto.areas = _context.Areas;
            selectProyecto.Tipos = _context.TiposInversiones;
            selectProyecto.proyectos = _context.Proyecto.Include(m => m.Rating).Include(m => m.ProyectoAreas).
                           ThenInclude<Proyecto, ProyectoAreas, Areas>(p => p.Areas).Include(p => p.ProyectoTiposInversiones).
                           ThenInclude<Proyecto, ProyectoTiposInversiones, TiposInversiones>(p => p.TiposInversiones).Where(m => m.RatingId == null);
            return View(selectProyecto);
        }
        // GET: Solicitudes/Details/5  async Task<IActionResult>
        public async Task<IActionResult> Details(DetailsViewModel model)//) List<int> id)
        {
            int[] ids = model.ids;
            if (ids.Length == 0)
            {
                return NotFound();
            }

            var solicitud = _context.Solicitud.Include(s => s.Proyecto).ThenInclude<Solicitud, Proyecto, Rating>(s => s.Rating).Where(s => ids.Contains(s.SolicitudId)).ToList();
            

            if (solicitud.Count == 0)
            {
                return NotFound();
            }
            return View(solicitud);
        }


        public  IActionResult ResumeSolicitudes(int[] ids)
        {
            if (ids.Length == 0)
            {
                return NotFound();
            }
            List<Solicitud> solicitudes = new List<Solicitud>();
            foreach (int idSolicitud in ids)
            {
                solicitudes.Add(_context.Solicitud.Include(s => s.Proyecto).ThenInclude<Solicitud, Proyecto, Rating>(s => s.Rating)
                    .Where(s => s.SolicitudId == idSolicitud).First());
            }

            if (solicitudes.Count == 0)
            {
                return NotFound();
            }
            ViewBag.solicitudes = solicitudes;
            return View(solicitudes);
        }

        // GET: Solicitudes/Create
        public IActionResult Create(SelectedProyectosForSolicitudViewModel selectedProyectos)
        {
            Proyecto proyecto;
            int id;
            SolicitudesCreateViewModel solicitud = new SolicitudesCreateViewModel();
            solicitud.Solicitudes = new List<SolicitudCreateViewModel>();
            Trabajador trabajador = _context.Users.OfType<Trabajador>().FirstOrDefault<Trabajador>(u => u.UserName.Equals(User.Identity.Name));

            if (selectedProyectos.IdsToAdd == null)
            {
                ModelState.AddModelError("ProyectoNoSeleccionado", "Por favor, selecciona un proyecto para poder crear la solicitud");
            }
            else if (selectedProyectos.IdsToAdd.Length == 0)
            {
                ModelState.AddModelError("ProyectoNoSeleccionado", "Por favor, selecciona un proyecto para poder crear la solicitud");
            }
            else
            {
                foreach (string ids in selectedProyectos.IdsToAdd)
                {
                    id = int.Parse(ids);
                    proyecto = _context.Proyecto.Include(m => m.Rating).Where(m => m.RatingId == null).FirstOrDefault<Proyecto>(p => p.ProyectoId.Equals(id));
                    solicitud.Solicitudes.Add(new SolicitudCreateViewModel()
                    {
                        FechaSolicitud = DateTime.Now,
                        solicitud = new Solicitud()
                        { Proyecto = proyecto, Trabajador = trabajador }
                    });
                }
            }

            solicitud.SecondSurname = trabajador.Apellido2;
            solicitud.FirstSurname = trabajador.Apellido1;
            solicitud.Name = trabajador.Nombre;


            ViewBag.Estados = new SelectList(Enum.GetNames(typeof(StartGrow.Models.Estados)));
            ViewBag.Rating = new SelectList(_context.Rating.Select(c => c.Nombre).Distinct());
            ViewBag.Trabajador = trabajador;

            return View(solicitud);
        }

        // POST: Solicitudes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SolicitudesCreateViewModel solicitudCreate)
        {
            var solicitudes = new List<SolicitudCreateViewModel>();
            Trabajador trabajador;
            Proyecto proyecto;
            trabajador = _context.Users.OfType<Trabajador>().FirstOrDefault<Trabajador>(u => u.UserName.Equals(User.Identity.Name));
            // List<int> idsSolicitud = new List<int>();
            int[] idsSolicitud;
            ModelState.Clear();


            foreach (SolicitudCreateViewModel solicitudCV in solicitudCreate.Solicitudes)
            {
                proyecto = await _context.Proyecto.FirstOrDefaultAsync<Proyecto>(m => m.ProyectoId == solicitudCV.solicitud.Proyecto.ProyectoId);
                Boolean aceptada = solicitudCV.estados == Estados.Aceptada;
                if ((solicitudCV.estados == Estados.Aceptada && solicitudCV.rating.Equals("F")) || (solicitudCV.estados == Estados.Rechazada && !solicitudCV.rating.Equals("F")))
                {
                    ModelState.AddModelError("SolicitudIncorrecta", $"La solicitud de  {solicitudCV.solicitud.Proyecto.Nombre}, no puede estar aprobada y tener una calificacion de F o viceversa");


                }
                else
                {
                    if (solicitudCV.estados == Estados.Aceptada && (solicitudCV.interes <= 0 || solicitudCV.interes == null || solicitudCV.plazo <= 0 || solicitudCV.plazo == null))
                    {
                        ModelState.AddModelError("NoInteresPlazo", $"No se ha introducido correctamente el plazo o el interes del proyecto {solicitudCV.solicitud.Proyecto.Nombre}");

                    }
                    else
                    {
                        if (aceptada)
                        {
                            proyecto.Interes = (float)solicitudCV.interes * (float)0.1;
                            proyecto.Plazo = solicitudCV.plazo;
                        }
                        proyecto.Rating = _context.Rating.Where(r => r.Nombre.Equals(solicitudCV.rating)).FirstOrDefault();
                        solicitudCV.solicitud.Estado = solicitudCV.estados;
                        solicitudCV.solicitud.Trabajador = trabajador;
                        solicitudCV.solicitud.FechaSolicitud = DateTime.Now;
                        _context.Add(solicitudCV.solicitud);
                    }
                }

            }

            if (ModelState.ErrorCount > 0)
            {
                solicitudCreate.Name = trabajador.Nombre;
                solicitudCreate.FirstSurname = trabajador.Apellido1;
                solicitudCreate.SecondSurname = trabajador.Apellido2;
                ViewBag.Estados = new SelectList(Enum.GetNames(typeof(StartGrow.Models.Estados)));
                ViewBag.Rating = new SelectList(_context.Rating.Select(c => c.Nombre).Distinct());

                return View(solicitudCreate);
            }

            await _context.SaveChangesAsync();

            idsSolicitud = new int[solicitudCreate.Solicitudes.Count];
            //foreach(SolicitudCreateViewModel solicitudCV in solicitudCreate.Solicitudes )
            //{
            //    idsSolicitud.Add(solicitudCV.solicitud.SolicitudId);
            //}
            for (int i = 0; i < idsSolicitud.Length; i++)
                idsSolicitud[i] = solicitudCreate.Solicitudes[i].solicitud.SolicitudId;
            //TempData["idsSolicitud"] = idsSolicitud;
            DetailsViewModel detailsViewModel = new DetailsViewModel();
            detailsViewModel.ids = idsSolicitud;
            return RedirectToAction("Details",detailsViewModel); //new { id = idsSolicitud });


        }

        // GET: Solicitudes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var solicitud = await _context.Solicitud.SingleOrDefaultAsync(m => m.SolicitudId == id);
            if (solicitud == null)
            {
                return NotFound();
            }
            ViewData["ProyectoId"] = new SelectList(_context.Proyecto, "ProyectoId", "Nombre", solicitud.ProyectoId);
            ViewData["TrabajadorId"] = new SelectList(_context.Trabajador, "Id", "Id", solicitud.TrabajadorId);
            return View(solicitud);
        }

        // POST: Solicitudes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SolicitudId,TrabajadorId,ProyectoId,Estado,FechaSolicitud")] Solicitud solicitud)
        {
            if (id != solicitud.SolicitudId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(solicitud);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SolicitudExists(solicitud.SolicitudId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProyectoId"] = new SelectList(_context.Proyecto, "ProyectoId", "Nombre", solicitud.ProyectoId);
            ViewData["TrabajadorId"] = new SelectList(_context.Trabajador, "Id", "Id", solicitud.TrabajadorId);
            return View(solicitud);
        }

        // GET: Solicitudes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var solicitud = await _context.Solicitud
                .Include(s => s.Proyecto)
                .Include(s => s.Trabajador)
                .SingleOrDefaultAsync(m => m.SolicitudId == id);
            if (solicitud == null)
            {
                return NotFound();
            }

            return View(solicitud);
        }

        // POST: Solicitudes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var solicitud = await _context.Solicitud.SingleOrDefaultAsync(m => m.SolicitudId == id);
            _context.Solicitud.Remove(solicitud);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SolicitudExists(int id)
        {
            return _context.Solicitud.Any(e => e.SolicitudId == id);
        }
    }
}
