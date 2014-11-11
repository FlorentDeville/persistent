using UnityEditor;
using UnityEngine;
using CinemaDirector;

[CutsceneTrackGroupAttribute(typeof(ActorTrackGroup))]
public class ActorTrackGroupControl : TrackGroupControl
{
    public override void Initialize()
    {
        base.Initialize();
        LabelPrefix = styles.ActorGroupIcon.normal.background;
    }

    protected override void addTrackContext()
    {
        GenericMenu createMenu = new GenericMenu();
        createMenu.AddItem(new GUIContent("Add Actor Track"), false, addActorTrack);
        createMenu.AddItem(new GUIContent("Add Curve Track"), false, addCurveTrack);

        createMenu.ShowAsContext();
    }

    private void addActorTrack()
    {
        ActorTrackGroup atg = TrackGroup.Behaviour as ActorTrackGroup;
        Undo.RegisterCreatedObjectUndo(CutsceneItemFactory.CreateActorItemTrack(atg).gameObject, "Create Actor Track");
    }

    private void addCurveTrack()
    {
        ActorTrackGroup atg = TrackGroup.Behaviour as ActorTrackGroup;
        Undo.RegisterCreatedObjectUndo(CutsceneItemFactory.CreateCurveTrack(atg).gameObject, "Create Curve Track");
    }

    protected override void updateHeaderControl4(Rect position)
    {
        Transform actor = (TrackGroup.Behaviour as ActorTrackGroup).Actor;

        Color temp = GUI.color;

        GUI.color = (actor == null) ? Color.red : Color.green;
        int controlID = GUIUtility.GetControlID("ActorTrackGroupControl".GetHashCode(), FocusType.Passive, position);

        GUI.enabled = !state.IsInPreviewMode;
        if (GUI.Button(position, string.Empty, styles.pickerStyle))
        {
            EditorGUIUtility.ShowObjectPicker<Transform>(actor, true, string.Empty, controlID);
        }
        GUI.enabled = true;

        if (Event.current.commandName == "ObjectSelectorUpdated")
        {
            if (EditorGUIUtility.GetObjectPickerControlID() == controlID)
            {
                GameObject pickedObject = EditorGUIUtility.GetObjectPickerObject() as GameObject;
                if (pickedObject != null)
                {
                    ActorTrackGroup atg = (TrackGroup.Behaviour as ActorTrackGroup);
                    Undo.RecordObject(atg, string.Format("Changed {0}", atg.name));
                    atg.Actor = pickedObject.transform;
                }
            }
        }
        GUI.color = temp;
    }

    protected override void showHeaderContextMenu()
    {
        GenericMenu createMenu = new GenericMenu();
        createMenu.AddItem(new GUIContent("Select"), false, focusActor);
        createMenu.AddItem(new GUIContent("Delete"), false, delete);

        createMenu.ShowAsContext();
    }

    private void focusActor()
    {
        Selection.activeTransform = (TrackGroup.Behaviour as ActorTrackGroup).Actor;
    }
    
}