using SwiftlyS2.Shared.Players;

namespace SwiftlyS2.Shared.Menu.Components;

public sealed class PlayerSelectionComponent : SelectionComponentBase<IPlayer>
{
    /// <summary>
    /// Creates a selection list.
    /// </summary>
    /// <param name="id">A stable id, or null to generate one.</param>
    public PlayerSelectionComponent( string? id = null ) : base(id)
    {
        Formatter = player => player.Controller?.PlayerName ?? string.Empty;

        List = new PlayerListComponent(id is null ? null : id + ".list") {
            RowRenderer = row => DrawRow(row.Session, row.Target, row.Index, row.IsFocused, row.Translate),
            OnPlayerSelected = ctx => Toggle(ctx.Session, ctx.Target),
        };
    }

    /// <summary>
    /// The list underneath. Use it for the search, filter, sorter, limits and the empty text.
    /// Its <c>RowRenderer</c>, <c>OnPlayerSelected</c> and formatters are owned by this component.
    /// </summary>
    public PlayerListComponent List { get; }

    /// <inheritdoc/>
    protected override IReadOnlyList<IMenuComponent> GetInnerRows( IMenuSession session, MenuRowContext context )
        => List.GetRows(session, context);

    /// <inheritdoc/>
    protected override bool TryGetCandidates( IMenuSession session, MenuRowContext context, out IEnumerable<IPlayer> candidates )
    {
        candidates = context.Players.FindTargettedPlayers(session.Player, "@all", TargetSearchMode.IncludeSelf);
        return true;
    }

    /// <inheritdoc/>
    protected override bool IsAlive( IPlayer item ) => item.IsValid;

    /// <inheritdoc/>
    protected override string FormatText( IPlayer item, Func<string, string> translate )
    {
        var name = Formatter(item);
        return name.Length > 0 ? name : translate("menu.player.unknown");
    }
}
