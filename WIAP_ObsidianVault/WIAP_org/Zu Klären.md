## Exceptions or nullable struct?
in GameImporter ist `ImportGame()` und `ImportGameData()` und wenn `ImportGameData()` einen Fehler findet gibt es unter anderem drei Lösungen:
- Standardwert festlegen (mMn falsch, da die .json invalide ist)
- Exception werfen, weiter oben fangen und bearbeiten
- returnvalue auf GameData****

### Möglichkeiten:
1. GameData? -> Nullable Value
2. Klassendatenelement und return boolean (-> klappt/klappt nicht)
### 3. throw exception -> try/catch
^beste möglichkeit
4. boolean feld in GameData "Valid" -> "ist valide gameData?"
5. Standardwerte für *alles* festlegen