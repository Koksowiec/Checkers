﻿// SignalR START
"use strict";
let connection = null;
let ROOM_NUMBER;
let CURRENT_PLAYER = null;
let CAN_PLAYER_MOVE = false;

$(document).ready(function () {
    connection = new signalR.HubConnectionBuilder().withUrl("gamehub").build();

    ROOM_NUMBER = createNewRoom();
    $("#newGameRoomNumber").html("Your room number: " + ROOM_NUMBER);

    connection.on("connected", function () {
        console.log("Connected to the hub.");
    });

    $("#createRoomNumber").val(generateNumber());

    connection.start().then(function () {
        HandleRequest();
        //CreateGameOnClick();
        //JoinGameOnClick();
        SendCheckerMove();

        connection.on("TableJoined", function (p1Name, p2Name) {
            TableJoined(p1Name, p2Name);
        });

        connection.on("YouJoined", function (p1Name, p2Name) {
            YouJoined(p1Name, p2Name);
        });

        connection.on("GameCreated", function () {
            GameCreated();
        });

        connection.on("EnemyMoved", function (newMove, checkerToDelete) {
            EnemyMoved(newMove, checkerToDelete);
        });

        connection.on("YouMoved", function () {
            YouMoved();
        });
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

function SendCheckerMove(previousCheckerRow, previousCheckerColumn, nextCheckerRow, nextCheckerColumn, checkerToDeleteRow, checkerToDeleteColumn, roomNumber, currentPlayer) {
    // Check if at least one of the parameters is not undefined, then proceed
    if (previousCheckerColumn != undefined) {

        if (checkerToDeleteRow == undefined) {
            checkerToDeleteRow = 0;
        }
        if (checkerToDeleteColumn == undefined) {
            checkerToDeleteColumn = 0;
        }

        connection.invoke("MakeMove", previousCheckerRow, previousCheckerColumn, nextCheckerRow, nextCheckerColumn, checkerToDeleteRow, checkerToDeleteColumn, roomNumber, currentPlayer).catch(function (err) {
            return console.error(err.toString());
        });
    }
}

function HandleRequest() {
    let method = $("#signalRMethod").attr("data-method");
    let gameId = $("#signalRGameId").attr("data-gameId");
    let p2Name = $("#signalRP2Name").attr("data-p2Name");
    p2Name = p2Name == undefined ? "" : p2Name;

    if (method == "join") {
        connection.invoke("JoinGame", gameId.toString(), p2Name).catch(function (err) {
            return console.error(err.toString());
        });
    }
    else if (method == "create") {
        connection.invoke("CreateGame", gameId.toString()).catch(function (err) {
            return console.error(err.toString());
        });
    }
}

function TableJoined(p1Name, p2Name) {
    $("#p1Name").html("P1: " + p1Name);
    $("#p2Name").html("P2: " + p2Name);

    console.log("Someone joined in on the game!");
    CAN_PLAYER_MOVE = true;
}

function YouJoined(p1Name, p2Name) {
    $("#p1Name").html("P1: " + p1Name);
    $("#p2Name").html("P2: " + p2Name);

    console.log("You joined in on the game!");
    CAN_PLAYER_MOVE = false;

    if (CURRENT_PLAYER == null) {
        CURRENT_PLAYER = "P2"
        $("#p2Name").addClass("active-player")
    }
}

function GameCreated() {
    console.log("You have successfully created a game");
    CURRENT_PLAYER = "P1";
    $("#p1Name").addClass("active-player")
}

function EnemyMoved(newMove, checkerToDelete) {
    UpdateTable(newMove, checkerToDelete);

    console.log("Enemy moved, now your turn");
    CAN_PLAYER_MOVE = true;
}

function YouMoved() {
    console.log("You moved, now enemy turn");
    CAN_PLAYER_MOVE = false;
}

// SignalR END

let rooms = [1];
let highcores = [['Andy', 122], ['Rock', 84], ['Wendy', 64], ['Danny', 51]]

function createNewRoom(){
    let number = generateNumber()
    rooms.push(number)
    return number;
}

function generateNumber(){
    return Math.floor(Math.random() * 899999 + 100000)
}

function generateRanking() {
    let highscoreElement = document.getElementById('highscore');

    // Wyczyszczenie aktualnej zawartości, żeby nie dublować wyników przy każdym wywołaniu
    highscoreElement.innerHTML = "<h2><b>Highscores:</b></h2>"; // Możesz zmodyfikować, aby pasowało do Twojego stylu

    // Iteracja przez wszystkie rekordy highscores i dodanie ich do diva highscore
    highcores.forEach(function(score) {
        highscoreElement.innerHTML += "<div class='rank'>" + score[0] + ": <span>" + score[1] + "</span></div>";
    });
}
generateRanking()

function goTo() {
    toRoom = document.getElementById('roomInput').value;

    if (rooms.includes(parseInt(toRoom))) {
        // Dodaj klasę slide-out do kontenera i highscore
        document.getElementById('glass').classList.add('slide-out');
        document.getElementById('highscore').classList.add('slide-out');
        document.getElementById('title').classList.add('slide-out');

        // Poczekaj na zakończenie animacji, a następnie wyświetl komunikat
        setTimeout(function () {
            alert("Pokój o numerze " + toRoom + " został znaleziony!");
        }, 1000);
    } else {
        alert("Pokój o numerze " + toRoom + " nie został znaleziony.");
    }
}