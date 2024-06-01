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

function login(e){
  e.preventDefault();
  if (username.trim() !== "" && password.trim() !== "") {
      document.getElementById('invalidinput').textContent = "";
      let url = `http://localhost:8080/ThorChat/users/${username}/${password}`;
      console.log(url)
      $.get(url, (data, status) => {
          console.log(data);
          if(data != null){
            localStorage.setItem('user', JSON.stringify(data));
            location.replace("./contacts.html")
          }
      });
    }
  }

