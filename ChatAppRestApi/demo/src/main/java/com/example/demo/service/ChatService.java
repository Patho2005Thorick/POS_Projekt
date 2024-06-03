package com.example.demo.service;

import com.example.demo.exception.RescourceNotFoundException;
import com.example.demo.model.Chat;
import com.example.demo.model.Message;
import com.example.demo.model.User;
import com.example.demo.repository.ChatRepository;
import com.example.demo.repository.UserRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
public class ChatService {

    // Automatische Injektion des ChatRepository
    @Autowired
    private ChatRepository chatRepository;

    // Automatische Injektion des UserRepository
    @Autowired
    private UserRepository userRepository;

    /**
     * Methode zum Erstellen eines neuen Chats mit den angegebenen Teilnehmern
     * @param participants Liste der Teilnehmer-Benutzernamen
     * @return Das erstellte Chat-Objekt
     */
    public Chat createChat(List<String> participants) {
        Chat chat = new Chat();
        chat.setParticipants(participants);
        chat = chatRepository.save(chat);
        for (String user : participants) {
            User participant = userRepository.findByUsername(user);
            participant.getChat_IDs().add(chat.getId());
            userRepository.save(participant);
        }
        return chat;
    }

    /**
     * Methode zum Senden einer Nachricht in einen bestimmten Chat
     * @param chatId Die ID des Chats
     * @param message Die zu sendende Nachricht
     * @return Das aktualisierte Chat-Objekt
     */
    public Chat sendMessage(String chatId, Message message) {
        System.out.println(message.getSender());
        System.out.println(message.getContent());
        Chat chat = chatRepository.findById(chatId).orElseThrow(() -> new RescourceNotFoundException("Chat nicht gefunden"));
        chat.getMessages().add(message);
        return chatRepository.save(chat);
    }

    /**
     * Methode zum Aktualisieren eines bestimmten Chats
     * @param chatId Die ID des Chats
     * @param updatedChat Das aktualisierte Chat-Objekt
     * @return Das aktualisierte und gespeicherte Chat-Objekt
     */
    public Chat updateChat(String chatId, Chat updatedChat) {
        System.out.println(updatedChat.getMessages().toString());
        Chat chat = chatRepository.findById(chatId).orElseThrow(() -> new RescourceNotFoundException("Chat nicht gefunden"));
        chat.setMessages(updatedChat.getMessages());
        return chatRepository.save(chat);
    }

    /**
     * Methode zum Abrufen eines bestimmten Chats
     * @param chatId Die ID des Chats
     * @return Das gefundene Chat-Objekt
     * @throws RescourceNotFoundException wenn der Chat nicht gefunden wird
     */
    public Chat getChat(String chatId) {
        return chatRepository.findById(chatId).orElseThrow(() -> new RescourceNotFoundException("Chat nicht gefunden"));
    }

    /**
     * Methode zum Löschen eines bestimmten Chats aus der Chat-Liste eines Benutzers
     * @param data Kommagetrennte Zeichenkette, die Chat-ID und Benutzer-ID enthält
     * @return Das aktualisierte User-Objekt
     * @throws RescourceNotFoundException wenn der Chat oder Benutzer nicht gefunden wird
     */
    public User deleteChat(String data) {
        String[] dataStrings = data.split(",");
        String chatId = dataStrings[0];
        String userId = dataStrings[1];
        Chat chat = chatRepository.findById(chatId).orElseThrow(() -> new RescourceNotFoundException("Chat nicht gefunden"));
        User user = userRepository.findById(userId).orElseThrow(() -> new RescourceNotFoundException("Benutzer nicht gefunden"));
        user.getChat_IDs().remove(chat.getId());
        return userRepository.save(user);
    }
}
