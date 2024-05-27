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
      const formData = {
      username: username,
      password:password
    };
    await fetch(url, { method: "POST",  headers: {'Content-Type': 'application/json'},body: JSON.stringify(formData), mode : "cors" })
    .then(response => response.json())
    .then( data => {
      
      console.log('Registration successful:', data);
      location.replace("./index.html");
      
    })
    .catch(error => {
      console.log(JSON.stringify(formData))
      console.error('Registration failed:', error);
      
    });
  }
  else{
    document.getElementById('invalidinput').textContent = "Invalid Input";
  }
  
}

