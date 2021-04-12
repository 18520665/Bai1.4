using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Timers;

namespace WindowsService1
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }
        System.Timers.Timer timer = new System.Timers.Timer();
        [DllImport("wtsapi32.dll", SetLastError = true)]
        static extern bool WTSSendMessage(
        IntPtr hServer,
        [MarshalAs(UnmanagedType.I4)] int SessionId,
        String pTitle,
        [MarshalAs(UnmanagedType.U4)] int TitleLength,
        String pMessage,
        [MarshalAs(UnmanagedType.U4)] int MessageLength,
        [MarshalAs(UnmanagedType.U4)] int Style,
        [MarshalAs(UnmanagedType.U4)] int Timeout,
        [MarshalAs(UnmanagedType.U4)] out int pResponse,
        bool bWait);
        public static IntPtr WTS_CURRENT_SERVER_HANDLE = IntPtr.Zero;
        public void Start(string userName)
        {
            try
            {
                bool result = false;
                String title = " ";
                int tlen = title.Length;
                if (userName != null || userName != "")
                {
                    string msg = "18520665";
                    int mlen = msg.Length;
                    int resp = 1;
                    result = WTSSendMessage(WTS_CURRENT_SERVER_HANDLE, 1, title, tlen, msg, mlen,1,0, out resp, false);
                    int err = Marshal.GetLastWin32Error();
                    if (err == 0)
                    {
                        if (result) //user responded to box
                        {
                            if (resp == 1)
                            {
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
            }
        }
        protected override void OnStart(string[] args)
        {
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Interval = 5000; //number in milisecinds
            timer.Enabled = true;
        }
        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            Start(userName);

        }
        protected override void OnStop()
        {
        }
    }
}