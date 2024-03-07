rooms = [1];
highcores = [['Andy' , 122], ['Rock' , 84], ['Wendy' , 64], ['Danny' , 51]]
function createNewRoom(){
    number = generateNumber()
    rooms.push(number)
    alert("Twój pokój ma numer: "+number)
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