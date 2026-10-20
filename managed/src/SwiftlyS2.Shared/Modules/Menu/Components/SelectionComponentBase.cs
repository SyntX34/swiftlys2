using SwiftlyS2.Shared.Players;

namespace SwiftlyS2.Shared.Menu.Components;

public readonly record struct MenuSelectionContext<TItem>
{
    /// <summary>
    /// The session that changed the selection.
    /// </summary>
    public required IMenuSession Session { get; init; }

    /// <summary>
    /// The item that was ticked or unticked.
    /// </summary>
    public required TItem Item { get; init; }

    /// <summary>
    /// Whether <see cref="Item"/> is now selected.
    /// </summary>
    public required bool IsSelected { get; init; }

    /// <summary>
    /// Every selected item after the change.
    /// </summary>
    public required IReadOnlyList<TItem> Selected { get; init; }

    /// <summary>
    /// The player who made the change.
    /// </summary>
    public IPlayer Player => Session.Player;
}

public readonly record struct MenuSelectionRowContext<TItem>
{
    /// <summary>
    /// The session being drawn.
    /// </summary>
    public required IMenuSession Session { get; init; }

    /// <summary>
    /// The item the row stands for.
    /// </summary>
    public required TItem Item { get; init; }

    /// <summary>
    /// The position of the row, from zero.
    /// </summary>
    public required int Index { get; init; }

    /// <summary>
    /// Whether the row holds the selection cursor.
    /// </summary>
    public required bool IsFocused { get; init; }

    /// <summary>
    /// Whether the item is ticked.
    /// </summary>
    public required bool IsSelected { get; init; }

    /// <summary>
    /// Translates a key for the player who is looking at the menu.
    /// </summary>
    public required Func<string, string> Translate { get; init; }

    /// <summary>
    /// The player who is looking at the menu.
    /// </summary>
    public IPlayer Player => Session.Player;
}

public abstract class SelectionComponentBase<TItem> : MenuComponentBase, IMenuRowSource
{
    private readonly IEqualityComparer<TItem>? comparer;

    /// <param name="id">A stable id, or null to generate one.</param>
    /// <param name="comparer">How items are compared, or null for the default.</param>
    protected SelectionComponentBase( string? id, IEqualityComparer<TItem>? comparer = null ) : base(id)
    {
        this.comparer = comparer;
    }

    /// <inheritdoc/>
    public event Action? RowsChanged;

    /// <summary>
    /// The text shown for an item.
    /// </summary>
    public Func<TItem, string> Formatter { get; set; } = item => item?.ToString() ?? string.Empty;

    /// <summary>
    /// Draws a row with full control. Takes over from <see cref="Formatter"/> when set.
    /// </summary>
    public Func<MenuSelectionRowContext<TItem>, MenuNode>? RowRenderer { get; set; }

    /// <summary>
    /// Put before the text of a selected item.
    /// </summary>
    public string SelectedMark { get; set; } = "[x] ";

    /// <summary>
    /// Put before the text of an item that is not selected.
    /// </summary>
    public string UnselectedMark { get; set; } = "[ ] ";

    /// <summary>
    /// The style of a selected row.
    /// </summary>
    public MenuTextStyle SelectedStyle { get; set; } = MenuTextStyle.Default.WithSize(MenuTextSize.SmallMedium).WithColor("#33FF66");

    /// <summary>
    /// The style of a row that is not selected.
    /// </summary>
    public MenuTextStyle UnselectedStyle { get; set; } = MenuTextStyle.Default.WithSize(MenuTextSize.SmallMedium).WithColor("#FFFFFF");

    /// <summary>
    /// The most items that can be selected at once, or null for no limit. Extra ticks are ignored.
    /// </summary>
    public int? MaxSelected { get; set; }

    /// <summary>
    /// Decides what starts selected, given the session and a candidate, or null to start with nothing.
    /// Runs once per session, as soon as the candidates are known, so later arrivals start unselected.
    /// </summary>
    public Func<IMenuSession, TItem, bool>? DefaultSelected { get; set; }

    /// <summary>
    /// Runs after an item is ticked or unticked.
    /// </summary>
    public Action<MenuSelectionContext<TItem>>? OnSelectionChanged { get; set; }

    /// <summary>
    /// The items one session has selected. Items that are gone are dropped.
    /// </summary>
    public IReadOnlyList<TItem> GetSelected( IMenuSession session )
    {
        var state = StateOf(session);

        lock (state)
        {
            _ = state.Items.RemoveWhere(item => !IsAlive(item));
            return [.. state.Items];
        }
    }

    /// <summary>
    /// Whether one session has selected an item.
    /// </summary>
    public bool IsSelected( IMenuSession session, TItem item )
    {
        var state = StateOf(session);

        lock (state)
        {
            return state.Items.Contains(item);
        }
    }

    /// <summary>
    /// Replaces the selection of one session.
    /// </summary>
    public void SetSelected( IMenuSession session, IEnumerable<TItem> items )
    {
        var state = StateOf(session);

        lock (state)
        {
            state.Items.Clear();
            state.Items.UnionWith(MaxSelected is { } max ? items.Take(Math.Max(0, max)) : items);
            state.Seeded = true;
        }

        session.Invalidate();
    }

    /// <summary>
    /// Clears the selection of one session.
    /// </summary>
    public void Clear( IMenuSession session ) => SetSelected(session, []);

    /// <inheritdoc/>
    public IReadOnlyList<IMenuComponent> GetRows( IMenuSession session, MenuRowContext context )
    {
        var state = StateOf(session);

        lock (state)
        {
            if (!state.Seeded)
            {
                if (DefaultSelected is not { } isDefault)
                {
                    state.Seeded = true;
                }
                else if (TryGetCandidates(session, context, out var candidates))
                {
                    state.Seeded = true;

                    var found = candidates.Where(item => isDefault(session, item));
                    state.Items.UnionWith(MaxSelected is { } max ? found.Take(Math.Max(0, max)) : found);
                }
            }
        }

        return GetInnerRows(session, context);
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Never drawn: the menu replaces this component with <see cref="GetRows"/> when it lays out the body.
    /// </remarks>
    public override MenuNode Render( IMenuComponentRenderContext context ) => MenuStackNode.Of();

    /// <summary>
    /// The rows of the list underneath.
    /// </summary>
    protected abstract IReadOnlyList<IMenuComponent> GetInnerRows( IMenuSession session, MenuRowContext context );

    /// <summary>
    /// Every item <see cref="DefaultSelected"/> should be asked about. Return false while they are not known yet.
    /// </summary>
    protected abstract bool TryGetCandidates( IMenuSession session, MenuRowContext context, out IEnumerable<TItem> candidates );

    /// <summary>
    /// Whether an item still exists. Gone items are dropped from the selection.
    /// </summary>
    protected virtual bool IsAlive( TItem item ) => true;

    /// <summary>
    /// Raises <see cref="RowsChanged"/>.
    /// </summary>
    protected void RaiseRowsChanged() => RowsChanged?.Invoke();

    /// <summary>
    /// Draws one row for the list underneath.
    /// </summary>
    protected MenuNode DrawRow( IMenuSession session, TItem item, int index, bool isFocused, Func<string, string> translate )
    {
        var selected = IsSelected(session, item);

        if (RowRenderer is { } render)
        {
            return render(new MenuSelectionRowContext<TItem> {
                Session = session,
                Item = item,
                Index = index,
                IsFocused = isFocused,
                IsSelected = selected,
                Translate = translate,
            });
        }

        return new MenuTextNode(
            (selected ? SelectedMark : UnselectedMark) + FormatText(item, translate),
            selected ? SelectedStyle : UnselectedStyle);
    }

    /// <summary>
    /// The text of an item in the default row. Uses <see cref="Formatter"/> unless overridden.
    /// </summary>
    protected virtual string FormatText( TItem item, Func<string, string> translate ) => Formatter(item);

    /// <summary>
    /// Ticks or unticks an item for the list underneath.
    /// </summary>
    protected void Toggle( IMenuSession session, TItem item )
    {
        var state = StateOf(session);
        bool now;
        TItem[] all;

        lock (state)
        {
            if (state.Items.Remove(item))
            {
                now = false;
            }
            else if (MaxSelected is { } max && state.Items.Count >= max)
            {
                return;
            }
            else
            {
                _ = state.Items.Add(item);
                now = true;
            }

            all = [.. state.Items];
        }

        session.Invalidate();

        OnSelectionChanged?.Invoke(new MenuSelectionContext<TItem> {
            Session = session,
            Item = item,
            IsSelected = now,
            Selected = all,
        });
    }

    private SelectionState StateOf( IMenuSession session )
    {
        var state = session.GetState<SelectionState>(this);

        lock (state)
        {
            state.Items ??= new HashSet<TItem>(comparer);
        }

        return state;
    }

    private sealed class SelectionState
    {
        public HashSet<TItem> Items { get; set; } = null!;

        public bool Seeded { get; set; }
    }
}
