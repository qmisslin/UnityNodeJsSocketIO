# Welcome

Here is a minimal code to create a NodeJS server with a Unity client using Socket.io.

## Create the NodeJS server
To create the server, you need NodeJS installed on your computer. Then create a new project with `npm init`.
Then you need some packages for your :
```bash
npm i -g nodemon
```

Next, create an input file named "server.js", then copy and paste the minimal code below. Finally, start the server with :
```bash
`nodemon server.js`
```

## Create the Unity client

To create the Unity client, you need to download the free "SocketIO" package from the asset store. Then, to avoid all warnings, you have to update the JSONObject folder (which is located in SocketIO) with the one downloadable here :

[https://github.com/mtschoen/JSONObject](https://github.com/mtschoen/JSONObject)


After that, you may still have two more warnings due to the declaration without using two variables in SocketIO/Scripts/SocketIOComponent.cs You can disable these errors by writing bellow code before and after the relevant lines.
```c#
#pragma warning disable 0168
// Code with unused variable
#pragma warning restore 0168
```

Then, still in SocketIOComponent.cs, we need to change the `Awake`, `Start` and `Update` methods to `virtual` methods in order to be able to override these methods in our client script.

For example:
```c#
public void Start
```
becomes:
```c#
public virtual void Start
```

That's it! Then you just have to create a "NetworkManager.cs" script, copy and paste the code below, and drag and drop this file on an object in your scene.

## Socket IO server in NodeJS

Here is a minimal code for the server side of the application.

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

## Socket IO client in Unity

Here is a minimal code for the client side of the application.

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
