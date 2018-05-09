using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class NetworkTest : MonoBehaviour {

	NetworkClient myClient;

	int port = 4545;

	float ttimer = 0;
	float DCSeconds = 3;

	Coroutine CurCor = null;

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
		if(NetworkServer.active){
			Debug.Log ("CurConnections:"+NetworkServer.connections.Count);
		}
	}

	IEnumerator StartServerE(){
		ttimer = 0;
		NetworkServer.Listen (port);
		NetworkServer.RegisterHandler (MsgType.Connect,OnSConnected);
		NetworkServer.RegisterHandler (MsgType.Disconnect,OnSDConnected);
		while (!NetworkServer.active) {
			Debug.Log ("Still creating server");
			ttimer += Time.deltaTime;
			if (ttimer > DCSeconds) {
				Debug.Log ("Cant create server");
				yield return null;
			}
			yield return new WaitForEndOfFrame ();
		}
		Debug.Log ("Server created");
		yield return null;
	}

	IEnumerator StartClientE(TMP_InputField IPAddress){
		ttimer = 0;
		myClient = new NetworkClient();
		myClient.RegisterHandler(MsgType.Connect, OnConnected);     
		myClient.Connect(IPAddress.text, port);
		while (!myClient.isConnected) {
			Debug.Log ("Still connecting to server");
			ttimer += Time.deltaTime;
			if (ttimer > DCSeconds) {
				Debug.Log ("Cant connect to server");
				yield return null;
			}
			yield return new WaitForEndOfFrame ();
		}
		yield return null;
	}

	public void OnConnected(NetworkMessage netMsg)
	{
		Debug.Log("Connected to server");
	}

	public void OnSConnected(NetworkMessage netMsg)
	{
		Debug.Log("Some one connected to server:"+netMsg.conn.connectionId);
	}

	public void OnSDConnected(NetworkMessage netMsg)
	{
		Debug.Log("Some one disconnected from server");
	}
}
