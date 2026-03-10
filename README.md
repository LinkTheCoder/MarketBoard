# Subscribers Web API med Annonssystem

Detta projekt innehåller ett Web API för hantering av tidningens prenumeranter samt ett integrerat annonssystem som möjliggör för både prenumeranter och företag att lägga in annonser.

## Funktioner

### Prenumerationssystem
- CRUD-operationer för prenumeranter
- Validering av prenumerationsnummer
- Olika prenumerationstyper (Månadsprenumeration, Kvartals-, Halvårs- och Årsprenumeration)
- RESTful API med Swagger-dokumentation

### Annonssystem
- **Prenumeranter**: Kan lägga in annonser gratis genom att ange sitt prenumerationsnummer
- **Företag**: Kan lägga in annonser för 40 kr genom att fylla i företagsinformation
- Automatisk hämtning av kontaktuppgifter för prenumeranter från API:et
- Kategorisering av annonser
- Publicering och statushantering
- Webb-gränssnitt för att lägga in och visa annonser (på engelska)

## Teknisk Implementation

### Backend (.NET 8)
- **Models**: `Subscriber`, `Advertisement` med tillhörande enums
- **DTOs**: `SubscriberInfoDto`, `CreateSubscriberDto`, `CreateAdvertisementDto`, `AdvertisementDto`
- **Services**: `SubscriberService`, `AdvertisementService`
- **Controllers**: `SubscribersController`, `AdvertisementsController`
- **Database**: Entity Framework Core med SQL Server
- **API-dokumentation**: Swagger/OpenAPI

### Frontend (HTML/JavaScript)
- **Huvudsida** (`/`): Formulär för att lägga in annonser (English interface)
- **Annonssida** (`/ads.html`): Visa publicerade annonser med filtrering (English interface)
- **Real-time integration**: JavaScript som konsumerar det riktiga API:et på port 7043

## Annonssystemets Arbetsflöde

1. **Val av användartyp**: Användaren väljer mellan prenumerant eller företag
2. **Prenumeranter**:
   - Anger prenumerationsnummer
   - Systemet hämtar automatiskt kontaktuppgifter från Subscribers API
   - Användaren kan redigera uppgifterna om något är felaktigt
   - Annonspris: 0 kr (gratis)
3. **Företag**:
   - Fyller i företagsinformation manuellt
   - Anger alla kontaktuppgifter själva
   - Annonspris: 40 kr
4. **Annonsdetaljer**: Alla användare fyller i titel, innehåll, kategori, varans pris och publiceringsdatum
5. **Skickning**: Annonsen sparas i databasen och publiceras automatiskt

## API-endpoints

### Prenumeranter
- `GET /api/subscribers` - Hämta alla aktiva prenumeranter
- `GET /api/subscribers/by-subscription/{subscriptionNumber}` - Hämta prenumerant via prenumerationsnummer
- `GET /api/subscribers/{id}` - Hämta prenumerant via ID
- `POST /api/subscribers` - Skapa ny prenumerant (använder CreateSubscriberDto)
- `PUT /api/subscribers/{id}` - Uppdatera prenumerant
- `DELETE /api/subscribers/{id}` - Ta bort prenumerant

### Annonser
- `GET /api/advertisements` - Hämta alla annonser
- `GET /api/advertisements/published` - Hämta publicerade annonser
- `GET /api/advertisements/{id}` - Hämta specifik annons
- `GET /api/advertisements/subscriber/{subscriptionNumber}` - Hämta annonser för prenumerant
- `POST /api/advertisements` - Skapa ny annons
- `PATCH /api/advertisements/{id}/status` - Uppdatera annonsstatus
- `DELETE /api/advertisements/{id}` - Ta bort annons
- `GET /api/advertisements/calculate-price/{advertiserType}` - Beräkna annonspris

## Konfiguration

### appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=SubscribersDb;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

### Databas
Systemet använder Entity Framework Code First och skapar automatiskt databasen vid första körningen med testdata för prenumeranter.

## Testdata

Systemet kommer med förkonfigurerade testprenumeranter:
- **SUB001**: Anna Andersson, Stockholm
- **SUB002**: Erik Eriksson, Göteborg  
- **SUB003**: Maria Johansson, Malmö

## Körning

1. Klona projektet
2. Öppna i Visual Studio eller VS Code
3. Kör `dotnet run` eller starta från Visual Studio
4. Navigera till `https://localhost:7043` för webbgränssnittet (English interface)
5. Navigera till `https://localhost:7043/swagger` för API-dokumentationen
6. Navigera till `https://localhost:7043/ads.html` för att se publicerade annonser (English interface)

## CORS-konfiguration

API:et är konfigurerat med CORS för att tillåta anrop från webbgränssnittet och externa system.

## Senaste Ändringar

- ? Fixat Entity Framework identity column-problem
- ? Lagt till CreateSubscriberDto för säker skapelse av prenumeranter
- ? Korrigerat API URL från port 7200 till 7043 i frontend
- ? Översatt webb-gränssnittet från svenska till engelska
- ? Förbättrat felhantering och användarfeedback