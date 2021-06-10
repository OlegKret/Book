using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using WebBook.Models;
using WebBook.Repository.IRepository;

namespace WebBook.Controllers
{
    [Authorize]
    public class AuthorsController : Controller
    {
        

            private readonly IAuthorsRepository _authorsRepository;
            private readonly ICountryRepository _countryRepository;
            

        public AuthorsController(IAuthorsRepository authorsRepository, ICountryRepository countryRepository)
            {
                _authorsRepository = authorsRepository;
                _countryRepository = countryRepository;
            }
            public IActionResult Index()
            {
                Authors obj = new Authors();
               // Country obj1 = new Country();
           
            return View(new Authors() {});
            }

            [Authorize(Roles = "Admin")]
            
            public async Task<IActionResult> Upsert(int? id)
            {
                Authors obj = new Authors();
                
                if (id == null)
                {
                    return View(obj);
                }

                obj = await _authorsRepository.GetAsync(SD.AuthorsAPIPath, id.GetValueOrDefault(), HttpContext.Session.GetString("JWToken"));
                //obj1 = await _authorsRepository.GetAsync(SD.AuthorsAPIPath, id.GetValueOrDefault(), HttpContext.Session.GetString("JWToken"));
            if (obj == null)
                {
                    return NotFound();
                }
                return View(obj);
            }

        public async Task<IActionResult> GetAllAuthors()
            {
                return Json(new { data = await _authorsRepository.GetAllAsync(SD.AuthorsAPIPath, HttpContext.Session.GetString("JWToken")) });
            }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Upsert(Authors obj)
        {
           
                if (obj.Id == 0)
                {
                    await _authorsRepository.CreateAsync(SD.AuthorsAPIPath, obj, HttpContext.Session.GetString("JWToken"));
                }
                else
                {
                    await _authorsRepository.UpdateAsync(SD.AuthorsAPIPath + obj.Id, obj, HttpContext.Session.GetString("JWToken"));
                }
                return RedirectToAction(nameof(Index));
            }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _authorsRepository.DeleteAsync(SD.AuthorsAPIPath, id, HttpContext.Session.GetString("JWToken"));
            //var status1 = await _countryRepository.DeleteAsync(SD.AuthorsAPIPath, id, HttpContext.Session.GetString("JWToken"));
            if (status)
            {
                return Json(new { success = true, message = "Delete Successful" });
            }
            return Json(new { success = false, message = "Delete Not Successful" });
        }


    }
}
