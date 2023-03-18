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

            foreach (KeyCode key in Keys.keys)
            {
                keyDic.Add(key, new Key());
            }
        }

        public virtual void ResetGyroAxes()
        {
            byte error;
            NetworkTransport.Send(hostId, connectionId, dataChannelId, Message.CreateResetGyroAxesMessage(), 1, out error);
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
            KEYSTAT keystatus;

            switch (messageType)
            {
                case Message.CameraRotationX:  // Camera Rotation In X Aixs
                    Message.GetCameraRotationXMessage(message, out CameraRotationX);
                    break;
                case Message.CameraRotationY:  // Camera Rotation In Y Aixs
                    Message.GetCameraRotationYMessage(message, out CameraRotationY);
                    break;
                case Message.Up:  // Up Key
                    Message.GetUpMessage(message, out type, out degree);
                    UpdateKeyStatus(Keys.UpCode, type,  degree);
                    break;
                case Message.Down:  // Down Key
                    Message.GetDownMessage(message, out type, out degree);
                    UpdateKeyStatus(Keys.DownCode, type, degree);
                    break;
                case Message.Left:  // Left Key
                    Message.GetLeftMessage(message, out type, out degree);
                    UpdateKeyStatus(Keys.LeftCode, type, degree);
                    break;
                case Message.Right:  // Right Key
                    Message.GetRightMessage(message, out type, out degree);
                    UpdateKeyStatus(Keys.RightCode, type, degree);
                    break;
                case Message.SwitchMode:  // Switch Mode
                    Message.GetSwitchModeMessage(message, out keystatus);
                    keyDic[Keys.ModeSwitchCode].Value = keystatus;
                    Controller.Instance.StartCoroutine(BackToIdle(Keys.ModeSwitchCode));
                    break;
                case Message.Jump:  // Jump Key
                    Message.GetJumpMessage(message, out keystatus);
                    keyDic[Keys.JumpCode].Value = keystatus;
                    if (keystatus == KEYSTAT.TAP)
                    {
                        Controller.Instance.StartCoroutine(BackToIdle(Keys.JumpCode));
                    }
                    break;
                case Message.Pause:  // Pause Key
                    Message.GetPauseMessage(message , out keystatus);
                    keyDic[Keys.PauseCode].Value = keystatus;
                    Controller.Instance.StartCoroutine(BackToIdle(Keys.PauseCode));
                    break;
                case Message.Continue:
                    Message.GetContinueMessage(message, out keystatus);
                    keyDic[Keys.ContinueCode].Value = keystatus;
                    Controller.Instance.StartCoroutine(BackToIdle(Keys.ContinueCode));
                    break;
                case Message.Fly_Up:
                    Message.GetFlyUpMessage(message, out keystatus, out degree);
                    keyDic[Keys.UpCode].Value = keystatus;
                    keyDic[Keys.UpCode].degree = degree;
                    break;
                case Message.Fly_Down:
                    Message.GetFlyDownMessage(message, out keystatus, out degree);
                    keyDic[Keys.DownCode].Value = keystatus;
                    keyDic[Keys.DownCode].degree = degree;
                    break;
                case Message.Fly_Left:
                    Message.GetFlyLeftMessage(message, out keystatus, out degree);
                    keyDic[Keys.LeftCode].Value = keystatus;
                    keyDic[Keys.LeftCode].degree = degree;
                    break;
                case Message.Fly_Right:
                    Message.GetFlyRightMessage(message, out keystatus, out degree);
                    keyDic[Keys.RightCode].Value = keystatus;
                    keyDic[Keys.RightCode].degree = degree;
                    break;
            }
        }

        void UpdateKeyStatus(KeyCode key, FingerType type, float degree)
        {
            if (type == FingerType.Press)
            {
                keyDic[key].Value = KEYSTAT.PRESS;
                keyDic[key].degree = degree;
            }
            else if (type == FingerType.Tap)
            {
                keyDic[key].Value = KEYSTAT.TAP;
                keyDic[key].degree = degree;
            }
            else
            {
                keyDic[key].Value = KEYSTAT.IDLE;
                keyDic[key].degree = degree;
            }
        }
        IEnumerator BackToIdle(KeyCode key)
        {
            yield return null;
            keyDic[key].Value = KEYSTAT.WAIT;
            yield return null;
            keyDic[key].Value = KEYSTAT.UP;
            yield return null;
            keyDic[key].Value = KEYSTAT.IDLE;
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
                        Logger.LogFormat("Connection received from host {0}, connection {1}, channel {2}", recHostId, connectionId, channelId);
                        this.connectionId = connectionId;
                        break;
                    case NetworkEventType.DataEvent:
                        Logger.LogFormat("Message received from host {0}, connection {1}, channel {2}", recHostId, connectionId, channelId);
                        ProcessMessage(recBuffer);
                        break;
                    case NetworkEventType.DisconnectEvent:
                        Logger.LogFormat("Disconnection received from host {0}, connection {1}, channel {2}", recHostId, connectionId, channelId);
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
            degree = keyDic[key].degree;
            return keyDic[key].Value == KEYSTAT.TAP;
        }

        public bool GetKeyDown(KeyCode key, out float degree)
        {
            degree = keyDic[key].degree;
            return keyDic[key].Value == KEYSTAT.DOWN;
        }

        public bool GetKeyWait(KeyCode key, out float degree)
        {
            degree = keyDic[key].degree;
            return keyDic[key].Value == KEYSTAT.WAIT;
        }

        public bool GetKeyUp(KeyCode key, out float degree)
        {
            degree = keyDic[key].degree;
            return keyDic[key].Value == KEYSTAT.UP;
        }

        public bool GetKeyIdle(KeyCode key, out float degree)
        {
            degree = keyDic[key].degree;
            return keyDic[key].Value == KEYSTAT.IDLE;
        }
    }
}

