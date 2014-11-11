using System.Collections.Generic;
using UnityEngine;

namespace CinemaDirector
{
    [CutsceneItem("Utility", "Mass Disabler")]
    public class MassDisabler : CinemaGlobalAction
    {
        public List<GameObject> GameObjects = new List<GameObject>();
        public List<string> Tags = new List<string>();

        private List<GameObject> tagsCache = new List<GameObject>();

        public override void Trigger()
        {
            tagsCache.Clear();
            foreach (string tag in Tags)
            {
                GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(tag);
                foreach (GameObject gameObject in gameObjects)
                {
                    tagsCache.Add(gameObject);
                }
            }

            setActive(false);
        }

        public override void End()
        {
            setActive(true);
        }

        private void setActive(bool enabled)
        {
            // Enable gameobjects
            foreach (GameObject gameObject in GameObjects)
            {
                gameObject.SetActive(enabled);
            }

            // Enable tags
            foreach (GameObject gameObject in tagsCache)
            {
                gameObject.SetActive(enabled);
            }
        }
    }
}