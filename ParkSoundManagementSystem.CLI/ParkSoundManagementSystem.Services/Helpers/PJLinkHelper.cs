using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Security.Cryptography;


namespace ParkSoundManagementSystem.Services.Helpers
{
    public class PJLinkHelper
    {
        
        private string _hostName = "";
       
        private int _port = 4352;
        
        private bool _useAuth = false;
       
        private string _passwd = "";
        
        private string _pjKey = "";
        
        TcpClient _client = null;
       
        NetworkStream _stream = null;

        #region C'tors

        public PJLinkHelper(string host, int port, string passwd)
        {
            _hostName = host;
            _port = port;
            _passwd = passwd;
            _useAuth = (passwd != "");
        }

        public PJLinkHelper(string host, string passwd)
        {
            _hostName = host;
            _passwd = passwd;
            _useAuth = (passwd != "");
        }

        public PJLinkHelper(string host, int port)
        {
            _hostName = host;
            _port = port;
            _useAuth = false;
        }

        public PJLinkHelper(string host)
        {
            _hostName = host;
            _useAuth = false;
        }

        #endregion

        public Command.Response sendCommand(Command cmd)
        {
            if (initConnection())
            {
                try
                {
                    string cmdString = cmd.getCommandString() + "\r";

                    if (_useAuth && _pjKey != "")
                        cmdString = getMD5Hash(_pjKey + _passwd) + cmdString;

                    byte[] sendBytes = Encoding.ASCII.GetBytes(cmdString);
                    _stream.Write(sendBytes, 0, sendBytes.Length);

                    byte[] recvBytes = new byte[_client.ReceiveBufferSize];
                    int bytesRcvd = _stream.Read(recvBytes, 0, (int)_client.ReceiveBufferSize);
                    string returndata = Encoding.ASCII.GetString(recvBytes, 0, bytesRcvd);
                    returndata = returndata.Trim();
                    cmd.processAnswerString(returndata);
                    return cmd.CmdResponse;
                }
                finally
                {
                    closeConnection();
                }
            }

            return Command.Response.COMMUNICATION_ERROR;
        }

        
        public void sendCommandAsync(Command cmd, Command.CommandResultHandler resultCallback)
        {
            System.Threading.Thread t = new System.Threading.Thread((System.Threading.ThreadStart)delegate
            {
                var response = sendCommand(cmd);
                resultCallback(cmd, response);
            });
            t.IsBackground = true;
            t.Start();
        }

        #region Shortcuts

       
        public bool turnOn()
        {
            PowerCommand pc = new PowerCommand(PowerCommand.Power.ON);
            return (sendCommand(pc) == Command.Response.SUCCESS);
        }

       
        public bool turnOff()
        {
            PowerCommand pc = new PowerCommand(PowerCommand.Power.OFF);
            return (sendCommand(pc) == Command.Response.SUCCESS);
        }

      
        public PowerCommand.PowerStatus powerQuery()
        {
            PowerCommand pc3 = new PowerCommand(PowerCommand.Power.QUERY);
            if (sendCommand(pc3) == Command.Response.SUCCESS)
                return pc3.Status;
            return PowerCommand.PowerStatus.UNKNOWN;
        }


        #endregion

        public string HostName
        {
            get { return _hostName; }
        }

        #region private methods

        private bool initConnection()
        {
            try
            {
                if (_client == null || !_client.Connected)
                {
                    _client = new TcpClient(_hostName, _port);
                    _stream = _client.GetStream();
                    byte[] recvBytes = new byte[_client.ReceiveBufferSize];
                    int bytesRcvd = _stream.Read(recvBytes, 0, (int)_client.ReceiveBufferSize);
                    string retVal = Encoding.ASCII.GetString(recvBytes, 0, bytesRcvd);
                    retVal = retVal.Trim();
                    

                    if (retVal.IndexOf("pjlink 0") >= 0)
                    {
                        _useAuth = false;  //pw provided but projector doesn't need it.
                        return true;
                    }
                    else if (retVal.IndexOf("PJLINK 1 ") >= 0)
                    {
                        _pjKey = retVal.Replace("PJLINK 1 ", "");
                        return true;
                    }
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }

        }

        private void closeConnection()
        {
            if (_client != null)
                _client.Close();
            if (_stream != null)
                _stream.Close();
        }

        private string getMD5Hash(string input)
        {
            MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
            byte[] bs = Encoding.ASCII.GetBytes(input);
            byte[] hash = x.ComputeHash(bs);

            string toRet = "";
            foreach (byte b in hash)
            {
                toRet += b.ToString("x2");
            }
            return toRet;
        }

        #endregion
    }
}
