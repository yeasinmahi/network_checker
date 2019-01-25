using System;
using System.Net.NetworkInformation;
using System.Timers;
using System.Windows.Forms;

namespace NetworkStatusChecker
{
    public partial class MainForm : Form
    {
        private System.Timers.Timer timer,pingTimer;
        private int startPosX;
        private int startPosY;
        private bool isShown = false,IsAvailableNetwork=true;
        private readonly int holdTimer = 300;
        private int counter = 0;
        public MainForm()
        {
            InitializeComponent();
            Init();
        }

        public void Init()
        {
            // We want our window to be the top most
            TopMost = true;
            // Pop doesn't need to be shown in task bar
            ShowInTaskbar = false;

        }
        public void CreateTimer()
        {
            // Create and run timer for animation
            timer = new System.Timers.Timer();
            timer.Interval = 5;
            timer.Elapsed += new ElapsedEventHandler(timer_Tick);

            pingTimer = new System.Timers.Timer();
            pingTimer.Interval = 1000;
            pingTimer.Elapsed += new ElapsedEventHandler(pingTimer_Tick);
            pingTimer.Enabled = true;
        }

        private void pingTimer_Tick(object sender, ElapsedEventArgs e)
        {
            if (GetNetworkStatus()!=IsAvailableNetwork)
            {
                IsAvailableNetwork = GetNetworkStatus();
                SetNetworkStatus(IsAvailableNetwork);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            // Move window out of screen
            startPosX = Screen.PrimaryScreen.WorkingArea.Width - Width;
            startPosY = Screen.PrimaryScreen.WorkingArea.Height;
            SetDesktopLocation(startPosX, startPosY);
            CreateTimer();
            timer.Enabled = true;
            base.OnLoad(e);

        }

        private void timer_Tick(object sender, EventArgs e)
        {
            //Lift window by 1 pixels
            if (isShown)
            {
                if (counter > holdTimer)
                {
                    startPosY += 1;
                }
                else
                {
                    counter++;
                }
            }
            else
            {
                startPosY -= 1;
            }

            //If window is fully visible stop the timer
            if (startPosY < Screen.PrimaryScreen.WorkingArea.Height - Height)
            {
                isShown = true;
                //timer.Stop();
            }
            else
            {
                if (InvokeRequired)
                {
                    // after we've done all the processing, 
                    Invoke(new MethodInvoker(delegate
                    {
                        SetDesktopLocation(startPosX, startPosY);
                    }));
                }
                else
                {
                    SetDesktopLocation(startPosX, startPosY);
                }

            }
            if (isShown && startPosY > Screen.PrimaryScreen.WorkingArea.Height + Height)
            {
                isShown = false;
                counter = 0;
                timer.Enabled = false;
            }

        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            SetNetworkStatus(GetNetworkStatus());
            NetworkChange.NetworkAvailabilityChanged += new NetworkAvailabilityChangedEventHandler(NetworkChange_NetworkAvailabilityChanged);

        }

        private void NetworkChange_NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {
            SetNetworkStatus(GetNetworkStatus());
        }

        private bool GetNetworkStatus()
        {
            return NetworkInterface.GetIsNetworkAvailable();
        }

        private void SetNetworkStatus(bool networkUp)
        {
            timer.Enabled = true;
            if (networkUp)
            {
                // ThreadHelper.SetText(this, labelStatus, "Network Ok");
                //labelStatus.Text = "Network Ok";
                if (InvokeRequired)
                {
                    // after we've done all the processing, 
                    Invoke(new MethodInvoker(delegate
                    {
                        SetNetworkUp();
                    }));
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
                    Invoke(new MethodInvoker(delegate
                    {
                        SetNetworkDown();
                    }));
                }
                else
                {
                    SetNetworkDown();
                }
            }
        }
        private void SetNetworkUp()
        {
            labelStatus.Text = "Network Ok";
            BackColor = System.Drawing.Color.Green;
            pictureBox.Image = Properties.Resources.ok;
        }
        private void SetNetworkDown()
        {
            labelStatus.Text = "Network Down";
            BackColor = System.Drawing.Color.Red;
            pictureBox.Image = Properties.Resources.notOk;
        }
    }
    
}
