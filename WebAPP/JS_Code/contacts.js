// Basis-URLs für die API-Endpunkte
const baseUrl = 'http://localhost:8080/ThorChat/users/contacts';
const baseUrl2 = 'http://localhost:8080/ThorChat/users';

// Ruft die aktuellen Benutzerinformationen aus dem lokalen Speicher ab und parst sie in ein JavaScript-Objekt
let currentUser = JSON.parse(localStorage.getItem('user')); 
let username = ''; // Variable zum Speichern des eingegebenen Benutzernamens
let currentContacts = currentUser.contacts || []; // Ruft die aktuelle Kontaktliste des Benutzers ab

// Fügt einen Event Listener hinzu, um Änderungen in der AddUserInput-Eingabe zu behandeln
document.getElementById('AddUserInput').addEventListener('input', handleInputChange);

// Fügt einen Event Listener hinzu, um Kontakte zu laden, wenn der DOM-Inhalt vollständig geladen ist
document.addEventListener('DOMContentLoaded', loadContacts);

/**
 * Behandelt das Eingabeänderungsereignis, um die Variable username zu aktualisieren
 * @param {Event} event - Das Eingabeänderungsereignis
 */
function handleInputChange(event) {
    username = event.target.value;
}

/**
 * Fügt einen neuen Kontakt zur Kontaktliste des aktuellen Benutzers hinzu
 * @param {Event} event - Das Formularübermittlungsereignis
 */
async function addContact(event) {
    event.preventDefault(); // Verhindert das standardmäßige Verhalten der Formularübermittlung

    // Überprüft, ob der Benutzername nicht leer ist
    if (username.trim() === '') {
        alert('Bitte geben Sie einen Benutzernamen ein');
        return;
    }

    // Datenobjekt, das den Benutzernamen des aktuellen Benutzers und den neuen Kontaktbenutzernamen enthält
    const data = {
        currentUser: currentUser.username,
        newContact: username
    };
    // Konstruiert die Anforderungs-URL, um die Details des neuen Kontakts abzurufen
    const requestUrl = `${baseUrl2}/${username}`;

    try {
        // Sendet eine GET-Anforderung, um die Details des neuen Kontakts abzurufen
        const response = await fetch(requestUrl);
        if (response.ok) {
            const newContact = await response.json();

            // Überprüft, ob der neue Kontakt nicht bereits in der Kontaktliste des aktuellen Benutzers enthalten ist
            if (!currentContacts.includes(newContact.username)) {
                // Sendet eine PUT-Anforderung, um den neuen Kontakt zur Kontaktliste des aktuellen Benutzers hinzuzufügen
                const putResponse = await fetch(baseUrl, {
                    method: 'PUT',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    // Sendet den Benutzernamen des aktuellen Benutzers und den Benutzernamen des neuen Kontakts als kommaseparierten String
                    body: JSON.stringify(currentUser.username + ","+ newContact.username),
                });

                if (putResponse.ok) {
                    // Aktualisiert die aktuelle Kontaktliste und die Benutzeroberfläche
                    currentContacts.push(newContact.username);
                    updateContactList(newContact.username);
                    alert('Kontakt erfolgreich hinzugefügt.');
                } else {
                    alert('Kontakt konnte nicht hinzugefügt werden.');
                }
            } else {
                alert('Kontakt existiert bereits.');
            }
        } else {
            alert('Kontakt nicht gefunden.');
        }
    } catch (error) {
        alert('Ein Fehler ist aufgetreten: ' + error.message);
    }
}

/**
 * Aktualisiert die Kontaktliste in der Benutzeroberfläche
 * @param {string} contactName - Der Name des neuen Kontakts, der zur Liste hinzugefügt werden soll
 */
function updateContactList(contactName) {
    const contactList = document.getElementById('contact_list_'); // Holt das Kontaktlistenelement
    const newContact = document.createElement('li'); // Erstellt ein neues Listenelement für den Kontakt
    newContact.textContent = contactName; // Setzt den Textinhalt auf den Namen des Kontakts

    // Fügt einen Klick-Event Listener hinzu, um zur Chat-Seite für den ausgewählten Kontakt zu navigieren
    newContact.addEventListener('click', () => {
        window.location.href = `chat.html?contact=${encodeURIComponent(contactName)}`;
    });

    contactList.appendChild(newContact); // Fügt den neuen Kontakt zur Kontaktliste hinzu
}

/**
 * Lädt die aktuellen Kontakte und zeigt sie in der Benutzeroberfläche an
 */
function loadContacts() {
    currentContacts.forEach(contactName => {
        updateContactList(contactName); // Aktualisiert die Kontaktliste für jeden Kontakt
    });
}
