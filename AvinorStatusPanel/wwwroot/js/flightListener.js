"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/flightHub").build();

function addCell(row, textValue) {
    const newCell = row.insertCell();
    const newText = document.createTextNode(textValue);
    newCell.appendChild(newText);
}

connection.on("UpdateFlightsView", function (data) {
    console.log("Received flights", data);
    const existingFlightsTable = document.getElementById("flightsTable");
    existingFlightsTable.innerHTML = "";
    
    for (const [callSign, flight] of Object.entries(data)) {
        const newRow = existingFlightsTable.insertRow();
        addCell(newRow, "12:32"); // Departure time
        addCell(newRow, "-"); // Airline logo
        addCell(newRow, callSign); // Call sign
        addCell(newRow, "A1"); // Gate
        addCell(newRow, "OSL"); // Destination
        addCell(newRow, "New time: 13:37"); // New time
    }
});

connection.on("FastenSeatbelt", function () {
    new Audio('/FastenSeatbelt.mp3').play();
});

connection.start().then(function () {
    connection.invoke("RequestFlights").catch(function (err) {
        return console.error(err.toString());
    });
}).catch(function (err) {
    return console.error(err.toString());
});
