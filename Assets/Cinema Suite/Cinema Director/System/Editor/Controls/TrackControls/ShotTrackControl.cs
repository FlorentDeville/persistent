// Cinema Suite
using UnityEditor;
using UnityEngine;
using CinemaDirector;

/// <summary>
/// Shot Track Control
/// </summary>
[CutsceneTrackAttribute(typeof(ShotTrack))]
public class ShotTrackControl : TimelineTrackControl
{
    /// <summary>
    /// Draw the header control at slot 3. In most cases this is the "Add" button.
    /// </summary>
    /// <param name="position">The position of the track's 3rd header control</param>
    protected override void updateHeaderControl3(Rect position)
    {
        ShotTrack shotTrack = TargetTrack.Behaviour as ShotTrack;

        Color temp = GUI.color;
        GUI.color = (shotTrack.Shots.Length > 0) ? Color.green : Color.red;

        if (GUI.Button(position, string.Empty, TrackGroupControl.styles.addIcon))
        {
            addNewShot(shotTrack);
        }
        GUI.color = temp;
    }

    private void addNewShot(ShotTrack shotTrack)
    {
        GameObject shot = CutsceneItemFactory.CreateNewShot(shotTrack).gameObject;
        Undo.RegisterCreatedObjectUndo(shot, string.Format("Create {0}", shot.name));
    }

    protected override void showBodyContextMenu(Event evt)
    {
        ShotTrack itemTrack = TargetTrack.Behaviour as ShotTrack;
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
        public ShotTrack track;

        public PasteContext(Vector2 mousePosition, ShotTrack track)
        {
            this.mousePosition = mousePosition;
            this.track = track;
        }
    }
}
