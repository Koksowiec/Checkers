let ROOM_NUMBER;

$(document).ready(function () {
    if (!ROOM_NUMBER) {
        ROOM_NUMBER = createNewRoom();
    }

    $("#createRoomNumber").val(ROOM_NUMBER);
});

function createNewRoom(){
    let number = generateNumber()
    return number;
}

function generateNumber(){
    return Math.floor(Math.random() * 899999 + 100000)
}