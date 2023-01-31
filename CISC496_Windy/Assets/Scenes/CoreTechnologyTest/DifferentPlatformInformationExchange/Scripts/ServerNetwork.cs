using UnityEngine;
using UnityEngine.Networking;
using System;


class Messages {

    public static readonly byte KEY = (byte)1;

    static byte[] allMessageTypes = { KEY };

    public static int maxMessageSize = 128; // maximum size of a message in bytes

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

	static void AddShortToByteArray(Int16 n, ref byte[] m, ref int count)
	{
		byte[] b = BitConverter.GetBytes(n);
		for (int i = 0; i < b.Length; i++)
		{
			m[count++] = b[i];
		}
	}

	public static byte[] CreateKeyMessage(float x, float y, float z)
	{
		// Format is:
		//	 - byte 0: message type
		//   - byte 1-2: x
		//   - byte 3-4: y
		//   - byte 5-6: z

		byte[] m = new byte[7];
		m[0] = Messages.KEY;
		int count = 1;
		AddShortToByteArray((short)(x), ref m, ref count);
		AddShortToByteArray((short)(y), ref m, ref count);
		AddShortToByteArray((short)(z), ref m, ref count);

		return m;
	}
	static float ExtractFloat(byte[] message, int startPos)
	{
		Int16 n = BitConverter.ToInt16(message, startPos);
		return n;
	}

	public static void GetKeyMessage(byte[] message, out float x, out float y, out float z)
	{
		x = ExtractFloat(message, 1);
		y = ExtractFloat(message, 3);
		z = ExtractFloat(message, 5);
	}

	public static byte GetMessageType(byte[] message)
	{
		return message[0];
	}
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

	void ProcessMessage(byte[] message)
	{
		// send to all clients other than originator

		// ...
		float x, y, z;
		Messages.GetKeyMessage(message, out x,out y, out z);
		sphe.transform.position = sphe.transform.position + 4.0f * new Vector3(x,y,z) * Time.deltaTime;

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
					// AddClient
					this.connectionId = connectionId;
					break;
				case NetworkEventType.DataEvent:
					Logger.LogFormat("Message received from host {0}, connection {1}, channel {2}", recHostId, connectionId, channelId);
					// Inform Handler To Process Information
					ProcessMessage(recBuffer);
					break;
				case NetworkEventType.DisconnectEvent:
					Logger.Log(string.Format("Disconnection received from host {0}, connection {1}, channel {2}", recHostId, connectionId, channelId));
					// removeClient
					break;
			}
		}
	}

	void InitNetwork()
	{
		// Establish connection to server and get client id.
		NetworkTransport.Init();

		// Set up channels for control messages (reliable) and movement messages (unreliable)
		ConnectionConfig config = new ConnectionConfig();
		controlChannelId = config.AddChannel(QosType.Reliable);
		dataChannelId = config.AddChannel(QosType.Unreliable);

		// Create socket end-point
		HostTopology topology = new HostTopology(config, maxConnections);
		hostId = NetworkTransport.AddHost(topology, serverPort);

		Logger.Log(string.Format("Server created with hostId {0}", hostId));
	}

	// Start is called before the first frame update
	void Start()
    {
		Application.runInBackground = true;

		sphe = GameObject.Find("Sphere");
		InitNetwork();
	}

    // Update is called once per frame
    void Update()
    {
		ReceiveMessagesFromClient();
	}
}
