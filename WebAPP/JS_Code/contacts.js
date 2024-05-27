const baseUrl = 'http://localhost:8080/ThorChat/users/contacts';
const baseUrl2 = 'http://localhost:8080/ThorChat/users';
let currentUser = JSON.parse(localStorage.getItem('user')); // Assuming currentUser is an object
let username = '';
let currentContacts = currentUser.contactList || []; // Initialize contacts list

document.getElementById('AddUserInput').addEventListener('input', handleInputChange);
document.addEventListener('DOMContentLoaded', loadContacts);

function handleInputChange(event) {
    username = event.target.value;
}

async function addContact(event) {
    event.preventDefault();

    if (username.trim() === '') {
        alert('Please enter a username');
        return;
    }

    const data = {
        currentUser: currentUser.username,
        newContact: username
    };
    const requestUrl = `${baseUrl2}/${username}`;

    try {
        const response = await fetch(requestUrl);
        if (response.ok) {
            const newContact = await response.json();

            if (!currentContacts.includes(newContact.username)) {
                const putResponse = await fetch(baseUrl, {
                    method: 'PUT',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify(data),
                });

                if (putResponse.ok) {
                    currentContacts.push(newContact.username);
                    updateContactList(newContact.username);
                    alert('Contact added successfully.');
                } else {
                    alert('Failed to add contact.');
                }
            } else {
                alert('Contact already exists.');
            }
        } else {
            alert('Contact not found.');
        }
    } catch (error) {
        alert('An error occurred: ' + error.message);
    }
}

function updateContactList(contactName) {
    const contactList = document.getElementById('contact_list_');
    const newContact = document.createElement('li');
    newContact.textContent = contactName;
    contactList.appendChild(newContact);
}

function loadContacts() {
    currentContacts.forEach(contactName => {
        updateContactList(contactName);
    });
}
