using System.Collections.Generic;
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
            foreach (Key key in keys)
            {
                switch (key)
                {
                    case Key.W:
                        _camera.RotateB(-_speed);
                        break;

                    case Key.S:
                        _camera.RotateB(_speed);
                        break;

                    case Key.A:
                        _camera.RotateA(_speed);
                        break;

                    case Key.D:
                        _camera.RotateA(-_speed);
                        break;

                    case Key.Q:
                        _camera.RotateC(-_speed);
                        break;

                    case Key.E:
                        _camera.RotateC(+_speed);
                        break;
                }
            }
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            if (_camera.IsConnected)
            {
                Key[] keys = _pressedKeys.ToArray();
                HandleKeys(keys);
            }

            _timer.Start();
        }

        private void ConnectOnClick(object sender, RoutedEventArgs e)
        {
            if (_camera.Connect("com3"))
            {
                ConnectButton.IsEnabled = false;
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

        private int _speed = 5;
        private readonly Timer _timer = new Timer(100);
        private readonly List<Key> _pressedKeys = new List<Key>();
        private readonly CameraController _camera = new CameraController();
    }
}
