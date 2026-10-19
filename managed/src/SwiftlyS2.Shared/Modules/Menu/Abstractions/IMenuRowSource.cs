using SwiftlyS2.Shared.Players;

namespace SwiftlyS2.Shared.Menu;

public readonly record struct MenuRowContext
{
    /// <summary>
    /// Looks up a menu translation by key.
    /// </summary>
    public required Func<string, string> Translate { get; init; }

    /// <summary>
    /// Resolves player searches.
    /// </summary>
    public required IPlayerManagerService Players { get; init; }
}

public interface IMenuRowSource
{
    /// <summary>
    /// The body entries this component stands for right now.
    /// </summary>
    /// <param name="session">The session the rows are for.</param>
    /// <param name="context">Services the rows need.</param>
    public IReadOnlyList<IMenuComponent> GetRows( IMenuSession session, MenuRowContext context );

    /// <summary>
    /// Raised when <see cref="GetRows"/> would return something different.
    /// </summary>
    public event Action? RowsChanged;
}
