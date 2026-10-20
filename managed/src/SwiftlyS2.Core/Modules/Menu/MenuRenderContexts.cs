using SwiftlyS2.Shared.Menu;
using SwiftlyS2.Shared.Players;

namespace SwiftlyS2.Core.Menu;

internal sealed class ComponentRenderContext(
    IMenu menu,
    IMenuSession session,
    IMenuComponent component,
    bool isFocused,
    bool isEnabled,
    string rendererId,
    Func<string, string> translate ) : IMenuComponentRenderContext
{
    public IMenu Menu { get; } = menu;

    public IMenuSession Session { get; } = session;

    public IPlayer Player => Session.Player;

    public IMenuComponent Component { get; } = component;

    public bool IsFocused { get; } = isFocused;

    public bool IsEnabled { get; } = isEnabled;

    public string RendererId { get; } = rendererId;

    public Func<string, string> Translate { get; } = translate;
}

internal sealed class MenuRenderContext(
    IMenu menu,
    IMenuSession session,
    MenuFrame frame,
    string rendererId,
    MenuRenderDiagnostics diagnostics ) : IMenuRenderContext
{
    public IMenu Menu { get; } = menu;

    public IMenuSession Session { get; } = session;

    public IPlayer Player => Session.Player;

    public MenuFrame Frame { get; } = frame;

    public void ReportUnsupported( MenuNode node ) => diagnostics.ReportUnsupported(rendererId, node);
}
