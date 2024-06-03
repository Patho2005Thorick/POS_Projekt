package com.example.demo.controller;

import com.example.demo.model.User;
import com.example.demo.service.UserService;
import io.swagger.v3.oas.annotations.Operation;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@CrossOrigin(origins = "http://127.0.0.1:5501") // Erlaubt Cross-Origin-Anfragen von der angegebenen URL
@RestController
@RequestMapping("ThorChat/users") // Basis-URL für alle Endpunkte in diesem Controller
public class UserController {

    // Automatische Injektion des UserService
    @Autowired
    private UserService userService;

    /**
     * Endpunkt zum Abrufen aller Benutzer
     * @return Liste aller Benutzer
     */
    @Operation(summary = "GET Operation für alle Benutzer")
    @GetMapping
    public List<User> getUsers() {
        return userService.getUsers();
    }

    /**
     * Endpunkt zum Abrufen eines Benutzers basierend auf Benutzername und Passwort
     * @param name Benutzername des gesuchten Benutzers
     * @param password Passwort des gesuchten Benutzers
     * @return Das gefundene Benutzer-Objekt
     */
    @Operation(summary = "GET Operation für einen einzelnen Benutzer")
    @GetMapping("/{name}/{password}")
    public User getUserByName_And_Password(@PathVariable String name, @PathVariable String password) {
        return userService.getUser(name, password);
    }

    /**
     * Endpunkt zum Abrufen eines Benutzers basierend auf dem Benutzernamen
     * @param name Benutzername des gesuchten Benutzers
     * @return Das gefundene Benutzer-Objekt
     */
    @Operation(summary = "GET Operation für einen einzelnen Benutzer")
    @GetMapping("/{name}")
    public User getUserByName(@PathVariable String name) {
        return userService.getNewContact(name);
    }

    /**
     * Endpunkt zum Erstellen eines neuen Benutzers
     * @param user Das zu erstellende Benutzer-Objekt
     * @return Das erstellte Benutzer-Objekt
     */
    @Operation(summary = "POST Operation zum Erstellen eines Benutzers")
    @PostMapping
    public User addUser(@RequestBody User user) {
        System.out.println("Received user: " + user);
        return userService.createUser(user);
    }

    /**
     * Endpunkt zum Hinzufügen eines neuen Kontakts zu einem Benutzer
     * @param data Kommagetrennte Zeichenkette, die Benutzernamen und neuen Kontaktnamen enthält
     * @return Das aktualisierte Benutzer-Objekt
     */
    @Operation(summary = "PUT Operation zum Hinzufügen eines Kontakts")
    @PutMapping("/contacts")
    public User addContact(@RequestBody String data) {
        return userService.newContact(data);
    }

    /**
     * Endpunkt zum Hinzufügen eines neuen Chats zu einem Benutzer
     * @param data Kommagetrennte Zeichenkette, die Benutzernamen, Kontaktnamen und Chat-ID enthält
     * @return Das aktualisierte Benutzer-Objekt
     */
    @Operation(summary = "PUT Operation zum Hinzufügen eines Chats")
    @PutMapping("/chats")
    public User addChat(@RequestBody String data) {
        return userService.newChat(data);
    }

    /**
     * Endpunkt zum Löschen eines Benutzers basierend auf dem Benutzernamen
     * @param username Der Benutzername des zu löschenden Benutzers
     */
    @Operation(summary = "DELETE Operation zum Löschen eines Benutzers")
    @DeleteMapping("/delete")
    public void deleteUser(@RequestBody String username) {
        userService.deleteUser(username);
    }
}
