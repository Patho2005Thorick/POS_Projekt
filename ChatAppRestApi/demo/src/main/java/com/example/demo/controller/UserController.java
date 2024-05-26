package com.example.demo.controller;

import com.example.demo.model.User;
import com.example.demo.service.UserService;
import io.swagger.v3.oas.annotations.Operation;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

import java.util.List;
@CrossOrigin(origins = "http://127.0.0.1:5501")
@RestController
@RequestMapping("ThorChat/users")
public class UserController {

    @Autowired
    private UserService userService;

    @Operation(summary = "GET Operation for all Users")
    @GetMapping
    public List<User> getUsers(){

        return userService.getUsers();
    }
    @Operation(summary = "GET Operation for a single User")
    @GetMapping("/{name}/{password}")
    public User getUserByName_And_Password(@PathVariable String name,@PathVariable String password){
        return userService.getUser(name, password);
    }

    @Operation(summary = "GET Operation for a single User")
    @GetMapping("/{name}")
    public User getUserByName(@PathVariable String name){
        return userService.getNewContact(name);
    }
    @Operation(summary = "POST Operation to post a User")
    @PostMapping
    public User addUser(@RequestBody User user){
        System.out.println("Received user: " + user);
        return userService.createUser(user);
    }

    @Operation(summary = "PUT Operation to add an Contact")
    @PutMapping("/contacts")
    public User addContact(@RequestBody String data){
        return userService.newContact(data);
    }


   

}
