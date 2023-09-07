﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShorderLink.Data;
using ShorderLink.Models;
using ShorderLink.Services;

namespace ShorderLink.Controllers
{
    public class LinksController : Controller
    {
        private readonly ShorderLinkContext _context;
        private const string SERVER_URL = "https://localhost:7288/";

        public LinksController(ShorderLinkContext context)
        {
            _context = context;
        }        

        /// <summary>
        /// Method for any user to get all links stored in database
        /// </summary>
        /// <returns>All links in JSON</returns>
        [HttpGet]
        [Route("links/GetAllLinks")]
        public async Task<IActionResult> GetAllLinks()
        {
            List<Link> links = await _context.Link.ToListAsync();
            return new JsonResult(links);
        }

        /// <summary>
        /// Server method to work with short Links generated by aplication
        /// When user use out short URL - server redirect user to original URL stored in database
        /// </summary>
        /// <param name="shortLink">16 characters hash wich server use to create short URL</param>
        /// <returns>Redirect to Original URL pagr</returns>
        [HttpGet("{shortLink}")]
        // GET: /{shortLink}
        public  ActionResult OpenOriginalURl(string? shortLink)
        {
            string fullShortLink = SERVER_URL + shortLink;
            Link link = _context.Link.FirstOrDefault(m => m.ShortLink == fullShortLink);
            if(link == null)
            {
                return BadRequest("Invalid Link");
            }
            link.ViewsCout++;
            _context.Update(link);
            _context.SaveChanges();
            return Redirect(link.OriginalLink);
        }

        /// <summary>
        /// Show user all details about Link if Link with Id exists
        /// Only Authorized users can see this page
        /// </summary>
        /// <param name="id">Link Id</param>
        /// <returns>Razor View with all information about Link</returns>
        [Authorize]
        // GET: Links/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Link == null)
            {
                return NotFound();
            }

            var link = await _context.Link
                .FirstOrDefaultAsync(m => m.Id == id);
            if (link == null)
            {
                return NotFound();
            }

            return View(link);
        }

        public class GetLink
        {
            public string OriginalLink { get; set; }
        }

        /// <summary>
        /// Creates new short link from original URL
        /// If link already exist return error
        /// Only Authorized users can use this method
        /// </summary>
        /// <param name="Link">URL needed to be shorted</param>
        /// <returns>200 with Link information</returns>
        /// <returns>400 Short link already exist</returns>
        // POST: Links/Create
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] GetLink Link)
        {
            if ((_context.Link?.Any(e => e.OriginalLink == Link.OriginalLink)).GetValueOrDefault())
            {
                return BadRequest("Short link already exist");
            }

            Link link = new Link() { 
                OriginalLink = Link.OriginalLink,
                CreatorId = User.FindFirst("userId").Value,
                CreationTime = DateTime.Now,
                ShortLink = SERVER_URL+URLShorter.HashString(Link.OriginalLink)
            };            
            link.Id = Guid.NewGuid();
            _context.Add(link);
            await _context.SaveChangesAsync();
            
            return Ok(link);
        }

        /// <summary>
        /// Delete links from data base
        /// Only admins can delete any link
        /// Only authorized user can delete his link
        /// </summary>
        /// <param name="id">Link ID</param>
        /// <returns>200 if link deleted</returns>
        /// <returns>400 if link doesn't deleted</returns>
        // POST: Links/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Link == null)
            {
                return Problem("Entity set 'ShorderLinkContext.Link'  is null.");
            }
            var link = await _context.Link.FindAsync(id);
            if (link != null)
            {
                if( User.FindFirst("userRole").Value == "admin" || User.FindFirst("userId").Value == link.CreatorId)
                {
                    _context.Link.Remove(link);          
                    await _context.SaveChangesAsync();
                    return Ok();
                }
            }
            return Problem("No Access");
        }
    }
}
