"use strict";
let connection;
let roomNumber;
$(document).ready(function () {
    connection = new signalR.HubConnectionBuilder().withUrl("gamehub").build();
    roomNumber = createNewRoom();
    $("#newGameRoomNumber").html("Your room number: " + roomNumber);

    connection.on("connected", function () {
        console.log("Connected to the hub.");
    });

    connection.start().then(function () {
        CreateGameOnClick();
    });

    /*
    connection.on("ReceiveMessage", function (user, message) {
        var li = document.createElement("li");
        document.getElementById("messagesList").appendChild(li);
        // We can assign user-supplied strings to an element's textContent because it
        // is not interpreted as markup. If you're assigning in any other way, you 
        // should be aware of possible script injection concerns.
        li.textContent = `${user} says ${message}`;
    });
    */
});

function CreateGameOnClick() {
    $(document).on("click", "#createNewGameBtn", function () {
        connection.invoke("CreateGame", roomNumber.toString()).catch(function (err) {
            return console.error(err.toString());
        });
    });
}

function SendCheckerMove(previousCheckerRow, previousCheckerColumn, nextCheckerRow, nextCheckerColumn) {
    connection.invoke("MakeMove", previousCheckerRow, previousCheckerColumn, nextCheckerRow, nextCheckerColumn).catch(function (err) {
        return console.error(err.toString());
    });
}