using System;
using UnityEditor;
using UnityEngine;
using CinemaDirector;

/// <summary>
/// Actor Item Track Control
/// </summary>
[CutsceneTrackAttribute(typeof(ActorItemTrack))]
public class ActorItemTrackControl : TimelineTrackControl
{
    protected override void updateHeaderControl3(UnityEngine.Rect position)
    {
        ActorItemTrack itemTrack = TargetTrack.Behaviour as ActorItemTrack;
        if (itemTrack == null) return;

        Color temp = GUI.color;
        GUI.color = (itemTrack.TimelineItems.Length > 0) ? Color.green : Color.red;

        if (GUI.Button(position, string.Empty, TrackGroupControl.styles.addIcon))
        {
            addNewActorItem(itemTrack);
        }
        GUI.color = temp;
    }

    private void addNewActorItem(ActorItemTrack itemTrack)
    {
        GenericMenu createMenu = new GenericMenu();

        Type actorActionType = typeof(CinemaActorAction);
        foreach (Type type in DirectorHelper.GetAllSubTypes(actorActionType))
        {
            ContextData userData = getContextDataFromType(type);
            string text = string.Format("{0}/{1}", userData.Category, userData.Label);
            createMenu.AddItem(new GUIContent(text), false, new GenericMenu.MenuFunction2(AddActorAction), userData);
        }

        Type actorEventType = typeof(CinemaActorEvent);
        foreach (Type type in DirectorHelper.GetAllSubTypes(actorEventType))
        {
            ContextData userData = getContextDataFromType(type);
            string text = string.Format("{0}/{1}", userData.Category, userData.Label);
            createMenu.AddItem(new GUIContent(text), false, new GenericMenu.MenuFunction2(AddActorEvent), userData);
        }
        createMenu.ShowAsContext();
    }

    private ContextData getContextDataFromType(Type type)
    {
        string category = string.Empty;
        string label = string.Empty;
        foreach (CutsceneItemAttribute attribute in type.GetCustomAttributes(typeof(CutsceneItemAttribute), true))
        {
            if (attribute != null)
            {
                category = attribute.Category;
                label = attribute.Label;
                break;
            }
        }
        ContextData userData = new ContextData { Type = type, Label = label, Category = category };
        return userData;
    }

    private void AddActorAction(object userData)
    {
        ContextData data = userData as ContextData;
        if (data != null)
        {
            string name = DirectorHelper.getCutsceneItemName(data.Label, data.Type);

            GameObject item = CutsceneItemFactory.CreateActorAction((TargetTrack.Behaviour as ActorItemTrack), data.Type, name, state.ScrubberPosition).gameObject;
            Undo.RegisterCreatedObjectUndo(item, string.Format("Created {0}", item.name));
        }
    }

    private void AddActorEvent(object userData)
    {
        ContextData data = userData as ContextData;
        if (data != null)
        {
            string name = DirectorHelper.getCutsceneItemName(data.Label, data.Type);

            float firetime = state.IsInPreviewMode ? state.ScrubberPosition : 0f;
            GameObject item = CutsceneItemFactory.CreateActorEvent((TargetTrack.Behaviour as ActorItemTrack), data.Type, name, firetime).gameObject;
            Undo.RegisterCreatedObjectUndo(item, string.Format("Created {0}", item.name));
        }
    }

    private class ContextData
    {
        public Type Type;
        public string Category;
        public string Label;
    }

    protected override void showBodyContextMenu(Event evt)
    {
        ActorItemTrack itemTrack = TargetTrack.Behaviour as ActorItemTrack;
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
        public ActorItemTrack track;

        public PasteContext(Vector2 mousePosition, ActorItemTrack track)
        {
            this.mousePosition = mousePosition;
            this.track = track;
        }
    }
}
