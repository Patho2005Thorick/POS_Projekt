package com.example.demo.repository;

import com.example.demo.model.ChatVerlauf;
import org.springframework.data.mongodb.repository.MongoRepository;
import org.springframework.data.rest.core.annotation.RepositoryRestResource;


public interface ChatverlaufRepository extends  MongoRepository<ChatVerlauf,String>{

}
