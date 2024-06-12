using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections;
using System.Collections.Generic;


// 파이썬에서 전송한 데이터를 받아오기위한 스크립트
public class UDPReceiver : MonoBehaviour
{
    // 파이썬에서 전송한 데이터를 저정하기 위한 변수
    private string _data;
    public string Data { get { return _data; } }


    // UDP 통신에 필요한 변수들
    private Thread receiveThread;
    private UdpClient client;
    [SerializeField]
    private int port = 5052;
    private bool startRecieving = true;
    [SerializeField]
    private bool printToConsole = false;


    public void Start()
    {
        // 데이터를 받아오는 스레드를 실행
        receiveThread = new Thread(
            new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }


    // 데이터를 받아오기 위한 스레드
    private void ReceiveData()
    {   
        client = new UdpClient(port);

        while (startRecieving)
        {
            try
            {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, port);
                byte[] dataByte = client.Receive(ref anyIP);
                _data = Encoding.UTF8.GetString(dataByte);

                if (printToConsole) { print(_data); }
            }
            catch (Exception err)
            {
                print(err.ToString());
            }
        }
    }

}
