using Assets.Scripts.Helper;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class Sequence_AudioPlayer : MonoBehaviour 
{
    //public AudioSource m_AudioSource;
    public bool m_Loop;
    public float m_Volume;
    public float m_FadeOutDuration;

    private bool m_FadeOut;
    private DamperFloat m_VolumeDamper;

	// Use this for initialization
	void Start () 
    {
        audio.loop = m_Loop;
        m_FadeOut = false;
        m_VolumeDamper = new DamperFloat(m_Volume / m_FadeOutDuration);
	}
	
	// Update is called once per frame
	void FixedUpdate () 
    {
	    if(m_FadeOut)
        {
            m_VolumeDamper.Speed = 1f / m_FadeOutDuration;
            m_VolumeDamper.CurrentValue = audio.volume;
            m_VolumeDamper.IdealValue = 0;
            audio.volume = m_VolumeDamper.ComputeValue(Time.fixedDeltaTime);
        }
	}

    public void Play()
    {
        audio.Play();
    }

    public void Stop()
    {
        audio.Stop();
    }

    public void Pause()
    {
        audio.Pause();
    }

    public void FadeOut()
    {
        m_FadeOut = true;
    }
}
