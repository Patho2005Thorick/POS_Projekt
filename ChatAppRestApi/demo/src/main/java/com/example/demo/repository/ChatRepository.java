package com.example.demo.repository;

import com.example.demo.model.Chat;
import org.springframework.data.mongodb.repository.MongoRepository;

import java.util.List;


public interface ChatRepository extends  MongoRepository<Chat,String>{
    List<Chat> findByParticipantsContaining(String userId);
}
