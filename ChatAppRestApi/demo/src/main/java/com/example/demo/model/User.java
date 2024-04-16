package com.example.demo.model;

import com.fasterxml.jackson.databind.annotation.JsonSerialize;
import com.fasterxml.jackson.databind.util.JSONPObject;
import org.springframework.data.annotation.Id;
import org.springframework.data.mongodb.core.mapping.Document;

import java.util.*;

@Document
public class User {
    @Id
    private String id;
    private String username;
    private String email;

    private String password;
    private List<String> chat_IDs;
    private List<User> contacts = new ArrayList<>();

    public String getId() {
        return id;
    }

    public void setId(String id) {
        this.id = id;
    }

    public String getUsername() {
        return username;
    }

    public void setUsername(String username) {
        this.username = username;
    }

    public String getEmail() {
        return email;
    }

    public void setEmail(String email) {
        this.email = email;
    }

    public String getPassword() {
        return password;
    }

    public void setPassword(String password) {
        this.password = password;
    }

    public List<String> getChat_IDs() {
        return chat_IDs;
    }

    public void setChat_IDs(String chat_ID) {
        chat_IDs.add(chat_ID);
    }

    public List<User> getContacts() {
        return contacts;
    }

    public void setContacts(User contact) {
        contacts.add(contact);
    }

    public String get(){
        String data;
        return data =  JsonSerialize.class.toString();
    }
}
