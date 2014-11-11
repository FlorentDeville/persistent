
namespace CinemaDirector
{
    /// <summary>
    /// The Director Group maintains tracks that are not attached to specific actors.
    /// </summary>
    public class DirectorGroup : TrackGroup
    {
        /// <summary>
        /// Get the TimelineTracks associated with this group.
        /// </summary>
        /// <returns>A list of global tracks</returns>
        public override TimelineTrack[] GetTracks()
        {
            return GetComponentsInChildren<GlobalTrack>();
        }
    }
}