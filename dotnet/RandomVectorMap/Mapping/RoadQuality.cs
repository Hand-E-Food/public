namespace RandomVectorMap.Mapping
{

    /// <summary>
    /// An enumeration of the various road qualities.
    /// </summary>
    public enum RoadQuality
    {
        /// <summary>
        /// The road quality is yet to be defined.
        /// </summary>
        Undefined,

        /// <summary>
        /// The span cannot be traversed.
        /// </summary>
        None,

        /// <summary>
        /// The road is actually a river.
        /// </summary>
        River,

        /// <summary>
        /// There is no road, but the span can be traversed.
        /// </summary>
        Wild,

        /// <summary>
        /// The road is a dirt track.
        /// </summary>
        Dirt,

        /// <summary>
        /// The road is properly paved.
        /// </summary>
        Paved,

        /// <summary>
        /// The road is a maintained highway.
        /// </summary>
        Highway,
    }
}
