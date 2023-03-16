using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.Controller
{
    public static class Message
    {
        public static readonly byte CAMERA_ROTATION_X = (byte)1;
        public static readonly byte CAMERA_ROTATION_Y = (byte)2;

        public static readonly byte UP    = (byte)3;
        public static readonly byte DOWN  = (byte)4;
        public static readonly byte LEFT  = (byte)5;
        public static readonly byte RIGHT = (byte)6;

        public static readonly byte JUMP = (byte)7;

        public static readonly byte Pause = (byte)8;

        public static int maxMessageSize = 128; // maximum size of a message in bytes

		#region Camera Rotation X
		public static byte[] CreateCameraRotationXMessage(float value)
		{
			// Format is:
			//	 - byte 0: message type
			//   - byte 1-4: value

			byte[] m = new byte[5];
			m[0] = CAMERA_ROTATION_X;
			int count = 1;
			AddFloatToByteArray(value, ref m, ref count);

			return m;
		}
		public static void GetCameraRotationXMessage(byte[] message, out float value)
		{
			value = ExtractFloat(message, 1);
		}
		#endregion

		#region Camera Rotation Y
		public static byte[] CreateCameraRotationYMessage(float value)
		{
			// Format is:
			//	 - byte 0: message type
			//   - byte 1-4: value

			byte[] m = new byte[5];
			m[0] = CAMERA_ROTATION_Y;
			int count = 1;
			AddFloatToByteArray(value, ref m, ref count);

			return m;
		}
		public static void GetCameraRotationYMessage(byte[] message, out float value)
		{
			value = ExtractFloat(message, 1);
		}
        #endregion

        #region Up Key    
		public static byte[] CreateUpMessage(FingerType type, float degree)
		{
			// Format is:
			//	 - byte 0: message type
			//   - byte 1-4: Type
			//   - byte 5-8: degree

			byte[] m = new byte[9];
			m[0] = UP;
			int count = 1;
			AddFloatToByteArray((float)type, ref m, ref count);
			AddFloatToByteArray(degree, ref m, ref count);

			return m;
		}
		public static void GetUpMessage(byte[] message, out FingerType type, out float degree)
		{
			type = (FingerType)ExtractFloat(message, 1);
			degree = ExtractFloat(message, 5);
		}
		#endregion

		#region Down Key    
		public static byte[] CreateDownMessage(FingerType type, float degree)
		{
			// Format is:
			//	 - byte 0: message type
			//   - byte 1-4: Type
			//   - byte 5-8: degree

			byte[] m = new byte[9];
			m[0] = DOWN;
			int count = 1;
			AddFloatToByteArray((float)type, ref m, ref count);
			AddFloatToByteArray(degree, ref m, ref count);

			return m;
		}
		public static void GetDownMessage(byte[] message, out FingerType type, out float degree)
		{
			type = (FingerType)ExtractFloat(message, 1);
			degree = ExtractFloat(message, 5);
		}
		#endregion

		#region Left Key    
		public static byte[] CreateLeftMessage(FingerType type, float degree)
		{
			// Format is:
			//	 - byte 0: message type
			//   - byte 1-4: Type
			//   - byte 5-8: degree

			byte[] m = new byte[9];
			m[0] = LEFT;
			int count = 1;
			AddFloatToByteArray((float)type, ref m, ref count);
			AddFloatToByteArray(degree, ref m, ref count);

			return m;
		}
		public static void GetLeftMessage(byte[] message, out FingerType type, out float degree)
		{
			type = (FingerType)ExtractFloat(message, 1);
			degree = ExtractFloat(message, 5);
		}
		#endregion

		#region Right Key    
		public static byte[] CreateRightMessage(FingerType type, float degree)
		{
			// Format is:
			//	 - byte 0: message type
			//   - byte 1-4: Type
			//   - byte 5-8: degree

			byte[] m = new byte[9];
			m[0] = RIGHT;
			int count = 1;
			AddFloatToByteArray((float)type, ref m, ref count);
			AddFloatToByteArray(degree, ref m, ref count);

			return m;
		}
		public static void GetRightMessage(byte[] message, out FingerType type, out float degree)
		{
			type = (FingerType)ExtractFloat(message, 1);
			degree = ExtractFloat(message, 5);
		}
		#endregion

		#region Jump Key    
		public static byte[] CreateJumpMessage(KEYSTAT keyStatus)
		{
			// Format is:
			//	 - byte 0: message type
			//   - byte 1-4: Key Status

			byte[] m = new byte[5];
			m[0] = JUMP;
			int count = 1;
			AddFloatToByteArray((float)keyStatus, ref m, ref count);

			return m;
		}
		public static void GetJumpMessage(byte[] message, out KEYSTAT keyStatus)
		{
			keyStatus = (KEYSTAT)ExtractFloat(message, 1);
		}
		#endregion

		#region Pause Key    
		public static byte[] CreatePauseMessage(KEYSTAT keyStatus)
		{
			// Format is:
			//	 - byte 0: message type
			//   - byte 1-4: Key Status

			byte[] m = new byte[5];
			m[0] = Pause;
			int count = 1;
			AddFloatToByteArray((float)keyStatus, ref m, ref count);

			return m;
		}
		public static void GetPauseMessage(byte[] message, out KEYSTAT keyStatus)
		{
			keyStatus = (KEYSTAT)ExtractFloat(message, 1);
		}
		#endregion

		#region Util
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
}

