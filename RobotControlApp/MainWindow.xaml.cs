using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Timers;
using System.Windows;
using System.Windows.Input;

namespace RobotControlApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            _timer.Elapsed += TimerOnElapsed;
            _timer.AutoReset = false;
            _timer.Start();
        }

        private void HandleKeys(Key[] keys)
        {
            int x = 0;
            int y = 0;

            foreach (Key key in keys)
            {
                switch (key)
                {
                    case Key.W:
                        _camera.RotateB(-_cameraSpeed);
                        break;

                    case Key.S:
                        _camera.RotateB(_cameraSpeed);
                        break;

                    case Key.A:
                        _camera.RotateA(_cameraSpeed);
                        break;

                    case Key.D:
                        _camera.RotateA(-_cameraSpeed);
                        break;

                    case Key.Q:
                        _camera.RotateC(-_cameraSpeed);
                        break;

                    case Key.E:
                        _camera.RotateC(+_cameraSpeed);
                        break;

                    case Key.Up:
                        y = 2;
                        break;

                    case Key.Down:
                        y = 1;
                        break;

                    case Key.Left:
                        x = 1;
                        break;

                    case Key.Right:
                        x = 2;
                        break;
                }
            }

            int motorsValue = x + y * 10;
            Drive(motorsValue);
        }

        private void Drive(int value)
        {
            int m1Speed = 0;
            int m2Speed = 0;

            switch (value)
            {
                case 0:
                    m1Speed = 0;
                    m2Speed = 0;
                    break;

                case 1:
                    m1Speed = _motorsSpeedRates[_motorsSpeed];
                    m2Speed = -_motorsSpeedRates[_motorsSpeed];
                    break;

                case 2:
                    m1Speed = -_motorsSpeedRates[_motorsSpeed];
                    m2Speed = _motorsSpeedRates[_motorsSpeed];
                    break;

                case 10:
                    m1Speed = -_motorsSpeedRates[_motorsSpeed];
                    m2Speed = -_motorsSpeedRates[_motorsSpeed];
                    break;

                case 20:
                    m1Speed = _motorsSpeedRates[_motorsSpeed];
                    m2Speed = _motorsSpeedRates[_motorsSpeed];
                    break;

                case 11:
                    m1Speed = -_motorsSpeedRates[_motorsSpeed];
                    m2Speed = -_motorsSpeedRates[_motorsSpeed] / 2;
                    break;

                case 12:
                    m1Speed = -_motorsSpeedRates[_motorsSpeed] / 2;
                    m2Speed = -_motorsSpeedRates[_motorsSpeed];
                    break;

                case 21:
                    m1Speed = _motorsSpeedRates[_motorsSpeed];
                    m2Speed = _motorsSpeedRates[_motorsSpeed] / 2;
                    break;

                case 22:
                    m1Speed = _motorsSpeedRates[_motorsSpeed] / 2;
                    m2Speed = _motorsSpeedRates[_motorsSpeed];
                    break;
            }

            _motors.Drive(m1Speed, m2Speed);
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            if (_camera != null && _motors != null)
            {
                Key[] keys = _pressedKeys.ToArray();
                HandleKeys(keys);
            }

            _timer.Start();
        }

        private void ConnectOnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                SerialPort port = new SerialPort("com7", 9600);
                port.Open();

                if (port.IsOpen)
                {
                    _camera = new CameraController(port);
                    _motors = new MotorsController(port);

                    ConnectButton.IsEnabled = false;
                }
            }
            catch(Exception)
            {
            }
        }
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (!_pressedKeys.Contains(e.Key))
            {
                _pressedKeys.Add(e.Key);
            }
        }
        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (_pressedKeys.Contains(e.Key))
            {
                _pressedKeys.Remove(e.Key);
            }
        }

        private int _cameraSpeed = 5;
        private int _motorsSpeed = 0;
        private CameraController _camera;
        private MotorsController _motors;
        private readonly int[] _motorsSpeedRates = { 70, 140, 255 };
        private readonly Timer _timer = new Timer(100);
        private readonly List<Key> _pressedKeys = new List<Key>();
    }
}
