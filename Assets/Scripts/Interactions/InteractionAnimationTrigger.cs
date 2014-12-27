using UnityEngine;
using System.Collections;

public class InteractionAnimationTrigger : Interaction 
{
    public string m_AnimationTrigger;

    private bool m_AlreadyEnabled = false;

    public override void ExecuteOnEnable()
    {
        if (!m_AlreadyEnabled)
        {
            Animator anim = gameObject.GetComponent<Animator>();
            anim.SetTrigger(m_AnimationTrigger);
            m_AlreadyEnabled = true;
        }
    }
}
