﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tprevision.Models.DataManager;
using tprevision.Models.ModelTemplate;
using TPRevision.Models.EntityFramework;

namespace tprevision.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarquesController : ControllerBase
    {
        private readonly MarqueManager marqueManager;

        [ActivatorUtilitiesConstructor]
        public MarquesController(MarqueManager _manager)
        {
            marqueManager = _manager;
        }

        public MarquesController()
        {
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Marque>>> GetMarques()
        {
            var result = await marqueManager.GetAllAsync();
            return result;
        }

        // GET: api/Marques/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Marque>> GetMarqueById(int id)
        {
            var marqueById = await marqueManager.GetByIdAsync(id);
            //var utilisateur = await _context.Utilisateurs.FindAsync(id);
            if (marqueById.Value == null)
            {
                return NotFound();
            }
            
            return marqueById;
        }


        // GET: api/Marques/5
        [HttpGet]
        [Route("[action]/{str}")]
        [ActionName("GetMarqueByString")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Marque>> GetMarqueByString(String? str)
        {
            var marqueByString = await marqueManager.GetByStringAsync(str);
            //var utilisateur = await _context.Utilisateurs.FindAsync(id);
            if (marqueByString == null)
            {
                return NotFound();
            }
            return marqueByString;
        }

        // PUT: api/Marques/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutMarque(int id, Marque marque)
        {
            if (id != marque.Idmarque)
            {
                return BadRequest();
            }
            var marqueToUpdate = await marqueManager.GetByIdAsync(id);
            if (marqueToUpdate == null)
            {
                return NotFound();
            }
            else
            {
                await marqueManager.PutAsync(marqueToUpdate.Value, marque);
                return NoContent();
            }
        }

        [HttpPost]
        public async Task<ActionResult<Marque>> PostMarque(MarqueSansNavigation marque)
        {
            Marque NouvelleMarque = new()
            {
                NomMarque = marque.NomMarque
            };

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await marqueManager.PostAsync(NouvelleMarque);
            return CreatedAtAction("GetMarqueById", new { id = NouvelleMarque.Idmarque }, NouvelleMarque);
        }


        // DELETE: api/Marques/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMarque(int id)
        {
            var marque = await marqueManager.GetByIdAsync(id);
            if (marque.Value == null) 
            {
                return NotFound();
            }
            await marqueManager.DeleteAsync(marque.Value);
            return NoContent();
        }


        /*        private bool MarqueExists(int id)
                {
                    return _context.Marques.Any(e => e.Idmarque == id);
                }*/
    }
}
