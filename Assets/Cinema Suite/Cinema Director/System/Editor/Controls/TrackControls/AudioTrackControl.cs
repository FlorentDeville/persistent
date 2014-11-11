// Cinema Suite
using UnityEditor;
using UnityEngine;
using CinemaDirector;

/// <summary>
/// Audio Track Control
/// </summary>
[CutsceneTrackAttribute(typeof(AudioTrack))]
public class AudioTrackControl : TimelineTrackControl
{
    protected override void updateHeaderControl3(UnityEngine.Rect position)
    {
        AudioTrack track = TargetTrack.Behaviour as AudioTrack;
        if (track == null) return;

        Color temp = GUI.color;
        GUI.color = (track.AudioClips.Length > 0) ? Color.green : Color.red;

        int controlID = GUIUtility.GetControlID(track.GetInstanceID(), FocusType.Passive, position);
        if (GUI.Button(position, string.Empty, TrackGroupControl.styles.addIcon))
        {
            EditorGUIUtility.ShowObjectPicker<AudioClip>(null, false, string.Empty, controlID);
        }
        
        if (Event.current.type == EventType.ExecuteCommand && Event.current.commandName == "ObjectSelectorClosed")
        {
            if (EditorGUIUtility.GetObjectPickerControlID() == controlID)
            {
                Object pickedObject = EditorGUIUtility.GetObjectPickerObject();
                AudioClip clip = (pickedObject as AudioClip);
                if (clip != null)
                {
                    CinemaAudio ca = CutsceneItemFactory.CreateCinemaAudio(track, clip, state.ScrubberPosition);
                    Undo.RegisterCreatedObjectUndo(ca, string.Format("Created {0}", ca.name));
                }
                Event.current.Use();
            }
        }
        GUI.color = temp;
    }

    public override void UpdateTrackContents(DirectorControlState state, Rect position)
    {
        handleDragInteraction(position, TargetTrack.Behaviour as AudioTrack, state.Translation, state.Scale);
        base.UpdateTrackContents(state, position);
    }

    private void handleDragInteraction(Rect position, AudioTrack track, Vector2 translation, Vector2 scale)
    {
        Rect controlBackground = new Rect(0, 0, position.width, position.height);
        switch (Event.current.type)
        {
            case EventType.DragUpdated:
                if (controlBackground.Contains(Event.current.mousePosition))
                {
                    bool audioFound = false;
                    foreach (Object objectReference in DragAndDrop.objectReferences)
                    {
                        AudioClip clip = objectReference as AudioClip;
                        if (clip != null)
                        {
                            audioFound = true;
                            break;
                        }
                    }
                    if (audioFound)
                    {
                        DragAndDrop.visualMode = DragAndDropVisualMode.Link;
                        Event.current.Use();
                    }
                }
                break;
            case EventType.DragPerform:
                if (controlBackground.Contains(Event.current.mousePosition))
                {
                    AudioClip clip = null;
                    foreach (Object objectReference in DragAndDrop.objectReferences)
                    {
                        AudioClip audioClip = objectReference as AudioClip;
                        if (audioClip != null)
                        {
                            clip = audioClip;
                            break;
                        }
                    }
                    if (clip != null)
                    {
                        float fireTime = (Event.current.mousePosition.x - translation.x) / scale.x;
                        CinemaAudio ca = CutsceneItemFactory.CreateCinemaAudio(track, clip, fireTime);
                        Undo.RegisterCreatedObjectUndo(ca, string.Format("Created {0}", ca.name));
                        Event.current.Use();
                    }
                }
                break;
        }
    }

    protected override void showBodyContextMenu(Event evt)
    {
        AudioTrack itemTrack = TargetTrack.Behaviour as AudioTrack;
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
        public AudioTrack track;

        public PasteContext(Vector2 mousePosition, AudioTrack track)
        {
            this.mousePosition = mousePosition;
            this.track = track;
        }
    }
}
