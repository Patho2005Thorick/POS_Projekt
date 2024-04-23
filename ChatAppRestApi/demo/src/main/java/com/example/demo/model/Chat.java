package com.example.demo.model;
import org.springframework.data.annotation.Id;
import org.springframework.data.mongodb.core.mapping.Document;

import java.util.List;

@Document
public class Chat {

    @Id private String id;
    private List<String> chatverlauf;

    public String getId() {
        return id;
    }

    public void setId(String id) {
        this.id = id;
    }

    public List<String> getChatverlauf() {
        return chatverlauf;
    }

    public void setChatverlauf(String message) {
        chatverlauf.add(message);
    }

    public void deleteMessage(String message) {
        chatverlauf.remove(message);
    }



}
