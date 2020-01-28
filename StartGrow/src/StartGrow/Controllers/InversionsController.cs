using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StartGrow.Data;
using StartGrow.Models;
using StartGrow.Models.InversionViewModels;

namespace StartGrow.Controllers
{
    [Authorize(Roles = "Inversor")]
    public class InversionsController : Controller
    {
        private readonly ApplicationDbContext _context;        

        public InversionsController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        //SELECT
        //GET : Inversions
        public IActionResult SelectProyectosForInversion(string[] ids_tiposInversiones, string[] ids_areas, string[] ids_rating, int? invMin, float? interes, int? plazo)
        {
            SelectProyectosForInversionViewModel selectProyectos = new SelectProyectosForInversionViewModel();

            selectProyectos.TiposInversiones = _context.TiposInversiones;
            selectProyectos.Areas = _context.Areas;
            selectProyectos.Rating = _context.Rating;

            selectProyectos.Proyectos = _context.Proyecto.Include(p => p.Rating).Include(p => p.ProyectoAreas).
                ThenInclude<Proyecto, ProyectoAreas, Areas>(p => p.Areas).Include(p => p.ProyectoTiposInversiones).
                ThenInclude<Proyecto, ProyectoTiposInversiones, TiposInversiones>(p => p.TiposInversiones).Where(p => p.Plazo != null).Where(p => p.RatingId != null);

            if (ids_tiposInversiones.Length != 0)
            {
                selectProyectos.Proyectos = selectProyectos.Proyectos.Where(p => p.ProyectoTiposInversiones.Any(t1 => ids_tiposInversiones.Contains(t1.TiposInversiones.Nombre)));
            }

            if (ids_areas.Length != 0)
            {
                selectProyectos.Proyectos = selectProyectos.Proyectos.Where(p => p.ProyectoAreas.Any(t1 => ids_areas.Contains(t1.Areas.Nombre)));
            }

            if (ids_rating.Length != 0)
            {
                selectProyectos.Proyectos = selectProyectos.Proyectos.Where(p => ids_rating.Contains(p.Rating.Nombre));          
            }

            if (invMin != null)
            {
                selectProyectos.Proyectos = selectProyectos.Proyectos.Where(p => p.MinInversion.CompareTo((float)invMin) >= 0);
            }

            if (interes != null)
            {
                selectProyectos.Proyectos = selectProyectos.Proyectos.Where(p => ((float)p.Interes).CompareTo(interes) >= 0);
            }

            if (plazo != null)
            {
                selectProyectos.Proyectos = selectProyectos.Proyectos.Where(p => ((int)p.Plazo).CompareTo((int)plazo) >= 0);
            }

            selectProyectos.Proyectos.ToList();
            return View(selectProyectos);
        }

        //POST: Inversions
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SelectProyectosForInversion(SelectedProyectosForInversionViewModel selectedProyectos)
        {
            if (selectedProyectos.IdsToAdd != null)
            {
                return RedirectToAction("Create", selectedProyectos);
            }

            //Se mostrará un mensaje de error al usuario para indicar que seleccione algún proyecto
            ModelState.AddModelError(string.Empty, "Debes seleccionar al menos un proyecto para invertir");

            //Se recreará otra vez el View Model
            SelectProyectosForInversionViewModel selectProyectos = new SelectProyectosForInversionViewModel();

            selectProyectos.TiposInversiones = _context.TiposInversiones;
            selectProyectos.Areas = _context.Areas;
            selectProyectos.Rating = _context.Rating;

            selectProyectos.Proyectos = _context.Proyecto.Include(p => p.Rating).Include(p => p.ProyectoAreas).
                ThenInclude<Proyecto, ProyectoAreas, Areas>(p => p.Areas).Include(p => p.ProyectoTiposInversiones).
                ThenInclude<Proyecto, ProyectoTiposInversiones, TiposInversiones>(p => p.TiposInversiones).Where(p => p.Plazo != null).Where(p => p.RatingId != null);            

            return View(selectProyectos);
        }

        //CREATE
        // GET: Inversions
        public IActionResult Create(SelectedProyectosForInversionViewModel selectedProyectos)
        {
            Proyecto proyecto;            
            int id;

            InversionesCreateViewModel inversion = new InversionesCreateViewModel();
            inversion.inversiones = new List<InversionCreateViewModel>();

            Inversor inversor = _context.Users.OfType<Inversor>().Include(m => m.Monedero).FirstOrDefault<Inversor>(p => p.UserName.Equals(User.Identity.Name));            
            

            inversion.Name = inversor.Nombre;
            inversion.FirstSurname = inversor.Apellido1;
            inversion.SecondSurname = inversor.Apellido2;
            inversion.Cantidad = inversor.Monedero.Dinero;

            if (selectedProyectos.IdsToAdd == null || selectedProyectos.IdsToAdd.Count() == 0)
            {
                return RedirectToAction("SelectProyectosForInversion");
            }

            else
            {
                foreach (string ids in selectedProyectos.IdsToAdd)
                {
                    id = int.Parse(ids);
                    proyecto = _context.Proyecto.Include(p => p.Rating).Include(p => p.ProyectoAreas).
                              ThenInclude<Proyecto, ProyectoAreas, Areas>(p => p.Areas).Include(p => p.ProyectoTiposInversiones).
                              ThenInclude<Proyecto, ProyectoTiposInversiones, TiposInversiones>(p => p.TiposInversiones).Where(p => p.Plazo != null).Where(p => p.RatingId != null).
                              FirstOrDefault<Proyecto>(p => p.ProyectoId.Equals(id));                    
                    inversion.inversiones.Add(new InversionCreateViewModel()
                    {
                        Cuota = 0, Interes = (float)proyecto.Interes, NombreProyecto = proyecto.Nombre, MinInver = proyecto.MinInversion,
                        TiposInversion = new SelectList(proyecto.ProyectoTiposInversiones.Select(pro => pro.TiposInversiones.Nombre).ToList()),
                        Rating = proyecto.Rating.Nombre, Plazo = (int)proyecto.Plazo, ProyectoId = proyecto.ProyectoId, Cantidad = inversion.Cantidad,

                        inversion = new Inversion()
                        {Proyecto = proyecto}
                        
                    });

                }
            }

            ViewBag.Cuota = new SelectList(_context.Inversion.Select(c => c.Cuota).Distinct());          
            ViewBag.Inversor = inversor;

            return View(inversion);
        }

        // POST: Inversions
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InversionesCreateViewModel inversionCreate)
        {
            //var inversiones = new List<InversionCreateViewModel>();
            Inversion inversion;
            Proyecto proyecto;

            Inversor inversor = _context.Users.OfType<Inversor>().Include(m => m.Monedero).FirstOrDefault<Inversor>(p => p.UserName.Equals(User.Identity.Name));

            int[] idsInversion;
            ModelState.Clear();

            int i = 0;
            foreach (InversionCreateViewModel itemInversion in inversionCreate.inversiones)
            {
                proyecto = await _context.Proyecto.FirstOrDefaultAsync <Proyecto>(m => m.ProyectoId == itemInversion.inversion.Proyecto.ProyectoId);                
                inversion = new Inversion();                                

                if (itemInversion.Cuota.CompareTo(itemInversion.MinInver) <= 0 && itemInversion.TiposInversionSelected == null)
                {
                    ModelState.AddModelError("Cuota y Tipo de Inversión incorrecto", $"Cuota y Tipo de Inversión incorrectos en {inversionCreate.inversiones[i].NombreProyecto}. Por favor, vuelva a introducir los datos para realizar las inversiones.");
                }
                else if (itemInversion.Cuota.CompareTo((float) itemInversion.Cantidad) >= 0 && itemInversion.TiposInversionSelected == null)
                {
                    ModelState.AddModelError("Cuota y Tipo de Inversión incorrecto", $"Cuota y Tipo de Inversión incorrectos en {inversionCreate.inversiones[i].NombreProyecto}. Por favor, vuelva a introducir los datos para realizar las inversiones.");
                }
                else if (itemInversion.Cuota.CompareTo(itemInversion.MinInver) <= 0)
                {
                    ModelState.AddModelError("Ha introducido una cuota incorrecta", $"Ha introducido una cuota incorrecta en {inversionCreate.inversiones[i].NombreProyecto}. Por favor, vuelva a introducir los datos para realizar las inversiones.");
                }
                else if (itemInversion.Cuota.CompareTo((float)itemInversion.Cantidad) >= 0)
                {
                    ModelState.AddModelError("Ha introducido una cuota incorrecta", $"Ha introducido una cuota incorrecta en {inversionCreate.inversiones[i].NombreProyecto}. Por favor, vuelva a introducir los datos para realizar las inversiones.");
                }
                else if (itemInversion.TiposInversionSelected == null)
                {
                    ModelState.AddModelError("No ha seleccionado un tipo de inversión", $"No ha seleccionado un tipo de inversión en {inversionCreate.inversiones[i].NombreProyecto}. Por favor, vuelva a introducir los datos para realizar las inversiones.");
                }
                else
                {
                    itemInversion.inversion.Cuota = itemInversion.Cuota;                    
                    itemInversion.inversion.Intereses = (float) itemInversion.Interes;
                    itemInversion.inversion.Inversor = inversor;
                    
                    if (itemInversion.TiposInversionSelected == "Business Angels")
                    {
                        itemInversion.inversion.TipoInversionesId = 1;
                    }
                    else if (itemInversion.TiposInversionSelected == "Crownfunding")
                    {
                        itemInversion.inversion.TipoInversionesId = 2;
                    }
                    else
                    {
                        itemInversion.inversion.TipoInversionesId = 3;
                    }
                    
                    itemInversion.inversion.EstadosInversiones = "En Curso";                    
                    itemInversion.inversion.Total = (itemInversion.Cuota * (itemInversion.Interes / 100)) + itemInversion.Cuota;
                    itemInversion.inversion.Inversor.Monedero.Dinero = itemInversion.Cantidad - (decimal)itemInversion.Cuota;                    

                    _context.Add(itemInversion.inversion);
                }
                i++;
            }

            if (ModelState.ErrorCount > 0)
            {
                inversionCreate.Name = inversor.Nombre;
                inversionCreate.FirstSurname = inversor.Apellido1;
                inversionCreate.SecondSurname = inversor.Apellido2;
                
                SelectedProyectosForInversionViewModel selectedProyectos = new SelectedProyectosForInversionViewModel();

                int j = 0;
                int tam = inversionCreate.inversiones.Count;
                String[] IdsToAdd2 = new string[tam];

                foreach (InversionCreateViewModel itemInversion2 in inversionCreate.inversiones)
                {                    
                    int proyId = itemInversion2.ProyectoId;                    
                    string str = Convert.ToString(proyId);    
                    IdsToAdd2[j] = str;
                    selectedProyectos.IdsToAdd = IdsToAdd2;
                    j++;
                }

                selectedProyectos.IdsToAdd = IdsToAdd2;
                return Create(selectedProyectos);
            }

            await _context.SaveChangesAsync();

            //DETAILS
            idsInversion = new int[inversionCreate.inversiones.Count];
            
            for (int k = 0; k < idsInversion.Length; k++)
                idsInversion[k] = inversionCreate.inversiones[k].inversion.InversionId;
            
            InversionDetailsViewModel detailsViewModel = new InversionDetailsViewModel();
            detailsViewModel.ids = idsInversion;

            return RedirectToAction("Details", detailsViewModel);
        }

        //DETAILS
        // GET: Inversions
        public async Task<IActionResult> Details(InversionDetailsViewModel detailsIds)
        {
            if (detailsIds.ids == null || detailsIds.ids.Count() == 0)
            {
                return RedirectToAction("Create");
            }

            int[] ids = detailsIds.ids;

            var inversion = _context.Inversion.Include(s => s.TipoInversiones).
                Include(s => s.Proyecto).                
                ThenInclude<Inversion, Proyecto, Rating>(s => s.Rating).                
                Where(s => ids.Contains(s.InversionId)).ToList();


            return View(inversion);
        }

        public IActionResult ResumeInversiones(int[] ids)
        {
            if (ids.Length == 0)
            {
                return NotFound();
            }
            List<Inversion> inversiones = new List<Inversion>();
            foreach (int idInversion in ids)
            {
                inversiones.Add(_context.Inversion.Include(s => s.Proyecto).ThenInclude<Inversion, Proyecto, Rating>(s => s.Rating)
                    .Where(s => s.InversionId == idInversion).First());
            }

            if (inversiones.Count == 0)
            {
                return NotFound();
            }
            ViewBag.inversiones = inversiones;
            return View(inversiones);
        }

        // GET: Inversions
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Inversion.Include(i => i.Inversor).Include(i => i.Proyecto).Include(i => i.TipoInversiones);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Inversions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inversion = await _context.Inversion.SingleOrDefaultAsync(m => m.InversionId == id);
            if (inversion == null)
            {
                return NotFound();
            }
            ViewData["InversorId"] = new SelectList(_context.Users, "Id", "Id", inversion.InversorId);
            ViewData["ProyectoId"] = new SelectList(_context.Proyecto, "ProyectoId", "Nombre", inversion.ProyectoId);
            ViewData["TipoInversionesId"] = new SelectList(_context.TiposInversiones, "TiposInversionesId", "Nombre", inversion.TipoInversionesId);
            return View(inversion);
        }

        // POST: Inversions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InversionId,ProyectoId,ApplicationUserId,TipoInversionesId,Cuota,Intereses,Total")] Inversion inversion)
        {
            if (id != inversion.InversionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inversion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InversionExists(inversion.InversionId))
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
            ViewData["InversorId"] = new SelectList(_context.Users, "Id", "Id", inversion.InversorId);
            ViewData["ProyectoId"] = new SelectList(_context.Proyecto, "ProyectoId", "Nombre", inversion.ProyectoId);
            ViewData["TipoInversionesId"] = new SelectList(_context.TiposInversiones, "TiposInversionesId", "Nombre", inversion.TipoInversionesId);
            return View(inversion);
        }

        // GET: Inversions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inversion = await _context.Inversion
                .Include(i => i.Inversor)
                .Include(i => i.Proyecto)
                .Include(i => i.TipoInversiones)
                .SingleOrDefaultAsync(m => m.InversionId == id);
            if (inversion == null)
            {
                return NotFound();
            }

            return View(inversion);
        }

        // POST: Inversions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var inversion = await _context.Inversion.SingleOrDefaultAsync(m => m.InversionId == id);
            _context.Inversion.Remove(inversion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InversionExists(int id)
        {
            return _context.Inversion.Any(e => e.InversionId == id);
        }
    }
}