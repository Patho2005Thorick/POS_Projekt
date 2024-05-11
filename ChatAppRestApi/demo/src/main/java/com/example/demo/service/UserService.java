package com.example.demo.service;

import com.example.demo.exception.RescourceNotFoundException;
import com.example.demo.model.User;
import com.example.demo.repository.UserRepository;
import com.fasterxml.jackson.databind.ObjectMapper;
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
        User user = new User();
        for (User user_:userlist) {
                if(user_.getUsername().equals(username)){

                    user = user_;
                }
            }
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
        try{

            System.out.println("Received data: " + data); // Log the received data
            ObjectMapper objectMapper = new ObjectMapper();
            User user = objectMapper.readValue(data, User.class);
            return userRepository.save(user);
        }catch(Exception ex){
            System.out.println(""+ex);
            return null;
        }

    }
}
