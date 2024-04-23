package com.example.demo.service;

import com.example.demo.model.Chat;
import com.example.demo.repository.ChatRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

@Service
public class ChatService {
    @Autowired
    ChatRepository chatRepository;
    public Chat createChat(Chat user){
        return chatRepository.save(user);
    }
}
