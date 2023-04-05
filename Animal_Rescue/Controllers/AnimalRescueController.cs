using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Animal_Rescue.Models;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;

namespace Animal_Rescue.Controllers
{
    public class AnimalRescueController : Controller
    {
        private readonly Contact_ProjectContext _context;

        public AnimalRescueController(Contact_ProjectContext context)
        {
            _context = context;
        }

        // GET: AnimalRescue
        public async Task<IActionResult> Index()
        {
            //Display animals sorted by species
              return View(await _context.AnimalRescue.OrderBy(c => c.Species).ToListAsync());
        }

        // GET: AnimalRescue/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.AnimalRescue == null)
            {
                return NotFound();
            }

            var animalRescue = await _context.AnimalRescue
                .FirstOrDefaultAsync(m => m.ID == id);
            if (animalRescue == null)
            {
                return NotFound();
            }

            return View(animalRescue);
        }

        // GET: AnimalRescue/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AnimalRescue/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,PrefixedID,Species,Name,Gender,Spayed,Breed,Colour,Birthday,Vaccine_Status,Identification,IdentificationNumber,Adoption_fee")] AnimalRescue animalRescue)
        {
            if (ModelState.IsValid)
            {
                // Adoption fee calculation based on age of animal
                TimeSpan ageDiff = (TimeSpan)(animalRescue.Birthday - DateTime.Now);
                double age = ageDiff.TotalDays / 365;
                age = System.Math.Abs(age);
                if(age <= 1)
                {
                    animalRescue.Adoption_fee = 300;
                }
                else if(age >= 10)
                {
                    animalRescue.Adoption_fee = 100;
                }
                else
                {
                    animalRescue.Adoption_fee = 200;
                }
                if(animalRescue.Vaccine_Status.IsNullOrEmpty())
                {
                    animalRescue.Vaccine_Status = "unknown";
                }
                if(animalRescue.Breed.IsNullOrEmpty())
                {
                    animalRescue.Breed = "unknown";
                }
                if(animalRescue.Species.IsNullOrEmpty())
                {
                    animalRescue.Species = "other";
                }
                _context.Add(animalRescue);
                await _context.SaveChangesAsync();
                TempData["message"] = "Entry successfully created";
                return RedirectToAction(nameof(Index));
            }
            return View(animalRescue);
        }

        // GET: AnimalRescue/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.AnimalRescue == null)
            {
                return NotFound();
            }

            var animalRescue = await _context.AnimalRescue.FindAsync(id);
            if (animalRescue == null)
            {
                return NotFound();
            }
            return View(animalRescue);
        }

        // POST: AnimalRescue/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,PrefixedID,Species,Name,Gender,Spayed,Breed,Colour,Birthday,Vaccine_Status,Identification,IdentificationNumber,Adoption_fee")] AnimalRescue animalRescue)
        {
            if (id != animalRescue.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(animalRescue);
                    await _context.SaveChangesAsync();
                    TempData["message"] = "Entry successfully edited";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnimalRescueExists(animalRescue.ID))
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
            return View(animalRescue);
        }

        // GET: AnimalRescue/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.AnimalRescue == null)
            {
                return NotFound();
            }

            var animalRescue = await _context.AnimalRescue
                .FirstOrDefaultAsync(m => m.ID == id);
            if (animalRescue == null)
            {
                return NotFound();
            }

            return View(animalRescue);
        }

        // POST: AnimalRescue/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.AnimalRescue == null)
            {
                return Problem("Entity set 'Contact_ProjectContext.AnimalRescue'  is null.");
            }
            var animalRescue = await _context.AnimalRescue.FindAsync(id);
            if (animalRescue != null)
            {
                _context.AnimalRescue.Remove(animalRescue);
            }
            
            await _context.SaveChangesAsync();
            TempData["message"] = "Entry successfully deleted";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult SearchByName(string Name)
        {
            if (Name == null)
            {
                return NotFound();
            }
            //Count will count number of results for search
            var count = 0;
            List<AnimalRescue> animalArray= new List<AnimalRescue>();
            foreach (var search in _context.AnimalRescue)
            {
                //Convert both data to lower case And compare for accurate results
                if (search.Name.ToLower().Contains(Name.ToLower()))
                {
                    count++;
                    animalArray.Add(search);
                }
                else if(search.Species.ToLower().Contains(Name.ToLower()))
                {
                    count++;
                    animalArray.Add(search);
                }
            }
            //if 0 results found in the DB
            if (count == 0)
            {
                ViewData["NoResult"] = "No result match with either name or species with the given string. Try again!!";
            }
            return View("Index", animalArray.AsEnumerable());
        }

        public IActionResult DisplayThreeOldest()
        {
            List<AnimalRescue> threeOldest = new List<AnimalRescue>();
            var animalsGroupBySpecies = _context.AnimalRescue.GroupBy(x => x.Species);
            foreach(var item in animalsGroupBySpecies)
            {
                List<AnimalRescue> animalArray = new List<AnimalRescue>();
                var count = 3;
                foreach (var animal in item)
                {
                    animalArray.Add(animal);
                    Console.WriteLine(animal);
                }
                Console.WriteLine(item.Key + " Count " +animalArray.Count);
                if(animalArray.Count < 3)
                { 
                    continue;
                }
                animalArray = animalArray.OrderBy(c => c.Birthday).Take(3).ToList();
                foreach(var entry in animalArray)
                {
                    threeOldest.Add(entry);
                }
            }
            return View("DisplayThreeOldest", threeOldest.AsEnumerable());
        }

        private bool AnimalRescueExists(int id)
        {
          return _context.AnimalRescue.Any(e => e.ID == id);
        }
    }
}
