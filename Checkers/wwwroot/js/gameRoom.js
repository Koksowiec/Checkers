let checker;
let checkerRow;
let checkerColumn;
let checkerDirection;
let checkerPlayer;

let possibleRows = [];
let possibleColumns = [];

$(document).ready(function () {
    CheckerOnClick();

    CellOnClick();
});

function CheckerOnClick() {
    $(document).on("click", ".checker", function () {
        checker = $(this);
        checkerRow = checker.attr("data-row");
        checkerColumn = checker.attr("data-column");
        checkerDirection = checker.attr("data-direction");
        checkerPlayer = checker.attr("data-player");

        clearAllCheckersActiveClassess();

        $(this).addClass("active");

        EvaluateCheckerSpots(checkerRow, checkerColumn, checkerDirection);
    });
}

function CellOnClick() {
    $(document).on("click", ".cell", function () {
        if ($(this).hasClass("active")) {
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

function CanJumpOver(possibleCell) {
    if (possibleCell.find(".checker").length == 0) {
        possibleCell.addClass("active")
    }
    else {
        //let columnDifference = parseInt(possibleCell.attr("data-column")) - parseInt(checkerColumn);
        //let rowDifference = parseInt(possibleCell.attr("data-row")) - parseInt(checkerRow);
        //possibleCell = $('div.cell[data-row="' + (possibleRows[0] + rowDifference) + '"][data-column="' + (possibleColumns[0] + columnDifference) + '"]');
        //CanJumpOver(possibleCell);
    }
}