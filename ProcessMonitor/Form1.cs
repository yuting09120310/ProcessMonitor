using System.Diagnostics;
using System;
using System.Threading;
using System.Windows.Forms;


namespace ProcessMonitor
{
    public partial class Form1 : Form
    {
        private Thread monitorThread;
        private bool monitoring;
        private string processName = Program.Configuration["ProcessName"]!;  // ���]�Ұʪ��i�{�O Java
        private string startCommand = Program.Configuration["StartApp"]!;  // �Ұ� start.bat �����|


        public Form1()
        {
            InitializeComponent();
        }


        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (!monitoring)
            {
                monitoring = true;
                monitorThread = new Thread(MonitorProcess);
                monitorThread.Start();
                labelStatus.Text = "�ʱ���.";
            }
        }


        private void buttonStop_Click(object sender, EventArgs e)
        {
            if (monitoring)
            {
                monitoring = false;
                monitorThread.Join();
                labelStatus.Text = "�ʱ�����.";
            }
        }


        private void MonitorProcess()
        {
            while (monitoring)
            {
                if (!IsProcessRunning(processName))
                {
                    Invoke((MethodInvoker)delegate
                    {
                        labelStatus.Text = $"{processName} ���B��C���b�Ұ�...";
                    });

                    StartProcess(startCommand);

                    Invoke((MethodInvoker)delegate
                    {
                        labelStatus.Text = $"{processName} �Ұʧ���.";
                    });
                }
                else
                {
                    Invoke((MethodInvoker)delegate
                    {
                        labelStatus.Text = $"{processName} �w�Ұ�";
                    });
                }

                Thread.Sleep(10000);  // �C 10 ���ˬd�@��
            }
        }


        private bool IsProcessRunning(string processName)
        {
            Process[] processes = Process.GetProcessesByName(processName);
            return processes.Length > 0;
        }


        private void StartProcess(string command)
        {
            try
            {
                Process.Start(command);
            }
            catch (Exception ex)
            {
                Invoke((MethodInvoker)delegate
                {
                    labelStatus.Text = $"�Ұʥ��� ��]: {ex.Message}";
                });
            }
        }

    }
}