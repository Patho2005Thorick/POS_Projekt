package com.example.demo.controller;

import com.example.demo.model.Chat;
import com.example.demo.model.Message;
import com.example.demo.model.User;
import com.example.demo.service.ChatService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@CrossOrigin(origins = "http://127.0.0.1:5501")
@RestController
@RequestMapping("ThorChat/chats")
public class ChatController {

    @Autowired
    private ChatService chatService;

    @PostMapping("/create")
    public Chat createChat(@RequestBody List<String> participants) {
        return chatService.createChat(participants);
    }

    @PostMapping("/{chatId}/message")
    public Chat sendMessage(@PathVariable String chatId, @RequestBody Message message)  {
        return chatService.sendMessage(chatId, message);
    }

    @PutMapping("/update/{chatId}")
    public Chat updateChat(@PathVariable String chatId, @RequestBody Chat chat) {
        return chatService.updateChat(chatId, chat);
    }

    @GetMapping("/{chatId}")
    public Chat getChat(@PathVariable String chatId) {
        return chatService.getChat(chatId);
    }

    @PutMapping("/delete")
    public User deleteChat(@RequestBody String data){
        return chatService.deleteChat(data);
    }
}
