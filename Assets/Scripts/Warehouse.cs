using UnityEngine;
using System.Collections.Generic;

public class Warehouse : MonoBehaviour 
{
    public List<Core_DialogActor> m_DialogActors;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public Core_DialogActor GetActor(string _name)
    {
        return m_DialogActors.Find(item => item.m_name == _name);
    }
}
