"use strict";

//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;

connection.on("ConnectionMessage", function (user, message) {
	var li = document.createElement("li");
	var text = document.createElement("p");
	text.textContent = `${user} ${message}`;
	text.style.textAlign = "center";

	li.append(text);

	li.style.padding = "10px";
	li.style.color = "black";
	li.style.backgroundColor = "white";

	document.getElementById("messagesList").appendChild(li);
})

connection.on("ReceiveMessage", function (user, message) {
	var li = document.createElement("li");
	document.getElementById("messagesList").appendChild(li);
	// We can assign user-supplied strings to an element's textContent because it
	// is not interpreted as markup. If you're assigning in any other way, you
	// should be aware of possible script injection concerns.

	var div = document.createElement("div");
	div.style.direction = "ltr";
	div.style.display = "flex";
	div.style.flexDirection = "row";

	var icon = document.createElement("img");
	icon.src = "https://st3.depositphotos.com/6672868/13701/v/450/depositphotos_137014128-stock-illustration-user-profile-icon.jpg";
	icon.style.borderRadius = "50%";
	icon.width = 30;
	icon.height = 30;


	var time = document.createElement("p");
	time.style.fontWeight = "normal";
	time.style.fontSize = "0.8em";
	time.style.position = "absolute";
	time.style.right = "180px";

	div.append(icon);
	div.append(time);

	var text = document.createElement("p");
	text.style.fontWeight = "normal";

	var sendingUser = document.createElement("p");
	sendingUser.style.fontWeight = "bold";

	time.textContent = new Date().toLocaleTimeString();
	text.textContent = message;
	user === userName ? sendingUser.textContent = "" : sendingUser.textContent = `${user}`;

	//li.textContent = `${user}: ${message}`;
	sendingUser.append(text);
	li.append(div);
	li.append(sendingUser);

	li.style.padding = "10px";
	li.style.color = "white";
	li.style.backgroundColor = "grey";
	li.style.borderRadius = "10px";
	li.style.marginRight = "150px";
	li.style.marginBottom = "10px";

	if (user === userName) {
		li.style.direction = "rtl";
		li.style.backgroundColor = "red";
		li.style.marginRight = "5px";
		li.style.marginLeft = "150px";
		time.style.right = "30px";
	}
});

connection.start().then(function () {
	document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
	return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
	var message = document.getElementById("messageInput").value;
	connection.invoke("SendMessage", message).catch(function (err) {
		return console.error(err.toString());
	});
	event.preventDefault();
});