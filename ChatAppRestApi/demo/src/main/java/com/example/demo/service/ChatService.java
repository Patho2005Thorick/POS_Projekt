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

    @Autowired
    private ChatRepository chatRepository;

    @Autowired
    private UserRepository userRepository;

    public Chat createChat(List<String> participants) {
        Chat chat = new Chat();
        chat.setParticipants(participants);
        chat = chatRepository.save(chat);
        for (String user: participants) {
            User participant = userRepository.findByUsername(user);
            participant.getChat_IDs().add(chat.getId());
            userRepository.save(participant);
        }
        return chat;
    }

    public Chat sendMessage(String chatId, Message message) {
        System.out.println(message.getSender());
        System.out.println(message.getContent());
        Chat chat = chatRepository.findById(chatId).orElseThrow(() -> new RescourceNotFoundException("Chat not found"));
        chat.getMessages().add(message);
        return chatRepository.save(chat);
    }

    public Chat updateChat(String chatId, Chat updatedChat) {
        System.out.println(updatedChat.getMessages().toString());
        Chat chat = chatRepository.findById(chatId).orElseThrow(() -> new RescourceNotFoundException("Chat not found"));
        chat.setMessages(updatedChat.getMessages());
        return chatRepository.save(chat);
    }

    public Chat getChat(String chatId) {
        return chatRepository.findById(chatId).orElseThrow(() -> new RescourceNotFoundException("Chat not found"));
    }

    public User deleteChat(String data){
        String [] dataStrings = data.split(",");
        String chatId = dataStrings[0];
        String userId = dataStrings[1];
        Chat chat = chatRepository.findById(chatId).orElseThrow(() -> new RescourceNotFoundException("Chat not found"));;
        User user = userRepository.findById(userId).orElseThrow(() -> new RescourceNotFoundException("User not found"));;
        user.getChat_IDs().remove(chat.getId());
        return userRepository.save(user);
    }
}
