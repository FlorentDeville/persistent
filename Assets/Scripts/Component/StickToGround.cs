using UnityEngine;
using System.Collections.Generic;
using System.Linq;

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
        Renderer[] renderers = _obj.GetComponentsInChildren<Renderer>();
        if (renderers == null || renderers.Length == 0)
            return;

        List<Renderer> listRenderers = new List<Renderer>(renderers);
        float min = listRenderers.Min(item => item.bounds.min.y);
        float offset = _obj.transform.position.y - min;

        //Start a cast to the ground
        RaycastHit hit;
        if (Physics.Raycast(_obj.transform.position, -Vector3.up, out hit))
        {
            //In here, the raycast got a result
            //Compute how much we went through the ground and move the entity up of this amount.
            Vector3 oldPos = _obj.transform.position;
            oldPos.y += offset - hit.distance;
            _obj.transform.position = oldPos;
        }
    }
}
