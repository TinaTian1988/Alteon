using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TcpServer
{

    public class StateObject
    {
        // Client socket.     
        public Socket workSocket = null;
        // Size of receive buffer.     
        public const int BufferSize = 1024;
        // Receive buffer.     
        public byte[] buffer = new byte[BufferSize];
        // Received data string.     
        //public StringBuilder sb = new StringBuilder();
    }
    public class AsynchronousSocketListener
    {
        public static ManualResetEvent allDone = new ManualResetEvent(false);

        public static void StartListening()
        {
            try
            {
                byte[] bytes = new Byte[1024];

                IPAddress ipAddress = IPAddress.Parse("10.104.182.224");//10.104.182.224
                IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 5000);
                Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //TcpClient tcpClient = new TcpClient();
                listener.Bind(localEndPoint);
                listener.Listen(100);
                while (true)
                {
                    allDone.Reset();
                    listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);
                    allDone.WaitOne();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("error1:" + e);
            }

        }
        public static void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                allDone.Set();
                Socket listener = (Socket)ar.AsyncState;
                Socket handler = listener.EndAccept(ar);
                StateObject state = new StateObject();
                state.workSocket = handler;
                //Console.WriteLine("001: "+state.buffer.Length);
                handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
            }
            catch (Exception e)
            {
                Console.WriteLine("error2:" + e);
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
                    if (!string.IsNullOrEmpty(content) && content.IndexOf("&") > -1)
                    {
                        #region 业务逻辑
                        content = content.Remove(content.LastIndexOf("&"));
                        string[] data = content.Split('&');
                        string userKey = string.Empty;
                        int intGatewayNo = 0;
                        string result = string.Empty;

                        
                        for (int i = 0; i < data.Length; i++)
                        {
                            Dictionary<string, object> dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(data[i]);
                            if (!dic.ContainsKey("userkey") || string.IsNullOrEmpty(dic["userkey"].ToString()))
                            {
                                Console.WriteLine("未接收到“userkey”字段或值为空");
                            }
                            if (!dic.ContainsKey("gatewayNo") || string.IsNullOrEmpty(dic["gatewayNo"].ToString()))
                            {
                                Console.WriteLine("未接收到“gatewayNo”字段或值为空");
                            }
                            if (!int.TryParse(dic["gatewayNo"].ToString(), out intGatewayNo) || intGatewayNo <= 0)
                            {
                                Console.WriteLine("非法网关标识");
                            }
                            var owner = Service.IsExistUser(dic["userkey"].ToString());
                            if (owner == null)
                            {
                                Console.WriteLine("非法用户");
                            }

                            userKey = owner.Id;
                            if (!dic.ContainsKey("data") || string.IsNullOrEmpty(dic["data"].ToString()))
                            {
                                Console.WriteLine("参数缺失，未接收到“data”字段");
                            }
                            List<EquipmentData> dataList = JsonConvert.DeserializeObject<List<EquipmentData>>(dic["data"].ToString());
                            if (dataList == null && dataList.Count <= 0)
                            {
                                Console.WriteLine("json反序列化异常");
                            }

                            int equipmentId = Service.GetEquipmentId(userKey, intGatewayNo);
                            if (equipmentId <= 0)
                            {
                                Console.WriteLine("不存在Id为" + equipmentId + "的设备");
                            }
                            Console.WriteLine("设备Id:" + equipmentId + "       接收时间：" + DateTime.Now);
                            Console.WriteLine(dataList[0].Name + ":" + dataList[0].Value);
                            Console.WriteLine("------------------------------------------------------------");
                            //更新设备数据
                            Service.UploadData(equipmentId, dataList);
                            userKey = string.Empty;
                            intGatewayNo = 0;
                        }
                        result = "success";

                        #endregion
                        Send(handler, ""); //Send(handler, result);
                    }
                    else if (!string.IsNullOrEmpty(content) && content.Contains("key"))
                    {
                        Console.WriteLine(content);
                        Send(handler, content);
                    }
                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("error3:" + e);
                //result = "fail";
            }
        }
        private static void Send(Socket handler, String data)
        {
            try
            {
                byte[] byteData = Encoding.ASCII.GetBytes(data);
                handler.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), handler);
            }
            catch (Exception e)
            {
                Console.WriteLine("error4:" + e);
            }
           
        }
        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket handler = (Socket)ar.AsyncState;
                int bytesSent = handler.EndSend(ar);
                
                //Console.WriteLine("Sent {0} bytes to client.", bytesSent);
                //handler.Shutdown(SocketShutdown.Both);
                //handler.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("error5:" + e);
            }
        }

    }
}
