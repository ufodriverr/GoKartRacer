using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

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
	int myReliableChannelId;
	int myUnreliableChannelId;
	int connectionId2=-1;
	int recHostId; 

	void Start(){
		pingList = new List<float> ();
	}

	void Update(){
		if (pingList.Count > 0) {
			NetPing = 0;
			for (int i = 0; i < pingList.Count; i++) {
				NetPing += pingList [i];
			}
			NetPing /= pingList.Count;
			if (pingList.Count > 10) {
				pingList.RemoveAt (0);
			}
		}
		if(StartRecieve){
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
				Stream stream = new MemoryStream (recBuffer);
				BinaryFormatter formatter = new BinaryFormatter ();
				string message = formatter.Deserialize (stream) as string;
				print ("incoming message event received: " + message);
				if (message == "Ping") {
					byte[] buffer = new byte[1024];
					Stream sstream = new MemoryStream(buffer);
					formatter.Serialize(sstream, "Pong");
					NetworkTransport.Send(hostId, connectionId, myReliableChannelId, buffer, bufferSize, out error);
				}
				if (message == "Pong") {
					pingList.Add(Mathf.Round(((Time.time - pingtime)*1000)/2));
					SendPing = true;
				}
				print ("DataEvent");
				break;
			case NetworkEventType.DisconnectEvent:
				print ("DisconnectEvent");
				break;
			case NetworkEventType.BroadcastEvent:
				print ("BroadcastEvent");
				break;
			}

			if (SendPing && connectionId2!=-1) {
				print ("SendPing");
				pingtime = Time.time;
				SendPing = false;
				byte[] buffer = new byte[1024];
				Stream stream = new MemoryStream(buffer);
				BinaryFormatter formatter = new BinaryFormatter();
				formatter.Serialize(stream, "Ping");
				NetworkTransport.Send(recHostId, connectionId2, myReliableChannelId, buffer, bufferSize, out error);
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
		myReliableChannelId  = config.AddChannel(QosType.Reliable);
		myUnreliableChannelId = config.AddChannel(QosType.Unreliable);
		topology = new HostTopology(config, 10);

	}

}
