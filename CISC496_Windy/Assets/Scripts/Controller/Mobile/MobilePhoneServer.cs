using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Windy.Controller
{
    public class MobilePhoneServer : IController
    {
        Dictionary<KeyCode, Key> keyDic;

        float CameraRotationX;
        float CameraRotationY;

        const int serverPort = 51343;  // port used by server
        const int maxConnections = 5;

        HostTopology topology;

        int hostId;
        int connectionId;     // the connection id is the client

        int controlChannelId;  // reliable channel for control messages
        int dataChannelId;  // unreliable channel for movement info

        public MobilePhoneServer()
        {
            keyDic = new();

            foreach (KeyCode key in Keys.PhoneKeys)
            {
                keyDic.Add(key, new Key());
            }
        }

        public virtual void InitNetwork()
        {
            NetworkTransport.Init();

            ConnectionConfig config = new ConnectionConfig();
            controlChannelId = config.AddChannel(QosType.Reliable);
            dataChannelId = config.AddChannel(QosType.Unreliable);

            topology = new HostTopology(config, maxConnections);
            hostId = NetworkTransport.AddHost(topology, serverPort);
        }

        void ProcessMessage(byte[] message)
        {
            byte messageType = Messages.GetMessageType(message);

            FingerType type;
            float degree;

            switch (messageType)
            {
                case 1:  // Camera Rotation In X Aixs
                    Message.GetCameraRotationXMessage(message, out CameraRotationX);
                    break;
                case 2:  // Camera Rotation In Y Aixs
                    Message.GetCameraRotationYMessage(message, out CameraRotationY);
                    break;
                case 3:  // Up Key
                    Message.GetUpMessage(message, out type, out degree);
                    if (type == FingerType.Press)
                    {
                        keyDic[Keys.UpCode].Value = KEYSTAT.PRESS;
                        keyDic[Keys.UpCode].degree = degree;
                    }
                    else
                    {
                        keyDic[Keys.UpCode].Value = KEYSTAT.IDLE;
                        keyDic[Keys.UpCode].degree = degree;
                    }
                    break;
                case 4:  // Down Key
                    Message.GetDownMessage(message, out type, out degree);
                    if (type == FingerType.Press)
                    {
                        keyDic[Keys.DownCode].Value = KEYSTAT.PRESS;
                        keyDic[Keys.DownCode].degree = degree;
                    }
                    else
                    {
                        keyDic[Keys.DownCode].Value = KEYSTAT.IDLE;
                        keyDic[Keys.DownCode].degree = degree;
                    }
                    break;
                case 5:  // Left Key
                    Message.GetLeftMessage(message, out type, out degree);
                    if (type == FingerType.Press)
                    {
                        keyDic[Keys.LeftCode].Value = KEYSTAT.PRESS;
                        keyDic[Keys.LeftCode].degree = degree;
                    }
                    else
                    {
                        keyDic[Keys.LeftCode].Value = KEYSTAT.IDLE;
                        keyDic[Keys.LeftCode].degree = degree;
                    }
                    break;
                case 6:  // Right Key
                    Message.GetRightMessage(message, out type, out degree);
                    if (type == FingerType.Press)
                    {
                        keyDic[Keys.RightCode].Value = KEYSTAT.PRESS;
                        keyDic[Keys.RightCode].degree = degree;
                    }
                    else
                    {
                        keyDic[Keys.RightCode].Value = KEYSTAT.IDLE;
                        keyDic[Keys.RightCode].degree = degree;
                    }
                    break;
            }

        }

        public void Update()
        {
            byte[] recBuffer = new byte[1024];
            int bufferSize = 1024;
            int dataSize;
            byte error;
            int connectionId;
            int channelId;
            int recHostId;
            bool messagesAvailable = true;

            while (messagesAvailable)
            {
                NetworkEventType recData = NetworkTransport.Receive(
                    out recHostId, out connectionId, out channelId, recBuffer, bufferSize, out dataSize, out error);

                switch (recData)
                {
                    case NetworkEventType.Nothing:
                        messagesAvailable = false;
                        break;
                    case NetworkEventType.ConnectEvent:
                        this.connectionId = connectionId;
                        break;
                    case NetworkEventType.DataEvent:
                        Logger.LogFormat("Message received from host {0}, connection {1}, channel {2}", recHostId, connectionId, channelId);
                        ProcessMessage(recBuffer);
                        break;
                    case NetworkEventType.DisconnectEvent:
                        Logger.Log(string.Format("Disconnection received from host {0}, connection {1}, channel {2}", recHostId, connectionId, channelId));
                        break;
                }
            }
        }

        public void Start()
        {
        }

        public void Quit()
        {

        }

        public float GetCameraMoveAxisX() => CameraRotationX;

        public float GetCameraMoveAxisY() => CameraRotationY;

        public bool GetKeyPress(KeyCode key, out float degree)
        {
            degree = keyDic[key].degree;
            return keyDic[key].Value == KEYSTAT.PRESS;
        }

        public bool GetKeyTap(KeyCode key, out float degree)
        {
            throw new System.NotImplementedException();
        }
    }
}

