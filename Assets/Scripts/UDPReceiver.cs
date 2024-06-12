using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections;
using System.Collections.Generic;


// ���̽㿡�� ������ �����͸� �޾ƿ������� ��ũ��Ʈ
public class UDPReceiver : MonoBehaviour
{
    // ���̽㿡�� ������ �����͸� �����ϱ� ���� ����
    private string _data;
    public string Data { get { return _data; } }


    // UDP ��ſ� �ʿ��� ������
    private Thread receiveThread;
    private UdpClient client;
    [SerializeField]
    private int port = 5052;
    private bool startRecieving = true;
    [SerializeField]
    private bool printToConsole = false;


    public void Start()
    {
        // �����͸� �޾ƿ��� �����带 ����
        receiveThread = new Thread(
            new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }


    // �����͸� �޾ƿ��� ���� ������
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
