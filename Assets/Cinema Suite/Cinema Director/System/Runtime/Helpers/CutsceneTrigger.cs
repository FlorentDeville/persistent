using UnityEngine;
using System.Collections;

namespace CinemaDirector
{
    public class CutsceneTrigger : MonoBehaviour
    {
        public StartMethod StartMethod;
        public Cutscene Cutscene;
        public GameObject TriggerObject;

        private bool hasTriggered = false;

        // Use this for initialization
        void Start()
        {
            if (StartMethod == StartMethod.OnStart && Cutscene != null)
            {
                hasTriggered = true;
                Cutscene.Play();
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (!hasTriggered && other.gameObject == TriggerObject)
            {
                hasTriggered = true;
                Cutscene.Play();
            }
        }
    }

    public enum StartMethod
    {
        OnStart,
        OnTrigger,
        None
    }
}