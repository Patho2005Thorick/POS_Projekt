const currentUser = 'currentUserName'; // Replace with actual current user name
const contactName = 'contactName'; // Replace with actual contact name
const baseUrl = 'http://localhost:8080/ThorChat/chats';

document.getElementById('contact-name').innerText = contactName;

const messagesList = document.getElementById('messages-list');
const messageInput = document.getElementById('message-input');
const sendButton = document.getElementById('send-button');

sendButton.addEventListener('click', sendMessage);
messageInput.addEventListener('keypress', event => {
    if (event.key === 'Enter') {
        sendMessage();
    }
});

async function loadMessages(chatId) {
    try {
        const response = await fetch(`${baseUrl}/${chatId}/messages`);
        if (response.ok) {
            const messages = await response.json();
            displayMessages(messages);
        } else {
            alert('Failed to load messages.');
        }
    } catch (error) {
        alert('An error occurred: ' + error.message);
    }
}

async function sendMessage() {
    const content = messageInput.value.trim();
    if (content === '') return;

    const message = {
        sender: currentUser,
        content: content
    };

    try {
        const response = await fetch(`${baseUrl}/${chatId}/message`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(message)
        });

        if (response.ok) {
            const newMessage = await response.json();
            addMessage(newMessage, 'me');
            messageInput.value = '';
        } else {
            alert('Failed to send message.');
        }
    } catch (error) {
        alert('An error occurred: ' + error.message);
    }
}

function displayMessages(messages) {
    messagesList.innerHTML = '';
    messages.forEach(message => {
        const messageClass = message.sender === currentUser ? 'me' : 'other';
        addMessage(message, messageClass);
    });
}

function addMessage(message, messageClass) {
    const messageElement = document.createElement('div');
    messageElement.classList.add('message', messageClass);
    const date = new Date(message.timestamp);
    const formattedDate = `${date.getDate()}-${date.getHours()}-${date.getMinutes()}`;
    messageElement.innerHTML = `<p>${message.sender}: ${message.content}</p><span>${formattedDate}</span>`;
    messagesList.appendChild(messageElement);
    messagesList.scrollTop = messagesList.scrollHeight;
}

let chatId = null;
async function initializeChat() {
    try {
        const response = await fetch(`${baseUrl}/create`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify([currentUser, contactName])
        });

        if (response.ok) {
            const chat = await response.json();
            chatId = chat.id;
            loadMessages(chatId);
        } else {
            alert('Failed to initialize chat.');
        }
    } catch (error) {
        alert('An error occurred: ' + error.message);
    }
}

document.addEventListener('DOMContentLoaded', initializeChat);
