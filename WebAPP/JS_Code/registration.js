// Definiert eine User-Klasse mit Konstruktor und Methode zum Abrufen des Benutzernamens
class User {
  constructor(username, password, chatIDs = [], contacts = []) {
      this.username = username; // Benutzername des Benutzers
      this.password = password; // Passwort des Benutzers
      this.chatIDs = chatIDs; // Array der Chat-IDs des Benutzers
      this.contacts = contacts; // Array der Kontakte des Benutzers
  }

  // Methode zum Abrufen des Benutzernamens
  getUsername() {
      return this.username;
  }
}

// Initialisierung der Variablen für Benutzername und Passwort
let username = ' ';
let password = ' ';

/**
 * Behandelt das Eingabeänderungsereignis, um die Variablen username und password zu aktualisieren
 * @param {Event} event - Das Eingabeänderungsereignis
 */
function handleInputChange(event) {
  const { id, value } = event.target;
  switch (id) {
      case 'usernameInput':
          username = value;
          break;
      case 'passwordInput':
          password = value;
          break;
  }
}

// Fügt Event Listener hinzu, um Änderungen an den Eingabefeldern für Benutzername und Passwort zu behandeln
document.getElementById('usernameInput').addEventListener('input', handleInputChange);
document.getElementById('passwordInput').addEventListener('input', handleInputChange);

/**
 * Funktion zum Hinzufügen eines neuen Benutzers
 */
async function addUser() {
  // Überprüft, ob Benutzername und Passwort nicht leer sind
  if (username != " " && password != " ") {
      document.getElementById('invalidinput').textContent = " ";
      let url = "http://localhost:8080/ThorChat/users"; 

      // Erstellt ein neues User-Objekt mit den eingegebenen Werten
      const user = new User(username, password, null, null);
      
      // Sendet eine POST-Anfrage, um den neuen Benutzer hinzuzufügen
      let response = fetch(url, {
          method: "POST",
          headers: {'Content-Type': 'application/json'},
          body: JSON.stringify(user),
          mode: "cors"
      });
      
      // Überprüft, ob die Antwort nicht null ist
      if (response != null) {
          alert("Benutzer erfolgreich hinzugefügt");
      }
  } else {
      // Zeigt eine Fehlermeldung an, wenn die Eingaben ungültig sind
      document.getElementById('invalidinput').textContent = "Ungültige Eingabe";
  }
}
