package com.example.demo.controller;

import com.example.demo.model.User;
import com.example.demo.service.UserService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.mongodb.repository.Update;
import org.springframework.web.bind.annotation.*;

import java.util.List;
@CrossOrigin(origins = "http://127.0.0.1:5501")
@RestController
@RequestMapping("ThorChat/users")
public class UserController {

    @Autowired
    private UserService userService;

    @GetMapping
    public List<User> getUsers(){

        return userService.getUsers();
    }

    @GetMapping("/{name}/{password}")
    public User getUserById(@PathVariable String name,@PathVariable String password){
        return userService.getUser(name, password);
    }

    @PostMapping
    public User addUser(@RequestBody User user){
        return userService.createUser(user);
    }


    @PutMapping("/{id}/{username}")
    public User addContact(@PathVariable String id, @PathVariable String username){
        return userService.newContact(id, username);
    }
}
