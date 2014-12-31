using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class CameraBehavior : MonoBehaviour 
{
    public float m_GameObjectAlpha;
	#region Variables Following
	
    [Header("Following a target")]
	public float m_CameraDistance;
	public float m_CameraHeight;
	public float m_CameraRotationSpeed;
	public float m_DampingSpeed;
	
	public GameObject m_Player_GameObject;
	
	public float m_FollowingStateMinHeight;
	public float m_FollowingStateMaxHeight;

    public float m_InitialOrientation;
	#endregion
	
	#region Variables Travelling
	
    [Header("Travelling to a point")]
	public float m_TravellingDuration;
	
	#endregion

	private FSMRunner m_Runner;
	
	private CameraStateTravelling m_StateTravelling;
	
	// Use this for initialization
	void Start () 
	{
		m_Runner = new FSMRunner(gameObject);
		
        m_Runner.RegisterState<CameraStateFollowing>();
        m_StateTravelling = m_Runner.RegisterState<CameraStateTravelling>();
        m_Runner.RegisterState<CameraStateFixed>();
		
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
