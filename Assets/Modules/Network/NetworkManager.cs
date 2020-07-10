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

