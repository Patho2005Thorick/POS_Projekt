let username=  ' ';

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




/*async*/ function addContact(){
    let newcontact = document.createElement("li");
    let name = document.createTextNode(`${username}`);
    newcontact.appendChild(name);
    document.getElementById("contact_list_").appendChild(newcontact);

}