using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;

namespace RobotControlApp
{
    public sealed class CameraController : IDisposable
    {
        public CameraController()
        {
            FillSegments();
        }
        ~CameraController()
        {
            Dispose();
        }

        public bool IsConnected { get; private set; }

        public bool Connect(string port)
        {
            IsConnected = OpenPort(port);
            if (IsConnected)
            {
                SetStartPosition();
            }

            return IsConnected;
        }

        public bool RotateA(int delta)
        {
            int value = _segments[0] + delta;
            return SetSegmentPosition(0, value);
        }
        public bool RotateB(int delta)
        {
            int value = _segments[1] + delta;
            return SetSegmentPosition(1, value);
        }
        public bool RotateC(int delta)
        {
            int value = _segments[2] + delta;
            return SetSegmentPosition(2, value);
        }

        public int GetPosition(int segment)
        {
            if (IsConnected && _segments.Keys.Contains(segment))
            {
                return _segments[segment];
            }

            return -1;
        }

        public void Dispose()
        {
            ClosePort();
        }

        private void FillSegments()
        {
            _segments.Add(0, 90);
            _segments.Add(1, 90);
            _segments.Add(2, 90);
        }
        private void SetStartPosition()
        {
            SetSegmentPosition(0, 110);
            SetSegmentPosition(1, 110);
            SetSegmentPosition(2, 110);
        }
        private bool SetSegmentPosition(int segment, int value)
        {
            if (!IsConnected)
            {
                return false;
            }

            if (segment < 0 || segment > 2)
            {
                return false;
            }

            if (value < 0)
            {
                value = 0;
            }

            if (value > 180)
            {
                value = 180;
            }

            if (_segments[segment] == value)
            {
                return false;
            }

            byte[] data = new byte[] { (byte)segment, (byte)value };
            _port.Write(data, 0, data.Length);

            _segments[segment] = value;
            return true;
        }

        private bool OpenPort(string port)
        {
            Dispose();

            try
            {
                _port = new SerialPort(port, _baudRate);
                _port.Open();
            }
            catch
            {
            }

            return _port.IsOpen;
        }
        private void ClosePort()
        {
            if (_port != null && _port.IsOpen)
            {
                _port.Close();
            }

            _port = null;
        }

        private SerialPort _port;
        private readonly Dictionary<int, int> _segments = new Dictionary<int, int>();

        private const int _baudRate = 9600;
    }
}
