# Rezeptverwaltung 


## Backend


### Installation und Ausführung


### Architekturüberblick


## Frontend
Dies ist ein **Frontend-Prototyp** für eine Rezeptverwaltungsanwendung, entwickelt mit **Angular 12**.  
Das Projekt demonstriert die wichtigsten Funktionen einer modernen Web-App für Benutzer-, Kategorien- und Rezeptverwaltung.


- Login & Registrierung (Demo-Formulare)
- Benutzerprofilseite mit Rezeptübersicht

### Rezeptverwaltung Funktionen
- Rezepte anzeigen, hinzufügen und durchsuchen
- Detailseite mit Zutaten und Zubereitungsschritten
- Rezepte mit Kategorien und Zutaten verknüpft

#### 🗂️ Kategorien
- Übersicht aller Rezeptkategorien
- Detailseite pro Kategorie mit zugehörigen Rezepten

#### 💡 Weitere Features
- Folgeliste mit anderen Benutzern

####  Projektstruktur
```
├── src/
│ ├── app/
│ │ ├── components/
│ │ │ ├── home/ → Startseite (Banner + Empfehlungen)
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
```
### Voraussetzungen
- Node.js **>=14**
- Angular CLI **12.x**
```
```
### Installation
# Abhängigkeiten installieren
```
npm install
npm start
```
###  Weitere Verbindungen
Die App kann leicht mit einem .NET-Backend verbunden werden.
Empfohlene Erweiterungen:

REST-API-Integration (Authentifizierung, Rezepte, Kategorien)

Persistente Favoriten & Benutzerprofile

Filter & Suchfunktion für Rezepte