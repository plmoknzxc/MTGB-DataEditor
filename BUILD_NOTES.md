# Build notes

## Completed checks

- All C# source files were parsed with a C# grammar; no syntax error nodes were found.
- The database schema and editor migration both define `card_strings` consistently.
- The editor continues to preserve hidden legacy `card_effects` rows when saving a card.
- No file in the MTGB Unity project was modified.

## Still requires a Windows check

This workspace does not provide the .NET 8 Windows Desktop SDK or a Windows GUI environment, so the WinForms application was not compiled or launched here.

Run the following on Windows from the project directory:

```powershell
dotnet build MTGB.CardDatabaseEditor.csproj
dotnet run --project MTGB.CardDatabaseEditor.csproj -- --self-test
```

After that, open a copy of an existing `.mtgbdb` file and verify the interface and Lua file workflow before using the editor on the only copy of a production database.
