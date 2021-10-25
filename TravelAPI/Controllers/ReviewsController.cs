using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelAPI.Models;

namespace TravelAPI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ReviewsController : ControllerBase
  {
    private readonly TravelAPIContext _db;

    public ReviewsController(TravelAPIContext db)
    {
      _db = db;
    }

    
// GET: api/destinations/5/reviews
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Review>>> Get(string country)
    {
      var query = _db.Reviews.AsQueryable();

      if (country != null)
      {
        query = query.Where(entry => entry.Country == country);
      }

      return await query.ToListAsync();
    }
    // POST api/Reviews
    [HttpPost]
    public async Task<ActionResult<Review>> Post(Review Review)
    {
      _db.Reviews.Add(Review);
      await _db.SaveChangesAsync();

      return CreatedAtAction("Post", new { id = Review.ReviewId }, Review);
    }

    // GET: api/Reviews/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Review>> GetReview(int id)
    {
        var review = await _db.Reviews.FindAsync(id);

        if (review == null)
        {
            return NotFound();
        }

        return review;
    }

  }
}