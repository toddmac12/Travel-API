using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;
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


    // GET: localhost:5000/api/reviews?sorted=true
    // get: http://localhost:5000/api/Reviews?sorted=true
    // get: http://localhost:5000/api/Reviews?country=france

    //As a user, I want to look up random cities just for fun.
    // input: ?random=true 
    // output:  what data do we want to return? - 
    // output: return a destination - return all reviews for a random city
    // which route would it fit in? or do we need a new route?

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Review>>> Get(string country, string city, bool sorted = false)
    {


      var query = _db.Reviews.AsQueryable();

      if (country != null)
      {
        query = query.Where(entry => entry.Country == country);
      }
      if (city != null)
      {
        //GET: http://localhost:5000/api/reviews/?city=random  string city = "random"

        //test if the user wants reviews for a random city
        if (city == "random")
        {
          //generate a random city (we already have the reviews from the database in the query variable)  c# has a Random class
          Random randomNumberGenerator = new Random();
          
          List<string> uniqueCities = new List<string>();
          foreach (Review review in query)
          {
            if (!uniqueCities.Contains(review.City))
            {
              uniqueCities.Add(review.City);
            }
          }

          int randomNumber = randomNumberGenerator.Next(uniqueCities.Count);
          city = uniqueCities[randomNumber];

        }
        query = query.Where(entry => entry.City == city);
      }
      if (sorted)
      {
        query = query.OrderByDescending(entry => entry.Rating);
      }


      return await query.ToListAsync();
    }
    

    // POST api/Reviews
    [HttpPost]
    public async Task<ActionResult<Review>> Post([FromBody] Review Review)
    {
      _db.Reviews.Add(Review);
      await _db.SaveChangesAsync();

      return CreatedAtAction("Post", new { id = Review.ReviewId }, Review);
    }

    [HttpGet("mostVisited")]
    public async Task<ActionResult<List<string>>> GetMostVisited()
    {
      //http://localhost:5000/api/Reviews/mostVisited
      // var query = .AsQueryable();

      var queryList = await _db.Reviews.ToListAsync();

      Dictionary<string, int> reviewsPerCity = new Dictionary<string, int>();
      for (int i = 0; i < queryList.Count; i++)
      {
        //if in already ++
        if (reviewsPerCity.ContainsKey(queryList[i].City))
        {
          reviewsPerCity[queryList[i].City]++;
        }
        else
        {
          reviewsPerCity.Add(queryList[i].City, 1);
        }

      }

      //sort
      var sortedDict = reviewsPerCity.OrderByDescending(entry => entry.Value).Take(3);

      List<string> result = new List<string>();
      foreach (var entry in sortedDict)
      {
        result.Add(entry.Key);
      }

      // foreach (var entry in result)
      // {
      //   Console.WriteLine($"City: {entry}");
      // }

      return result;
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
    // DELETE: api/Reviews/5
    // DELETE: http://localhost:5000/api/Reviews/5?userName=lisa

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReview(int id, string userName)
    {
      var reviewToDelete = await _db.Reviews.FirstOrDefaultAsync(entry => entry.ReviewId == id);
      if (reviewToDelete == null)
      {
        return NotFound();
      }

      if (reviewToDelete.User != userName)
      {
        return Unauthorized();
      }

      _db.Reviews.Remove(reviewToDelete);
      await _db.SaveChangesAsync();

      return NoContent();
    }

    // PUT: api/Reviews/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutReview(int id, Review review, string userName)
    {
      if (id != review.ReviewId)
      {
        return BadRequest();
      }
      if (review.User != userName)
      {
        return Unauthorized();
      }
      _db.Entry(review).State = EntityState.Modified;
      await _db.SaveChangesAsync();

      return NoContent();
    }
  }
}
