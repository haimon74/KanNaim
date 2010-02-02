using System;
//using Microsoft.VisualBasic.Devices;
//using Microsoft.VisualBasic;
//using System.Runtime.InteropServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading;
using Microsoft.VisualBasic.Devices;


namespace Kan_Naim_Main
{

    

    public partial class FormVbDevices : Form
    {
        public delegate void PlaySoundDelegate(string fileName);
        public delegate void StartRecordinDelegate();
        public delegate void StopRecordinDelegate(string fileName);
        //public delegate void UpdateTimeDelegate();
        //public delegate void UpdateRandomNumbers(int[] numbers);

        private Thread currentTimeThread = null;

        //private System.Windows.Forms.TextBox currentTime;
				
        //public void Invoke(System.Delegate delegate);
        //public void Invoke(System.Delegate delegate, object [] args);

        public FormVbDevices()
        {
            InitializeComponent();

            //PlaySound(null, null);

            currentTimeThread = new Thread(new ThreadStart(CountTime));
            currentTimeThread.IsBackground = true;
           currentTimeThread.Start();

            //Thread thread = new Thread(MyTask);
            //thread.IsBackground = true;
            //thread.Start();

        }
        
        private void PlaySound(string fileToPlay)
        {
            var myComputer = new Computer();
            myComputer.Audio.Play(@fileToPlay);
        }
        
        private void CountTime()
        {
            RecordStart();
            Thread.Sleep(100);
            RecordStop(@"c:\\record0.wav");
            RecordStart();
            Thread.Sleep(1000);
            RecordStop(@"c:\\record1.wav");
            RecordStart();
            

            while (true)
            {
                for (int i = 2; i >= 0; i--)
                {
                    Thread.Sleep(1000);
                    string filename1 = @String.Format("c:\\record{0}.wav", i);
                    string filename2 = @String.Format("c:\\record{0}.wav", (i+1)%3);
                    string filename3 = @String.Format("c:\\record{0}.wav", (i+2)%3);
                    Invoke(new StopRecordinDelegate(RecordStop), filename1);
                    Invoke(new StartRecordinDelegate(RecordStart));
                    Invoke(new PlaySoundDelegate(PlaySound), filename3);
                }
            }
        }

        private void RecordStop(string filename)
        {
            mciSendString("save recsound " + filename, "", 0, 0);
            mciSendString("close recsound ", "", 0, 0); 
            Computer c = new Computer(); 
            c.Audio.Stop();
        }

        [DllImport("winmm.dll", EntryPoint = "mciSendStringA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int mciSendString(string lpstrCommand, string lpstrReturnString, int uReturnLength, int hwndCallback);
        private void RecordStart()
        {
            mciSendString("open new Type waveaudio Alias recsound", "", 0, 0);
            mciSendString("record recsound", "", 0, 0);
        }

        private void updateCurrentTime()
        {
            ;// currentTime.Text = DateTime.Now.ToLongTimeString();
        }
        private void RepeatPlay(string fileName)
        {
            //Timer timer = new Timer();
            //timer.Interval = 2000;

            ////buttonRecordPlay_Click(null, null);
            //timer.Start();
            //timer.Tick += PlaySound;

            //while (true)
            //{
            //}
        }
        //[DllImport("winmm.dll", EntryPoint = "mciSendStringA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        //private static extern int mciSendString(string lpstrCommand, string lpstrReturnString, int uReturnLength, int hwndCallback);
        private void buttonRecrdstart_Click(object sender, EventArgs e)
        {
            //currentTimeThread.Start();
            //mciSendString("open new Type waveaudio Alias recsound", "", 0, 0); 
            //mciSendString("record recsound", "", 0, 0);
        }


        private void buttonRecordPlay_Click(object sender, EventArgs e)
        {
            //Computer computer = new Computer(); 
            //computer.Audio.Play("c:\\record.wav", AudioPlayMode.Background);
        }


        private void MyTask()
        {

            //Invoke(new TaskDelegateType(RepeatPlay), new object[] { "chimes.wav" });


            //Timer timer = new Timer();
            //timer.Interval = 2000;
            
            ////buttonRecordPlay_Click(null, null);
            ////timer.Start();
            //while (true)
            //{
            //    timer.Start();
            //    timer.Tick += PlaySound;
            //    PlaySound(null,null);
        //    }
        //}
        //private void SayHi(string msg)
        //{
        //    buttonRecrdstart.Text = msg;
        }


        private void buttonRecordStop_Click(object sender, EventArgs e)
        {
            //currentTimeThread.Abort();
            //currentTimeThread.Join();

            //currentTimeThread = new Thread(new ThreadStart(CountTime));
            //currentTimeThread.IsBackground = true;

            //mciSendString("save recsound c:\\record.wav", "", 0, 0); 
            //mciSendString("close recsound ", "", 0, 0); 
            //Computer c = new Computer(); 
            //c.Audio.Stop();
        }
    }
}
