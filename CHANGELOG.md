# Changelog

## Editor redesign draft

- Rebuilt the main window around Basic Info, Prompt Text, and Lua Script tabs.
- Replaced the visible effect/JSON grid with prompt text editing.
- Added backward-compatible `card_strings` storage.
- Preserved hidden legacy `card_effects` rows during card saves.
- Added an embedded Lua editor with line numbers and basic syntax highlighting.
- Added Lua file selection, creation, reload, save, and folder opening commands.
- Added separate unsaved-state tracking for database data and Lua files.
- Kept all Lua path behavior compatible with the current Unity runtime.
- Did not modify the MTGB Unity project.
