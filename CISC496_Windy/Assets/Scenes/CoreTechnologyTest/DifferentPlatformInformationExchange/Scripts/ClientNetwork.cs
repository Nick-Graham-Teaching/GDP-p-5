using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class ClientNetwork : MonoBehaviour
{
    float speed = 4.0f;
    const int _serverPort = 51343;  // port used by server

    const int _maxConnections = 5;

    string _serverHostIP = "192.168.2.15";
    //string _serverHostIP = "127.0.0.1";

    bool _isConnected = true;


    // To send messages, we will need a host id (this client), a connection id (reference to server),
    // and a channel id (socket connection itself.) These are all set when this client is initialized.
    int _hostId;
    int _connectionId;

    /// <summary>
    /// We have two types of channel - control and movement info
    /// </summary>
    int _controlChannelId;  // reliable channel for control messages
    int _dataChannelId;  // unreliable channel for movement info

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
        if (error != (byte)NetworkError.Ok)
        {
            Logger.LogFormat("Network error: {0}", Messages.NetworkErrorToString(error));
        }

        Logger.LogFormat("Establishing network connection with hostId {0}; connectionId {1}",
            _hostId, _connectionId);

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


        if (_isConnected)
        {

            messagesAvailable = true;

            while (messagesAvailable)
            {
                NetworkEventType recData = NetworkTransport.Receive(
                    out recHostId, out connectionId, out channelId,
                    recBuffer, bufferSize, out dataSize, out error);
                Messages.LogNetworkError(error);

                switch (recData)
                {
                    case NetworkEventType.Nothing:
                        // We have processed the last pending message
                        messagesAvailable = false;
                        break;
                    case NetworkEventType.ConnectEvent:
                        Logger.LogFormat("Connection received from host {0}, connectionId {1}, channel {2}",
                            recHostId, connectionId, channelId);
                        Messages.LogConnectionInfo(_hostId, connectionId);
                        break;
                    case NetworkEventType.DisconnectEvent:
                        Logger.LogFormat("Disconnection received from host {0}, connectionId {1}, channel {2}",
                            recHostId, connectionId, channelId);
                        _isConnected = false;
                        break;
                }
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

    public void GoForward() 
    {
        SendMessageToServer(Messages.CreateKeyMessage(Vector3.forward.x, Vector3.forward.y, Vector3.forward.z));
    }
    public void GoBackward()
    {
        SendMessageToServer(Messages.CreateKeyMessage(Vector3.back.x, Vector3.back.y, Vector3.back.z));
    }
    public void GoLeft()
    {
        SendMessageToServer(Messages.CreateKeyMessage(Vector3.left.x, Vector3.left.y, Vector3.left.z));
    }
    public void GoRight()
    {
        SendMessageToServer(Messages.CreateKeyMessage(Vector3.right.x, Vector3.right.y, Vector3.right.z));
    }


    // Start is called before the first frame update
    void Start()
    {
        Application.runInBackground = true;

        InitNetwork();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessIncomingMessages();
    }
}
