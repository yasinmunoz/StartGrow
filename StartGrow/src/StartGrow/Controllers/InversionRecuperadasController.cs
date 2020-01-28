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
using StartGrow.Models.InversionRecuperadaViewModels;

namespace StartGrow.Controllers
{
    [Authorize(Roles = "Inversor")]
    public class InversionRecuperadasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InversionRecuperadasController(ApplicationDbContext context)
        {
            _context = context;
        }






        //SELECT (GET)
        public IActionResult SelectInversionForRecuperarInversion(int? idInv, string inversionAreaSeleccionada,
            string inversionEstadoSeleccionado, string inversionTipoSeleccionado, string inversionRatingSeleccionado)
        {
            //Creamos un OBJETO de tipo SelectInversionForRecuperarInversionViewModel usado para renderizar la vista SelectInversionForRecuperarInversion.
            SelectInversionForRecuperarInversionViewModel selectInversiones = new SelectInversionForRecuperarInversionViewModel();


            //Solo mostrará las inversiones que estén en estado FINALIZADO o EN CURSO


            selectInversiones.Inversiones = _context.Inversion.Include(m => m.TipoInversiones)
             .Include(m => m.Proyecto)
             .ThenInclude(p => p.ProyectoAreas).ThenInclude(pa => pa.Areas)
             .Include(m => m.Proyecto).ThenInclude(r => r.Rating)
             .Where(m => m.EstadosInversiones != "Recaudacion" && m.Inversor.UserName == User.Identity.Name);


            //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@

            //******* FILTRAR POR ID *********
            if (idInv != null && idInv > 0)
                selectInversiones.Inversiones = selectInversiones.Inversiones.Where(id => id.InversionId == idInv);



            //******* FILTRAR POR AREA *******

            //Para que el desplegable ofrezca la lista de Estados que hay en la BD.
            selectInversiones.Areas = new SelectList(_context.Areas.Select(a => a.Nombre).ToList());

            //Utilizado si el usuario selecciona un Area en el desplegable. Al seleccionar dicha Area, 
            //se añadirá al IEnumerable Inversiones todas las inversiones donde el Area sea el Area seleccionado.
            if (inversionAreaSeleccionada != null)
                selectInversiones.Inversiones = selectInversiones.Inversiones.Where(i => i.Proyecto.ProyectoAreas.Any(p => p.Areas.Nombre.Contains(inversionAreaSeleccionada)));



            //******* FILTRAR POR ESTADO *******

            //Para que el desplegable ofrezca la lista de Estados que hay en la BD.
            selectInversiones.Estados = new SelectList(Enum.GetNames(typeof(StartGrow.Models.EstadosInversiones)));

            //Utilizado si el usuario selecciona un Estado en el desplegable. Al seleccionar dicho Estado, 
            //se añadirá al IEnumerable Inversiones todas las inversiones donde el Estado sea el Estado seleccionado.
            if (inversionEstadoSeleccionado != null)
                selectInversiones.Inversiones = selectInversiones.Inversiones.Where(i => i.EstadosInversiones.Equals(inversionEstadoSeleccionado));


            //******* FILTRAR POR TIPO *******

            //Para que el desplegable ofrezca la lista de TIPOS que hay en la BD.
            selectInversiones.Tipos = new SelectList(_context.TiposInversiones.Select(t => t.Nombre).ToList());

            //Utilizado si el usuario selecciona un Tipo en el desplegable. Al seleccionar dicho Tipo, 

            //se añadirá al IEnumerable Inversiones todas las inversiones donde el Tipo sea el Tipo seleccionado.
            if (inversionTipoSeleccionado != null)
                selectInversiones.Inversiones = selectInversiones.Inversiones.Where(i => i.TipoInversiones.Nombre.Contains(inversionTipoSeleccionado));



            //******* FILTRAR POR RATING *******

            //Para que el desplegable ofrezca la lista de Ratings que hay en la BD.
            selectInversiones.Ratings = new SelectList(_context.Rating.Select(r => r.Nombre).ToList());

            //Utilizado si el usuario selecciona un Rating en el desplegable. Al seleccionar dicho Rating, 
            //se añadirá al IEnumerable Inversiones todas las inversiones donde el rating sea el rating seleccionado.
            if (inversionRatingSeleccionado != null)
                selectInversiones.Inversiones = selectInversiones.Inversiones.Where(i => i.Proyecto.Rating.Nombre.Contains(inversionRatingSeleccionado));

            //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@


            //En este punto ejecuta la consulta con los filtros que hemos establecido.
            selectInversiones.Inversiones.ToList();
            return View(selectInversiones);
        }



        //SELECT (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SelectInversionForRecuperarInversion(SelectedInversionForRecuperarInversionViewModel selectedInversiones)
        {
            if (selectedInversiones.IdsToAdd != null)
            {
                return RedirectToAction("Create", selectedInversiones);
            }

            //Se mostrará un mensaje de error al usuario para indicar que seleccione alguna inversión.
            ModelState.AddModelError(string.Empty, "Debes seleccionar al menos una inversión");
            SelectInversionForRecuperarInversionViewModel selectInversiones = new SelectInversionForRecuperarInversionViewModel();
            selectInversiones.Areas = new SelectList(_context.Areas.Select(a => a.Nombre).ToList());
            selectInversiones.Estados = new SelectList(Enum.GetNames(typeof(StartGrow.Models.EstadosInversiones)));
            selectInversiones.Tipos = new SelectList(_context.TiposInversiones.Select(t => t.Nombre).ToList());
            selectInversiones.Ratings = new SelectList(_context.Rating.Select(r => r.Nombre).ToList());

            selectInversiones.Inversiones = _context.Inversion.Include(m => m.TipoInversiones)
             .Include(m => m.Proyecto)
             .ThenInclude(p => p.ProyectoAreas).ThenInclude(pa => pa.Areas)
             .Include(m => m.Proyecto).ThenInclude(r => r.Rating)
             .Where(m => m.EstadosInversiones != "Recaudacion" && m.Inversor.UserName == User.Identity.Name);

            return View(selectInversiones);
        }












        //CREATE (GET)
        public IActionResult Create(SelectedInversionForRecuperarInversionViewModel selectedInversiones)
        {
            Inversion inversion;
            int id;
            InversionRecuperadaCreateViewModel inv = new InversionRecuperadaCreateViewModel();
            inv.Inversiones = new List<Inversion>();
            Inversor inversor = _context.Users.OfType<Inversor>().FirstOrDefault<Inversor>(u => u.UserName.Equals(User.Identity.Name));

            if (selectedInversiones.IdsToAdd == null || selectedInversiones.IdsToAdd.Count() == 0)
            {
                return RedirectToAction("SelectInversionForRecuperarInversion");
               //ModelState.AddModelError("InversionNoSeleccionada", "Por favor, selecciona al menos una inversión para recuperarla");
            }
            else
            {
                foreach (string ids in selectedInversiones.IdsToAdd)
                {
                    id = int.Parse(ids);
                    inversion = _context.Inversion.Include(m => m.TipoInversiones)
                        .Include(m => m.Proyecto)
                        .ThenInclude(p => p.ProyectoAreas).ThenInclude(pa => pa.Areas)
                        .Include(m => m.Proyecto).ThenInclude(r => r.Rating)
                        .Where(m => m.EstadosInversiones != "Recaudacion" && m.Inversor.UserName == User.Identity.Name)
                        .FirstOrDefault<Inversion>(i => i.InversionId.Equals(id));

                    inv.Inversiones.Add(inversion);
                }
            }
            return View(inv);
        }



        
        
        //CREATE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InversionRecuperadaCreateViewModel invRecViewModel)
        {
            Inversion inversion;
            List<int> idsInversionesRecuperadas = new List<int>();
            ModelState.Clear();
            for (int i = 0; i < invRecViewModel.Inversiones.Count(); i++)
            {
                inversion = await _context.Inversion.FirstOrDefaultAsync<Inversion>(m => m.InversionId == invRecViewModel.Inversiones[i].InversionId);
                InversionRecuperada invRec = invRecViewModel.InversionesRecuperadas[i];
                if (invRec.CantidadRecuperada == 0)
                {
                    ModelState.AddModelError("CantidadNoIndicada", $"Debe indicar una cantidad mayor que 0 en la inversión {invRecViewModel.Inversiones[i].InversionId}.");
                }
                else if (invRec.Comentario == null)
                {
                    ModelState.AddModelError("ComentarioNoIndicado", $"Debe indicar un comentario para la inversión {invRecViewModel.Inversiones[i].InversionId}.");
                }
                else if (invRec.CantidadRecuperada > (inversion.Total))
                {
                    ModelState.AddModelError("CantidadNoPermitida", $"No se puede recuperar dicha cantidad para la inversión {invRecViewModel.Inversiones[i].InversionId}, eliga una cantidad menor.");
                }
                else
                {
                    invRec.Inversion = inversion;
                    invRec.FechaRecuperacion = DateTime.Now;

                    //Recuperamos el inversor que está conectado, a la vez que recuperamos su monedero.
                    inversion.Inversor = await _context.Users.OfType<Inversor>().Include(m => m.Monedero).FirstOrDefaultAsync<Inversor>(c => c.UserName.Equals(User.Identity.Name));

                    if (invRec.CantidadRecuperada <= inversion.Intereses)
                    {
                        inversion.Total = inversion.Total - invRec.CantidadRecuperada;
                        inversion.Intereses = inversion.Intereses - invRec.CantidadRecuperada;
                    }
                    else
                    {
                        inversion.Total = inversion.Total - invRec.CantidadRecuperada;
                        inversion.Intereses = 0;
                        float resto = invRec.CantidadRecuperada - inversion.Intereses;
                        inversion.Cuota = inversion.Cuota - resto;
                    }

                    //Actualizamos el dinero del monedero del inversor.    
                    ((Inversor)inversion.Inversor).Monedero.Dinero = ((Inversor)inversion.Inversor).Monedero.Dinero + (decimal) invRec.CantidadRecuperada;
                    invRec.Monedero = ((Inversor)inversion.Inversor).Monedero;
                    _context.Add(invRec);
                }
            }
            if (ModelState.ErrorCount > 0)
            {
                return View(invRecViewModel);
            }

            await _context.SaveChangesAsync();


            InversionRecuperadaDetailsViewModel detailsVM = new InversionRecuperadaDetailsViewModel();

            detailsVM.IdsToAdd = invRecViewModel.InversionesRecuperadas.Select(invrec => invrec.InversionRecuperadaId).ToArray();

            return RedirectToAction("Details", detailsVM);
        }









        // GET: InversionRecuperadas/Details/5
        public async Task<IActionResult> Details(InversionRecuperadaDetailsViewModel detailsVM)
        {
            Inversor inversor = _context.Users.OfType<Inversor>().FirstOrDefault<Inversor>(u => u.UserName.Equals(User.Identity.Name));
            InversionRecuperadaDetailsViewModel inv = new InversionRecuperadaDetailsViewModel();
            int[]ids = detailsVM.IdsToAdd;

            if (detailsVM.IdsToAdd == null || detailsVM.IdsToAdd.Count() == 0)
            {
                return RedirectToAction("Create");
            }
          
                var invRec = _context.InversionRecuperada.Include(m => m.Inversion).ThenInclude(t => t.TipoInversiones)
                    .Include(m => m.Inversion).ThenInclude(p => p.Proyecto).ThenInclude(pa => pa.ProyectoAreas).ThenInclude(a => a.Areas)
                    .Include(m => m.Inversion).ThenInclude(p => p.Proyecto).ThenInclude(r => r.Rating)
                    .Where(i => ids.Contains(i.InversionRecuperadaId)).ToList();


            return View(invRec);
        }









        // GET: InversionRecuperadas
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.InversionRecuperada.Include(i => i.Inversion).Include(i => i.Monedero);
            return View(await applicationDbContext.ToListAsync());
        }

        


        // GET: InversionRecuperadas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inversionRecuperada = await _context.InversionRecuperada.SingleOrDefaultAsync(m => m.InversionRecuperadaId == id);
            if (inversionRecuperada == null)
            {
                return NotFound();
            }
            ViewData["InversionId"] = new SelectList(_context.Inversion, "InversionId", "Id", inversionRecuperada.InversionId);
            ViewData["MonederoId"] = new SelectList(_context.Monedero, "MonederoId", "Id", inversionRecuperada.MonederoId);
            return View(inversionRecuperada);
        }

        // POST: InversionRecuperadas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InversionRecuperadaId,FechaRecuperacion,Comentario,CantidadRecuperada,MonederoId,InversionId")] InversionRecuperada inversionRecuperada)
        {
            if (id != inversionRecuperada.InversionRecuperadaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inversionRecuperada);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InversionRecuperadaExists(inversionRecuperada.InversionRecuperadaId))
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
            ViewData["InversionId"] = new SelectList(_context.Inversion, "InversionId", "Id", inversionRecuperada.InversionId);
            ViewData["MonederoId"] = new SelectList(_context.Monedero, "MonederoId", "Id", inversionRecuperada.MonederoId);
            return View(inversionRecuperada);
        }

        // GET: InversionRecuperadas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inversionRecuperada = await _context.InversionRecuperada
                .Include(i => i.Inversion)
                .Include(i => i.Monedero)
                .SingleOrDefaultAsync(m => m.InversionRecuperadaId == id);
            if (inversionRecuperada == null)
            {
                return NotFound();
            }

            return View(inversionRecuperada);
        }

        // POST: InversionRecuperadas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var inversionRecuperada = await _context.InversionRecuperada.SingleOrDefaultAsync(m => m.InversionRecuperadaId == id);
            _context.InversionRecuperada.Remove(inversionRecuperada);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InversionRecuperadaExists(int id)
        {
            return _context.InversionRecuperada.Any(e => e.InversionRecuperadaId == id);
        }
    }
}