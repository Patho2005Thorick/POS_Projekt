package com.example.demo.controller;

import com.example.demo.model.Chat;
import com.example.demo.model.Message;
import com.example.demo.model.User;
import com.example.demo.service.ChatService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@CrossOrigin(origins = "http://127.0.0.1:5501") // Erlaubt Cross-Origin-Anfragen von der angegebenen URL
@RestController
@RequestMapping("ThorChat/chats") // Basis-URL für alle Endpunkte in diesem Controller
public class ChatController {

    // Automatische Injektion des ChatService
    @Autowired
    private ChatService chatService;

    /**
     * Endpunkt zum Erstellen eines neuen Chats mit den angegebenen Teilnehmern
     * @param participants Liste der Teilnehmer-Benutzernamen
     * @return Das erstellte Chat-Objekt
     */
    @PostMapping("/create")
    public Chat createChat(@RequestBody List<String> participants) {
        return chatService.createChat(participants);
    }

    /**
     * Endpunkt zum Senden einer Nachricht in einen bestimmten Chat
     * @param chatId Die ID des Chats
     * @param message Die zu sendende Nachricht
     * @return Das aktualisierte Chat-Objekt
     */
    @PostMapping("/{chatId}/message")
    public Chat sendMessage(@PathVariable String chatId, @RequestBody Message message)  {
        return chatService.sendMessage(chatId, message);
    }

    /**
     * Endpunkt zum Aktualisieren eines bestimmten Chats
     * @param chatId Die ID des Chats
     * @param chat Das aktualisierte Chat-Objekt
     * @return Das aktualisierte und gespeicherte Chat-Objekt
     */
    @PutMapping("/update/{chatId}")
    public Chat updateChat(@PathVariable String chatId, @RequestBody Chat chat) {
        return chatService.updateChat(chatId, chat);
    }

    /**
     * Endpunkt zum Abrufen eines bestimmten Chats
     * @param chatId Die ID des Chats
     * @return Das gefundene Chat-Objekt
     */
    @GetMapping("/{chatId}")
    public Chat getChat(@PathVariable String chatId) {
        return chatService.getChat(chatId);
    }

    /**
     * Endpunkt zum Löschen eines bestimmten Chats aus der Chat-Liste eines Benutzers
     * @param data Kommagetrennte Zeichenkette, die Chat-ID und Benutzer-ID enthält
     * @return Das aktualisierte User-Objekt
     */
    @PutMapping("/delete")
    public User deleteChat(@RequestBody String data) {
        return chatService.deleteChat(data);
    }
}
