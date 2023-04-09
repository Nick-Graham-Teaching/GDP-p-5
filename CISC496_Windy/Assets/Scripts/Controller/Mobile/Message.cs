using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Windy.Controller
{
    public static class Message
    {
        public const byte CameraRotationX = 1;
        public const byte CameraRotationY = 2;

        public const byte Up              = 3;
        public const byte Down            = 4;
        public const byte Left            = 5;
        public const byte Right           = 6;
									      
		public const byte SwitchMode      = 7;
		public const byte Jump            = 8;
									      
        public const byte Pause           = 9;   // The message used by client and server
        public const byte Continue        = 10;  // The message used by client and server

		public const byte Fly_Up          = 11;
		public const byte Fly_Down        = 12;
		public const byte Fly_Left        = 13;
		public const byte Fly_Right       = 14;

		public const byte ResetGyroAxes   = 15;   // The message from server
		public const byte UseGyro		  = 16;   // The message from server

		public const byte GyroForwardUp   = 17;

		public const byte ToFlyingMode	  = 18; // The message from server
		public const byte ToWalkMode	  = 19; // The message from server

		public const byte DisplayAllTut	  = 20;
									      
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

		#region Use Gyro Message
		public static byte[] CreateUseGyroMessage(bool flag) // 0 - false; 1 - true
		{
			// Format is:
			//	 - byte 0: message type
			//	 - byte 1: flag value

			byte[] m = new byte[2];
			m[0] = UseGyro;
			int count = 1;
			AddBoolToByteArray(flag, ref m, ref count);

			return m;
		}
		public static void GetUseGyroMessage(byte[] message, out bool flag)
		{ 
			flag = ExtractBool(message, 1);
		}
		#endregion

		#region Gyro Attitude Message
		public static byte[] CreateGyroForwardUpMessage(Vector3 forward, Vector3 up) 
		{
			// Format is:
			//	 - byte 0: message type
			//	 - byte 1  - 4:  forward.x
			//	 - byte 5  - 8:  forward.y
			//	 - byte 9  - 12: forward.z
			//	 - byte 13 - 16: up.x
			//	 - byte 17 - 20: up.y
			//	 - byte 21 - 24: up.z

			byte[] m = new byte[25];
			m[0] = GyroForwardUp;
			int count = 1;
			AddFloatToByteArray(forward.x, ref m, ref count);
			AddFloatToByteArray(forward.y, ref m, ref count);
			AddFloatToByteArray(forward.z, ref m, ref count);
			AddFloatToByteArray(up.x, ref m, ref count);
			AddFloatToByteArray(up.y, ref m, ref count);
			AddFloatToByteArray(up.z, ref m, ref count);

			return m;
		}
		public static void GetGyroForwardUpMessage(byte[] message, out Vector3 forward, out Vector3 up)
		{
			forward = new Vector3(ExtractFloat(message, 1),  ExtractFloat(message, 5),  ExtractFloat(message, 9));
			up      = new Vector3(ExtractFloat(message, 13), ExtractFloat(message, 17), ExtractFloat(message, 21));
		}
		#endregion

		#region To Flying Mode Message
		public static byte[] CreateToFlyingModeMessage()
		{
			// Format is:
			//	 - byte 0: message type

			byte[] m = new byte[1];
			m[0] = ToFlyingMode;

			return m;
		}
		#endregion

		#region To Walking Mode Message
		public static byte[] CreateToWalkingModeMessage()
		{
			// Format is:
			//	 - byte 0: message type

			byte[] m = new byte[1];
			m[0] = ToWalkMode;

			return m;
		}
        #endregion

        #region Display all Tut Message
		public static byte[] CreateDisplayAllTutMessage()
        {
			// Format is:
			//	 - byte 0: message type

			byte[] m = new byte[1];
			m[0] = DisplayAllTut;

			return m;
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
		static void AddBoolToByteArray(bool flag, ref byte[] m, ref int count)
        {
			byte[] b = BitConverter.GetBytes(flag);
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
		static bool ExtractBool(byte[] message, int startPos)
        {
			return BitConverter.ToBoolean(message, startPos);
        }

		public static byte GetMessageType(byte[] message)
		{
			return message[0];
		}
		#endregion
	}
}

