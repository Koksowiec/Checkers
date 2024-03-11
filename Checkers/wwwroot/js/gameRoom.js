let checker;
let checkerRow;
let checkerColumn;
let checkerDirection;
let checkerPlayer;

let possibleRows = [];
let possibleColumns = [];

let CAN_JUMP_OVER = false;

// Take into consideration the global variables created in site.js

$(document).ready(function () {
    CheckerOnClick();

    CellOnClick();
});

function CheckerOnClick() {
    $(document).on("click", ".checker", function () {
        if (CAN_PLAYER_MOVE) {
            checker = $(this);
            checkerRow = checker.attr("data-row");
            checkerColumn = checker.attr("data-column");
            checkerDirection = checker.attr("data-direction");
            checkerPlayer = checker.attr("data-player");

            if (CURRENT_PLAYER == checkerPlayer) {
                clearAllCheckersActiveClassess();

                $(this).addClass("active");

                EvaluateCheckerSpots(checkerRow, checkerColumn, checkerDirection);
            }
        }
    });
}

function CellOnClick() {
    $(document).on("click", ".cell", function () {
        if ($(this).hasClass("active") && CAN_PLAYER_MOVE) {
            let previousCheckerRow = parseInt(checkerRow);
            let previousCheckerColumn = parseInt(checkerColumn);
            let nextCheckerRow = parseInt($(this).attr("data-row"));
            let nextCheckerColumn = parseInt($(this).attr("data-column"));

            checker.appendTo($(this));

            checker.attr("data-row", $(this).attr("data-row"));
            checker.attr("data-column", $(this).attr("data-column"));

            clearAllCellsActiveClasses();
            clearAllCheckersActiveClassess();

            UpdateDirectionWhenOnBoardEnd(checker.attr("data-player"));

            if (checker.attr("data-direction") == 'Both') {
                if (checker.find(".king").length == 0) {
                    checker.append($('<span class="king">K</span>'));
                }
            }

            gameId = $("#signalRGameId").attr("data-gameId");
            SendCheckerMove(previousCheckerRow, previousCheckerColumn, nextCheckerRow, nextCheckerColumn, gameId, CURRENT_PLAYER);
        }
    });
}

function EvaluateCheckerSpots(row, column, direction) {
    possibleRows = [];
    possibleColumns = [];

    if (direction == 'Down' || direction == 'Both') {
        possibleRows.push(parseInt(row) + (1 * 1));
    }
    if (direction == 'Up' || direction == 'Both') {
        possibleRows.push(parseInt(row) + (1 * (-1)));
    }

    possibleColumns.push(parseInt(column) - 1);
    possibleColumns.push(parseInt(column) + 1);

    clearAllCellsActiveClasses();
    
    // Check if there is no checker here
    // Update on it: if there is a checker here, check if this is from the opposite team
    // If the checker is from the opposite team then to the possibleRows value add +1 and the same for possibleColumns (in theory)
    // Do the part after in the separate method
    // You have to bare in mind the direction of the checker
    // Maybe to speed things up create a checker parameters in the beginning like the let checker; let checkerRow; let checkerColumn;...
    var possibleCell1 = $('div.cell[data-row="' + possibleRows[0] + '"][data-column="' + possibleColumns[0] + '"]');
    var possibleCell2 = $('div.cell[data-row="' + possibleRows[0] + '"][data-column="' + possibleColumns[1] + '"]');

    CanJumpOver(possibleCell1);
    CanJumpOver(possibleCell2);

    if (direction == 'Both') {
        var possibleCell3 = $('div.cell[data-row="' + possibleRows[1] + '"][data-column="' + possibleColumns[0] + '"]');
        var possibleCell4 = $('div.cell[data-row="' + possibleRows[1] + '"][data-column="' + possibleColumns[1] + '"]');

        CanJumpOver(possibleCell3);
        CanJumpOver(possibleCell4);
    }
}

function clearAllCellsActiveClasses() {
    $('div.cell').removeClass('active');
}

function clearAllCheckersActiveClassess() {
    $('div.checker').removeClass('active');
}

function UpdateDirectionWhenOnBoardEnd(player) {
    if (player == 'P1' && parseInt(checker.attr("data-row")) == 0) {
        checker.attr("data-direction", 'Both');
    }
    if (player == 'P2' && parseInt(checker.attr("data-row")) == 7) {
        checker.attr("data-direction", 'Both');
    }
}

function CanJumpOver(possibleCell, isSecondTime = false) {
    let possibleCellRow = parseInt(possibleCell.attr("data-row"));
    let possibleCellColumn = parseInt(possibleCell.attr("data-column"));

    if (possibleCell.find(".checker").length == 0) {
        possibleCell.addClass("active")
    }
    else {
        // Check if can Jump over
        // Check if the checker in the cell is on enemy team, if so then proceed
        let possibleCellChecker = possibleCell.children().eq(0);

        if (isSecondTime) {
            return;
        }

        if (possibleCellChecker.attr("data-player") != checkerPlayer) {
            let columnDifference = parseInt(possibleCell.attr("data-column")) - parseInt(checkerColumn);
            let rowDifference = parseInt(possibleCell.attr("data-row")) - parseInt(checkerRow);

            possibleCell = $('div.cell[data-row="' + (possibleCellRow + rowDifference) + '"][data-column="' + (possibleCellColumn + columnDifference) + '"]');
            
            CanJumpOver(possibleCell, true);
        }
    }
}

function UpdateTable(newMove) {
    let move = newMove.toString();
    let movesArray = move.split(";");
    let lastMove = movesArray[movesArray.length - 1];

    let movesToUpdate = lastMove.split("-");
    let previousMove = movesToUpdate[0].split("_");
    let nextMove = movesToUpdate[1].split("_");

    let previousCell = $('div.cell[data-row="' + previousMove[1] + '"][data-column="' + previousMove[0] + '"]');
    let nextCell = $('div.cell[data-row="' + nextMove[1] + '"][data-column="' + nextMove[0] + '"]');

    let checkerToMove = previousCell.children(".checker");

    console.log("Previous: " + (parseInt(previousMove[1]) + 1) + " " + (parseInt(previousMove[0]) + 1));
    console.log("Next: " + (parseInt(nextMove[1]) + 1) + " " + (parseInt(nextMove[0]) + 1));

    checkerToMove.appendTo(nextCell);
}