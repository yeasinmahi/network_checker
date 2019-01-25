using System;
using System.Net.NetworkInformation;
using System.Timers;
using System.Windows.Forms;

namespace NetworkStatusChecker
{
    public partial class MainForm : Form
    {
        private System.Timers.Timer timer;
        private int startPosX;
        private int startPosY;
        private bool isShown = false;
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

            //timer.Tick += timer_Tick;
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
                    this.Invoke(new MethodInvoker(delegate
                    {
                        SetDesktopLocation(startPosX, startPosY);
                    }));
                }
                else
                {
                    SetDesktopLocation(startPosX, startPosY);
                }

            }
            if (isShown && startPosY > Screen.PrimaryScreen.WorkingArea.Height + (Height / 2))
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
                ThreadHelper.SetText(this, labelStatus, "Network Ok");
                //labelStatus.Text = "Network Ok";
            }
            else
            {
                ThreadHelper.SetText(this, labelStatus, "Network Down");
                //labelStatus.Text = "Network Down";
            }
        }
    }
}
