### Socket IO client in unity
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

namespace module.network {
    public class NetworkManager : SocketIOComponent {

        private int time;
        private int lastTime;

        private int getTime() {
            return (int)(Time.time * 1000.0f);
        }

        public override void Awake() {
            base.url = "ws://127.0.0.1:80/socket.io/?EIO=4&transport=websocket";
            base.Awake();
        }


        public override void Start() {
            base.Start();

            time = getTime();
            lastTime = getTime();

            On("open", (E) => {
                Debug.Log("Connection made to the server");
            });

            On("message", (E) => {
                Debug.Log("[message from server] " + E.data["data"]);
            });
        }

        public override void Update() {
            base.Update();

            time = getTime();
            if (time - lastTime > 1000) {
                JSONObject data = new JSONObject();
                data.AddField("data", time);
                Emit("message", data);
                lastTime = time;
            }
        }
    }
}
```
### Socket IO server in NodeJS
```js
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
```
