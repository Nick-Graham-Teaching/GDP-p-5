using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class ClientNetwork : MonoBehaviour
{
    const int _serverPort = 51343;

    const int _maxConnections = 5;

    string _serverHostIP;

    bool _isConnected = false;

    int _hostId;
    int _connectionId;

    int _controlChannelId;
    int _dataChannelId;

    UdpClient _client;
    IPEndPoint _endpoint;
    const int _broadcastPort = 51340;
    public UnityEngine.UI.Text message;

    bool initInternet = false;
    bool startInternet = false;

    ThreadStart _broadcastThreadDelegate;
    Thread _broadcastThread;

    void InitNetwork()
    {
        // Establish connection to server and get client id.
        NetworkTransport.Init();

        // Set up channels for control messages (reliable) and movement messages (unreliable)
        ConnectionConfig config = new ConnectionConfig();
        _controlChannelId = config.AddChannel(QosType.Reliable);
        _dataChannelId = config.AddChannel(QosType.Unreliable);

        // Create socket end-point
        HostTopology topology = new HostTopology(config, _maxConnections);
        _hostId = NetworkTransport.AddHost(topology);

        // Establish connection to server
        byte error;
        _connectionId = NetworkTransport.Connect(_hostId, _serverHostIP, _serverPort, 0, out error);

        startInternet = true;
        initInternet = false;
        message.text = _serverHostIP;
        //if (error != (byte)NetworkError.Ok)
        //{
        //    Logger.LogFormat("Network error: {0}", Messages.NetworkErrorToString(error));
        //}

        //Logger.LogFormat("Establishing network connection with hostId {0}; connectionId {1}",
        //    _hostId, _connectionId);

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
            //Messages.LogNetworkError(error);

            switch (recData)
            {
                case NetworkEventType.Nothing:
                    messagesAvailable = false;
                    break;
                case NetworkEventType.ConnectEvent:
                    //Logger.LogFormat("Connection received from host {0}, connectionId {1}, channel {2}",
                    //    recHostId, connectionId, channelId);
                    //Messages.LogConnectionInfo(_hostId, connectionId);
                    _isConnected = true;
                    break;
                case NetworkEventType.DisconnectEvent:
                    //Logger.LogFormat("Disconnection received from host {0}, connectionId {1}, channel {2}",
                    //    recHostId, connectionId, channelId);

                    _connectionId = NetworkTransport.Connect(_hostId, _serverHostIP, _serverPort, 0, out error);

                    //if (error != (byte)NetworkError.Ok)
                    //    Logger.LogFormat("Network error: {0}", Messages.NetworkErrorToString(error));
                    //else 
                    //    Logger.LogFormat("Establishing network connection with hostId {0}; connectionId {1}", _hostId, _connectionId);

                    break;
            }
        }
    }

    void SendMessageToServer(byte[] message, int channelId)
    {
        byte error;
        NetworkTransport.Send(_hostId, _connectionId, channelId, message, message.Length, out error);
        Messages.LogNetworkError(error);
    }
    void SendMessageToServer(byte[] message)
    {
        byte error;
        NetworkTransport.Send(_hostId, _connectionId, _dataChannelId, message, message.Length, out error);
        Messages.LogNetworkError(error);
    }


    void InitBroadcastInternet()
    {
        _client = new UdpClient(new IPEndPoint(IPAddress.Any, _broadcastPort));
        _endpoint = new IPEndPoint(IPAddress.Any, 0);

        _broadcastThreadDelegate = new ThreadStart(RecevieBoardcastMessage);
        _broadcastThread = new Thread(_broadcastThreadDelegate);

        message.text = "StartReceviingBoardcastMessage";
        _broadcastThread.Start();
    }
    void RecevieBoardcastMessage()
    {
        byte[] message = _client.Receive(ref _endpoint);
        string ip = Encoding.UTF8.GetString(message);

        _serverHostIP = ip;
        initInternet = true;
        _client.Close();
    }


    void Start()
    {
        InitBroadcastInternet();
    }
    void Update()
    {
        if (initInternet) InitNetwork();
        if (startInternet) ProcessIncomingMessages();
    }


}
