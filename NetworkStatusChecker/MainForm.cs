﻿using System;
using System.Net.NetworkInformation;
using System.Timers;
using System.Windows.Forms;

namespace NetworkStatusChecker
{
    public partial class MainForm : Form
    {
        private System.Timers.Timer _timer,_pingTimer,_speedTestTimer;
        private int _startPosX;
        private int _startPosY;
        private bool _isShown = false,_isAvailableNetwork=true,_networkStatus;
        private readonly int _holdTimer = 300;
        private int _counter = 0;

        private int animationInterval = 5;
        private int networkCheckInterval = 1000; // 1 sec
        private int speedCheckInterval = 3600000; // 1 hour

        public MainForm()
        {
            InitializeComponent();
        }
        public void CreateTimer()
        {
            // Create and run timer for animation
            _timer = new System.Timers.Timer();
            _timer.Interval = animationInterval;
            _timer.Elapsed += new ElapsedEventHandler(timer_Tick);

            //create and run timer for ping and network test 
            _pingTimer = new System.Timers.Timer();
            _pingTimer.Interval = networkCheckInterval;
            _pingTimer.Elapsed += new ElapsedEventHandler(pingTimer_Tick);
            _pingTimer.Enabled = true;

            //create and run timer for upload and download speed test
            _speedTestTimer = new System.Timers.Timer();
            _speedTestTimer.Interval = speedCheckInterval;
            _speedTestTimer.Elapsed += new ElapsedEventHandler(speedTestTimer_Tick);
            _speedTestTimer.Enabled = true;
        }

        private void speedTestTimer_Tick(object sender, ElapsedEventArgs e)
        {
            double downloadSpeed = MyNetwork.GetDownloadSpeed();
            double uploadSpeed = MyNetwork.GetUploadSpeed();

            if (downloadSpeed > 0)
            {
                if (InvokeRequired)
                {
                    // after we've done all the processing, 
                    Invoke(new MethodInvoker(delegate
                    {
                        SetUploadSpeedAndDownloadSpeed(downloadSpeed, uploadSpeed);
                    }));
                }
                else
                {
                    SetUploadSpeedAndDownloadSpeed(downloadSpeed, uploadSpeed);
                }
                _timer.Enabled = true;
            }
            else
            {
                // no speed found
            }
        }

        private void pingTimer_Tick(object sender, ElapsedEventArgs e)
        {
            _networkStatus = MyNetwork.GetNetworkStatus();
            if (_networkStatus != _isAvailableNetwork)
            {
                _isAvailableNetwork = _networkStatus;
                SetNetworkStatus(_isAvailableNetwork);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            // Move window out of screen
            _startPosX = Screen.PrimaryScreen.WorkingArea.Width - Width;
            _startPosY = Screen.PrimaryScreen.WorkingArea.Height;
            SetDesktopLocation(_startPosX, _startPosY);
            CreateTimer();
            _timer.Enabled = true;
            base.OnLoad(e);

        }

        private void timer_Tick(object sender, EventArgs e)
        {
            //Lift window by 1 pixels
            if (_isShown)
            {
                if (_counter > _holdTimer)
                {
                    _startPosY += 1;
                }
                else
                {
                    _counter++;
                }
            }
            else
            {
                _startPosY -= 1;
            }

            //If window is fully visible stop the timer
            if (_startPosY < Screen.PrimaryScreen.WorkingArea.Height - Height)
            {
                _isShown = true;
                //timer.Stop();
            }
            else
            {
                if (InvokeRequired)
                {
                    // after we've done all the processing, 
                    Invoke(new MethodInvoker(delegate
                    {
                        SetDesktopLocation(_startPosX, _startPosY);
                    }));
                }
                else
                {
                    SetDesktopLocation(_startPosX, _startPosY);
                }

            }
            if (_isShown && _startPosY > Screen.PrimaryScreen.WorkingArea.Height + Height)
            {
                _isShown = false;
                _counter = 0;
                _timer.Enabled = false;
            }

        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            SetNetworkStatus(MyNetwork.GetNetworkStatus());
            NetworkChange.NetworkAvailabilityChanged += new NetworkAvailabilityChangedEventHandler(NetworkChange_NetworkAvailabilityChanged);

        }

        private void NetworkChange_NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {
            SetNetworkStatus(MyNetwork.GetNetworkStatus());
        }

        

        private void SetNetworkStatus(bool networkUp)
        {
            _timer.Enabled = true;
            if (networkUp)
            {
                // ThreadHelper.SetText(this, labelStatus, "Network Ok");
                //labelStatus.Text = "Network Ok";
                if (InvokeRequired)
                {
                    // after we've done all the processing, 
                    Invoke(new MethodInvoker(SetNetworkUp));
                }
                else
                {
                    SetNetworkUp();
                }
            }
            else
            {
                //ThreadHelper.SetText(this, labelStatus, "Network Down");
                //labelStatus.Text = "Network Down";
                if (InvokeRequired)
                {
                    // after we've done all the processing, 
                    Invoke(new MethodInvoker(SetNetworkDown));
                }
                else
                {
                    SetNetworkDown();
                }
            }
        }
        private void SetNetworkUp()
        {
            labelStatus.Text = Messages.NetworkHead;
            labelInformation.Text = Messages.NetworkMessageUp;
            //BackColor = System.Drawing.Color.Green;
            pictureBox.Image = Properties.Resources.success;
            LogWriter.WriteLog("'Network UP'",MyNetwork.GetDownloadSpeed(), MyNetwork.GetUploadSpeed());
        }
        private void SetNetworkDown()
        {
            labelStatus.Text = Messages.NetworkHead;
            labelInformation.Text = Messages.NetworkMessageDown;
            //BackColor = System.Drawing.Color.Red;
            pictureBox.Image = Properties.Resources.error;
            LogWriter.WriteLog("'Network Down'", 0,0);
        }
        private void SetUploadSpeedAndDownloadSpeed(double downloadSpeed, double uploadSpeed)
        {
            labelStatus.Text = Messages.SpeedHead;
            labelInformation.Text = Messages.GetSpeedMessage(downloadSpeed, uploadSpeed);
            //BackColor = System.Drawing.Color.Red;
            pictureBox.Image = Properties.Resources.information;
            LogWriter.WriteLog("'Net Speed Test'", downloadSpeed, uploadSpeed);
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.E))
            {
                DialogResult dialog = MessageBox.Show("Do you really want to close the program?", "Network Status Checker ", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    Application.Exit();
                }
                
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

    }

}
