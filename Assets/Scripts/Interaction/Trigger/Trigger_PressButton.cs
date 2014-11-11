using UnityEngine;
using System.Collections;

using AssemblyCSharp;

public class Trigger_PressButton : MonoBehaviour 
{	
	public Collider m_collider;
	
	public Interaction[] m_interactions;
	
	public InputButton m_button;
	
	public bool m_autoReset;
	
	private GameObject m_player;
	
	private bool m_showFeedback;
	
	private Texture2D m_texFeedback;
	
	private bool m_waitForInteractionEnd;
	
	// Use this for initialization
	void Start () 
	{
		m_waitForInteractionEnd = false;
		m_player = GameObjectHelper.getPlayer();
		m_showFeedback = false;
		
		m_texFeedback = InputHelper.GetButtonTexture(m_button);
		if(!m_texFeedback)
			Debug.LogError("texture Xbox360_Button_A missing");
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(m_autoReset && m_waitForInteractionEnd)
			updateAutoReset();
		
		if(!m_showFeedback)
			return;
		
		string buttonName = InputHelper.GetButtonName(m_button);
		if(Input.GetButtonDown(buttonName))
		{
			foreach(Interaction inter in m_interactions)
				inter.enabled = true;
			
			if(m_autoReset)
			{
				m_waitForInteractionEnd = true;
				m_showFeedback = false;
			}
			else
				enabled = false;
		}
	}
	
	void OnTriggerEnter(Collider _col)
	{
		if(m_waitForInteractionEnd)
			return;
		
		if(_col.gameObject.name != GameObjectHelper.getPlayerName())
			return;
		
		m_showFeedback = true;
	}
	
	void OnTriggerStay(Collider _col)
	{
		if(m_waitForInteractionEnd)
			return;
		
		if(_col.gameObject.name != GameObjectHelper.getPlayerName())
			return;
		
		m_showFeedback = true;
	}
	
	void OnTriggerExit(Collider _col)
	{
		if(m_waitForInteractionEnd)
			return;
		
		if(_col.gameObject.name != GameObjectHelper.getPlayerName())
			return;
		
		m_showFeedback = false;
	}
	
	void OnGUI()
	{
		if(m_showFeedback)
			showFeedback();
	}
	
	void showFeedback()
	{
		Bounds aabb = transform.renderer.bounds;
		Vector3 worldPos = aabb.center + Vector3.up * aabb.extents.y;
		
		Camera camera = GameObjectHelper.getCamera();
		Vector3 screenPos = camera.WorldToScreenPoint(worldPos);
		
		const int size = 32;
		const int halfSize = size / 2;
		Rect texRect = new Rect(screenPos.x - halfSize , Screen.height - screenPos.y - size, size, size);
		GUI.DrawTexture(texRect, m_texFeedback, ScaleMode.ScaleToFit, true, 0);
	}
	
	void updateAutoReset()
	{
		foreach(Interaction inter in m_interactions)
		{
			if(inter.enabled)
				return;
		}
		
		m_waitForInteractionEnd = false;
			
	}
}
