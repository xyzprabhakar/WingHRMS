using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projContext;
using projContext.DB;

namespace projAPI.Controllers
{
#if(false)
    [Route("api/[controller]")]
    [ApiController]
    public class apiCountryController : ControllerBase
    {
        private readonly Context _context;

        public apiCountryController(Context context)
        {
            _context = context;
        }

        // GET: api/apiCountry
        [HttpGet]
        //[Authorize(Policy = "2001")]
        public IEnumerable<tbl_country> Gettbl_country()
        {
            return _context.tbl_country;
        }

        // GET: api/apiCountry/5
        [HttpGet("{id}")]
        ////[Authorize]
        //[Authorize(Policy = "2001")]
        public async Task<IActionResult> Gettbl_country([FromRoute] int CountryId)
        {
            try
            {
                if (CountryId > 0)
                {
                    var result = (from a in _context.tbl_country.Where(x => x.country_id == CountryId) select a).ToList();
                    var data = new { data = result };
                    return Ok(data);
                }
                else
                {
                    var result = (from a in _context.tbl_country select a).ToList();
                    var data = new { data = result };
                    return Ok(data);
                }


            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }


        // GET: api/apiCountry/5
        [Route("[action]/{StateId}/{CountryId}")]
        [HttpGet()]
        ////[Authorize]
        //[Authorize(Policy = "2002")]
        public async Task<IActionResult> GetStateList([FromRoute] int StateId, int CountryId)
        {
            try
            {

                if (StateId > 0 && CountryId == 0)
                {
                    var result = (from a in _context.tbl_state.Where(x => x.state_id == StateId) select a).ToList();
                    var data = new { data = result };
                    return Ok(data);
                }
                else if (CountryId > 0)
                {
                    var result = (from a in _context.tbl_state.Where(x => x.country_id == CountryId) select a).ToList();
                    var data = new { data = result };
                    return Ok(data);
                }
                else
                {
                    var result = (from a in _context.tbl_state
                                  join b in _context.tbl_country on a.country_id equals b.country_id
                                  select new
                                  {
                                      stateid = a.state_id,
                                      statename = a.name,
                                      countryname = b.name,
                                      shortname = b.sort_name.ToUpper()
                                  }).ToList();

                    var data = new { data = result };
                    return Ok(data);
                }


            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        // PUT: api/apiCountry/5
        [HttpPost("{id}")]
        ////[Authorize]
        //[Authorize(Policy = "2003")]
        public async Task<IActionResult> Puttbl_country([FromRoute] int id, [FromBody] tbl_country tbl_country)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbl_country.country_id)
            {
                return BadRequest();
            }

            _context.Entry(tbl_country).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tbl_countryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/apiCountry
        [HttpPost]
        ////[Authorize]
        //[Authorize(Policy = "2004")]
        public async Task<IActionResult> Posttbl_country([FromBody] tbl_country tbl_country)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.tbl_country.Add(tbl_country);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Gettbl_country", new { id = tbl_country.country_id }, tbl_country);
        }

        // DELETE: api/apiCountry/5
        [HttpDelete("{id}")]
        ////[Authorize]
        //[Authorize(Policy = "2005")]
        public async Task<IActionResult> Deletetbl_country([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tbl_country = await _context.tbl_country.FindAsync(id);
            if (tbl_country == null)
            {
                return NotFound();
            }

            _context.tbl_country.Remove(tbl_country);
            await _context.SaveChangesAsync();

            return Ok(tbl_country);
        }

        private bool tbl_countryExists(int id)
        {
            return _context.tbl_country.Any(e => e.country_id == id);
        }
    }

#endif
}