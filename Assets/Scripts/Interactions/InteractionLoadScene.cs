using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Interactions
{
    public class InteractionLoadScene : Interaction 
    {
        public string m_SceneName;

        public override void ExecuteOnEnable()
        {
            UnityEngine.Application.LoadLevel(m_SceneName);
        }
    }
}
