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