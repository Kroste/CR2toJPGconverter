# CR2 → JPG Konverter (Windows-EXE & .NET CLI)

Dieses Tool konvertiert Canon-RAW-Dateien (**.cr2**) in **.jpg**. Es kann Unterordner rekursiv durchsuchen und auf Wunsch die Originale nach erfolgreicher Konvertierung löschen. Die Anwendung schreibt Status- und Fehlermeldungen sowohl in die Konsole als auch in eine Logdatei.

---

## Was das Programm macht (Ablauf)

1. **Start & Argumente einlesen**  
   Beim Start werden die übergebenen Kommandozeilenargumente geprüft (Ordner, `-r`, `-d`). Fehlen Pflichtangaben, wird eine Hilfe-/Fehlermeldung ausgegeben.

2. **Logging initialisieren**  
   Die Anwendung protokolliert in **Konsole** und in die Datei **`conversion_log.txt`** im aktuellen Arbeitsverzeichnis.

3. **Ordner verarbeiten**  
   - Es werden alle Dateien mit Muster **`*.cr2`** im angegebenen Ordner nach **`*.jpg`** konvertiert (gleicher Dateiname, nur Endung `.jpg`).  
   - **Rekursiv (`-r`)**: Optional werden alle **Unterordner** in gleicher Weise durchlaufen.  
   - **Löschen (`-d`)**: Optional wird die **Original-CR2** **nur nach erfolgreicher** Konvertierung gelöscht.

4. **Meldungen**  
   - **Erfolg**: „Successfully converted …” (bzw. „… and deleted …”).  
   - **Fehler**: „Error converting …: <Fehlermeldung>”. In diesem Fall bleibt die Originaldatei erhalten.

> **Wichtig:** Das Dateimuster ist exakt `*.cr2`. Auf **case-sensitiven** Dateisystemen (z. B. Linux/macOS) werden `.CR2` etc. so **nicht** gefunden.

---

## Systemvoraussetzungen

- Windows 10/11 (für die EXE-Variante).  
- Alternativ jede Plattform mit installiertem **.NET 8 Runtime/SDK** (für `dotnet`-Aufruf der DLL).
- Ausreichende Lese-/Schreibrechte im Quellordner.

---

## Verwendung

### Syntax
```text
CR2toJPGconverter.exe <directory> [-r] [-d]
# oder (Framework-abhängig):
dotnet CR2toJPGconverter.dll <directory> [-r] [-d]
```

### Parameter
- **`<directory>`** (erforderlich): Ordner mit den zu konvertierenden **.cr2**-Dateien.
- **`-r` / `--recursive`** (optional): Unterordner rekursiv verarbeiten.
- **`-d` / `--delete`** (optional): Original-**.cr2** nach erfolgreicher Konvertierung **löschen**.

### Beispiele
```powershell
# Nur aktueller Ordner
CR2toJPGconverter.exe "D:\Fotos\RAW"

# Rekursiv (Unterordner einbeziehen)
CR2toJPGconverter.exe "D:\Fotos\RAW" -r

# Rekursiv + Originale nach Erfolg löschen
CR2toJPGconverter.exe "D:\Fotos\RAW" -r -d

# Hilfe (über Standardparser)
CR2toJPGconverter.exe --help
```

**Ausgaben/Logs:**  
- Konsole (live Status).  
- Datei **`conversion_log.txt`** im aktuellen Arbeitsverzeichnis.

---

## Build & Bereitstellung

Du hast bereits die Projektdatei und Lösung. Im Folgenden zwei bewährte Wege – **EXE** (self-contained) und **DLL** (framework-abhängig):

### A) Release-EXE (self‑contained, Single File) für Windows x64
Erzeugt eine eigenständige **`CR2toJPGconverter.exe`**, die ohne separate .NET-Installation lauffähig ist.
```powershell
dotnet publish -c Release -r win-x64 --self-contained true ^
  -p:PublishSingleFile=true -p:PublishTrimmed=false ^
  -p:IncludeNativeLibrariesForSelfExtract=true
```
**Pfad zur EXE:**  
`.in\Release
et8.0\win-x64\publish\CR2toJPGconverter.exe`

> Hinweis: `PublishTrimmed=false` ist bei nativen Bibliotheken meist robuster. Bei Bedarf später optimieren.

### B) Framework-abhängig (DLL ausführen)
```powershell
dotnet build -c Release
dotnet .in\Release
et8.0\CR2toJPGconverter.dll "D:\Fotos\RAW" -r
```

---

## Verhalten & Nebenbedingungen

- **Eins-zu-eins-Umwandlung im selben Ordner:**  
  `IMG_0001.cr2` → `IMG_0001.jpg` (im gleichen Ordner).
- **Löschsemantik:**  
  Nur wenn die Konvertierung **erfolgreich** war und `-d` gesetzt ist, wird die **Originaldatei** entfernt.
- **Fehlertoleranz:**  
  Schlägt eine Datei fehl, wird dies geloggt; die Verarbeitung anderer Dateien geht weiter.
- **Case-Sensitivity:**  
  Das Suchmuster ist `*.cr2`. Auf Windows (case-insensitive) ist das unkritisch; auf Linux/macOS ggf. `.CR2` separat behandeln (oder Code anpassen).
- **Leistung:**  
  Verarbeitung erfolgt sequentiell; für große Mengen ggf. in Batches arbeiten.

---

## Troubleshooting

| Symptom | Ursache | Abhilfe |
|---|---|---|
| „The directory cannot be null or empty.” | Pflichtargument fehlt/leer | Pfad korrekt angeben, ggf. in Anführungszeichen. |
| „Error converting …” | Datei defekt, fehlende Codecs, Rechteproblem | Andere Datei testen; Ordnerrechte prüfen; EXE/DLL mit ausreichenden Rechten starten. |
| Keine JPGs erzeugt | Kein Treffer für `*.cr2` | Groß-/Kleinschreibung prüfen; Ordner korrekt? |
| Logdatei fehlt | Arbeitsverzeichnis abweichend | Im Zielordner starten oder Pfade explizit verwenden. |

---

## Sicherheit & Best Practices

- **Backup empfohlen**, bevor `-d` (Löschen) aktiviert wird.
- Erst im **kleinen Testordner** verifizieren, dann auf große Bildmengen anwenden.
- **Logdatei** aufbewahren (Nachvollziehbarkeit von Fehlern).

---