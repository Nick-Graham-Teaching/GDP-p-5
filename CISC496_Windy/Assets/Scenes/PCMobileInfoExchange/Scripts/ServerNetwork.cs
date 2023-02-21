using UnityEngine;
using UnityEngine.Networking;
using System;
using UnityEngine.UI;


static class Messages {

    public static readonly byte MOVEMENT = (byte)1;
    public static readonly byte ROTATIONX = (byte)2;
    public static readonly byte ROTATIONY = (byte)3;

	static byte[] allMessageTypes = { MOVEMENT, ROTATIONX, ROTATIONY };

    public static int maxMessageSize = 128; // maximum size of a message in bytes

    #region config
    public static string NetworkErrorToString(byte error)
	{
		switch (error)
		{
			case (byte)NetworkError.BadMessage:
				return "BadMessage";
			case (byte)NetworkError.CRCMismatch:
				return "CRCMismatch";
			case (byte)NetworkError.DNSFailure:
				return "DNSFailure";
			case (byte)NetworkError.MessageToLong:
				return "MessageToLong (sic)";
			case (byte)NetworkError.NoResources:
				return "NoResources";
			case (byte)NetworkError.Timeout:
				return "TimeOut";
			case (byte)NetworkError.VersionMismatch:
				return "VersionMismatch";
			case (byte)NetworkError.WrongChannel:
				return "WrongChannel";
			case (byte)NetworkError.WrongConnection:
				return "WrongConnection";
			case (byte)NetworkError.WrongHost:
				return "WrongHost";
			case (byte)NetworkError.WrongOperation:
				return "WrongOperation";
			case (byte)NetworkError.Ok:
				return "Ok";
		}
		return "Unknown network errror";
	}

	public static void LogNetworkError(byte error)
	{
		if (error != (byte)NetworkError.Ok)
		{
			Logger.LogFormat("Got network error: {0}", NetworkErrorToString(error));
		}
	}

	public static void LogConnectionInfo(int hostId, int connectionId)
	{
		string address;
		int port;
		UnityEngine.Networking.Types.NetworkID network;
		UnityEngine.Networking.Types.NodeID dstNode;
		byte error;

		NetworkTransport.GetConnectionInfo(
			hostId, connectionId, out address, out port, out network,
			out dstNode, out error);

        Logger.LogFormat("Connection info: from IP {0}:{1}", address, port);
	}
    #endregion

    #region Movement Message
    public static byte[] CreateMovementMessage(float x, float y, float z)
	{
		// Format is:
		//	 - byte 0: message type
		//   - byte 1-4: x
		//   - byte 5-8: y
		//   - byte 9-12: z

		byte[] m = new byte[13];
		m[0] = MOVEMENT;
		int count = 1;
		AddFloatToByteArray(x, ref m, ref count);
		AddFloatToByteArray(y, ref m, ref count);
		AddFloatToByteArray(z, ref m, ref count);

		return m;
	}
	public static void GetMovementMessage(byte[] message, out float x, out float y, out float z)
	{
		x = ExtractFloat(message, 1);
		y = ExtractFloat(message, 5);
		z = ExtractFloat(message, 9);
	}
    #endregion

    #region Rotation Message
    public static byte[] CreateRotationMessage(byte messageType, float rotationDeg)
    {
        // Format is:
        //	 - byte 0: message type
        //   - byte 1-4: rotationDeg

        byte[] m = new byte[5];
        m[0] = messageType;
        int count = 1;
        AddFloatToByteArray(rotationDeg, ref m, ref count);

        return m;
    }
	public static float GetRotationMessage(byte[] message) {
		return ExtractFloat(message, 1);
	}
    #endregion

    #region util
    static void AddFloatToByteArray(float n, ref byte[] m, ref int count)
	{
		byte[] b = BitConverter.GetBytes(n);
		for (int i = 0; i < b.Length; i++)
		{
			m[count++] = b[i];
		}
	}
	static float ExtractFloat(byte[] message, int startPos)
	{
		float n = BitConverter.ToSingle(message, startPos);
		return n;
	}

	public static byte GetMessageType(byte[] message)
	{
		return message[0];
	}
    #endregion
}
static class Logger
{
	public static void Log(String msg)
	{
		Debug.Log(msg);
	}

	public static void LogFormat(String msg, params object[] formatters)
	{
		String formattedMsg = String.Format(msg, formatters);
		Log(formattedMsg);
	}
}

public class ServerNetwork : MonoBehaviour
{

	const int serverPort = 51343;  // port used by server
	const int maxConnections = 5;

	int hostId;
	int connectionId;     // the connection id is the client

	int controlChannelId;  // reliable channel for control messages
	int dataChannelId;  // unreliable channel for movement info

	GameObject sphe;
	GameObject cubeRotation;

	void ProcessMessage(byte[] message)
	{
		byte messageType = Messages.GetMessageType(message);
		float rotation;
		switch (messageType) {
			case (byte)1:  // Messages.MOVEMENT
				float x, y, z;
				Messages.GetMovementMessage(message, out x, out y, out z);
				sphe.transform.position = sphe.transform.position + 11.0f * new Vector3(x, y, z) * Time.deltaTime;
				break;
			case (byte)2:  // Messages.XROTATION
				rotation = Messages.GetRotationMessage(message);
				cubeRotation.transform.rotation *= Quaternion.Euler(rotation, 0, 0);
				break;
			case (byte)3:  // Messages.YROTATION
				rotation = Messages.GetRotationMessage(message);
				cubeRotation.transform.rotation *= Quaternion.Euler(0, rotation, 0);
				break;
		}

	}
	void ReceiveMessagesFromClient()
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
			Messages.LogNetworkError(error);

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

	void InitNetwork()
	{
		NetworkTransport.Init();

		ConnectionConfig config = new ConnectionConfig();
		controlChannelId = config.AddChannel(QosType.Reliable);
		dataChannelId = config.AddChannel(QosType.Unreliable);

		HostTopology topology = new HostTopology(config, maxConnections);
		hostId = NetworkTransport.AddHost(topology, serverPort);

		Logger.Log(string.Format("Server created with hostId {0}", hostId));
	}

	void Start()
    {
		Application.runInBackground = true;

		sphe = GameObject.Find("Sphere");
		cubeRotation = GameObject.Find("cubeRotation");
		InitNetwork();
	}

    void Update()
    {
		ReceiveMessagesFromClient();
	}
}
