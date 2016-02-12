using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using IMU_gNeC;

namespace IMU_gNeC_test
{
    public partial class Form1 : Form
    {
        #region attributes

            IMUClass myIMU = new IMUClass();
            System.Timers.Timer aTimer = new System.Timers.Timer();

        #endregion

        #region methods

            public Form1()
            {
                myIMU.IMUAng.ImuYPR += new IMUEventHandler(ImuYPRreceived);
                myIMU.imuOR.ImuOR += new ImuOREventHandler(ImuORreceived);
                myIMU.imuPerm.ImuPerm += new ImuPermEventHandler(ImuPermReceived);
                myIMU.imuROM.ImuROM += new ImuROMEventHandler(ImuRomOKReceived);
                myIMU.imuReady.ImuRea += new ImuReadyEventHandler(ImuReadyReceived);


                aTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEvent);
                aTimer.Interval = 500;
                //aTimer.Enabled = true;

                
                InitializeComponent();
            }


            private void OnTimedEvent(object source, System.Timers.ElapsedEventArgs e)
            {
                //read imu data
                Console.WriteLine("globalPitch: " + myIMU.globalPitch.ToString());
            }
            
            private void buttonConnect_Click(object sender, EventArgs e)
            {
                if (myIMU.startReadingIMU("COM45", 1))
                {
                    aTimer.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Port not opened");
                }
            }

            private void buttonDisconnect_Click(object sender, EventArgs e)
            {
                myIMU.disconnectIMU();
                Application.Exit();
            }
                
            private void buttonCalibrate_Click(object sender, EventArgs e)
            {
                myIMU.ImuConfigPar.Calibrate = true;
                myIMU.ImuConfigPar.Rotate = false;
                myIMU.ImuConfigPar.SetUp = false;
                myIMU.ImuConfigPar.SendSetUp();
            }
        
            private void buttonRotate_Click(object sender, EventArgs e)
            {
                myIMU.ImuConfigPar.Calibrate = false;
                myIMU.ImuConfigPar.Rotate = true;
                myIMU.ImuConfigPar.SetUp = false;
                myIMU.ImuConfigPar.SendSetUp();
            }

            private void buttonSetUp_Click(object sender, EventArgs e)
            {
                myIMU.ImuConfigPar.Calibrate = false;
                myIMU.ImuConfigPar.Rotate = false;
                myIMU.ImuConfigPar.SetUp = true;
                myIMU.ImuConfigPar.MainAngleROMrange = 30;
                myIMU.ImuConfigPar.MainAngleROMangle = "Y";
                myIMU.ImuConfigPar.SecundaryAngle1ROMrange = 10;
                myIMU.ImuConfigPar.SecundaryAngle1ROMangle = "P";
                myIMU.ImuConfigPar.SecundaryAngle2ROMrange = 10;
                myIMU.ImuConfigPar.SecundaryAngle2ROMangle = "R";
                myIMU.ImuConfigPar.TimePerm = 1;
                myIMU.ImuConfigPar.ThetaPerm = 10;
                myIMU.ImuConfigPar.SendSetUp();
            }

            private void buttonInitGame_Click(object sender, EventArgs e)
            {
                myIMU.ImuInitgame.InitGame = true;
                myIMU.ImuInitgame.SendInitEndGame();
            }

            private void buttonEndGame_Click(object sender, EventArgs e)
            {
                myIMU.ImuInitgame.InitGame = false;
                myIMU.ImuInitgame.SendInitEndGame();
            }

















            
            public void ImuYPRreceived(object sender, IMUEventArgs e)
            {
                //Console.WriteLine("Yaw " + Math.Floor(e.Yaw).ToString()
                //                 + ", Pitch " + Math.Floor(e.Pitch).ToString()
                //                 + ", Roll " + Math.Floor(e.Roll).ToString());

                //WriteLabel(label1, Math.Floor(e.Yaw).ToString());
                //WriteLabel(label2, Math.Floor(e.Pitch).ToString());
                //WriteLabel(label3, Math.Floor(e.Roll).ToString());
            }

            public void ImuORreceived(object sender, ImuOREventArgs e)
            {
                 WriteLabel(label8, Math.Floor(e.MainAngle).ToString());
                 WriteLabel(label9, "-");
            }
            
            public void ImuPermReceived(object sender, ImuPermEventArgs e)
            {
                WriteLabel(label9, "PERMANENCIA");
                Console.WriteLine("Permanencia en ángulo: " + Math.Floor(e.Alpha).ToString());
            }

            public void ImuRomOKReceived(object sender, ImuROMEventArgs e)
            {               
                if (e.RomOK)
                {
                    Console.WriteLine("En rango");
                    WriteLabel(label10, "En rango");
                }
                else
                {
                    Console.WriteLine("FUERA DE RANGO");
                    WriteLabel(label10, "FUERA DE RANGO");
                }
            }

            public void ImuReadyReceived(object sender, ImuReadyEventArgs e)
            {

            }













            
            delegate void WriteLabelDelegate(Label label, string text);
            private void WriteLabel(Label label, string text)
            {
                if (label.InvokeRequired)
                    label.Invoke(new WriteLabelDelegate(this.WriteLabel), new object[] { label, text });
                else
                {
                    label.Text = text;
                }
            }
        
        #endregion    
    
    }
}
