---
applyTo:
  - "*.csproj"
  - "*.props"
  - "Directory.Build.props"
---

Syfte

- Ge riktlinjer till Copilot för att skapa eller modifiera MSBuild-projektfiler i detta repository.

Riktlinjer

- Följ repositoryns `Directory.Build.props` som källa för standardinställningar (TargetFramework, LangVersion, Nullable, ImplicitUsings).
- Använd SDK-stil projektfiler (`<Project Sdk=...>`).
- Håll `TargetFramework` satt till `net10.0` för projekt i detta repo, om inget annat anges.
- Lägg till paketreferenser i `<ItemGroup>` med explicita versionsnummer.
- Använd `ProjectReference` för lokala projekt i lösningen och undvik absoluta sökvägar.
- Lägg till `UserSecretsId` endast för exekverbara host-projekt som behöver det.
- För release builds: sätt `Optimize` och `DebugType` i `Directory.Build.props` eller en `PropertyGroup` med Condition på `Release`.
- Dokumentera ändringar i projektfilen med kommentarer när du lägger till nya mål eller importer.

Konstraint

- Ändra inte `Aspire.AppHost.Sdk` version utan att verifiera kompatibilitet med andra projekt.
- Se till att alla nya analyzers eller package references är kompatibla med .NET 10.
