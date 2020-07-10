var io = require('socket.io')(80);
console.log('Server started !');

function getTime() {
    const hrtime = process.hrtime(); 
    return parseInt(((hrtime[0] * 1e3) + (hrtime[1]) * 1e-6));
}

io.on('connection', function(socket) {
    console.log("Connection made !");

    socket.on('disconnect', function(socket) {
        console.log("Disonnect made !");
    });
    socket.on('message', function(data) {
        console.log("[message from client] " +  data.data);
    });

    
});

function loop() {
    io.sockets.emit("message", {data: getTime()});
    setTimeout(loop, 1000);
}

loop();
