package com.example.demo.service;

import com.example.demo.exception.RescourceNotFoundException;
import com.example.demo.model.User;
import com.example.demo.repository.UserRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
public class UserService {

    // Automatische Injektion des UserRepository
    @Autowired
    private UserRepository userRepository;

    /**
     * Methode zum Abrufen aller Benutzer
     * @return Liste aller Benutzer
     */
    public List<User> getUsers() {
        return userRepository.findAll();
    }

    /**
     * Methode zum Abrufen eines neuen Kontakts basierend auf dem Benutzernamen
     * @param username Benutzername des gesuchten Kontakts
     * @return User-Objekt des gefundenen Kontakts
     * @throws RescourceNotFoundException wenn der Benutzer nicht gefunden wird
     */
    public User getNewContact(String username) {
        List<User> userlist = userRepository.findAll();
        for (User user : userlist) {
            if (user.getUsername().equals(username)) {
                return user;
            }
        }
        throw new RescourceNotFoundException("Der Benutzer mit dem Namen " + username + " existiert nicht");
    }

    /**
     * Methode zum Abrufen eines Benutzers basierend auf Benutzername und Passwort
     * @param username Benutzername des gesuchten Benutzers
     * @param password Passwort des gesuchten Benutzers
     * @return User-Objekt des gefundenen Benutzers
     * @throws RescourceNotFoundException wenn der Benutzer nicht gefunden wird
     */
    public User getUser(String username, String password) {
        List<User> userlist = userRepository.findAll();
        User user = userRepository.findByUsername(username);
        if (user.getPassword().equals(password)) {
            return user;
        } else {
            throw new RescourceNotFoundException("Der Benutzer mit dem Namen " + username + " existiert nicht");
        }
    }

    /**
     * Methode zum Erstellen eines neuen Benutzers
     * @param user Das zu erstellende User-Objekt
     * @return Das erstellte User-Objekt
     */
    public User createUser(User user) {
        return userRepository.save(user);
    }

    /**
     * Methode zum Hinzufügen eines neuen Kontakts zu einem Benutzer
     * @param data Kommagetrennte Zeichenkette, die Benutzernamen und neuen Kontaktnamen enthält
     * @return Das aktualisierte User-Objekt
     */
    public User newContact(String data) {
        System.out.println(data);
        String[] datastrings = data.split(",");
        String username = datastrings[0];
        String newcontactname = datastrings[1];
        username = username.replace("\"", "");
        newcontactname = newcontactname.replace("\"", "");
        System.out.println(username);
        System.out.println(newcontactname);
        User user = userRepository.findByUsername(username);
        User contact = userRepository.findByUsername(newcontactname);

        if (user != null && contact != null && !user.getContacts().contains(newcontactname)) {
            user.getContacts().add(newcontactname);
            System.out.println(user.getContacts().toString());
            userRepository.save(user);
        }
        return user;
    }

    /**
     * Methode zum Hinzufügen eines neuen Chats zwischen Benutzern
     * @param data Kommagetrennte Zeichenkette, die Benutzernamen, Kontaktnamen und Chat-ID enthält
     * @return Das aktualisierte User-Objekt
     */
    public User newChat(String data) {
        System.out.println(data);
        String[] datastrings = data.split(",");
        String username = datastrings[0];
        String contactname = datastrings[1];
        String chat_id = datastrings[2];
        username = username.replace("\"", "");
        contactname = contactname.replace("\"", "");
        chat_id = chat_id.replace("\"", "");

        User user = userRepository.findByUsername(username);
        User contact = userRepository.findByUsername(contactname);
        user.getChat_IDs().add(chat_id);
        contact.getChat_IDs().add(chat_id);

        userRepository.save(user);
        userRepository.save(contact);
        return user;
    }

    /**
     * Methode zum Löschen eines Benutzers basierend auf dem Benutzernamen
     * @param username Der Benutzername des zu löschenden Benutzers
     */
    public void deleteUser(String username) {
        User user = userRepository.findByUsername(username);
        userRepository.delete(user);
    }
}
