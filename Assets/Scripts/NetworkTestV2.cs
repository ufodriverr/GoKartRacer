using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class NetworkTestV2 : MonoBehaviour {

	NetworkClient myClient;

	int port = 4545;

	float ttimer = 0;
	float DCSeconds = 3;

	Coroutine CurCor = null;

	public float NetPing = 0;

	List<float> pingList;

	float pingtime = 0;
	bool SendPing = true;

	ConnectionConfig config;
	HostTopology topology;
	GlobalConfig gConfig;
	int hostId; 
	int connectionId; 

	bool StartRecieve = false;

	void Start(){
		pingList = new List<float> ();
	}

	void Update(){
		if(StartRecieve){
			int recHostId; 
			int connectionId2; 
			int channelId; 
			byte[] recBuffer = new byte[1024]; 
			int bufferSize = 1024;
			int dataSize;
			byte error;
			NetworkEventType recData = NetworkTransport.Receive(out recHostId, out connectionId2, out channelId, recBuffer, bufferSize, out dataSize, out error);
			switch (recData){
			case NetworkEventType.Nothing:
				print ("Nothing");
				break;
			case NetworkEventType.ConnectEvent:
				print ("ConnectEvent");
				break;
			case NetworkEventType.DataEvent:
				print ("DataEvent");
				break;
			case NetworkEventType.DisconnectEvent:
				print ("DisconnectEvent");
				break;
			case NetworkEventType.BroadcastEvent:
				print ("BroadcastEvent");
				break;
			}

		}
	}

	public void StartServer(TMP_InputField IPAddress){
		if (CurCor == null) {
			CurCor = StartCoroutine (StartServerE(IPAddress));
		}
	}

	public void StartClient(TMP_InputField IPAddress){
		if (CurCor == null) {
			CurCor = StartCoroutine (StartClientE(IPAddress));
		}
	}

	IEnumerator StartServerE(TMP_InputField IPAddress){
		BaseInit ();
		hostId = NetworkTransport.AddHost(topology, port);
		byte error;
		StartRecieve = true;
		yield break;
	}

	IEnumerator StartClientE(TMP_InputField IPAddress){
		BaseInit ();
		hostId = NetworkTransport.AddHost(topology, port+1);
		byte error;
		StartRecieve = true;
		connectionId = NetworkTransport.Connect(hostId, IPAddress.text, port, 0, out error);
		yield break;
	}

	void BaseInit(){
		gConfig = new GlobalConfig();
		gConfig.MaxPacketSize = 1500;
		NetworkTransport.Init(gConfig);
		config = new ConnectionConfig();
		int myReliableChannelId  = config.AddChannel(QosType.Reliable);
		int myUnreliableChannelId = config.AddChannel(QosType.Unreliable);
		topology = new HostTopology(config, 10);

	}

}
