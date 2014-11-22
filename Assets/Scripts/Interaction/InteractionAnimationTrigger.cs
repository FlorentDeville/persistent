using UnityEngine;
using System.Collections;

public class InteractionAnimationTrigger : Interaction 
{
    public string m_AnimationTrigger;

    public override void ExecuteOnEnable()
    {
        Animator anim = gameObject.GetComponent<Animator>();
        anim.SetTrigger(m_AnimationTrigger);
    }
}
