using NetworkStatusChecker.Properties;
using System;
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
        private bool _isShown,_isAvailableNetwork=true,_networkStatus;
        private readonly int _holdTimer = 300;
        private int _counter;

        private readonly int _animationInterval = 5;
        private readonly int _networkCheckInterval = 5000; // 1 sec
        private readonly int _speedCheckInterval = 3600000; // 1 hour

        private NotifyIcon _trayIcon;
        public MainForm()
        {
            InitializeComponent();
            InitSystemTray();
        }
        public void InitSystemTray()
        {
            _trayIcon = new NotifyIcon()
            {
                Icon = Resources.network,
                ContextMenu = new ContextMenu(new[] {
                new MenuItem("Exit", Exit),
                new MenuItem("Show Status",ShowStatus),
                new MenuItem("Show Speed",ShowSpeed),
            }),
                Visible = true
            };
        }

        private void ShowSpeed(object sender, EventArgs e)
        {
            speedTestTimer_Tick(null,null);
        }

        private void ShowStatus(object sender, EventArgs e)
        {
            SetNetworkStatus(MyNetwork.GetNetworkStatus());
        }

        void Exit(object sender, EventArgs e)
        {
            // Hide tray icon, otherwise it will remain shown until user mouses over it
            _trayIcon.Visible = false;

            Application.Exit();
        }
        public void CreateTimer()
        {
            // Create and run timer for animation
            _timer = new System.Timers.Timer {Interval = _animationInterval};
            _timer.Elapsed += timer_Tick;

            //create and run timer for ping and network test 
            _pingTimer = new System.Timers.Timer {Interval = _networkCheckInterval};
            _pingTimer.Elapsed += pingTimer_Tick;
            _pingTimer.Enabled = true;

            //create and run timer for upload and download speed test
            _speedTestTimer = new System.Timers.Timer {Interval = _speedCheckInterval};
            _speedTestTimer.Elapsed += speedTestTimer_Tick;
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

        //public void popUpNotifier()
        //{
        //    PopupNotifier popup = new PopupNotifier();
        //    popup.TitleText = "BE HAPPY";
        //    popup.ContentText = "Thank you";
        //    popup.Popup();
        //}
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
                StopAnimation();
            }

        }
        public void StopAnimation()
        {
            _isShown = false;
            _counter = 0;
            _timer.Enabled = false;
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            SetNetworkStatus(MyNetwork.GetNetworkStatus());
            NetworkChange.NetworkAvailabilityChanged += NetworkChange_NetworkAvailabilityChanged;

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
            pictureBox.Image = Resources.success;
            LogWriter.WriteLog("'Network UP'",MyNetwork.GetDownloadSpeed(), MyNetwork.GetUploadSpeed());
        }
        private void SetNetworkDown()
        {
            labelStatus.Text = Messages.NetworkHead;
            labelInformation.Text = Messages.NetworkMessageDown;
            //BackColor = System.Drawing.Color.Red;
            pictureBox.Image = Resources.error;
            LogWriter.WriteLog("'Network Down'", 0,0);
        }
        private void SetUploadSpeedAndDownloadSpeed(double downloadSpeed, double uploadSpeed)
        {
            labelStatus.Text = Messages.SpeedHead;
            labelInformation.Text = Messages.GetSpeedMessage(downloadSpeed, uploadSpeed);
            //BackColor = System.Drawing.Color.Red;
            pictureBox.Image = Resources.information;
            LogWriter.WriteLog("'Net Speed Test'", downloadSpeed, uploadSpeed);
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.E))
            {
                DialogResult dialog = MessageBox.Show(@"Do you really want to close the program?", @"Network Status Checker ", MessageBoxButtons.YesNo);
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
