using System;
using System.Collections;
using UnityEngine;


[System.Serializable]
public class Core_DialogPart
{
	public string m_text;
	
	public Core_DialogActor m_actor;
}

[System.Serializable]
public class Core_Dialog : ScriptableObject
{
	public string m_name;
	public Core_DialogPart[] m_parts;
}

