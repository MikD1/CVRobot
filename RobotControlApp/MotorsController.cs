using System;
using System.IO.Ports;

namespace RobotControlApp
{
    public sealed class MotorsController
    {
        public MotorsController(SerialPort port)
        {
            _port = port;
        }

        public bool Drive(int m1Speed, int m2Speed)
        {
            if (m1Speed < -255 || m1Speed > 255)
            {
                return false;
            }

            if (m2Speed < -255 || m2Speed > 255)
            {
                return false;
            }

            byte m1Value = (byte)Math.Abs(m1Speed);
            byte m2Value = (byte)Math.Abs(m2Speed);
            byte direction = 0;

            if (m1Speed < 0)
            {
                direction |= 2;
            }

            if (m2Speed < 0)
            {
                direction |= 1;
            }

            byte[] data = new byte[] { 1, m1Value, m2Value, direction };
            _port.Write(data, 0, data.Length);

            return true;
        }

        private SerialPort _port;
    }
}
