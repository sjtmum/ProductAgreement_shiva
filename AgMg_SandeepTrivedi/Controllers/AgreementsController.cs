using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AgMg_SandeepTrivedi.Data;
using AgMg_SandeepTrivedi.Models;

namespace AgMg_SandeepTrivedi.Controllers
{
    public class AgreementsController : Controller
    {
        private readonly ApplicationDbContext _context;
        DBCommoncs dBCommoncs;
        public AgreementsController(ApplicationDbContext context)
        {
            _context = context;
            dBCommoncs = new DBCommoncs(_context);
        }

        // GET: Agreements
        public async Task<IActionResult> Index()
        {
            //return View(await _context.Agreements.ToListAsync()); //call for sysnc
            return View(await dBCommoncs.GetAgreements()); //call for Index view
            //return View(); //call for Index_paging view
        }
        public async Task<IActionResult> GetAgreements()
        {
            var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            //var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            //string sortColumn = Request.Form["order[0][column]"].FirstOrDefault();
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;

            var agreements = await dBCommoncs.GetAgreements();

            //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            //{
            //    agreements = agreements.OrderBy(sortColumn + " " + sortColumnDirection);
            //}

            if (!string.IsNullOrEmpty(searchValue))
            {
                agreements = agreements.Where(m => m.ProductDesc == searchValue || m.ProductGroupDesc == searchValue);
            }

            int recordsTotal = agreements.Count();
            
            var data = agreements.Skip(skip).Take(pageSize).ToList();

            return Json(new { draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
        }
        

        // GET: Agreements/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var agreement = await _context.Agreements
                .FirstOrDefaultAsync(m => m.Id == id);
            if (agreement == null)
            {
                return NotFound();
            }

            return View(agreement);
        }
        [HttpGet]
        [Route("Agreements/GetProductList")]
        public async Task<JsonResult> GetProductList(int prodGroupId)
        {
            var prodList = new SelectList(await dBCommoncs.GetProductList(prodGroupId,true), "Id", "ProductDescription");
            return Json(prodList);
        }   

        [HttpGet]
        public JsonResult GetProductList2(int prodGroupId)
        {
            var prodList = new SelectList(_context.Products.ToList(), "Id", "ProductDescription");
            return Json(prodList);
        }

        // GET: Agreements/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.prodGroupList = new SelectList(await dBCommoncs.GetProductGroupList(true), "Id", "GroupDescription");
            //return View();
            return PartialView("Create");
        }

        // POST: Agreements/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,ProductGroupId,ProductId,ProductPrice,NewPrice,EffectiveDate,ExpirationDate,Active")] Agreement agreement)
        {
            if (ModelState.IsValid)
            {
                if (!ValidateAgreement(agreement)) return ValidationProblem();
                agreement.UserId = User.Identity.Name;
                _context.Add(agreement);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(agreement);
        }

        // GET: Agreements/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var agreement = await _context.Agreements.FindAsync(id);
            if (agreement == null)
            {
                return NotFound();
            }
            ViewBag.prodGroupList = new SelectList(await dBCommoncs.GetProductGroupList(true), "Id", "GroupDescription");
            return View(agreement);
        }

        // POST: Agreements/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,ProductGroupId,ProductId,ProductPrice,NewPrice,EffectiveDate,ExpirationDate,Active")] Agreement agreement)
        {
            if (id != agreement.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (!ValidateAgreement(agreement)) return ValidationProblem();
                    agreement.UserId = User.Identity.Name;
                    _context.Update(agreement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AgreementExists(agreement.Id))
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
            return View(agreement);
        }

        // GET: Agreements/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var agreement = await _context.Agreements
                .FirstOrDefaultAsync(m => m.Id == id);
            if (agreement == null)
            {
                return NotFound();
            }

            return View(agreement);
        }

        // POST: Agreements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var agreement = await _context.Agreements.FindAsync(id);
            _context.Agreements.Remove(agreement);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AgreementExists(int id)
        {
            return _context.Agreements.Any(e => e.Id == id);
        }
        private bool ValidateAgreement(Agreement agreement)
        {
            if ((agreement.ProductId == 0) || (agreement.ProductGroupId == 0)) return false;
            if (agreement.EffectiveDate > agreement.ExpirationDate) return false;
            return true;
        }
    }
}
