# Rezeptverwaltung 
```bash
Angular 12 (Frontend)
   ↓
ASP.NET Core Web API (Zwischenschicht)
   ↓
DataContext + JSON (Datenschicht)
```

## 🟦 Backend (Aufgabe)
Dies ist ein **Backend-Prototyp** eine einfache **.NET 8 Bibliothek** zur Verwaltung von Benutzern und Rezepten sowie eine kleine Konsolenanwendung zur Demonstration der Bibliotheksfunktionen. 
Die Anwendung speichert Daten in JSON‑Dateien im Verzeichnis `data`.

####  Projektstruktur
- **`RecipeLibrary`** – .NET‑Klassbibliothek mit allen Domänenklassen (Benutzer, Rezept, Zutat, Kategorie), einem Persistenzlayer (`DataContext`) und Serviceklassen für die Verwaltung von Benutzern, Zutaten, Kategorien, Rezepten und Favoriten.
- **`RecipeConsoleDemo`** – Konsolenanwendung, die die Bibliothek nutzt. Benutzer können sich registrieren und anmelden, Kategorien und Zutaten anlegen, Rezepte erstellen und abfragen sowie fremde Rezepte als Favorit markieren.

### Installation und Ausführung 

#### Voraussetzungen
Die Projekte sind auf .NET 8 ausgelegt. Stellen Sie sicher, dass ein aktuelles .NET 8 SDK auf Ihrem System installiert ist. 
Beim ersten Start wird im Unterordner `data` eine Reihe von JSON‑Dateien (`users.json`, `ingredients.json`, `categories.json`, `recipes.json`) erzeugt, in denen die Objekte persistiert werden. Die Daten bleiben zwischen Programmläufen erhalten.

#### Abhängigkeiten installieren (cd Backend)
```bash
# Wiederherstellen von Abhängigkeiten 
dotnet restore

# Projekt kompilieren
dotnet build

# Demoanwendung starten (cd Backend)
dotnet run --project RecipeConsoleDemo
```

#### demo_input.txt
```bash
1
user1
pass1
1
user2
pass2
1
user3
pass3
2
user1
pass1
1
Dessert
1
Vegetarian
2
Flour
2
Sugar
3
Cake
Flour|200g
Sugar|100g

Mix and bake
Dessert

0
2
user2
pass2
1
Main
2
Oil
2
Salt
3
Soup
Oil|10ml
Salt|5g

Boil and serve
Main

0
2
user3
pass3
1
Snack
2
Chocolate
3
Cookie
Chocolate|50g
Sugar|20g

Bake cookies
Snack

14
user1
15
9
0
```

#### User Interface
```bash
Hauptmenü
1) Benutzer registrieren
2) Anmelden
0) Beenden
```
```bash
Benutzermenü
1) Kategorie anlegen
2) Zutat anlegen
3) Rezept anlegen
4) Eigene Rezepte anzeigen
5) Rezepte nach Kategorie anzeigen
6) Rezepte nach Zutat anzeigen
7) Rezept favorisieren
8) Favorisierung entfernen
9) Eigene Favoriten anzeigen
10) Rezept bearbeiten
11) Rezept löschen
12) Kategorie umbenennen
13) Kategorie löschen
14) Rezepte eines Nutzers anzeigen
15) Alle Zutaten anzeigen
0) Abmelden
```

#### 💡Option
##### Ein neues Eingabeskript, auto_demo_script.txt. Dieses Skript enthält die vollständige Eingabesequenz von der Registrierung zweier Benutzer bis zur Erstellung und Speicherung von Rezepten als Favoriten. Sie können es verwenden, um die Funktionalität der Konsolenanwendung automatisch zu demonstrieren.
Die Anwendung liest alle erforderlichen Eingaben aus der Datei „auto_demo_script.txt“ und gibt den Ausführungsprozess ohne manuelles Eingreifen aus.

```bash
# Automatisches Demonstrationsverfahren:
## Verwende cmd.exe
dotnet run --project RecipeConsoleDemo < auto_demo_script.txt
## Verwende PowerShell
Get-Content auto_demo_script.txt | dotnet run --project RecipeConsoleDemo
```

### Architekturüberblick
Die Bibliothek basiert auf einer einfachen Schichtenarchitektur:

| Schicht | Aufgabe |
| --- | --- |
| **Modelle** | Klassen zur Darstellung von Benutzern (`User`), Rezepten (`Recipe`), Zutaten (`Ingredient`), Kategorien (`Category`) und der Verwendung von Zutaten in einem Rezept (`IngredientUsage`). |
| **Persistenz** | `DataContext` lädt die JSON‑Dateien beim Programmstart und speichert Änderungen. |
| **Services** | Serviceklassen kapseln die Geschäftslogik: `UserService` (Registrierung/Authentifizierung), `IngredientService`, `CategoryService`, `RecipeService` (CRUD für Rezepte samt Validierung) und `FavouriteService` (Verwalten von Favoriten). |
| **Demo** | Das Konsolenprogramm stellt ein Menü bereit, über das ein Anwender die verschiedenen Funktionen nutzen kann. |

### Persistenzmechanismus

Die Persistenz erfolgt mittels `System.Text.Json`. Beim Instanziieren des `DataContext` wird pro Sammlung (Benutzer, Zutaten, Kategorien, Rezepte) die entsprechende JSON‑Datei geladen. Änderungen an den Objekten werden über den Aufruf von `SaveChanges()` gespeichert. Die Dateiablage ist bewusst einfach gehalten, um die Konzentration auf die fachliche Logik zu ermöglichen.

### Eingabebeschränkungen und Geschäftsregeln

Die Bibliothek setzt folgende Regeln durch:

- **Benutzerverwaltung**: Benutzername muss eindeutig sein. Passwörter werden unverschlüsselt gespeichert (nur für Demonstrationszwecke!).
- **Rezeptverwaltung**: Rezeptnamen sind global eindeutig. Ein Rezept muss mindestens eine Zutat, mindestens einen Zubereitungsschritt und mindestens eine Kategorie besitzen.
- **Zutaten** und **Kategorien**: Namen müssen eindeutig sein. Zutaten sind global und unabhängig vom Benutzer. Kategorien können nicht gelöscht werden, wenn sie von einem Rezept verwendet werden.
- **Favoriten**: Benutzer dürfen nur Rezepte anderer Benutzer als Favorit markieren. Favoriten werden als Liste von Rezept‑IDs beim jeweiligen Benutzer gespeichert.


## 🟩 Frontend (In Bearbeitung)
Dies ist ein **Frontend-Prototyp** für eine Rezeptverwaltungsanwendung, entwickelt mit **Angular 12**.  
Das Projekt demonstriert die wichtigsten Funktionen einer modernen Web-Rezeptverwaltung.
- Login & Registrierung
- Benutzerprofilseite mit Rezeptübersicht

### Rezeptverwaltung Funktionen
- Rezepte anzeigen, hinzufügen und durchsuchen
- Detailseite mit Zutaten und Zubereitungsschritten
- Rezepte mit Kategorien und Zutaten verknüpft
- Übersicht aller Rezeptkategorien
- Detailseite pro Kategorie mit zugehörigen Rezepten
💡 Weitere Features
- Folgeliste mit anderen Benutzern

####  Projektstruktur
```
├── src/
│ ├── app/
│ │ ├── components/
│ │ │ ├── home/ → Startseite 
│ │ │ ├── categories/ → Kategorienübersicht
│ │ │ ├── category/ → Detailansicht einer Kategorie
│ │ │ ├── recipe/ → Rezeptdetailseite
│ │ │ ├── add-recipe/ → Formular „Rezept hinzufügen“
│ │ │ ├── login/ → Login-Seite
│ │ │ ├── register/ → Registrierung
│ │ │ ├── profile/ → Benutzerprofil
│ │ │ ├── following/ → Gefolgte Benutzer
│ │ │ ├── ingredients/ → Zutatenliste
│ │ │ └── directions/ → Zubereitungsschritte
│ ├── assets/
│ │ └── images/ → Beispielbilder / Screenshots
│ ├── styles.scss → Globales Design & Farbkonzept
│ ├── index.html → Einstiegspunkt der Anwendung
│ └── main.ts → Angular Bootstrap
├── package.json → Abhängigkeiten & Skripte
└── angular.json, tsconfig.* → Angular CLI Konfiguration

```

### Installation und Ausführung

#### Voraussetzungen
```
- Node.js **>=14**
- Angular CLI **12.x**
```
# Abhängigkeiten installieren (cd Frontend)
```
npm install
```
```
npm start
```
###  Web Prototype
<img width="1133" height="874" alt="image" src="https://github.com/user-attachments/assets/91143e69-9d1d-44bd-9de5-0e9ec1026ae3" />
<img width="1139" height="857" alt="image" src="https://github.com/user-attachments/assets/196ed14b-5442-413d-8c11-f266d148e9bb" />
<img width="1102" height="716" alt="image" src="https://github.com/user-attachments/assets/4aaeeba5-d21b-40ee-a5fd-4d498108542a" />
<img width="1128" height="618" alt="image" src="https://github.com/user-attachments/assets/9469339e-85ff-4e31-b802-5d259260c00e" />
<img width="1164" height="822" alt="image" src="https://github.com/user-attachments/assets/958ea4fc-86ea-4566-ba09-025c7ebb749b" />
<img width="1122" height="871" alt="image" src="https://github.com/user-attachments/assets/7b2faed9-37a6-4485-9f82-d4c03a7a55b9" />
<img width="1149" height="686" alt="image" src="https://github.com/user-attachments/assets/278b7044-d507-44bf-8306-81b36acb4c80" />
<img width="1143" height="733" alt="image" src="https://github.com/user-attachments/assets/7eca2097-afe8-4c59-987c-35930054ca3d" />
<img width="1146" height="702" alt="image" src="https://github.com/user-attachments/assets/4dc92e4f-2ee0-430b-a004-947cb9100ed3" />

###  Weitere Verbindungen
Die App kann leicht mit einem .NET-Backend verbunden werden.

- REST-API-Integration (Authentifizierung, Rezepte, Kategorien)

- Persistente Favoriten & Benutzerprofile

- Filter & Suchfunktion für Rezepte

## 🟧 DevOps (In Bearbeitung)
