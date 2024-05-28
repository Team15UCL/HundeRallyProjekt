# Team 15 - HundeRally Projekt

Formålet med projektet var at skabe en portal for hunderally udøvere til at lave, dele og samarbejde om kreation af rally baner.
Projektet består primært af 3 dele:
  - En Asp.Net Core web app til styring af brugerlogin: **HundeRallyWebApp**
  - En javascript-fil der gør brug af Konva.js biblioteket til selve banen: **RallyTrack.js**
>kan findes i *wwwroot/js/* folderen i web app'en
  - En Asp.Net Core web API til formidling af banedata til og fra en MongoDB database: **RallyAPI**

  - Derudover gøres der brug af en SQL-Server database til brugere og en MongoDB database til baner og øvelser.

## Arkitektur

Overordnet er projektet et distribueret system, da web app'en og api'en kan ligge på forskellige computere og forbindes via netværk og der bruges en client-server arkitektur.

De er begge bygget op med en MVC struktur

## Features
Systemet kræver adgang til ucls netværk, evt. via VPN for at virke

### Brugeradministration
![image](https://github.com/Team15UCL/HundeRallyProjekt/assets/111608008/524add82-4b36-4ee3-954c-99c4ce083d09)

Web appen indeholder bruger logins, der består af 4 forskellige brugerniveau'er: *Admin, Handler, Judge og Instructor*
> Der er opsat 4 testbrugere i systemet:
> - Alle bruger har passwordet: Test1234
> - Admin: admin@admin.com
> - Instructor: instr@instr.com
> - Judge: judge@judge.com
> - Handler: doglover@handler.com

Brugerne ligger i en SQL-Server database.

### Bane-Designer
![image](https://github.com/Team15UCL/HundeRallyProjekt/assets/111608008/9c5304dc-f0b6-4703-8567-2dd176dbf7a4)

- Logikken for banedesigneren ligger i RallyTrack.js filen, som står for at rendere objekter på et html canvas.
- Man indsætter øvelser ved at drag & drop fra øvelsesmenuen til højre.
- Herefter kan man vende og dreje øvelser efter behov.
- Programmet holder selv styr på rækkefølgen af øvelserne og laver selv ruten som hunden skal følge.

Banerne gemmes og hentes gennem kald til vores RallyAPI. API'en kommunikerer så dataen videre til vores MongoDB database. Kommunkation med API sker i JSON-format.

![image](https://github.com/Team15UCL/HundeRallyProjekt/assets/111608008/bb45ba58-f09e-4c1c-a7d0-589caae329b8)

Der bruges Json Web Tokens til at authenticate brugerne til API'en

![image](https://github.com/Team15UCL/HundeRallyProjekt/assets/111608008/6a92b91f-ba8a-420d-8eb3-d4547fba84e8)


### Fælles Chat og Banesamarbejde
Banedesigner siden indeholder også en fælles chat til kommunikation mellem brugere. Chatten fungerer vha SignalR, som bruger websockets til at sende beskeder mellem brugerne

![image](https://github.com/Team15UCL/HundeRallyProjekt/assets/111608008/21902c66-3747-4da9-a5e9-809b40bb7101)

Derudover kan man vha. SignalR samarbejde om at lave en bane.
Pt. fungerer det sådan at hvis 2 brugere er logget på banedesigneren opdateres banen live for den ene bruger når den anden ændrer på banen.

![Optagelse2024-05-27103642-ezgif com-video-to-gif-converter](https://github.com/Team15UCL/HundeRallyProjekt/assets/111608008/2ac0cc72-bc24-474e-bfc3-72c4ed512106)


