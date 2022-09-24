// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var spent = setInterval(AmountSpent, 10)
var received = setInterval(AmountReceived, 10)
var borrow = setInterval(AmountBorrowed, 1)
var charged = setInterval(AmountCharged, 1)



    let count1 = 1;
    let count2 = 1;
    let count3 = 1;
    let count4 = 1;

function AmountSpent() {

    count1++

    document.querySelector("#number1").innerHTML = count1

    if (count1 == item.AmountSpent) {
        clearInterval(spent)
    }
}

function AmountReceived() {

    count2++

    document.querySelector("#number2").innerHTML = count2

    if (count2 == item.AmountReceived) {
        clearInterval(received)
    }
}

function AmountBorrowed() {

    count3++

    document.querySelector("#number3").innerHTML = count3

    if (count3 == item.AmountBorrowed) {
        clearInterval(borrow)
    }
}

function AmountCharged() {

    count4++

    document.querySelector("#number4").innerHTML = count4

    if (count3 == item.AmountCharged) {
        clearInterval(charged)
    }
}