using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;

public class MyMessageTypes
{
	public static short Ping = 1005;
	public static short Pong = 1005;
};

public class TipTap:MessageBase{
	
};

public class NetworkTest : MonoBehaviour {

	NetworkClient myClient;

	int port = 4545;

	float ttimer = 0;
	float DCSeconds = 3;

	Coroutine CurCor = null;

	public float NetPing = 0;

	List<float> pingList;

	float pingtime = 0;
	bool SendPing = true;

	void Start(){
		pingList = new List<float> ();
	}

	public void StartServer(){
		if (CurCor == null) {
			CurCor = StartCoroutine (StartServerE());
		}
	}

	public void StartClient(TMP_InputField IPAddress){
		if (CurCor == null) {
			CurCor = StartCoroutine (StartClientE(IPAddress));
		}
	}

	void Update(){
		if (myClient != null) {
			if (myClient.isConnected && SendPing) {
				pingtime = Time.time;
				myClient.Send (MyMessageTypes.Ping,new TipTap());
				SendPing = false;
			}
		}
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
	}



	IEnumerator StartServerE(){
		ttimer = 0;
		ConnectionConfig config = new ConnectionConfig ();
		config.AddChannel(QosType.ReliableSequenced);
		config.AddChannel(QosType.UnreliableSequenced);
		config.PacketSize = 1500;
		config.SendDelay = 0;
		config.AckDelay = 0;
		NetworkServer.Configure (config,8);
		NetworkServer.Listen (port);
		NetworkServer.RegisterHandler (MsgType.Connect,OnSConnected);
		NetworkServer.RegisterHandler (MsgType.Disconnect,OnSDConnected);
		NetworkServer.RegisterHandler (MyMessageTypes.Ping,OnPingS);
		while (!NetworkServer.active) {
			Debug.Log ("Still creating server");
			ttimer += Time.deltaTime;
			if (ttimer > DCSeconds) {
				Debug.Log ("Cant create server");
				yield break;
			}
			yield return new WaitForEndOfFrame ();
		}
		Debug.Log ("Server created");
		yield break;
	}

	IEnumerator StartClientE(TMP_InputField IPAddress){
		ttimer = 0;
		myClient = new NetworkClient();
		ConnectionConfig config = new ConnectionConfig ();
		config.AddChannel(QosType.ReliableSequenced);
		config.AddChannel(QosType.UnreliableSequenced);
		config.PacketSize = 1500;
		config.SendDelay = 0;
		config.AckDelay = 0;
		myClient.Configure (config,8);
		myClient.RegisterHandler(MsgType.Connect, OnConnected);     
		myClient.RegisterHandler(MyMessageTypes.Pong, OnPong);     
		myClient.Connect(IPAddress.text, port);
		while (!myClient.isConnected) {
			Debug.Log ("Still connecting to server");
			ttimer += Time.deltaTime;
			if (ttimer > DCSeconds) {
				Debug.Log ("Cant connect to server");
				yield break;
			}
			yield return new WaitForEndOfFrame ();
		}
		yield break;
	}

	//Client messages

	public void OnConnected(NetworkMessage netMsg)
	{
		Debug.Log("Connected to server");
	}

	public void OnPong(NetworkMessage netMsg){
		pingList.Add(Mathf.Round(((Time.time - pingtime)*1000)/2));

		SendPing = true;
	}
	//Server messages

	public void OnSConnected(NetworkMessage netMsg)
	{
		Debug.Log("Some one connected to server:"+netMsg.conn.connectionId);
	}

	public void OnSDConnected(NetworkMessage netMsg)
	{
		Debug.Log("Some one disconnected from server");
	}

	public void OnPingS(NetworkMessage netMsg){
		NetworkServer.SendToClient (netMsg.conn.connectionId,MyMessageTypes.Pong,new TipTap());
	}
}
