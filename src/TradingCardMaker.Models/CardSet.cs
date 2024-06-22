namespace TradingCardMaker.Models;

using Drawing;

/// <summary>
/// Represents all of the designs available for a trading card
/// </summary>
public class CardSet
{
    private CardUnit? _width;
    private CardUnit? _height;
    private CardUnit? _fontSize;
    private CardUnitContextData? _context;

    /// <summary>
    /// The name of the card set
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    /// <summary>
    /// The description of the card set
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>
    /// The width of the card (cannot be relative or percentage)
    /// </summary>
    [JsonPropertyName("width")]
    public required CardUnit Width
    {
        get => _width ?? CardUnit.Zero;
        set { _width = value; ClearContext(); }
    }

    /// <summary>
    /// The height of the card (cannot be relative or percentage)
    /// </summary>
    [JsonPropertyName("height")]
    public required CardUnit Height
    {
        get => _height ?? CardUnit.Zero;
        set { _height = value; ClearContext(); }
    }

    /// <summary>
    /// The default font size for the card (cannot be relative or percentage)
    /// </summary>
    [JsonPropertyName("fontSize")]
    public required CardUnit FontSize
    {
        get => _fontSize ?? CardUnit.Zero;
        set { _fontSize = value; ClearContext(); }
    }

    /// <summary>
    /// The render pipeline for the back of the card
    /// </summary>
    [JsonPropertyName("back")]
    public CardFace Back { get; set; } = new();

    /// <summary>
    /// The different types of render pipelines for the front of the card
    /// </summary>
    [JsonPropertyName("variants")]
    public Dictionary<string, CardFace> Variants { get; set; } = [];

    /// <summary>
    /// The resources available to the card
    /// </summary>
    [JsonPropertyName("resources")]
    public CardResources Resources { get; set; } = new();

    /// <summary>
    /// Clears the context for the card, so that it is recalculated on next request
    /// </summary>
    public void ClearContext()
    {
        _context = null;
    }

    /// <summary>
    /// Gets the cached unit context for the card
    /// </summary>
    /// <returns>The unit context for card roots</returns>
    /// <exception cref="NullReferenceException">Thrown if width, height, or font size is null</exception>
    public CardUnitContextData GetContext()
    {
        if (_context is not null) return _context.Value;

        if (_width is null || _height is null || _fontSize is null)
            throw new NullReferenceException("Width, height, or font size is missing");

        var tempRoot = new CardUnitContextData(null, null, null);
        var tempCtx = new CardUnitContext(tempRoot, null);

        var width = _width.Value.Pixels(tempCtx, true);
        var height = _height.Value.Pixels(tempCtx, false);
        var fontSize = _fontSize.Value.Pixels(tempCtx, null);
        return (_context = new CardUnitContextData(width, height, fontSize)).Value;
    }
}
