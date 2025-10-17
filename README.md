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
# Abhängigkeiten installieren
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
