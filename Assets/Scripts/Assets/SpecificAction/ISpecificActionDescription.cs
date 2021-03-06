﻿using Assets.Scripts.Component.Actions;
using UnityEngine;

namespace Assets.Scripts.Assets.SpecificAction
{
    public class ISpecificActionDescription : ScriptableObject
    {
        public string m_DisplayName;

        public PowerDescription m_Power;

        public ReactionType m_Reaction;

        public Transform m_EventMarker;

        public bool m_ShowHistoryPreview;
    }
}
