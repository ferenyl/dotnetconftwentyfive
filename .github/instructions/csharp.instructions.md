---
applyTo:
  - "*.cs"
---

Syfte

- Ge riktlinjer till Copilot för att generera eller föreslå C#-kod som följer repositoryns standarder.

Riktlinjer

- Följ `.editorconfig` och `Directory.Build.props` i repo-roten (t.ex. `nullable enable`, `LangVersion`, indentering).
- Använd explicita accessibility-modifikatorer för typer och medlemmar (t.ex. `public`, `internal`).
- Föredra `var` när typen är uppenbar från höger sida, annars undvik `var`.
- Följ Microsofts namngivningskonventioner: PascalCase för typer/medlemmar, camelCase för lokala variabler.
- Lägg till XML-dokumentation för public API:er (`/// <summary>`).
- För bakgrundstjänster i detta repo (Worker Service) använd `BackgroundService` när du implementerar långkörande bakgrundsjobb.
- Respektera `async`/`await`-mönster; undvik `async void` utom för event handlers.
- Skriv testbar kod: föredra DI (constructor injection) och små metoder.
- Undvik hårdkodade konfigurationer och hemligheter i koden — använd konfiguration och User Secrets vid behov.
- Håll genererade förslag kompatibla med .NET 10 och C# 14.

Exempel på önskat beteende

- Generera kod som kompilerar med repo:s `Directory.Build.props` inställningar.
- För .NET Worker Service-klasser generera en klass som ärver `BackgroundService` om avsikten är en bakgrundsprocess.

Konstraint

- Introducera inte nya toppnivåändringar som bryter befintliga projektinställningar utan kommentar eller PR-beskrivning.
- Förändringar i offentliga API:er ska vara tydligt dokumenterade i kommentaren för förslaget.
