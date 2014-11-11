using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class Dialog : MonoBehaviour 
{
	public bool m_renderText;
	
	public Texture2D m_avatar;
	
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	void OnGUI()
	{
		const string text = "Etre ou bien n'etre pas, voila la question. Est-il plus noble en notre for de supporter les traits dont nous meurtrit l'outrageuse Fortune, ou bien de s'insurger contre une mer d'ennuis. De lutter et d'en triompher ? Mourir, dormir, Pas davantage, et, par un sommeil mettre fin Aux maux du coeur, aux mille atteintes naturelles, Le lot de toute chair, c'est la un denouement a souhaiter de tout son coeur.";
		if(m_renderText)
			RendererDialog.Render(m_avatar, "Sangoku", text, InputButton.INPUT_BUTTON_A);
	}
}
