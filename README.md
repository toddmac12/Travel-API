# TravelAPI

## API Documentation

### Example Query

```
https://localhost:5000/api/reviews/?country=france&city=paris
```

### Endpoints

Base URL: `https://localhost:5000`

#### HTTP Request Structure

```
GET /api/reviews
GET /api/reviews/mostVisited
POST /api/reviews
GET /api/reviews/{id}
PUT /api/reviews/{id}?userName={user-name-of-reviewer}
DELETE /api/reviews/{id}?userName={user-name-of-reviewer}

Note: The `userName` query parameter is being used as pseudo-authentication. If it does not match the username on the review, it will throw a 401 unauthorized error.


4 hours:

- random endpoint

- pagination

- date property/date parameters in our get route

- enable CORS

- versioning 
- JWT - no
