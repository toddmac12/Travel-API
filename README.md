# TravelAPI

This is an API for reviews of travel destinations around the world.

## API Documentation

### Example Query

```https://localhost:5000/api/reviews/?country=france&city=paris
```

### Endpoints

Base URL: `https://localhost:5000`

### HTTP Request Structure

```GET /api/reviews
GET /api/reviews/mostVisited
POST /api/reviews
GET /api/reviews/{id}
PUT /api/reviews/{id}?userName={user-name-of-reviewer}
DELETE /api/reviews/{id}?userName={user-name-of-reviewer}
```

### Details for each endpoint

### GET /api/reviews

A user can get reviews about travel destinations.

### Query Parameters

| Parameter | Type | Default | Description | Example Query |
| :---: | :---: | :---: | :---: | --- |
| country | string | none | Get reviews for the specified country. | api/reviews/?country=france |
| city | string | none | Get reviews for the specified city. | api/reviews/?city=paris |
| city=random | string | none | Get reviews for a random city. | api/reviews/?city=random |
| sorted | bool | false | Sorts reviews by rating (highest to lowest) | api/reviews/?sorted=true |
| pageNumber | int | 1 | Gets the specified page of results | api/reviews/?pageNumber=2 |
| pageSize | int | 10 | Gets the specified number of results | api/reviews/?pageSize=5 |
| startDate | DateTime | none | Gets reviews after the specified date | api/reviews/?startDate=2002-01-01 |
| endDate | DateTime | none | Gets reviews before the specified date | api/reviews/?endDate=2002-01-01 |

### GET /api/reviews/mostVisited

A user can get the top 3 most visited cities as strings (no reviews are returned).
Example query: `http://localhost:5000/api/Reviews/mostVisited`
Example returned JSON:

```[
  "Paris",
  "Brussels",
  "New York"
]
```

### POST /api/reviews

A user can add a review.
Example query: `http:localhost:5000/api/reviews/`
Example returned JSON: No response.

### GET /api/reviews/{id}

A user can get one review by ID.
Example query: `http:localhost:5000/api/reviews/5`
Example returned JSON:

```{
  reviewId:5,
  user: "lisa",
  country: "france",
  city: "paris",
  rating: 7
}
```

### PUT /api/reviews/{id}?userName={user-name-of-reviewer}

A user can update details about a review they created.
Example query: `http://localhost:5000/api/Reviews/5?userName=lisa`
Example returned JSON:

```{
  reviewId:5,
  user: "lisa",
  country: "france",
  city: "paris",
  rating: 7
}
```

Note: The `userName` query parameter is being used as pseudo-authentication. If it does not match the username on the review, it will throw a 401 unauthorized error.

### DELETE /api/reviews/{id}?userName={user-name-of-reviewer}

A user can delete a review they created.
Example query: `http:localhost:5000/api/reviews/5?UserName=lisa`
Example returned JSON:

```{
  reviewId:5,
  user: "lisa",
  country: "france",
  city: "paris",
  rating: 7
}
```

Note: The `userName` query parameter is being used as pseudo-authentication. If it does not match the username on the review, it will throw a 401 unauthorized error.

4 hours:

- random endpoint

- pagination

- date property/date parameters in our get route

- enable CORS

- versioning
- JWT - no
