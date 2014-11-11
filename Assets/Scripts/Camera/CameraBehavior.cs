using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class CameraBehavior : MonoBehaviour 
{
    public float m_GameObjectAlpha;
	#region Variables Following
	
	public float m_CameraDistance;
	public float m_CameraHeight;
	public float m_CameraRotationSpeed;
	public float m_DampingSpeed;
	
	public GameObject m_Player_GameObject;
	
	public float m_FollowingStateMinHeight;
	public float m_FollowingStateMaxHeight;
	
	#endregion
	
	#region Variables Travelling
	
	/// <summary>
	/// The duration of the travelling.
	/// </summary>
	public float m_TravellingDuration;
	
	#endregion
	private FSMRunner m_Runner;
	
	private CameraStateTravelling m_StateTravelling;
	
	// Use this for initialization
	void Start () 
	{
		m_Runner = new FSMRunner();
		
		CameraStateFollowing StateFollowing = new CameraStateFollowing(m_Runner, this.gameObject);
		m_StateTravelling = new CameraStateTravelling(m_Runner, this.gameObject);
		CameraStateFixed StateFixed = new CameraStateFixed();
		
		m_Runner.AddState((int)CameraState.eFollowing, StateFollowing);
		m_Runner.AddState((int)CameraState.eTravelling, m_StateTravelling);
		m_Runner.AddState((int)CameraState.eFixed, StateFixed);
		
		m_Runner.SetCurrentState((int)CameraState.eFollowing, "set initial state");
	}
	
	// Update is called once per frame
	void LateUpdate () 
	{
		m_Runner.Update();
	}
	
	public void FixedCamera()
	{
		m_Runner.SetCurrentState((int)CameraState.eFixed, "fixed camera requested");
	}
	
	public void FollowPlayer()
	{
		m_Runner.SetCurrentState((int)CameraState.eFollowing, "following player requested");
	}
	
	public void Travel(Vector3 Target)
	{
		m_Runner.SetCurrentState((int)CameraState.eTravelling, "travelling to " + Target.ToString() + " requested", Target);
		m_StateTravelling.TravelOver = false;
	}
	
	public bool TravelOver()
	{
		if(m_Runner.GetCurrentState() != (int)CameraState.eTravelling)
			return true;
		
		return m_StateTravelling.TravelOver;
	}
}
