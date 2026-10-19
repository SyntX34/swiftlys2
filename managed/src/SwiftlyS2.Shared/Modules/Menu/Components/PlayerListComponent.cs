using SwiftlyS2.Shared.Players;

namespace SwiftlyS2.Shared.Menu.Components;

public readonly record struct MenuPlayerContext
{
    /// <summary>
    /// The session that selected the player.
    /// </summary>
    public required IMenuSession Session { get; init; }

    /// <summary>
    /// The player that was selected.
    /// </summary>
    public required IPlayer Target { get; init; }

    /// <summary>
    /// The player who made the selection.
    /// </summary>
    public IPlayer Player => Session.Player;
}

public readonly record struct MenuPlayerRowContext
{
    /// <summary>
    /// The session being drawn.
    /// </summary>
    public required IMenuSession Session { get; init; }

    /// <summary>
    /// The player the row stands for.
    /// </summary>
    public required IPlayer Target { get; init; }

    /// <summary>
    /// The position of the row among the listed players, from zero.
    /// </summary>
    public required int Index { get; init; }

    /// <summary>
    /// Whether the row holds the selection.
    /// </summary>
    public required bool IsFocused { get; init; }

    /// <summary>
    /// The player who is looking at the menu.
    /// </summary>
    public IPlayer Player => Session.Player;
}

public sealed class PlayerListComponent : MenuComponentBase, IMenuRowSource
{
    /// <summary>
    /// Creates a player list.
    /// </summary>
    /// <param name="id">A stable id, or null to generate one.</param>
    public PlayerListComponent( string? id = null ) : base(id)
    {
        SearchInput = new InputComponent("Search", string.Empty, onChanged: ctx => Apply(ctx.Session)) {
            EmptyText = "(all players)",
            HintMessage = "Type a name, #slot, SteamID or a selector like @ct in chat.",
            MaxLength = 32,
        };

        ClearButton = new ButtonComponent("Clear search", ctx => {
            _ = SearchInput.SetValue(ctx.Session, string.Empty);
            return ValueTask.CompletedTask;
        });
    }

    /// <inheritdoc/>
    public event Action? RowsChanged {
        add { }
        remove { }
    }

    /// <summary>
    /// The search line. Use it to change its label, texts, colours or length limit.
    /// </summary>
    public InputComponent SearchInput { get; }

    /// <summary>
    /// The button that clears the search. Only shown while a search is active.
    /// </summary>
    public ButtonComponent ClearButton { get; }

    /// <summary>
    /// Whether the search line is shown. The filter can still be set with <see cref="SetFilter"/>.
    /// </summary>
    public bool ShowSearch { get; set; } = true;

    /// <summary>
    /// Whether <see cref="ClearButton"/> is shown while a search is active.
    /// </summary>
    public bool ShowClearButton { get; set; } = true;

    /// <summary>
    /// The target used when the search is empty.
    /// </summary>
    public string DefaultTarget { get; set; } = "@all";

    /// <summary>
    /// The search mode passed to <see cref="IPlayerManagerService.FindTargettedPlayers"/>.
    /// </summary>
    public TargetSearchMode SearchMode { get; set; } = TargetSearchMode.IncludeSelf;

    /// <summary>
    /// How player names are compared.
    /// </summary>
    public StringComparison NameComparison { get; set; } = StringComparison.OrdinalIgnoreCase;

    /// <summary>
    /// An extra filter applied after the search, or null to keep every match. Receives the viewer and the candidate.
    /// </summary>
    public Func<IPlayer, IPlayer, bool>? Filter { get; set; }

    /// <summary>
    /// Orders the matches, or null to keep the order the search returns.
    /// </summary>
    public Func<IEnumerable<IPlayer>, IEnumerable<IPlayer>>? Sorter { get; set; }

    /// <summary>
    /// The most players listed, or null for no limit.
    /// </summary>
    public int? MaxResults { get; set; }

    /// <summary>
    /// How often the list is looked up again so joins and leaves show up.
    /// </summary>
    public TimeSpan RefreshInterval { get; set; } = TimeSpan.FromSeconds(1);

    /// <summary>
    /// Turns a player into the text shown for them.
    /// </summary>
    public Func<IPlayer, string> Formatter { get; set; } = player => player.Controller?.PlayerName ?? "unknown";

    /// <summary>
    /// Turns a player into a full node, for lines with several colours. Takes over from <see cref="Formatter"/> when set.
    /// </summary>
    public Func<IPlayer, MenuNode>? NodeFormatter { get; set; }

    /// <summary>
    /// Draws a player row with full control over its layout, including its index and focus. Takes over from <see cref="NodeFormatter"/> and <see cref="Formatter"/> when set.
    /// The busy placeholder still shows while a row waits on an action.
    /// </summary>
    public Func<MenuPlayerRowContext, MenuNode>? RowRenderer { get; set; }

    /// <summary>
    /// The style of a player row.
    /// </summary>
    public MenuTextStyle ItemStyle { get; set; } = MenuTextStyle.Default.WithSize(MenuTextSize.SmallMedium).WithColor("#FFFFFF");

    /// <summary>
    /// The hint shown while a player row holds the selection, or null for none.
    /// </summary>
    public Func<IPlayer, string?>? ItemHint { get; set; }

    /// <summary>
    /// Runs when a player activates a row.
    /// </summary>
    public Action<MenuPlayerContext>? OnPlayerSelected { get; set; }

    /// <summary>
    /// Runs when the focus leaves a player row. <see cref="MenuPlayerContext.Target"/> is the player it left.
    /// </summary>
    public Action<MenuPlayerContext>? OnPlayerDeselected { get; set; }

    /// <summary>
    /// The text shown when nobody matches, or null for the translated default.
    /// </summary>
    public string? EmptyText { get; set; }

    /// <summary>
    /// The colour of <see cref="EmptyText"/>, or null for the default colour.
    /// </summary>
    public string? EmptyColor { get; set; } = "#999999";

    /// <summary>
    /// Reads the search text of one session.
    /// </summary>
    public string GetFilter( IMenuSession session ) => SearchInput.GetValue(session);

    /// <summary>
    /// Sets the search text of one session.
    /// </summary>
    public void SetFilter( IMenuSession session, string text ) => SearchInput.SetValue(session, text);

    /// <summary>
    /// Looks the list up again for one session on its next draw.
    /// </summary>
    public void Refresh( IMenuSession session )
    {
        session.GetState<ListState>(this).Stale = true;
        session.Invalidate();
    }

    /// <inheritdoc/>
    public IReadOnlyList<IMenuComponent> GetRows( IMenuSession session, MenuRowContext context )
    {
        var state = session.GetState<ListState>(this);
        var first = state.Menu is null;

        Watch(session, state);

        IReadOnlyList<IMenuComponent> rows;

        lock (state)
        {
            if (state.Stale || state.Rows is null || DateTime.UtcNow - state.BuiltAt >= RefreshInterval)
            {
                Rebuild(session, state, context);
            }

            rows = state.Rows!;
        }

        // Record the focus the menu opens on, so the first move off a player row still reports it.
        if (first)
        {
            TrackFocus(session, state);
        }

        return rows;
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Never drawn: the menu replaces this component with <see cref="GetRows"/> when it lays out the body.
    /// </remarks>
    public override MenuNode Render( IMenuComponentRenderContext context ) => MenuStackNode.Of();

    private void Apply( IMenuSession session ) => Refresh(session);

    private void Watch( IMenuSession session, ListState state )
    {
        if (state.Menu is not null)
        {
            return;
        }

        state.Menu = session.Menu;
        state.FocusHandler = menuSession => {
            if (ReferenceEquals(menuSession, session))
            {
                TrackFocus(session, state);
            }
        };
        state.CloseHandler = menuSession => {
            if (ReferenceEquals(menuSession, session))
            {
                Unwatch(state);
            }
        };

        state.Menu.FocusChanged += state.FocusHandler;
        state.Menu.Closed += state.CloseHandler;
    }

    private void Unwatch( ListState state )
    {
        if (state.Menu is null)
        {
            return;
        }

        state.Menu.FocusChanged -= state.FocusHandler;
        state.Menu.Closed -= state.CloseHandler;
        state.Menu = null;
        state.FocusHandler = null;
        state.CloseHandler = null;
        state.Focused = null;
    }

    private void TrackFocus( IMenuSession session, ListState state )
    {
        var focused = (session.FocusedComponent as PlayerRow)?.Target;
        IPlayer? previous;

        lock (state)
        {
            if (ReferenceEquals(focused, state.Focused))
            {
                return;
            }

            previous = state.Focused;
            state.Focused = focused;
        }

        if (previous is not null)
        {
            OnPlayerDeselected?.Invoke(new MenuPlayerContext { Session = session, Target = previous });
        }
    }

    private void Rebuild( IMenuSession session, ListState state, MenuRowContext context )
    {
        var translate = context.Translate;
        var filter = GetFilter(session).Trim();
        var found = context.Players.FindTargettedPlayers(session.Player, filter.Length == 0 ? DefaultTarget : filter, SearchMode, NameComparison)
            .Where(player => Filter?.Invoke(session.Player, player) ?? true);

        if (Sorter is not null)
        {
            found = Sorter(found);
        }

        if (MaxResults is { } max)
        {
            found = found.Take(Math.Max(0, max));
        }

        var matches = found.ToList();
        var rows = new List<IMenuComponent>(matches.Count + 2);

        if (ShowSearch)
        {
            rows.Add(SearchInput);

            if (ShowClearButton && filter.Length > 0)
            {
                rows.Add(ClearButton);
            }
        }

        var cache = new Dictionary<IPlayer, PlayerRow>(matches.Count);

        var index = 0;

        foreach (var player in matches)
        {
            // Reuse rows so the selection stays on the same player between lookups.
            cache[player] = state.Cache.GetValueOrDefault(player) ?? new PlayerRow(this, player);
            cache[player].Index = index++;
            rows.Add(cache[player]);
        }

        if (matches.Count == 0)
        {
            rows.Add(new StatusRow(EmptyText ?? translate("menu.collection.empty"), EmptyColor));
        }

        var changed = state.Rows is not null && !state.Cache.Keys.SequenceEqual(cache.Keys);

        state.Cache = cache;
        state.Rows = rows;
        state.BuiltAt = DateTime.UtcNow;
        state.Stale = false;

        if (changed)
        {
            session.Invalidate();
        }
    }

    private sealed class ListState
    {
        public IReadOnlyList<IMenuComponent>? Rows { get; set; }

        public Dictionary<IPlayer, PlayerRow> Cache { get; set; } = [];

        public DateTime BuiltAt { get; set; }

        public bool Stale { get; set; }

        public IMenu? Menu { get; set; }

        public Action<IMenuSession>? FocusHandler { get; set; }

        public Action<IMenuSession>? CloseHandler { get; set; }

        public IPlayer? Focused { get; set; }
    }

    private sealed class StatusRow( string text, string? color ) : MenuComponentBase
    {
        public override MenuNode Render( IMenuComponentRenderContext context )
            => new MenuTextNode(text, MenuTextStyle.Default.WithSize(MenuTextSize.SmallMedium).WithColor(color));
    }

    private sealed class PlayerRow( PlayerListComponent owner, IPlayer target ) : MenuComponentBase
    {
        public override bool IsFocusable => true;

        public IPlayer Target => target;

        public int Index { get; set; }

        public override string? GetHint( IMenuSession session ) => owner.ItemHint?.Invoke(target);

        public override MenuNode Render( IMenuComponentRenderContext context )
        {
            if (IsBusy(context.Session))
            {
                return new MenuTextNode(WaitingText, owner.ItemStyle.WithColor(WaitingColor));
            }

            if (owner.RowRenderer is { } render)
            {
                return render(new MenuPlayerRowContext {
                    Session = context.Session,
                    Target = target,
                    Index = Index,
                    IsFocused = context.IsFocused,
                });
            }

            return owner.NodeFormatter?.Invoke(target) ?? new MenuTextNode(owner.Formatter(target), owner.ItemStyle);
        }

        public override ValueTask<bool> HandleActionAsync( MenuActionContext context )
        {
            if (!Matches(context, MenuActions.Select))
            {
                return ValueTask.FromResult(false);
            }

            return ActivateAsync(context, ctx => {
                // The row can outlive its player between lookups, so skip it once the player is gone.
                if (!target.IsValid || !ctx.Session.GetState<ListState>(owner).Cache.ContainsKey(target))
                {
                    return ValueTask.CompletedTask;
                }

                owner.OnPlayerSelected?.Invoke(new MenuPlayerContext { Session = ctx.Session, Target = target });
                return ValueTask.CompletedTask;
            });
        }
    }
}
