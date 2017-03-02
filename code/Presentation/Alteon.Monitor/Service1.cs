using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

using System.Threading;
using System.IO;
using System.Net.Sockets;

namespace Alteon.Monitor
{
    public partial class Service1 : ServiceBase
    {
        ////定时器  
        //System.Timers.Timer t = null;
        
        public Service1()
        {
            InitializeComponent();
            //启用暂停恢复  
            base.CanPauseAndContinue = true;

            ////每5秒执行一次  
            //t = new System.Timers.Timer(5000);
            ////设置是执行一次（false）还是一直执行(true)；  
            //t.AutoReset = true;
            ////是否执行System.Timers.Timer.Elapsed事件；  
            //t.Enabled = true;
            ////到达时间的时候执行事件(theout方法)；  
            //t.Elapsed += new System.Timers.ElapsedEventHandler(theout);  
        }

        //启动服务执行  
        protected override void OnStart(string[] args)
        {
            string state = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "服务启动";
            WriteLog(state);
            //try
            //{
            //    Thread th = new Thread(new ThreadStart(AsynchronousSocketListener.StartListening));//创建一个新的线程专门用于处理监听
            //    th.Start();//启动线程
                
            //}
            //catch (SocketException se)
            //{
            //    WriteLog("error2:" + se);
            //}
            //catch (ArgumentNullException ae)//参数为空异常
            //{
            //    WriteLog("error3:" + ae);
            //}


            var db = RedisHelper.getredisdb();

        }

        //停止服务执行  
        protected override void OnStop()
        {
            string state = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "停止";
            WriteLog(state);
        }

        //恢复服务执行  
        protected override void OnContinue()
        {
            //string state = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "继续";
            //WriteLog(state);
            //t.Start();
        }

        //暂停服务执行  
        protected override void OnPause()
        {
            //string state = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "暂停";
            //WriteLog(state);

            //t.Stop();
        }


        public void WriteLog(string str)
        {
            using (StreamWriter sw = File.AppendText(@"d:\Log\Monitor\log.txt"))
            {
                sw.WriteLine(str);
                sw.Flush();
            }
        }

        //public void theout(object source, System.Timers.ElapsedEventArgs e)
        //{

        //    WriteLog("theout:" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
        //} 
    }
}
