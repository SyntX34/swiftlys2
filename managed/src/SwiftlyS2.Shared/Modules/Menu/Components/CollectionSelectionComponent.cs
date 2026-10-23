namespace SwiftlyS2.Shared.Menu.Components;

public sealed class CollectionSelectionComponent<TItem> : SelectionComponentBase<TItem>
{
    private readonly Lock gate = new();
    private IReadOnlyList<TItem>? loaded;

    /// <summary>
    /// Creates a selection list.
    /// </summary>
    /// <param name="loader">Produces the items. Runs on the collection loader thread.</param>
    /// <param name="id">A stable id, or null to generate one.</param>
    /// <param name="comparer">How items are compared, or null for the default. Items are kept across reloads by this comparison.</param>
    public CollectionSelectionComponent( Func<Task<IReadOnlyList<TItem>>> loader, string? id = null, IEqualityComparer<TItem>? comparer = null ) : base(id, comparer)
    {
        List = new PagedCollectionComponent<TItem>(async () => {
            var items = await loader();

            lock (gate)
            {
                loaded = items;
            }

            return items;
        }, id is null ? null : id + ".list") {
            RowRenderer = row => DrawRow(row.Session, row.Item, row.Index, row.IsFocused, row.Translate),
            OnItemSelected = ctx => Toggle(ctx.Session, ctx.Item),
        };

        List.RowsChanged += RaiseRowsChanged;
    }

    /// <summary>
    /// The list underneath. Use it for the loading, empty and failed texts.
    /// Its <c>RowRenderer</c>, <c>OnItemSelected</c> and formatters are owned by this component.
    /// </summary>
    public PagedCollectionComponent<TItem> List { get; }

    /// <summary>
    /// Runs the loader again. Selected items that are still loaded stay selected.
    /// </summary>
    public void Reload() => List.Reload();

    /// <inheritdoc/>
    protected override IReadOnlyList<IMenuComponent> GetInnerRows( IMenuSession session, MenuRowContext context )
        => List.GetRows(session, context);

    /// <inheritdoc/>
    protected override bool TryGetCandidates( IMenuSession session, MenuRowContext context, out IEnumerable<TItem> candidates )
    {
        lock (gate)
        {
            candidates = loaded ?? [];
            return loaded is not null;
        }
    }
}
