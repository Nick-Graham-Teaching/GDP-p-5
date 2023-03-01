using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class ClientNetwork : MonoBehaviour
{
    const int _serverPort = 51343;

    const int _maxConnections = 5;

    [SerializeField]
    string _serverHostIP = "192.168.43.193";  // hotspot
    //string _serverHostIP = "192.168.2.15";  // LAN

    bool _isConnected = true;

    int _hostId;
    int _connectionId;

    int _controlChannelId;
    int _dataChannelId;


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
        SendMessageToServer(Messages.CreateMovementMessage(Vector3.forward.x, Vector3.forward.y, Vector3.forward.z));
    }
    public void GoBackward()
    {
        SendMessageToServer(Messages.CreateMovementMessage(Vector3.back.x, Vector3.back.y, Vector3.back.z));
    }
    public void GoLeft()
    {
        SendMessageToServer(Messages.CreateMovementMessage(Vector3.left.x, Vector3.left.y, Vector3.left.z));
    }
    public void GoRight()
    {
        SendMessageToServer(Messages.CreateMovementMessage(Vector3.right.x, Vector3.right.y, Vector3.right.z));
    }

    IEnumerator SendGyroRotation() { 
        while (true)
        {
            if (_isConnected && GyroRotationDetector.isRotateX())
            {
                SendMessageToServer(Messages.CreateRotationMessage(Messages.ROTATIONX, GyroRotationDetector.rotationX()));
            }
            if (_isConnected && GyroRotationDetector.isRotateY())
            {
                SendMessageToServer(Messages.CreateRotationMessage(Messages.ROTATIONY, GyroRotationDetector.rotationY()));
            }
            yield return new WaitForSeconds(0f);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Application.runInBackground = true;

        InitNetwork();
        StartCoroutine("SendGyroRotation");
    }

    // Update is called once per frame
    void Update()
    {
        ProcessIncomingMessages();
    }



}
