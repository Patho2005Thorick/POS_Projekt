package com.example.demo.repository;

import com.example.demo.model.Chat;
import org.springframework.data.mongodb.repository.MongoRepository;


public interface ChatRepository extends  MongoRepository<Chat,String>{

}
