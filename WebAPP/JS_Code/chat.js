

// Basis-URLs für verschiedene API-Endpunkte
const baseUrl = 'http://localhost:8080/ThorChat/users';
const getChatUrl = 'http://localhost:8080/ThorChat/chats';
const updateChatUrl = 'http://localhost:8080/ThorChat/chats/update';
const createChatUrl = 'http://localhost:8080/ThorChat/chats/create';
// Extrahieren des Kontaktnamens aus der URL
let contactName;
let contact = null;
let currentChat = null;
let currentChatID = null;

// Laden des aktuellen Benutzers aus dem lokalen Speicher
let currentUser = JSON.parse(localStorage.getItem('user'));

// Hinzufügen von Event-Listenern für Schaltflächen
document.getElementById('send-button').addEventListener('click', sendMessage);
document.getElementById('delete-button').addEventListener('click', deleteChat);

async function initializeChat(e) {
    e.preventDefault();
    alert('initializeChat called');
    try {
        contactName = new URLSearchParams(window.location.search).get('contact');
        // URL für den Kontaktaufbau
        const contactUrl = `${baseUrl}/${contactName}`;
        

        // Abrufen der Kontaktdaten
        const contactResponse = await fetch(contactUrl);
      
        if (contactResponse.ok) {
            contact = await contactResponse.json();
            // Überprüfen, ob bereits ein gemeinsamer Chat existiert
            let chatFound = false;
            outerLoop:
            for (const id of currentUser.chat_IDs) {
                for (const id2 of contact.chat_IDs) {
                    if (id === id2) {
                        const chatResponse = await fetch(`${getChatUrl}/${id}`);
                        if (chatResponse.ok) {
                            currentChat = await chatResponse.json();
                            currentChatID = id;
                            currentChat.messages.forEach(message => displayMessage(message.content, message.sender === currentUser.username));
                            chatFound = true;
                            break outerLoop; // Breaks out of both loops
                        }
                    }
                }
            }
            

    
          

            if (!chatFound) {
                alert('No existing chat found, creating new chat');
                // Erstellen eines neuen Chats, falls keiner gefunden wurde
                const response = await fetch(createChatUrl, {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify([currentUser.username, contact.username])
                });

                alert(`createChat response status: ${response.status}`);
                if (response.ok) {
                    currentChat = await response.json();
                    alert('Chat added successfully.');
                } else {
                    alert('Failed to add chat.');
                }
            }
        } else {
            alert('Failed to get contact details.');
        }
    } catch (error) {
        alert(`An error occurred: ${error.message}`);
    }


    setInterval(checkForUpdates, 5000);
}

async function checkForUpdates() {
    const chatResponse = await fetch(`${getChatUrl}/${currentChatID}`);
    if (chatResponse.ok) {
        update = await chatResponse.json();
        if(update.messages.length != currentChat.messages.length){
            const messagesList = document.getElementById('messages-list') = null;
            update.messages.forEach(message => displayMessage(update.content, message.sender === currentUser.username));
        }
    }
}

async function sendMessage() {
    const messageInput = document.getElementById('message-input');
    const messageContent = messageInput.value.trim();

    if (messageContent && currentChat) {
        const message = {
            id: `${Math.floor(Math.random() * (500000 - 100000 + 1)) + 100000}`,
            sender: currentUser.username,
            content: messageContent,
        };

        // Nachricht zur aktuellen Chat-Nachrichtenliste hinzufügen
        currentChat.messages.push(message);
        displayMessage(message.content, true);

        try {
            const response = await fetch(`${updateChatUrl}/${currentChat.id}`, {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(currentChat)
            });

         
            if (!response.ok) {
                alert('Failed to update chat on server.');
            }
        } catch (error) {
            alert(`Error updating chat on server: ${error.message}`);
        }

        // Eingabefeld nach dem Senden der Nachricht leeren
        messageInput.value = '';
    }
}

async function deleteChat() {
    if (currentChat) {
        const deleteUrl = 'http://localhost:8080/ThorChat/chats/delete';
        alert(`deleteUrl: ${deleteUrl}`);

        try {
            const response = await fetch(deleteUrl, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ chatId: currentChat.id, userName: currentUser.username })
            });

            if (response.ok) {
                alert('Succeeded to delete the chat.');
                document.getElementById('messages-list').innerHTML = '';
                currentChat = null;
            } else {
                alert('Failed to delete the chat');
            }
        } catch (error) {
            alert(`An error occurred: ${error.message}`);
        }
    }
}

function displayMessage(content, isOwnMessage) {
    const messagesList = document.getElementById('messages-list');
    const messageElement = document.createElement('div');
    messageElement.className = isOwnMessage ? 'message own-message' : 'message';
    messageElement.textContent = content;
    messagesList.appendChild(messageElement);
}
