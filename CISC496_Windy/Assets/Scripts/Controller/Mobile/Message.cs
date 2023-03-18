using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Windy.Controller
{
    public static class Message
    {
        public const byte CameraRotationX = (byte)1;
        public const byte CameraRotationY = (byte)2;

        public const byte Up              = (byte)3;
        public const byte Down            = (byte)4;
        public const byte Left            = (byte)5;
        public const byte Right           = (byte)6;
									      
		public const byte SwitchMode      = (byte)7;
		public const byte Jump            = (byte)8;
									      
        public const byte Pause           = (byte)9;
        public const byte Continue        = (byte)10;
									      
		public const byte Fly_Up          = (byte)11;
		public const byte Fly_Down        = (byte)12;
		public const byte Fly_Left        = (byte)13;
		public const byte Fly_Right       = (byte)14;

		public const byte ResetGyroAxes   = (byte)15;   // The message from server
									      
        public static int maxMessageSize = 128; // maximum size of a message in bytes

		#region Camera Rotation X
		public static byte[] CreateCameraRotationXMessage(float value)
		{
			// Format is:
			//	 - byte 0: message type
			//   - byte 1-4: value

			byte[] m = new byte[5];
			m[0] = CameraRotationX;
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
			m[0] = CameraRotationY;
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
			m[0] = Up;
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
			m[0] = Down;
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
			m[0] = Left;
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
			m[0] = Right;
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
			m[0] = Jump;
			int count = 1;
			AddFloatToByteArray((float)keyStatus, ref m, ref count);

			return m;
		}
		public static void GetJumpMessage(byte[] message, out KEYSTAT keyStatus)
		{
			keyStatus = (KEYSTAT)ExtractFloat(message, 1);
		}
        #endregion

		#region ModeSwitch Key    
		public static byte[] CreateSwitchModeMessage(KEYSTAT keyStatus)
		{
			// Format is:
			//	 - byte 0: message type
			//   - byte 1-4: Key Status

			byte[] m = new byte[5];
			m[0] = SwitchMode;
			int count = 1;
			AddFloatToByteArray((float)keyStatus, ref m, ref count);

			return m;
		}
		public static void GetSwitchModeMessage(byte[] message, out KEYSTAT keyStatus)
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

		#region Continue Key    
		public static byte[] CreateContinueMessage(KEYSTAT keyStatus)
		{
			// Format is:
			//	 - byte 0: message type
			//   - byte 1-4: Key Status

			byte[] m = new byte[5];
			m[0] = Continue;
			int count = 1;
			AddFloatToByteArray((float)keyStatus, ref m, ref count);

			return m;
		}
		public static void GetContinueMessage(byte[] message, out KEYSTAT keyStatus)
		{
			keyStatus = (KEYSTAT)ExtractFloat(message, 1);
		}
		#endregion

		#region Fly Up Key    
		public static byte[] CreateFlyUpMessage(KEYSTAT keyStatus, float degree)
		{
			// Format is:
			//	 - byte 0: message type
			//   - byte 1-4: Key Status
			//   - byte 5-8: Degree

			byte[] m = new byte[9];
			m[0] = Fly_Up;
			int count = 1;
			AddFloatToByteArray((float)keyStatus, ref m, ref count);
			AddFloatToByteArray(degree, ref m, ref count);

			return m;
		}
		public static void GetFlyUpMessage(byte[] message, out KEYSTAT keyStatus, out float degree)
		{
			keyStatus = (KEYSTAT)ExtractFloat(message, 1);
			degree    = ExtractFloat(message, 5);
		}
		#endregion

		#region Fly Down Key    
		public static byte[] CreateFlyDownMessage(KEYSTAT keyStatus, float degree)
		{
			// Format is:
			//	 - byte 0: message type
			//   - byte 1-4: Key Status
			//   - byte 5-8: Degree

			byte[] m = new byte[9];
			m[0] = Fly_Down;
			int count = 1;
			AddFloatToByteArray((float)keyStatus, ref m, ref count);
			AddFloatToByteArray(degree, ref m, ref count);

			return m;
		}
		public static void GetFlyDownMessage(byte[] message, out KEYSTAT keyStatus, out float degree)
		{
			keyStatus = (KEYSTAT)ExtractFloat(message, 1);
			degree = ExtractFloat(message, 5);
		}
		#endregion

		#region Fly Left Key    
		public static byte[] CreateFlyLeftMessage(KEYSTAT keyStatus, float degree)
		{
			// Format is:
			//	 - byte 0: message type
			//   - byte 1-4: Key Status
			//   - byte 5-8: Degree

			byte[] m = new byte[9];
			m[0] = Fly_Left;
			int count = 1;
			AddFloatToByteArray((float)keyStatus, ref m, ref count);
			AddFloatToByteArray(degree, ref m, ref count);

			return m;
		}
		public static void GetFlyLeftMessage(byte[] message, out KEYSTAT keyStatus, out float degree)
		{
			keyStatus = (KEYSTAT)ExtractFloat(message, 1);
			degree = ExtractFloat(message, 5);
		}
		#endregion

		#region Fly Right Key    
		public static byte[] CreateFlyRightMessage(KEYSTAT keyStatus, float degree)
		{
			// Format is:
			//	 - byte 0: message type
			//   - byte 1-4: Key Status
			//   - byte 5-8: Degree

			byte[] m = new byte[9];
			m[0] = Fly_Right;
			int count = 1;
			AddFloatToByteArray((float)keyStatus, ref m, ref count);
			AddFloatToByteArray(degree, ref m, ref count);

			return m;
		}
		public static void GetFlyRightMessage(byte[] message, out KEYSTAT keyStatus, out float degree)
		{
			keyStatus = (KEYSTAT)ExtractFloat(message, 1);
			degree = ExtractFloat(message, 5);
		}
		#endregion

		#region Reset Gyroscope Axes
		public static byte[] CreateResetGyroAxesMessage()
		{
			// Format is:
			//	 - byte 0: message type

			byte[] m = new byte[1];
			m[0] = ResetGyroAxes;

			return m;
		}
		public static void GetResetGyroAxesMessage(byte[] message)
		{
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

