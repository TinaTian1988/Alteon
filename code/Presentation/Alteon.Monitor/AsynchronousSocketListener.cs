using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Sockets;
using System.Threading;
using System.Net;
using Newtonsoft.Json;
using System.IO;

namespace Alteon.Monitor
{
    // State object for reading client data asynchronously     
    public class StateObject
    {
        // Client socket.     
        public Socket workSocket = null;
        // Size of receive buffer.     
        public const int BufferSize = 1024;
        // Receive buffer.     
        public byte[] buffer = new byte[BufferSize];
        // Received data string.     
        public StringBuilder sb = new StringBuilder();
    }
    public class AsynchronousSocketListener
    {
        public static void WriteLog(string str)
        {
            using (StreamWriter sw = File.AppendText(@"d:\Log\Monitor\log.txt"))
            {
                sw.WriteLine(str);
                sw.Flush();
            }
        }


        // Thread signal.     
        public static ManualResetEvent allDone = new ManualResetEvent(false);

        public static void StartListening()
        {
            try
            {
                // Data buffer for incoming data.     
                byte[] bytes = new Byte[1024];

                IPAddress ipAddress = IPAddress.Parse("127.0.0.1");//10.104.182.224
                IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 5000);
                // Create a TCP/IP socket.     
                Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                listener.Bind(localEndPoint);
                listener.Listen(100);
                while (true)
                {
                    //// Set the event to nonsignaled state.     
                    allDone.Reset();
                    //// Start an asynchronous socket to listen for connections.
                    DateTime dt = DateTime.Now;
                    WriteLog("终端已连接");
                    listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);
                    // Wait until a connection is made before continuing.     
                    allDone.WaitOne();
                }
            }
            catch (Exception e)
            {
                WriteLog("error1:" + e.ToString());
            }

        }
        public static void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                // Signal the main thread to continue.     
                allDone.Set();
                // Get the socket that handles the client request.     
                Socket listener = (Socket)ar.AsyncState;
                Socket handler = listener.EndAccept(ar);
                // Create the state object.     
                StateObject state = new StateObject();
                state.workSocket = handler;
                handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
            }
            catch (Exception e)
            {
                WriteLog("error2:" + e.Message);
            }

        }
        public static void ReadCallback(IAsyncResult ar)
        {
            try
            {
                string content = string.Empty;
                StateObject state = (StateObject)ar.AsyncState;
                Socket handler = state.workSocket;
                int bytesRead = handler.EndReceive(ar);
                if (bytesRead > 0)
                {
                    content = Encoding.ASCII.GetString(state.buffer, 0, bytesRead);
                    if (content.IndexOf("&") > -1)
                    {
                        //Console.WriteLine("Read {0} bytes from socket. \n Data : {1}", content.Length, content);
                        #region 业务逻辑
                        content = content.Remove(content.LastIndexOf("&"));
                        string[] data = content.Split('&');//content.Remove(content.LastIndexOf("&^!"));

                        try
                        {
                            for (int i = 0; i < data.Length; i++)
                            {
                                Dictionary<string, object> dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(data[i]);
                                if (!dic.ContainsKey("method") || string.IsNullOrEmpty(dic["method"].ToString()))
                                {
                                    WriteLog("未接收到“method”字段或值为空");
                                    return;
                                }
                                string method = dic["method"].ToString().ToLower();
                                switch (method)
                                {
                                    //登录
                                    case "update":
                                        if (!dic.ContainsKey("userkey") || string.IsNullOrEmpty(dic["userkey"].ToString()))
                                        {
                                            WriteLog("未接收到“userkey”字段或值为空");
                                            return;
                                        }
                                        if (!dic.ContainsKey("gatewayNo") || string.IsNullOrEmpty(dic["gatewayNo"].ToString()))
                                        {
                                            WriteLog("未接收到“gatewayNo”字段或值为空");
                                            return;
                                        }
                                        string userkey = dic["userkey"].ToString();
                                        var owner = DataService.IsExistUser(userkey);
                                        if (owner == null)
                                        {
                                            WriteLog("非法用户");
                                            return;
                                        }
                                        if (!DataService.IsSet("userkey"))
                                            DataService.Set("userkey", owner.Id, 1);
                                        if (!DataService.IsSet("gatewayNo"))
                                            DataService.Set("gatewayNo", dic["gatewayNo"], 1);
                                        break;

                                    //发送数据
                                    case "upload":
                                        if (!DataService.IsSet("userkey"))
                                        {
                                            WriteLog("未在内存中找到“userkey”，没有登录，或登录超时");
                                            return;
                                        }
                                        if (!DataService.IsSet("gatewayNo"))
                                        {
                                            WriteLog("未在内存中找到“gatewayNo”，没有登录，或登录超时");
                                            return;
                                        }
                                        if (!dic.ContainsKey("data") || string.IsNullOrEmpty(dic["data"].ToString()))
                                        {
                                            WriteLog("参数缺失，未接收到“data”字段");
                                            return;
                                        }
                                        int intGatewayNo = 0;
                                        string gatewayNo = DataService.Get<string>("gatewayNo");
                                        if (!int.TryParse(gatewayNo, out intGatewayNo) || intGatewayNo <= 0)
                                        {
                                            WriteLog("非法设备号");
                                            return;
                                        }
                                        
                                        List<EquipmentData> dataList = JsonConvert.DeserializeObject<List<EquipmentData>>(dic["data"].ToString());
                                        if (dataList == null && dataList.Count <= 0)
                                        {
                                            WriteLog("json反序列化异常");
                                            return;
                                        }
                                        int equipmentId = DataService.GetEquipmentId(DataService.Get<string>("userkey"), intGatewayNo);
                                        if (equipmentId <= 0)
                                        {
                                            WriteLog("不存在Id为" + equipmentId + "的设备");
                                            return;
                                        }
                                    
                                        //更新设备数据
                                        DataService.UploadData(equipmentId, dataList);
                                        break;
                                }
                            }

                        }
                        catch (Exception e)
                        {
                            WriteLog("error4:" + e.Message);
                        }
                        finally
                        {
                            Send(handler, content);
                            content = string.Empty;
                            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
                        }

                        #endregion
                    }
                }
            }
            catch (Exception e)
            {
                WriteLog("error3:" + e.Message);
            }
        }
        private static void Send(Socket handler, String data)
        {
            // Convert the string data to byte data using ASCII encoding.     
            byte[] byteData = Encoding.ASCII.GetBytes(data);
            // Begin sending the data to the remote device.     
            handler.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), handler);
        }
        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.     
                Socket handler = (Socket)ar.AsyncState;
                // Complete sending the data to the remote device.     
                int bytesSent = handler.EndSend(ar);
                WriteLog("Sent {0} bytes to client.");
                //handler.Shutdown(SocketShutdown.Both);
                //handler.Close();
            }
            catch (Exception e)
            {
                WriteLog("error4:" + e.ToString());
            }
        }

    }
}
