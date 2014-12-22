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
        Apply(gameObject);
	}

    public static void Apply(GameObject _obj)
    {
        if (_obj.renderer == null)
            return;

        //Start a cast to the ground
        RaycastHit hit;
        if (Physics.Raycast(_obj.transform.position, -Vector3.up, out hit))
        {
            //In here, the raycast got a result
            //Compute how much we went through the ground and move the entity up of this amount.
            Vector3 oldPos = _obj.transform.position;
            oldPos.y += _obj.transform.renderer.bounds.extents.y - hit.distance;
            _obj.transform.position = oldPos;
        }
    }
}
