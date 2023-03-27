using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

namespace Windy.Controller
{
    public class MobilePhoneClient : MonoBehaviour
    {
        #region Network
        const int _serverPort = 51343;

        const int _maxConnections = 5;

        string _serverHostIP;  
        //string _serverHostIP = "192.168.43.193";  
        //string _serverHostIP = "192.168.2.15";  

        bool _isConnected = false;

        int _hostId;
        int _connectionId;

        int _controlChannelId;
        int _dataChannelId;
        #endregion

        #region UDP Broadcast Network
        UdpClient _client;
        IPEndPoint _endpoint;
        const int _broadcastPort = 51340;

        bool startInternet = false;

        ThreadStart _broadcastThreadDelegate;
        Thread _broadcastThread;
        #endregion

        bool useGyro;
        bool isFlying;

        #region Process Incoming Messages
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
                    if (!useGyro)
                    {
                        SendGyroToHandPanelResetMessage();
                    }
                    break;
                case Message.ToFlyingMode:
                    if (!isFlying)
                    {
                        isFlying = true;
                        UI.MP_UIEvents.OnToInAirMode.Invoke();
                    }
                    break;
                case Message.ToWalkMode:
                    if (isFlying)
                    {
                        isFlying = false;
                        UI.MP_UIEvents.OnBackToGroundMode?.Invoke();
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
        #endregion

        #region Send Messages
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
            SendMessageToServer(Message.CreateCameraRotationXMessage(TouchHandler.GetCameraAxisX()));
            SendMessageToServer(Message.CreateCameraRotationYMessage(TouchHandler.GetCameraAxisY()));
        }
        #endregion

        #region Initialize Network
        void InitNetwork()
        {
            NetworkTransport.Init();

            ConnectionConfig config = new ConnectionConfig();
            _controlChannelId = config.AddChannel(QosType.Reliable);
            _dataChannelId = config.AddChannel(QosType.Unreliable);

            HostTopology topology = new HostTopology(config, _maxConnections);
            _hostId = NetworkTransport.AddHost(topology);

            _connectionId = NetworkTransport.Connect(_hostId, _serverHostIP, _serverPort, 0, out byte error);

            startInternet = true;
        }
        void InitBroadcastNetwork()
        {
            _client = new UdpClient(new IPEndPoint(IPAddress.Any, _broadcastPort));
            _endpoint = new IPEndPoint(IPAddress.Any, 0);

            _broadcastThreadDelegate = new ThreadStart(RecevieBoardcastMessage);
            _broadcastThread = new Thread(_broadcastThreadDelegate);

            _broadcastThread.Start();
        }
        void RecevieBoardcastMessage()
        {
            byte[] message = _client.Receive(ref _endpoint);
            string ip = Encoding.UTF8.GetString(message);
            _serverHostIP = ip;

            _client.Close();

            InitNetwork();
        }
        #endregion

        private void Update()
        {
            if (startInternet)
            {
                ProcessIncomingMessages();
                if (_isConnected)
                {
                    SendCameraMessages();
                    if (!useGyro) SendControlPanelMessages();
                    else SendGyroMessages();
                }
            }
        }

        private void Start()
        {
            InitBroadcastNetwork();
        }

        private void OnApplicationQuit()
        {
            NetworkTransport.Disconnect(_hostId, _connectionId, out byte error);
            NetworkTransport.RemoveHost(_hostId);
            NetworkTransport.Shutdown();
        }

        #region callback functions
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
        #endregion
    }
}
