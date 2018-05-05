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

    void Awake()
    {
        instance = this;
    }
}
