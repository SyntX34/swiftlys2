namespace SwiftlyS2.Shared.Menu;

public interface IMenuRowSource
{
    /// <summary>
    /// The body entries this component stands for right now.
    /// </summary>
    /// <param name="translate">Looks up a menu translation by key.</param>
    public IReadOnlyList<IMenuComponent> GetRows( Func<string, string> translate );

    /// <summary>
    /// Raised when <see cref="GetRows"/> would return something different.
    /// </summary>
    public event Action? RowsChanged;
}
