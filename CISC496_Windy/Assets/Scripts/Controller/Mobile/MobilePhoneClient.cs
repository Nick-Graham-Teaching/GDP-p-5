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

        void SendMessages()
        {
            float temp;
            if (TouchHandler.CameraFinger.Type != FingerType.Available)
            {
                SendMessageToServer(Message.CreateCameraRotationXMessage(TouchHandler.GetCameraAxisX()));
                SendMessageToServer(Message.CreateCameraRotationYMessage(TouchHandler.GetCameraAxisY()));
            }
            if (TouchHandler.GetUpKey(out temp))
            {
                SendMessageToServer(Message.CreateUpMessage(TouchHandler.PlayerFinger.Type, temp));
            }
            if (TouchHandler.GetDownKey(out temp))
            {
                SendMessageToServer(Message.CreateDownMessage(TouchHandler.PlayerFinger.Type, temp));
            }
            if (TouchHandler.GetLeftKey(out temp))
            {
                SendMessageToServer(Message.CreateLeftMessage(TouchHandler.PlayerFinger.Type, temp));
            }
            if (TouchHandler.GetRightKey(out temp))
            {
                SendMessageToServer(Message.CreateRightMessage(TouchHandler.PlayerFinger.Type, temp));
            }
        }

        private void Update()
        {
            ProcessIncomingMessages();
            if (_isConnected) SendMessages();
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
