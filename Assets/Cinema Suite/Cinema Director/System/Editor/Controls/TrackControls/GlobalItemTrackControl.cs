using System;
using UnityEditor;
using UnityEngine;
using CinemaDirector;

/// <summary>
/// Global Item Track Control
/// </summary>
[CutsceneTrackAttribute(typeof(GlobalItemTrack))]
public class GlobalItemTrackControl : TimelineTrackControl
{
    protected override void updateHeaderControl3(UnityEngine.Rect position)
    {
        GlobalItemTrack itemTrack = TargetTrack.Behaviour as GlobalItemTrack;
        if (itemTrack == null) return;

        Color temp = GUI.color;
        GUI.color = (itemTrack.TimelineItems.Length > 0) ? Color.green : Color.red;

        if (GUI.Button(position, string.Empty, TrackGroupControl.styles.addIcon))
        {
            addNewGlobalItem(itemTrack);
        }
        GUI.color = temp;
    }

    

    private void addNewGlobalItem(GlobalItemTrack itemTrack)
    {
        GenericMenu createMenu = new GenericMenu();

        foreach (Type type in DirectorHelper.GetAllSubTypes(typeof(CinemaGlobalAction)))
        {
            string text = string.Empty;
            string category = string.Empty;
            string label = string.Empty;
            foreach (CutsceneItemAttribute attribute in type.GetCustomAttributes(typeof(CutsceneItemAttribute), true))
            {
                if (attribute != null)
                {
                    category = attribute.Category;
                    label = attribute.Label;
                    text = string.Format("{0}/{1}", attribute.Category, attribute.Label);
                    break;
                }
            }
            if (label != string.Empty)
            {
                TrackItemInfoContextData userData = new TrackItemInfoContextData { Type = type, Label = label, Category = category };
                createMenu.AddItem(new GUIContent(text), false, new GenericMenu.MenuFunction2(AddGlobalAction), userData);
            }
        }

        foreach (Type type in DirectorHelper.GetAllSubTypes(typeof(CinemaGlobalEvent)))
        {
            string text = string.Empty;
            string category = string.Empty;
            string label = string.Empty;
            foreach (CutsceneItemAttribute attribute in type.GetCustomAttributes(typeof(CutsceneItemAttribute), true))
            {
                if (attribute != null)
                {
                    category = attribute.Category;
                    label = attribute.Label;
                    text = string.Format("{0}/{1}", attribute.Category, attribute.Label);
                    break;
                }
            }
            if (label != string.Empty)
            {
                TrackItemInfoContextData userData = new TrackItemInfoContextData { Type = type, Label = label, Category = category };
                createMenu.AddItem(new GUIContent(text), false, new GenericMenu.MenuFunction2(AddGlobalEvent), userData);
            }
        }
        createMenu.ShowAsContext();
    }

    private void AddGlobalAction(object userData)
    {
        TrackItemInfoContextData data = userData as TrackItemInfoContextData;
        if (data != null)
        {
            string name = DirectorHelper.getCutsceneItemName(data.Label, data.Type);

            float firetime = state.IsInPreviewMode ? state.ScrubberPosition : 0f;
            GameObject item = CutsceneItemFactory.CreateGlobalAction((TargetTrack.Behaviour as GlobalItemTrack), data.Type, name, firetime).gameObject;
            Undo.RegisterCreatedObjectUndo(item, string.Format("Created {0}", item.name));
        }
    }

    private void AddGlobalEvent(object userData)
    {
        TrackItemInfoContextData data = userData as TrackItemInfoContextData;
        if (data != null)
        {
            string name = DirectorHelper.getCutsceneItemName(data.Label, data.Type);

            float firetime =  state.IsInPreviewMode ? state.ScrubberPosition : 0f;
            GameObject item = CutsceneItemFactory.CreateGlobalEvent((TargetTrack.Behaviour as GlobalItemTrack), data.Type, name, firetime).gameObject;
            Undo.RegisterCreatedObjectUndo(item, string.Format("Created {0}", item.name));
        }
    }

    protected override void showBodyContextMenu(Event evt)
    {
        GlobalItemTrack itemTrack = TargetTrack.Behaviour as GlobalItemTrack;
        if (itemTrack == null) return;

        Behaviour b = DirectorCopyPaste.Peek();

        PasteContext pasteContext = new PasteContext(evt.mousePosition, itemTrack);
        GenericMenu createMenu = new GenericMenu();
        if (b != null && DirectorHelper.IsTrackItemValidForTrack(b, itemTrack))
        {
            createMenu.AddItem(new GUIContent("Paste"), false, pasteItem, pasteContext);
        }
        else
        {
            createMenu.AddDisabledItem(new GUIContent("Paste"));
        }
        createMenu.ShowAsContext();
    }

    private void pasteItem(object userData)
    {
        PasteContext data = userData as PasteContext;
        if (data != null)
        {
            float firetime = (data.mousePosition.x - state.Translation.x) / state.Scale.x;

            GameObject clone = DirectorCopyPaste.Paste(data.track.transform);
            clone.GetComponent<TimelineItem>().Firetime = firetime;

            Undo.RegisterCreatedObjectUndo(clone, "Pasted " + clone.name);
        }
    }

    private class PasteContext
    {
        public Vector2 mousePosition;
        public GlobalItemTrack track;

        public PasteContext(Vector2 mousePosition, GlobalItemTrack track)
        {
            this.mousePosition = mousePosition;
            this.track = track;
        }
    }

    private class TrackItemInfoContextData
    {
        public Type Type;
        public string Category;
        public string Label;
    }
}
