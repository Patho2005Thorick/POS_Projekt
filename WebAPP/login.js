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
      default:
          break;
  }
}


document.getElementById('usernameInput').addEventListener('input', handleInputChange);
document.getElementById('passwordInput').addEventListener('input', handleInputChange);


async function login(){
  if(username != " " && password != " "){
    document.getElementById('invalidinput').textContent = " ";
    let url = `http://localhost:8080/ThorChat/users/${username}/${password}`; 
    const data ={}
    try {
      const response = await fetch(url, { 
        method: "GET",
        headers: {'Accept': 'application/json'}, 
        cache: "default",
        mode: "cors" 
      });
      data = await response.json();
      console.log('Login successful:', data);
    } catch(error) {
      console.error('Login failed:', error);
    }
  } else {
    document.getElementById('invalidinput').textContent = "Invalid Input";
  }
  }


