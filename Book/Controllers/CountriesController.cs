using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Book.Dtos;
using Book.Models;
using Book.Services;
using Microsoft.AspNetCore.Mvc;

namespace Book.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private ICountryReposotory _countryReposotory;
        private IAuthorRepository _authorRepository;

        public CountriesController(ICountryReposotory countryReposotory, IAuthorRepository authorRepository)
        {
            _countryReposotory = countryReposotory;
            _authorRepository = authorRepository;
        }

        //api/countries
        /// <summary>
        /// Get Countries
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CountryDto>))]
        public IActionResult GetCountries()
        {
            var countries = _countryReposotory.GetCountries().ToList();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

                var countriesDto = new List<CountryDto>();
            foreach (var country in countries)
            {
                countriesDto.Add(new CountryDto
                {
                    Id = country.Id,
                    Name = country.Name
                });
            }

            return Ok(countriesDto);
        }

        //api/countries/countryId

        /// <summary>
        /// Get Country
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        [HttpGet("{countryId}", Name = "GetCountry")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(CountryDto))]
        public IActionResult GetCountry(int countryId)
        {
            if (!_countryReposotory.CountryExists(countryId))
                return NotFound();

                var country = _countryReposotory.GetCountry(countryId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var countryDto = new CountryDto()
            {
                Id = country.Id,
                Name = country.Name
            };
           

            return Ok(countryDto);
        }

        //api/countries/authors/authorId
        /// <summary>
        /// Get Author Country
        /// </summary>
        /// <param name="authorId"></param>
        /// <returns></returns>
        [HttpGet("authors/{authorId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(CountryDto))]
        public IActionResult GetCountryOfAnAuthor(int authorId)
        {
            if (!_authorRepository.AuthorExists(authorId))
                return NotFound();

                var country = _countryReposotory.GetCountryOfAnAuthor(authorId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var countryDto = new CountryDto()
            {
                Id = country.Id,
                Name = country.Name
            };

            return Ok(countryDto);
        }

        //api/countries/55/authors
        /// <summary>
        /// Get Authors From a Country
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        [HttpGet("{countryId}/authors")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<AuthorDto>))]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public IActionResult GetAuthorsFromACountry(int countryId)
        {
            if (!_countryReposotory.CountryExists(countryId))
                return NotFound();

            var authors = _countryReposotory.GetAuthorsFromACountry(countryId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var authorsDto = new List<AuthorDto>();

            foreach (var author in authors)
            {
                authorsDto.Add(new AuthorDto
                {
                    Id = author.Id,
                    FirstName = author.FirstName,
                    LastName = author.LastName
                });
            }

            return Ok(authorsDto);
        }
        /// <summary>
        /// Create Country
        /// </summary>
        /// <param name="countryToCreate"></param>
        /// <returns></returns>
        //api/countries
        //api/countries
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Country))]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult CreateCountry([FromBody] Country countryToCreate)
        {
            if (countryToCreate == null)
                return BadRequest(ModelState);

            var country = _countryReposotory.GetCountries()
                .Where(c => c.Name.Trim().ToUpper() == countryToCreate.Name.Trim().ToUpper())
                .FirstOrDefault();

            if (country != null)
            {
                ModelState.AddModelError("", $"Country {countryToCreate.Name} already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_countryReposotory.CreateCountry(countryToCreate))
            {
                ModelState.AddModelError("", $"Something went wrong saving {countryToCreate.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetCountry", new { countryId = countryToCreate.Id }, countryToCreate);
        }
        //api/countries/countryId
        /// <summary>
        /// Update Country
        /// </summary>
        /// <param name="countryId"></param>
        /// <param name="updatedCountryInfo"></param>
        /// <returns></returns>
        [HttpPut("{countryId}")]
        [ProducesResponseType(204)] //no content
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]

        public IActionResult UpdateCountry(int countryId, [FromBody] Country updatedCountryInfo)
        {
            if (updatedCountryInfo == null)
                return BadRequest(ModelState);

            if (countryId != updatedCountryInfo.Id)
                return BadRequest(ModelState);

            if (!_countryReposotory.CountryExists(countryId))
                return NotFound();

            if (_countryReposotory.IsDuplicateCountryName(countryId, updatedCountryInfo.Name))
            {
                ModelState.AddModelError("", $"Country {updatedCountryInfo.Name} already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_countryReposotory.UpdateCountry(updatedCountryInfo))
            {
                ModelState.AddModelError("", $"Something went wrong updating {updatedCountryInfo.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        //api/countries/countryId
        /// <summary>
        /// Delete Country
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        [HttpDelete("{countryId}")]
        [ProducesResponseType(204)] //no content
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public IActionResult DeleteCountry(int countryId)
        {
            if (!_countryReposotory.CountryExists(countryId))
                return NotFound();

            var countryToDelete =_countryReposotory.GetCountry(countryId);

            if (_countryReposotory.GetAuthorsFromACountry(countryId).Count() > 0)
            {
                ModelState.AddModelError("", $"Country {countryToDelete.Name} " +
                                             "cannot be deleted because it is used by at least one author");
                return StatusCode(409, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_countryReposotory.DeleteCountry(countryToDelete))
            {
                ModelState.AddModelError("", $"Something went wrong deleting {countryToDelete.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }


    }
}
