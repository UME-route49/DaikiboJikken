using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class SmoothCamera2D : MonoBehaviour
{
    /// The damp time
    public float DampTime = 0.15f;
    /// The velocity
    Vector3 velocity = Vector3.zero;
    /// The target
    public Transform Target;
    /// The minx
    public float MINX = float.NegativeInfinity;
    /// The maxx
    public float MAXX = float.PositiveInfinity;
    /// The miny
    public float MINY = float.NegativeInfinity;
    /// The maxy
    public float MAXY = float.PositiveInfinity;


    // Update is called once per frame
    void Update ()
	{
		if (Target) {
			Vector3 point = GetComponent<Camera> ().WorldToViewportPoint (Target.position);
			Vector3 delta = Target.position - GetComponent<Camera> ().ViewportToWorldPoint (new Vector3 (0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
			Vector3 destination = transform.position + delta;
			destination = new Vector3(
				Mathf.Clamp(destination.x, MINX, MAXX),
				Mathf.Clamp(destination.y, MINY, MAXY),
				destination.z);
			
			transform.position = Vector3.SmoothDamp (transform.position, destination, ref velocity, DampTime);
		}
	}
}
	
