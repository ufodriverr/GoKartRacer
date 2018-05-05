using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

[ExecuteInEditMode]
public class scr_UpdateSceneViewWhenMouseMove : MonoBehaviour {
	void OnDrawGizmos(){
		//print (Event.current.type);
		if (Event.current.type == EventType.Repaint) {
			SceneView.RepaintAll ();
		}
		transform.Rotate (Vector3.up);
	}
}
#endif
