namespace TradingCardMaker.Models;

/// <summary>
/// Animation seconds for outputting gifs
/// </summary>
public class CardAnimate
{
    /// <summary>
    /// How many seconds the animation should last
    /// </summary>
    [JsonPropertyName("seconds")]
    public double Seconds { get; set; } = 3;

    /// <summary>
    /// How many frames per second the animation should be
    /// </summary>
    [JsonPropertyName("fps")]
    public int Fps { get; set; } = 15;

    /// <summary>
    /// The number of times the animation should repeat
    /// </summary>
    /// <remarks>-1 doesn't repeat, 0 repeats infinitely, 1+ repeats that number of times</remarks>
    [JsonPropertyName("repeat")]
    public int Repeat { get; set; } = 0;

    /// <summary>
    /// The total number of frames in the animation
    /// </summary>
    [JsonIgnore]
    public int TotalFrames => (int)(Seconds * Fps);

    /// <summary>
    /// The number of milliseconds between each frame
    /// </summary>
    [JsonIgnore]
    public int FrameDelay => 1000 / Fps;
}
