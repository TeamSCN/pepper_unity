using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

public class CommPepper2 : MonoBehaviour {
    private byte error;

    // Use this for initialization
    void Start () {
        NetworkTransport.Init();
        ConnectionConfig config = new ConnectionConfig();
        int myReiliableChannelId = config.AddChannel(QosType.Reliable);
        HostTopology topology = new HostTopology(config, 1);

        int hostId = NetworkTransport.AddHost(topology, 8888);
        int connectionId = NetworkTransport.Connect(hostId, "192.16.7.21", 8888, 0, out error);
        //NetworkTransport.Disconnect(hostId, connectionId, out error);
        //NetworkTransport.Send(hostId, connectionId, myReiliableChannelId, buffer, bufferLength, out error);

        Debug.Log(connectionId);
    }
	
	// Update is called once per frame
	void Update () {
        int recHostId;
        int connectionId;
        int channelId;
        byte[] recBuffer = new byte[1024];
        int bufferSize = 1024;
        int dataSize;
        byte error;
        NetworkEventType recData = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer, bufferSize, out dataSize, out error);
        bool sendData = NetworkTransport.Send(recHostId, connectionId, channelId, recBuffer, dataSize, out error);
        switch (recData)
        {
            case NetworkEventType.Nothing:         //1
                break;
            case NetworkEventType.ConnectEvent:    //2
                break;
            case NetworkEventType.DataEvent:       //3
                break;
            case NetworkEventType.DisconnectEvent: //4
                break;
        }
    }
}
