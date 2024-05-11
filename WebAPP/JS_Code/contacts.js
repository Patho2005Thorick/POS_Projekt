let username=  ' ';

let url = "http://localhost:8080/ThorChat/users/contacts"

function handleInputChange(event) {
  const { id, value } = event.target;
  switch (id) {
      case 'AddUserInput':
          username = value;
          break;
      default:
          break;
  }
}


document.getElementById('AddUserInput').addEventListener('input', handleInputChange);

function getUser(){
    
}


/*async*/ function addContact(){
    let newcontact = document.createElement("li");
    let name = document.createTextNode(`${username}`);
    newcontact.appendChild(name);
    document.getElementById("contact_list_").appendChild(newcontact);

    

}


async function put(url, data) { 
  
    // Awaiting fetch which contains method, 
    // headers and content-type and body 
    const response = await fetch(url, { 
      method: 'PUT', 
      headers: { 
        'Content-type': 'application/json'
      }, 
      body: JSON.stringify(data) 
    }); 
}