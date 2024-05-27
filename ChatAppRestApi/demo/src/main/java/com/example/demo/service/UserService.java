package com.example.demo.service;

import com.example.demo.exception.RescourceNotFoundException;
import com.example.demo.model.User;
import com.example.demo.repository.UserRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
public class UserService {
    @Autowired
    private UserRepository userRepository;


    public List<User> getUsers(){
        return userRepository.findAll();
    }


    public User getNewContact(String username){

        List<User> userlist = userRepository.findAll();
        for (User user_:userlist) {
            if(user_.getUsername().equals(username)){
                return user_;
            }
        }

        throw new RescourceNotFoundException("The User with the Name " + username + " doesn't exist");

    }

    public User getUser(String username, String password){

        List<User> userlist = userRepository.findAll();
        User user = userRepository.findByUsername(username);
        if(user.getPassword().equals(password)){
            return user;
        }
        else{
            throw new RescourceNotFoundException("The User with the Name " + username + " doesn't exist");
        }
    }

    public User createUser(User user){
        return userRepository.save(user);
    }

    public User newContact(String data){
        System.out.println(data);
        String [] datastrings = data.split(",");
        String username = datastrings[0];
        String newcontactname =  datastrings[1];
        username = username.replace("\"", "");
        newcontactname = newcontactname.replace("\"", "");
        System.out.println(username);
        System.out.println(newcontactname);
        User user = userRepository.findByUsername(username);
        User contact = userRepository.findByUsername(newcontactname);

        if (user != null && contact != null && !user.getContacts().contains(newcontactname)) {
            user.getContacts().add(newcontactname);
            System.out.println(user.getContacts().toString());
            userRepository.save(user);
        }
        return user;

    }

    public User newChat(String data){
        System.out.println(data);
        String [] datastrings = data.split(",");
        String username = datastrings[0];
        String contactname =  datastrings[1];
        String chat_id = datastrings[2];
        username = username.replace("\"", "");
        contactname = contactname.replace("\"", "");
        chat_id = chat_id.replace("\"", "");

        User user = userRepository.findByUsername(username);
        User contact = userRepository.findByUsername(contactname);
        user.getChat_IDs().add(chat_id);
        contact.getChat_IDs().add(chat_id);

        userRepository.save(user);
        userRepository.save(contact);
        return user;
    }


    public void deleteUser(String username){
        User user  = userRepository.findByUsername(username);
        userRepository.delete(user);
    }


}
