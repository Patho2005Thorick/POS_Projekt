
class User {
  constructor(username, password, chatIDs = [], contacts = []) {
      this.username = username;
      this.password = password;
      this.chatIDs = chatIDs;
      this.contacts = contacts;
  }

  getUsername() {
      return this.username;
  }
}

let username=  ' ';
let password =  ' ';

function handleInputChange(event) {
  const { id, value } = event.target;
  switch (id) {
      case 'usernameInput':
          username = value;
          break;
      case 'passwordInput':
          password = value;
          break;
    
  }
}


document.getElementById('usernameInput').addEventListener('input', handleInputChange);
document.getElementById('passwordInput').addEventListener('input', handleInputChange);



async function addUser() {
  if(username != " " && password != " "){
      document.getElementById('invalidinput').textContent = " ";
      let url = "http://localhost:8080/ThorChat/users" ; 

      const user = new User(username, password, null, null);
    let response = fetch(url, { method: "POST",  headers: {'Content-Type': 'application/json'},body: JSON.stringify(user), mode : "cors" })
   
    if(response !=  null){
      alert("Succeded to add user")
    }
    
      
   
  }
  else{
    document.getElementById('invalidinput').textContent = "Invalid Input";
  }
  
}

