using UnityEngine;
using System.Collections;

public class StickToGround : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void LateUpdate () 
	{
		//Start a cast to the ground
		RaycastHit hit;
		if (Physics.Raycast (gameObject.transform.position, -Vector3.up, out hit)) 
		{
			//In here, the raycast got a result
			//Compute how much we went through the ground and move the entity up of this amount.
			Vector3 oldPos = gameObject.transform.position;
			oldPos.y += gameObject.transform.collider.bounds.extents.y - hit.distance;
			gameObject.transform.position = oldPos;
		}
	}
}
