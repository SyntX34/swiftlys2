using SwiftlyS2.Shared.Players;

namespace SwiftlyS2.Shared.Menu.Components;

public readonly record struct MenuCollectionItemContext<TItem>
{
    /// <summary>
    /// The session that selected the item.
    /// </summary>
    public required IMenuSession Session { get; init; }

    /// <summary>
    /// The item that was selected.
    /// </summary>
    public required TItem Item { get; init; }

    /// <summary>
    /// The player who selected the item.
    /// </summary>
    public IPlayer Player => Session.Player;
}

public readonly record struct MenuCollectionRowContext<TItem>
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
    /// The position of the item in the loaded list, from zero.
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

public sealed class PagedCollectionComponent<TItem> : MenuComponentBase, IMenuRowSource
{
    private readonly Func<Task<IReadOnlyList<TItem>>> loader;
    private readonly Lock gate = new();
    private volatile IReadOnlyList<IMenuComponent> rows = [];
    private Func<string, string> translate = key => key;
    private bool started;
    private int generation;

    /// <summary>
    /// Creates a collection.
    /// </summary>
    /// <param name="loader">Produces the items. Runs on the collection loader thread.</param>
    /// <param name="id">A stable id, or null to generate one.</param>
    public PagedCollectionComponent( Func<Task<IReadOnlyList<TItem>>> loader, string? id = null ) : base(id)
    {
        this.loader = loader;
    }

    /// <inheritdoc/>
    public event Action? RowsChanged;

    /// <summary>
    /// Turns an item into the text shown for it.
    /// </summary>
    public Func<TItem, string> Formatter { get; set; } = item => item?.ToString() ?? string.Empty;

    /// <summary>
    /// Turns an item into a full node, for lines with several colours. Takes over from <see cref="Formatter"/> when set.
    /// </summary>
    public Func<TItem, MenuNode>? NodeFormatter { get; set; }

    /// <summary>
    /// Draws an item row with full control over its layout, including its index and focus. Takes over from <see cref="NodeFormatter"/> and <see cref="Formatter"/> when set.
    /// The busy placeholder still shows while a row waits on an action.
    /// </summary>
    public Func<MenuCollectionRowContext<TItem>, MenuNode>? RowRenderer { get; set; }

    /// <summary>
    /// The style of an item row.
    /// </summary>
    public MenuTextStyle ItemStyle { get; set; } = MenuTextStyle.Default.WithSize(MenuTextSize.SmallMedium).WithColor("#FFFFFF");

    /// <summary>
    /// The hint shown while an item row holds the selection, or null for none.
    /// </summary>
    public Func<TItem, string?>? ItemHint { get; set; }

    /// <summary>
    /// Runs when a player activates an item row.
    /// </summary>
    public Action<MenuCollectionItemContext<TItem>>? OnItemSelected { get; set; }

    /// <summary>
    /// The text shown while the loader is running.
    /// </summary>
    public string? LoadingText { get; set; }

    /// <summary>
    /// The colour of <see cref="LoadingText"/>, or null for the default colour.
    /// </summary>
    public string? LoadingColor { get; set; } = "#FFFF00";

    /// <summary>
    /// The text shown when the loader returns no items.
    /// </summary>
    public string? EmptyText { get; set; }

    /// <summary>
    /// The colour of <see cref="EmptyText"/>, or null for the default colour.
    /// </summary>
    public string? EmptyColor { get; set; } = "#999999";

    /// <summary>
    /// The text shown when the loader throws.
    /// </summary>
    public string? FailedText { get; set; }

    /// <summary>
    /// The colour of <see cref="FailedText"/>, or null for the default colour.
    /// </summary>
    public string? FailedColor { get; set; } = "#FF3333";

    /// <summary>
    /// Whether the exception message is appended to <see cref="FailedText"/>.
    /// </summary>
    public bool ShowFailureMessage { get; set; }

    /// <inheritdoc/>
    public IReadOnlyList<IMenuComponent> GetRows( IMenuSession session, MenuRowContext context )
    {
        lock (gate)
        {
            translate = context.Translate;

            if (!started)
            {
                started = true;
                Load();
            }
        }

        return rows;
    }

    /// <summary>
    /// Runs the loader again. Open menus update when it finishes. Does nothing before the first layout.
    /// </summary>
    public void Reload()
    {
        lock (gate)
        {
            if (!started)
            {
                return;
            }

            Load();
        }

        RowsChanged?.Invoke();
    }

    /// <inheritdoc/>
    /// <remarks>
    /// Never drawn: the menu replaces this component with <see cref="GetRows"/> when it lays out the body.
    /// </remarks>
    public override MenuNode Render( IMenuComponentRenderContext context ) => MenuStackNode.Of();

    private void Load()
    {
        var current = ++generation;
        rows = [new StatusRow(LoadingText ?? translate("menu.collection.loading"), LoadingColor)];

        MenuCollectionLoader.Enqueue(async () => {
            IReadOnlyList<IMenuComponent> next;

            try
            {
                var items = await loader();

                next = items.Count == 0
                    ? [new StatusRow(EmptyText ?? translate("menu.collection.empty"), EmptyColor)]
                    : items.Select((item, index) => (IMenuComponent)new ItemRow(this, item, index)).ToList();
            }
            catch (Exception ex)
            {
                var failed = FailedText ?? translate("menu.collection.failed");
                next = [new StatusRow(ShowFailureMessage ? $"{failed}: {ex.Message}" : failed, FailedColor)];
            }

            Publish(current, next);
        });
    }

    private void Publish( int current, IReadOnlyList<IMenuComponent> next )
    {
        lock (gate)
        {
            // A reload started while this loader ran, so its result is stale.
            if (current != generation)
            {
                return;
            }

            rows = next;
        }

        RowsChanged?.Invoke();
    }

    private sealed class StatusRow( string text, string? color ) : MenuComponentBase
    {
        public override MenuNode Render( IMenuComponentRenderContext context )
            => new MenuTextNode(text, MenuTextStyle.Default.WithSize(MenuTextSize.SmallMedium).WithColor(color));
    }

    private sealed class ItemRow( PagedCollectionComponent<TItem> owner, TItem item, int index ) : MenuComponentBase
    {
        public override bool IsFocusable => true;

        public override string? GetHint( IMenuSession session ) => owner.ItemHint?.Invoke(item);

        public override MenuNode Render( IMenuComponentRenderContext context )
        {
            if (IsBusy(context.Session))
            {
                return new MenuTextNode(WaitingText, owner.ItemStyle.WithColor(WaitingColor));
            }

            if (owner.RowRenderer is { } render)
            {
                return render(new MenuCollectionRowContext<TItem> {
                    Session = context.Session,
                    Item = item,
                    Index = index,
                    IsFocused = context.IsFocused,
                });
            }

            return owner.NodeFormatter?.Invoke(item) ?? new MenuTextNode(owner.Formatter(item), owner.ItemStyle);
        }

        public override ValueTask<bool> HandleActionAsync( MenuActionContext context )
        {
            if (!Matches(context, MenuActions.Select))
            {
                return ValueTask.FromResult(false);
            }

            return ActivateAsync(context, ctx => {
                owner.OnItemSelected?.Invoke(new MenuCollectionItemContext<TItem> { Session = ctx.Session, Item = item });
                return ValueTask.CompletedTask;
            });
        }
    }
}
