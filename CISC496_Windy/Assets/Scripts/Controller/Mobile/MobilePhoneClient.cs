using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Windy.Controller
{
    public class MobilePhoneClient : MonoBehaviour
    { 
        const int _serverPort = 51343;

        const int _maxConnections = 5;

        [SerializeField]
        string _serverHostIP = "192.168.43.193";  
        //string _serverHostIP = "192.168.2.15";  

        bool _isConnected = false;

        int _hostId;
        int _connectionId;

        int _controlChannelId;
        int _dataChannelId;

        bool useGyro;

        void ProcessIncomingData(byte[] message)
        {
            byte messageType = Message.GetMessageType(message);

            switch (messageType)
            {
                case Message.ResetGyroAxes:
                    GyroAttitudeHandler.Instance.ResetGyroAxes();
                    break;
                case Message.Pause:
                    UI.MP_UIEvents.OnPauseActive?.Invoke();
                    break;
                case Message.Continue:
                    UI.MP_UIEvents.OnContinueActive?.Invoke();
                    break;
                case Message.UseGyro:
                    Message.GetUseGyroMessage(message, out useGyro);
                    GameObject.Find("text3").GetComponent<Text>().text = useGyro.ToString();
                    if (!useGyro)
                    {
                        SendGyroToHandPanelResetMessage();
                    }
                    break;
            }
        }

        void ProcessIncomingMessages()
        {

            byte[] recBuffer = new byte[Messages.maxMessageSize];
            int bufferSize = Messages.maxMessageSize;
            int dataSize;
            byte error;
            int connectionId;
            int channelId;
            int recHostId;
            bool messagesAvailable;


            messagesAvailable = true;

            while (messagesAvailable)
            {
                NetworkEventType recData = NetworkTransport.Receive(
                    out recHostId, out connectionId, out channelId,
                    recBuffer, bufferSize, out dataSize, out error);

                switch (recData)
                {
                    case NetworkEventType.Nothing:
                        messagesAvailable = false;
                        break;
                    case NetworkEventType.ConnectEvent:
                        _isConnected = true;
                        break;
                    case NetworkEventType.DataEvent:
                        ProcessIncomingData(recBuffer);
                        break;
                    case NetworkEventType.DisconnectEvent:
                        _isConnected = false;
                        _connectionId = NetworkTransport.Connect(_hostId, _serverHostIP, _serverPort, 0, out error);

                        break;
                }
            }
        }
        void SendMessageToServer(byte[] message)
        {
            byte error;
            NetworkTransport.Send(_hostId, _connectionId, _dataChannelId, message, message.Length, out error);
        }

        void SendImportantMessageToServer(byte[] message)
        {
            byte error;
            NetworkTransport.Send(_hostId, _connectionId, _controlChannelId, message, message.Length, out error);
        }

        private void SendGyroToHandPanelResetMessage()
        {
            SendMessageToServer(Message.CreateFlyUpMessage(KEYSTAT.IDLE, 0.0f));
            SendMessageToServer(Message.CreateFlyDownMessage(KEYSTAT.IDLE, 0.0f));
            SendMessageToServer(Message.CreateFlyLeftMessage(KEYSTAT.IDLE, 0.0f));
            SendMessageToServer(Message.CreateFlyRightMessage(KEYSTAT.IDLE, 0.0f));
        }

        private void SendGyroMessages()
        {
            // Fly X -- Up (+) And Down (-)
            if (GyroAttitudeHandler.LastCosX > 0.0f)
            {
                SendMessageToServer(Message.CreateFlyUpMessage(KEYSTAT.PRESS, GyroAttitudeHandler.LastCosX));
                SendMessageToServer(Message.CreateFlyDownMessage(KEYSTAT.IDLE, 0.0f));
            }
            else
            {
                SendMessageToServer(Message.CreateFlyUpMessage(KEYSTAT.IDLE, 0.0f));
                SendMessageToServer(Message.CreateFlyDownMessage(KEYSTAT.PRESS, Mathf.Abs(GyroAttitudeHandler.LastCosX)));
            }
            // Fly Y -- Left (+) And Right (-)
            if (GyroAttitudeHandler.LastCosY > 0.0f)
            {
                SendMessageToServer(Message.CreateFlyLeftMessage(KEYSTAT.PRESS, GyroAttitudeHandler.LastCosY));
                SendMessageToServer(Message.CreateFlyRightMessage(KEYSTAT.IDLE, 0.0f));
            }
            else
            {
                SendMessageToServer(Message.CreateFlyLeftMessage(KEYSTAT.IDLE, 0.0f));
                SendMessageToServer(Message.CreateFlyRightMessage(KEYSTAT.PRESS, Mathf.Abs(GyroAttitudeHandler.LastCosY)));
            }

            SendMessageToServer(Message.CreateGyroForwardUpMessage(GyroAttitudeHandler.NewForward, GyroAttitudeHandler.NewUp));
            //// Fly Z -- Left (+) And Right (-)
            //if (GyroAttitudeHandler.LastCosZ > 0.0f)
            //{
            //    SendMessageToServer(Message.CreateFlyLeftMessage(KEYSTAT.PRESS, GyroAttitudeHandler.LastCosZ));
            //    SendMessageToServer(Message.CreateFlyRightMessage(KEYSTAT.IDLE, 0.0f));
            //}
            //else
            //{
            //    SendMessageToServer(Message.CreateFlyLeftMessage(KEYSTAT.IDLE, 0.0f));
            //    SendMessageToServer(Message.CreateFlyRightMessage(KEYSTAT.PRESS, Mathf.Abs(GyroAttitudeHandler.LastCosZ)));
            //}
        }

        private void SendControlPanelMessages()
        {
            float degree;
            if (TouchHandler.GetUpKey(out degree))
            {
                SendMessageToServer(Message.CreateUpMessage(TouchHandler.PlayerFinger.Type, degree));
            }
            if (TouchHandler.GetDownKey(out degree))
            {
                SendMessageToServer(Message.CreateDownMessage(TouchHandler.PlayerFinger.Type, degree));
            }
            if (TouchHandler.GetLeftKey(out degree))
            {
                SendMessageToServer(Message.CreateLeftMessage(TouchHandler.PlayerFinger.Type, degree));
            }
            if (TouchHandler.GetRightKey(out degree))
            {
                SendMessageToServer(Message.CreateRightMessage(TouchHandler.PlayerFinger.Type, degree));
            }
        }

        private void SendCameraMessages()
        {
            if (TouchHandler.CameraFinger.Type != FingerType.Available)
            {
                SendMessageToServer(Message.CreateCameraRotationXMessage(TouchHandler.GetCameraAxisX()));
                SendMessageToServer(Message.CreateCameraRotationYMessage(TouchHandler.GetCameraAxisY()));
            }
        }

        private void Update()
        {
            ProcessIncomingMessages();
            if (_isConnected)
            {
                SendCameraMessages();
                if (!useGyro) SendControlPanelMessages();
                else SendGyroMessages();
            }
                
        }

        private void Start()
        {
            NetworkTransport.Init();

            ConnectionConfig config = new ConnectionConfig();
            _controlChannelId = config.AddChannel(QosType.Reliable);
            _dataChannelId = config.AddChannel(QosType.Unreliable);

            HostTopology topology = new HostTopology(config, _maxConnections);
            _hostId = NetworkTransport.AddHost(topology);

            byte error;
            _connectionId = NetworkTransport.Connect(_hostId, _serverHostIP, _serverPort, 0, out error);
        }


        public void OnJumpPress()
        {
            SendMessageToServer(Message.CreateJumpMessage(KEYSTAT.PRESS));
        }
        public void OnJumpPressEnd()
        {
            SendMessageToServer(Message.CreateJumpMessage(KEYSTAT.IDLE));
        }
        public void OnJumpClick()
        {
            SendMessageToServer(Message.CreateJumpMessage(KEYSTAT.TAP));
        }
        public void OnJumpClickEnd()
        {
            SendMessageToServer(Message.CreateJumpMessage(KEYSTAT.IDLE));
        }

        public void OnSwitchModePointerDown()
        {
            SendMessageToServer(Message.CreateSwitchModeMessage(KEYSTAT.DOWN));
        }
        public void OnSwitchModeClick()
        {
            SendMessageToServer(Message.CreateSwitchModeMessage(KEYSTAT.TAP));
        }
        public void OnSwitchModeClickEnd()
        {
            SendMessageToServer(Message.CreateSwitchModeMessage(KEYSTAT.IDLE));
        }

        public void OnPausePointerDown()
        {
            SendMessageToServer(Message.CreatePauseMessage(KEYSTAT.DOWN));
        }
        public void OnPauseClick()
        {
            SendMessageToServer(Message.CreatePauseMessage(KEYSTAT.TAP));
        }
        public void OnPauseClickEnd()
        {
            SendMessageToServer(Message.CreatePauseMessage(KEYSTAT.IDLE));
        }

        public void OnContinuePointerDown()
        {
            SendMessageToServer(Message.CreateContinueMessage(KEYSTAT.DOWN));
        }
    }
}
