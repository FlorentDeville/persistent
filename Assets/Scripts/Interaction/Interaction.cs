using UnityEngine;
using System.Collections;

public class Interaction : MonoBehaviour 
{

	//Owner of this interaction
	protected GameObject m_owner;
	
	void Awake()
	{
		ExecuteAwake();
	}
	
	// Use this for initialization
	void Start () 
	{
		ExecuteStart();
	}
	
	// Update is called once per frame
	void Update () 
	{
		ExecuteUpdate();
	}
	
	void OnGUI()
	{
		ExecuteOnGUI();
	}
	
	void OnEnable()
	{
		ExecuteOnEnable();
	}
	
	public virtual void ExecuteAwake(){}
	
	public virtual void ExecuteOnEnable(){}
	
	public virtual void ExecuteStart(){}
	
	public virtual void ExecuteUpdate(){}
	
	public virtual void ExecuteEnd(){}
	
	public virtual void ExecuteReset(){}
	
	public virtual void ExecuteOnGUI(){}
	
}
