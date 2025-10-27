# EPD Console applicatie - Kamiel Ceuppens

## Design keuzes
- **Lagen architectuur**: UI Layer, Business Logic Layer en Data Access Layer. Dit zorgt voor duidelijke seperation of concerns waarbij de business logica is losgekoppeld van de UI en database. Dit zorgt ook voor gemakkelijkere uitbreiding en onderhoud.
- **Repository pattern**: Zorgt voor abstractie van de data access logica via interfaces. 
- **SOLID Principes**: Elke klasse heeft slechts 1 verantwoordelijkheid. Door gebruik van interfaces kan functionaliteit worden uitgebreid zonder bestaande code aan te passen. Klassen zijn afhankelijk van abstracties en niet van concrete implementaties.

