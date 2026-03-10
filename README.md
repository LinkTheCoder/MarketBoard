# MarketBoard - Announcement System for Subscribers & Businesses

A full-stack web application built with .NET 8 and Entity Framework Core that allows subscribers and businesses to create and publish announcements. The system features automatic fetching of subscriber contact information and a web interface for managing announcements.

## Features

### Core Functionality
- Automatic retrieval of subscriber contact information from the API
- Advertisement categorization
- Publishing and status management
- Web interface for creating and viewing advertisements (in English)

## Technical Implementation

### Backend (.NET 8)
- **Models**: `Subscriber`, `Advertisement` with associated enums
- **DTOs**: `SubscriberInfoDto`, `CreateSubscriberDto`, `CreateAdvertisementDto`, `AdvertisementDto`
- **Services**: `SubscriberService`, `AdvertisementService`
- **Controllers**: `SubscribersController`, `AdvertisementsController`
- **Database**: Entity Framework Core with SQL Server
- **API Documentation**: Swagger/OpenAPI

### Frontend (HTML/JavaScript)
- **Main Page** (`/`): Form for creating advertisements (English interface)
- **Ads Page** (`/ads.html`): Display published advertisements with filtering (English interface)
- **Real-time Integration**: JavaScript consuming the actual API on port 7043

## Advertisement System Workflow

1. **User Type Selection**: User chooses between subscriber or business
2. **Subscribers**:
   - Enter subscription number
   - System automatically fetches contact information from Subscribers API
   - User can edit the information if something is incorrect
   - Advertisement price: 0 kr (free)
3. **Businesses**:
   - Fill in company information manually
   - Enter all contact details themselves
   - Advertisement price: 40 kr
4. **Advertisement Details**: All users fill in title, content, category, item price, and publication date
5. **Submission**: Advertisement is saved to the database and published automatically

## API Endpoints

### Subscribers
- `GET /api/subscribers` - Get all active subscribers
- `GET /api/subscribers/by-subscription/{subscriptionNumber}` - Get subscriber by subscription number
- `GET /api/subscribers/{id}` - Get subscriber by ID
- `POST /api/subscribers` - Create new subscriber (uses CreateSubscriberDto)
- `PUT /api/subscribers/{id}` - Update subscriber
- `DELETE /api/subscribers/{id}` - Delete subscriber

### Advertisements
- `GET /api/advertisements` - Get all advertisements
- `GET /api/advertisements/published` - Get published advertisements
- `GET /api/advertisements/{id}` - Get specific advertisement
- `GET /api/advertisements/subscriber/{subscriptionNumber}` - Get advertisements for subscriber
- `POST /api/advertisements` - Create new advertisement
- `PATCH /api/advertisements/{id}/status` - Update advertisement status
- `DELETE /api/advertisements/{id}` - Delete advertisement
- `GET /api/advertisements/calculate-price/{advertiserType}` - Calculate advertisement price

## Configuration

### appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=SubscribersDb;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

### Database
The system uses Entity Framework Code First and automatically creates the database on first run with test data for subscribers.

## Test Data

The system comes with preconfigured test subscribers:
- **SUB001**: Anna Andersson, Stockholm
- **SUB002**: Erik Eriksson, Göteborg  
- **SUB003**: Maria Johansson, Malmö

## Running the Application

1. Clone the project
2. Open in Visual Studio or VS Code
3. Run `dotnet run` or start from Visual Studio
4. Navigate to `https://localhost:7043` for the web interface (English interface)
5. Navigate to `https://localhost:7043/swagger` for API documentation
6. Navigate to `https://localhost:7043/ads.html` to view published advertisements (English interface)

## CORS Configuration

The API is configured with CORS to allow calls from the web interface and external systems.
