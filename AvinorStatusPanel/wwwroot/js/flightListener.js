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
        addCell(newRow, flight.flightNumber);

        var departureTime = formatAs24HourTime(flight.departureTime);
        var arrivalTime = formatAs24HourTime(flight.arrivalTime);


        addCell(newRow, `${departureTime} - ${arrivalTime}`);
        
        addCell(newRow, `${flight.origin}`);
        addCell(newRow, `${flight.destination}`);
        addCell(newRow, "");
        /*
        Flight
        Origin - Destination
        
        addCell(newRow, formatAs24HourTime(flight.arrivalTime)); // Departure time
        addCell(newRow, formatAs24HourTime(flight.departureTime)); // Airline logo
        addCell(newRow, flight.flightNumber); // Call sign
        addCell(newRow, ""); // Gate
        addCell(newRow, flight.destination); // Destination
        addCell(newRow, flight.origin); // New time
        
         */
    }
});

function formatAs24HourTime(dateString) {
    const date = new Date(dateString);

    const hours = date.getUTCHours();
    const minutes = date.getMinutes();

    return `${hours.toString().padStart(2, "0")}:${minutes.toString().padStart(2, "0")}`;
}

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
