using System;
using System.Collections.Generic;
using UnityEngine;

namespace CinemaDirector
{
    [Serializable]
    public class CinemaMultiActorCurveClip : CinemaClipCurve
    {
        public List<Component> Components = new List<Component>();
        public List<string> Properties = new List<string>();

        private List<object> cache = new List<object>();
        public CinemaMultiActorCurveClip()
        {
            CurveData.Add(new MemberClipCurveData());
        }

        public void SampleTime(float time)
        {
            if (Firetime <= time && time <= Firetime + Duration)
            {
                MemberClipCurveData data = CurveData[0];
                if (data == null) return;

                if (data.PropertyType == PropertyTypeInfo.None)
                {
                    return;
                }

                for (int i = 0; i < Components.Count; i++)
                {
                    object value = null;
                    switch (data.PropertyType)
                    {
                        case PropertyTypeInfo.Color:
                            Color c;
                            c.r = data.Curve1.Evaluate(time);
                            c.g = data.Curve2.Evaluate(time);
                            c.b = data.Curve3.Evaluate(time);
                            c.a = data.Curve4.Evaluate(time);
                            value = c;
                            break;

                        case PropertyTypeInfo.Double:
                        case PropertyTypeInfo.Float:
                        case PropertyTypeInfo.Int:
                        case PropertyTypeInfo.Long:
                            value = data.Curve1.Evaluate(time);
                            break;

                        case PropertyTypeInfo.Quaternion:
                            Quaternion q;
                            q.x = data.Curve1.Evaluate(time);
                            q.y = data.Curve2.Evaluate(time);
                            q.z = data.Curve3.Evaluate(time);
                            q.w = data.Curve4.Evaluate(time);
                            value = q;
                            break;

                        case PropertyTypeInfo.Vector2:
                            Vector2 v2;
                            v2.x = data.Curve1.Evaluate(time);
                            v2.y = data.Curve2.Evaluate(time);
                            value = v2;
                            break;

                        case PropertyTypeInfo.Vector3:
                            Vector3 v3;
                            v3.x = data.Curve1.Evaluate(time);
                            v3.y = data.Curve2.Evaluate(time);
                            v3.z = data.Curve3.Evaluate(time);
                            value = v3;
                            break;

                        case PropertyTypeInfo.Vector4:
                            Vector4 v4;
                            v4.x = data.Curve1.Evaluate(time);
                            v4.y = data.Curve2.Evaluate(time);
                            v4.z = data.Curve3.Evaluate(time);
                            v4.w = data.Curve4.Evaluate(time);
                            value = v4;
                            break;
                    }
                    if (Components[i] != null && Properties[i] != null && Properties[i] != "None")
                    {
                        Components[i].GetType().GetProperty(Properties[i]).SetValue(Components[i], value, null);
                    }
                }
            }
        }

        public List<Transform> Actors
        {
            get
            {
                List<Transform> actors = new List<Transform>();
                if (transform.parent != null)
                {
                    MultiCurveTrack track = transform.parent.GetComponent<MultiCurveTrack>();
                    MultiActorTrackGroup trackgroup = (track.TrackGroup as MultiActorTrackGroup);
                    actors = trackgroup.Actors;
                }
                return actors;
            }
        }

        public override void Initialize()
        {
            MemberClipCurveData data = CurveData[0];
            if (data == null) return;
            cache.Clear();

            for (int i = 0; i < Actors.Count; i++)
            {
                object value = null;
                if (Components[i] != null && Properties[i] != null && Properties[i] != "None")
                {
                    value = Components[i].GetType().GetProperty(Properties[i]).GetValue(Components[i], null);
                }
                cache.Add(value);
            }
        }

        internal void Revert()
        {
            if (CurveData.Count <= 0) return;
            for (int i = 0; i < Actors.Count; i++)
            {
                if (Components[i] != null && Properties[i] != null && Properties[i] != "None")
                {
                    Components[i].GetType().GetProperty(Properties[i]).SetValue(Components[i], cache[i], null);
                }
            }
        }
    }
}