using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebAppWithQueries.Data;
using WebAppWithQueries.Models;

namespace WebAppWithQueries.Controllers
{
    public class MyItemController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MyItemController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MyItem
        public async Task<IActionResult> Index()
        {
            return View(await _context.MyItems.ToListAsync());
        }

        // GET: MyItem/QueriedData
        public async Task<IActionResult> QueriedData()
        {
            var topX = 2;
            var myItems =
              (from m in _context.MyItems.TagWith($"This retrieves top {topX} Items!")
               orderby m.Id ascending
               select m).Take(topX);

            return View(await myItems.ToListAsync());
        }

        IQueryable<MyItem> GetTopItems(int topX) =>
            (from m in _context.MyItems.TagWith($"This retrieves top {topX} Items!")
             orderby m.Id ascending
             select m).Take(topX);

        IQueryable<T> GetSubset<T>(IQueryable<T> source, int subset) =>
            source.TagWith($"Getting subset {subset}").Take(subset);

        // GET: MyItem/QueriedDataWithTags
        public IActionResult QueriedDataWithTags()
        {
            var startWithX = 3;
            var subsetY = 1;
            var myItems = GetSubset(GetTopItems(startWithX), subsetY).ToList();

            return View(myItems);
        }

        // GET: MyItem/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var myItem = await _context.MyItems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (myItem == null)
            {
                return NotFound();
            }

            return View(myItem);
        }

        // GET: MyItem/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MyItem/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MyItemName,MyItemDescription")] MyItem myItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(myItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(myItem);
        }

        // GET: MyItem/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var myItem = await _context.MyItems.FindAsync(id);
            if (myItem == null)
            {
                return NotFound();
            }
            return View(myItem);
        }

        // POST: MyItem/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MyItemName,MyItemDescription")] MyItem myItem)
        {
            if (id != myItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(myItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MyItemExists(myItem.Id))
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
            return View(myItem);
        }

        // GET: MyItem/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var myItem = await _context.MyItems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (myItem == null)
            {
                return NotFound();
            }

            return View(myItem);
        }

        // POST: MyItem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var myItem = await _context.MyItems.FindAsync(id);
            _context.MyItems.Remove(myItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MyItemExists(int id)
        {
            return _context.MyItems.Any(e => e.Id == id);
        }
    }
}
