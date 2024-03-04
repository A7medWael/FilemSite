using FilemSite.ContextModel;
using FilemSite.Models;
using FilemSite.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FilemSite.Controllers
{
    public class MoviesController : Controller
    {
        private readonly FilmContext filmContext;
       
        public MoviesController( FilmContext _filmContext )
        {
            filmContext = _filmContext;
            
        }
        public async Task<IActionResult> Index()
        {
            var movi = await filmContext.Movies.ToListAsync();
            return View(movi);
        }
        public async Task<IActionResult> Creat()
        {
           var createMovie=new ViewFormCreat()
           {
               genre=await filmContext.Genres.OrderBy(o=>o.Name).ToListAsync(),
           };
            return View("MovieForm", createMovie);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Creat(ViewFormCreat formCreat)
        {
            //if(!ModelState.IsValid)
            //{
            //  formCreat.genre =await filmContext.Genres.OrderBy(o=>o.Name).ToListAsync();
            //    return View(formCreat);
            //}

            var file = Request.Form.Files;
            if (!file.Any())
            {
                formCreat.genre = await filmContext.Genres.OrderBy(o => o.Name).ToListAsync();
                ModelState.AddModelError("Poster", "Select Any File");
                return View("MovieForm", formCreat);
            }
            var poste=file.FirstOrDefault();
            var allowedextention=new List<string>{ ".jpg",".png"};
            if (!allowedextention.Contains(Path.GetExtension(poste.FileName.ToLower())))
            {
                formCreat.genre = await filmContext.Genres.OrderBy(o => o.Name).ToListAsync();
                ModelState.AddModelError("Poster", "Only .png and .jpg");
                return View("MovieForm", formCreat);
            }
            if (poste.Length > 1048576)
            {
                formCreat.genre = await filmContext.Genres.OrderBy(o => o.Name).ToListAsync();
                ModelState.AddModelError("Poster", "Select small File");
                return View("MovieForm", formCreat);
            }
           using var stream=new MemoryStream();
            await poste.CopyToAsync(stream);
            var movi = new Movie()
            {
                Title=formCreat.Title,
                Year=formCreat.Year,
                Rate=formCreat.Rate,
                StoryLine=formCreat.StoryLine,
                Poster=stream.ToArray(),
                genreId=formCreat.genreId
            };
            filmContext.Movies.Add(movi);
            filmContext.SaveChanges();
           
             return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return BadRequest();

            var movi=await filmContext.Movies.SingleOrDefaultAsync(o => o.Id == id);
            if (movi == null)
                return NotFound();

            var createMovie = new ViewFormCreat()
            {
                genreId=movi.genreId,
               Id=movi.Id,
               Rate=movi.Rate,
               Year=movi.Year,
              Title=movi.Title,
               StoryLine=movi.StoryLine,
                Poster=movi.Poster,
                genre = await filmContext.Genres.OrderBy(o => o.Name).ToListAsync()
            };
            return View("MovieForm",createMovie);
        }
        [HttpPost]
        public async Task<IActionResult>Edit(ViewFormCreat movi)
        {
            var mov=await filmContext.Movies.FindAsync(movi.Id);
            if (mov == null)
                return NotFound();

            var file = Request.Form.Files;
            if(file.Any())
            {
                var poster=file.FirstOrDefault();
                 using  var stream=new MemoryStream();
                await poster.CopyToAsync(stream);
                movi.Poster=stream.ToArray();
                mov.Poster = movi.Poster;

            }
            mov.genreId = movi.genreId;
            mov.Id = movi.Id;
          mov.Rate = movi.Rate;
            mov.Year = movi.Year;
            mov.Title = movi.Title;
            mov.StoryLine = movi.StoryLine;
           
          
            filmContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult>Details(int Id)
        {
            if(Id == 0)
            {
                return RedirectToAction(nameof(Creat));
            }
            else
            {
                
             var detail = await filmContext.Movies.Include(n=>n.genre).SingleOrDefaultAsync(m=>m.Id==Id);
                return View(detail);
            }
           
           
        }
        public async Task<IActionResult>Delete(int Id)
        {
            var delm = await filmContext.Movies.FindAsync(Id);
            filmContext.Movies.Remove(delm);
            filmContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
