package com.example.demo.service;

import com.example.demo.exception.RescourceNotFoundException;
import com.example.demo.model.User;
import com.example.demo.repository.UserRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.Optional;

@Service
public class UserService {
    @Autowired
    private UserRepository userRepository;

    public List<User> getUsers(){
        return userRepository.findAll();
    }

    public User getUserByID(String userId){
        Optional<User> user = userRepository.findById(userId);
        if(user.isPresent()){
            return user.get();
        }
        else{
            throw new RescourceNotFoundException("The User with the ID " + userId + " doesn't exist");
        }
    }

    public User createUser(User user){
        return userRepository.save(user);
    }

    public User newContact(String userId, String newContact){
        Optional<User> user = userRepository.findById(userId);
        if(user.isPresent()){
            User yourUser =  user.get();
            List<User> userlist = userRepository.findAll();

            for (User user_:userlist) {
                if(user_.getUsername().equals(newContact)){

                    yourUser.setContacts(user_);
                }
            }
            if(yourUser.getContacts().size() == 0){
                throw new RescourceNotFoundException("The User with the Name " + newContact + " doesn't exist");
            }
            return userRepository.save(yourUser);
        }
        else{
            throw new RescourceNotFoundException("The User with the ID " + userId + " doesn't exist");
        }


    }
}
