using AssemblyCSharp;
using UnityEngine;

public class Sidekick_Behavior : MonoBehaviour 
{
    private NavMeshAgent m_agent;

	// Use this for initialization
	void Start () 
    {
        m_agent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        m_agent.SetDestination(GameObjectHelper.getPlayer().transform.position);
	}
}
