package com.example.demo.model;

import java.util.Date;

public class Message {
    private String sender;
    private String content;
    private Date timestamp;

    public Message() {}

    public Message(String sender, String content) {
        this.sender = sender;
        this.content = content;
        this.timestamp = new Date();
    }

}
