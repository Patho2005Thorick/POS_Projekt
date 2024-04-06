let username=  ' ';
let email = ' ';
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
      case 'emailInput':
          email = value;
          break;
      default:
          break;
  }
}


document.getElementById('usernameInput').addEventListener('input', handleInputChange);
document.getElementById('passwordInput').addEventListener('input', handleInputChange);
document.getElementById('emailInput').addEventListener('input', handleInputChange);




async function addUser() {
  let url = "http://localhost:8080/ThorChat/users" ; // ⚠️ whatever url that accept a post request
  const formData = {
    username: username,
    email: email,
    password:password
  };
  await fetch(url, { method: "POST",  headers: {'Content-Type': 'application/json'},body: JSON.stringify(formData), mode : "cors" })
  then(response => response.json())
  .then( data => {
    
    console.log('Registration successful:', data);
    
  })
  .catch(error => {
    console.log(JSON.stringify(formData))
    console.error('Registration failed:', error);
    
  });
}