package com.example.demo.model;

import com.fasterxml.jackson.annotation.JsonCreator;
import com.fasterxml.jackson.annotation.JsonProperty;
import com.fasterxml.jackson.databind.annotation.JsonSerialize;
import org.springframework.data.annotation.Id;
import org.springframework.data.mongodb.core.mapping.Document;

import java.util.ArrayList;
import java.util.List;

@Document
public class User {



    @Id
    private String username;

    private String password;
    private List<String> chat_IDs = new ArrayList<>();
    private List<String> contacts = new ArrayList<>();


    public User() {

    }

    @JsonCreator
    public User(@JsonProperty("username") String username,
                @JsonProperty("password") String password,
                @JsonProperty("contacts") List<String> contacts,
                @JsonProperty("chat_IDs") List<String> chat_IDs) {
        this.username = username;
        this.password = password;
        this.contacts = contacts != null ? contacts : new ArrayList<>();
        this.chat_IDs = chat_IDs != null ? chat_IDs : new ArrayList<>();
    }

    public String getUsername() {
        return username;
    }

    public void setUsername(String username) {
        this.username = username;
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

    public List<String> getContacts() {
        return contacts;
    }

    public void setContacts(String contact) {
        contacts.add(contact);
    }

    public String get(){
        String data;
        return data =  JsonSerialize.class.toString();
    }
}
