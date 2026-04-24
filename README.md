# ryby

Szkielet aplikacji `C#` do obslugi wyjazdu wedkarskiego.

## Architektura

- `FishingTrip.App` - warstwa `UI` w `Blazor Web App`
- `FishingTrip.Application` - przypadki uzycia i kontrakty
- `FishingTrip.Domain` - encje i reguly domenowe
- `FishingTrip.Infrastructure` - implementacje repozytoriow i composition root

## Uruchomienie

```powershell
dotnet build .\FishingTripApp.sln
dotnet run --project .\FishingTrip.App\FishingTrip.App.csproj
```

Po uruchomieniu otworz przegladarke pod adresem pokazanym w konsoli, zwykle `https://localhost:xxxx`.

Na ten moment infrastruktura uzywa danych w pamieci. Kolejny naturalny krok to podmiana repozytoriow na `SQLite`.
