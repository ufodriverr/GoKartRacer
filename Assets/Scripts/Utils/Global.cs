using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class Global : MonoBehaviour
{

    private static Global instance;

    public static Global Instance { get { return instance; } }

	public float SFXVolume = 1;
	public float MusicVolume = 1;

	public Transform[] MapNodes;
	public Transform[] MapSpawnPoints;

	public KartAI KartAI;

	public Rigidbody TntItem;

	public Transform ItemsParent;

    void Awake()
    {
        instance = this;
		SpawnAIs ();
    }

	void SpawnAIs(){
		for (int i = 0; i < MapSpawnPoints.Length; i++) {
			Instantiate (KartAI,MapSpawnPoints[i].transform.position,MapSpawnPoints[i].transform.rotation).SpawnPointID = i;
		}
	}
}
