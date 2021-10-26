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
    // get: http://localhost:5000/api/Reviews?country=france&pageNumber=2&pageSize=5
    // get: http://localhost:5000/api/Reviews?country=france&pageNumber=2&pageSize=5

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Review>>> Get(string country, string city, DateTime startDate, DateTime endDate, bool sorted = false, int pageNumber = 1, int pageSize = 10)
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
      //if requested by user, sort by rating,
      if (sorted)
      {
        query = query.OrderByDescending(entry => entry.Rating);
      }
      //otherwise sort by id by default
      else
      {
        query = query.OrderBy(review => review.ReviewId);
      }

      // As a user, I want to input date parameters and retrieve only reviews posted during that timeframe.
      // get: http://localhost:5000/api/Reviews?startDate=2018-01-01 - get everything after
      // get: http://localhost:5000/api/Reviews?endDate=2021-01-01 - get everything before
      // get: http://localhost:5000/api/Reviews?startDate=2018-01-01&endDate=2021-01-01 - get everything between

      // Date filtering?
      if(startDate != DateTime.MinValue)
      {
        // int compareResult = DateTime.Compare(re)
        query = query.Where(review => DateTime.Compare(review.Date, startDate) > 0);
      }
      if(endDate != DateTime.MinValue)
      {
        // int compareResult = DateTime.Compare(re)
        query = query.Where(review => DateTime.Compare(review.Date, endDate) < 0);
      }

      //do pagination stuff after query is set using other parameters
      //divide total by pageSize
      //.Skip the pages you don't need
      //.Take the ones you do need

      //return some error if no records exist for the desired page number.
      int pagesToSkip = pageSize * (pageNumber - 1);
      query = query.Skip(pagesToSkip).Take(pageSize);

      //after we filter everything, throw an error if it's empty
      if(query.ToList().Count == 0)
      {
        return NotFound();
      }

      //how does the user know? they don't - usage needs to be documented - or we need to change the return data type to have a wrapping object with the list as a 
      return await query.ToListAsync();
    }
    

    // POST api/Reviews
    [HttpPost]
    public async Task<ActionResult<Review>> Post([FromBody] Review Review)
    {
      // if (Review)
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
