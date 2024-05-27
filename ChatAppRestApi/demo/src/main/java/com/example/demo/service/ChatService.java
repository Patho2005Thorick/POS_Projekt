package com.example.demo.service;

import com.example.demo.exception.RescourceNotFoundException;
import com.example.demo.model.Chat;
import com.example.demo.model.Message;
import com.example.demo.repository.ChatRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
public class ChatService {

    @Autowired
    private ChatRepository chatRepository;

    public Chat createChat(List<String> participants) {
        Chat chat = new Chat();
        chat.setParticipants(participants);
        return chatRepository.save(chat);
    }

    public Chat sendMessage(String chatId, Message message) {
        Chat chat = chatRepository.findById(chatId).orElseThrow(() -> new RescourceNotFoundException("Chat not found"));
        chat.getMessages().add(message);
        return chatRepository.save(chat);
    }

    public Chat updateChat(String chatId, Chat updatedChat) {
        Chat chat = chatRepository.findById(chatId).orElseThrow(() -> new RescourceNotFoundException("Chat not found"));
        chat.setMessages(updatedChat.getMessages());
        return chatRepository.save(chat);
    }

    public Chat getChat(String chatId) {
        return chatRepository.findById(chatId).orElseThrow(() -> new RescourceNotFoundException("Chat not found"));
    }

    public void deleteChat(String chatId){
        Chat chat = chatRepository.findById(chatId).orElseThrow(() -> new RescourceNotFoundException("Chat not found"));;
        chatRepository.delete(chat);
    }
}
