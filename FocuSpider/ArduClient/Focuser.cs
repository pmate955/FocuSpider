﻿using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FocuSpider
{
    public class Focuser
    {
        public Focuser()
        {
            this.IsConnected = false;
            this.IsBusy = false;
        }

        public bool IsConnected { get; private set; }
        public bool IsBusy { get; private set; }
        private SerialPort _port;

        public async Task Step(bool isForward, int steps)
        {
            if (!this.IsConnected) return;

            await Task.Run(() =>
            {
                try
                {
                    this.IsBusy = true;
                    string command = isForward ? "+" : "-" + steps;
                    //this._port.WriteLine(command);
                    //string response = this._port.ReadLine();
                    Thread.Sleep(500);
                }
                catch (Exception ex)
                {
                    Logger.Log(LogLevel.ERROR, ex.ToString());
                }

                this.IsBusy = false;
            });
        }

        public bool TryConnect(string portName)
        {
            try
            {
                this._port = new SerialPort(portName);
                this._port.BaudRate = 57600;                                                         //Baudrate
                this._port.StopBits = StopBits.One;
                this._port.Parity = Parity.None;
                this._port.DataBits = 8;
                this._port.DtrEnable = true;
                this._port.Open();
                this.IsConnected = true;
                return true;
            }
            catch (Exception e)
            {
                Logger.Log(LogLevel.ERROR, e.ToString());
            }

            return false;
        }

        public bool Disconnect()
        {
            try
            {
                this._port.Close();
                this._port.Dispose();
                this._port = null;
                this.IsConnected = false;
                return true;
            }
            catch (Exception e)
            {
                Logger.Log(LogLevel.ERROR, e.ToString());
            }

            return false;
        }
        //parancs küldés, válaszra várással

    }
}
