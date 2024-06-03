//Variablen für User-Input
let username=  ' ';
let password =  ' ';

//Bei dieser Funktion werden die Daten vom User an die passenden Variablen übergeben
function handleInputChange(event) {
const { id, value } = event.target;
switch (id) {
    case 'usernameInput':
        username = value;
        break;
    case 'passwordInput':
        password = value;
        break;
    default:
        break;
}
}

//Hier wird die Input Eingabe behandelt. Bei Eingabe wird diese an eine Funktion übergeben
document.getElementById('usernameInput').addEventListener('input', handleInputChange);
document.getElementById('passwordInput').addEventListener('input', handleInputChange);

// Diese Funktion wird beim Login-Formular-Submit aufgerufen
function login(e) {
// Verhindert das Standardverhalten des Formulars (Seitenneuladung)
e.preventDefault();

// DIe IF-Abfrage überprüft, ob die Eingabefelder für Benutzername und Passwort nicht leer sind
if (username.trim() !== "" && password.trim() !== "") {
    // Setzt die Fehlermeldung zurück, falls vorhanden
    document.getElementById('invalidinput').textContent = "";

    // Erstellt die URL für die API-Anfrage mit Benutzername und Passwort
    let url = `http://localhost:8080/ThorChat/users/${username}/${password}`;
    console.log(url); // Gibt die URL zur Debugging-Zwecken in der Konsole aus

    // Sendet eine GET-Anfrage an die erstellte URL
    $.get(url, (data, status) => {
        console.log(data); // Gibt die erhaltenen Daten zur Debugging-Zwecken in der Konsole aus

        // Überprüft, ob die Daten nicht null sind (Benutzer ist authentifiziert)
        if (data != null) {
            // Speichert die Benutzerdaten im lokalen Speicher des Browsers
            localStorage.setItem('user', JSON.stringify(data));

            // Leitet den Benutzer zur "contacts.html"-Seite weiter
            location.replace("./contacts.html");
        }
    });
}
}
