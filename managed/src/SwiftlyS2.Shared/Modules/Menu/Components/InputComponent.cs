namespace SwiftlyS2.Shared.Menu.Components;

/// <summary>
/// A selectable line whose value the player types into chat.
/// </summary>
/// <remarks>
/// Activating starts a chat capture for that player only: the next message they send becomes the
/// value and never reaches the chat. Activating again while waiting cancels.
/// </remarks>
public class InputComponent : MenuValueComponent<string>
{
    private int maxLength = 16;

    /// <summary>
    /// Creates an input.
    /// </summary>
    /// <param name="text">The label to display.</param>
    /// <param name="defaultValue">The value a player starts with.</param>
    /// <param name="validator">Decides whether a typed value is acceptable.</param>
    /// <param name="onChanged">Runs after the value changes.</param>
    /// <param name="id">A stable id, or null to generate one.</param>
    public InputComponent(
        string text = "",
        string defaultValue = "",
        Func<string, bool>? validator = null,
        Action<MenuValueChangedContext<string>>? onChanged = null,
        string? id = null ) : base(text, id)
    {
        DefaultValue = defaultValue;
        Validator = validator;
        OnChanged = onChanged;
    }

    /// <inheritdoc/>
    public override string? GetHint( IMenuSession session, Func<string, string> translate )
        => Comment ?? translate("menu.input.chat_hint");

    /// <summary>
    /// The longest value accepted. Longer input is cut. Never below one.
    /// </summary>
    public int MaxLength {
        get => maxLength;
        set => maxLength = Math.Max(1, value);
    }

    /// <summary>
    /// Decides whether a typed value is acceptable, or null to accept anything non-empty.
    /// </summary>
    public Func<string, bool>? Validator { get; set; }

    /// <summary>
    /// The message sent to the player when the capture starts. Null sends the translated default, and an empty string sends nothing.
    /// </summary>
    public string? HintMessage { get; set; }

    /// <summary>
    /// The text shown while no value has been entered, or null for the translated default.
    /// </summary>
    public string? EmptyText { get; set; }

    /// <summary>
    /// The text shown while waiting for the player to type, or null for the translated default.
    /// </summary>
    public string? WaitingForInputText { get; set; }

    /// <summary>
    /// The text shown briefly after a value is accepted, or null for the translated default.
    /// </summary>
    public string? AcceptedText { get; set; }

    /// <summary>
    /// The text shown briefly after a value is rejected, or null for the translated default.
    /// </summary>
    public string? InvalidText { get; set; }

    /// <summary>
    /// The colour of <see cref="WaitingForInputText"/>.
    /// </summary>
    public string? WaitingForInputColor { get; set; } = "#C0FF3E";

    /// <summary>
    /// The colour of <see cref="AcceptedText"/>.
    /// </summary>
    public string? AcceptedColor { get; set; } = "#00FF00";

    /// <summary>
    /// The colour of <see cref="InvalidText"/>.
    /// </summary>
    public string? InvalidColor { get; set; } = "#FF0000";

    /// <summary>
    /// How long an accepted or invalid status stays on screen, in milliseconds.
    /// </summary>
    public int StatusDurationMs { get; set; } = 2000;

    /// <inheritdoc/>
    public override bool NeedsRedraw( IMenuSession session, DateTime now )
    {
        var state = session.GetState<InputState>(this);

        return base.NeedsRedraw(session, now) || (state.Status is not null && now >= state.StatusUntil);
    }

    /// <inheritdoc/>
    public override MenuNode Render( IMenuComponentRenderContext context )
    {
        var state = context.Session.GetState<InputState>(this);

        if (state.Status is not null && DateTime.UtcNow >= state.StatusUntil)
        {
            state.Status = null;
            state.StatusColor = null;
        }

        var parts = new List<MenuNode>(2);

        if (RenderLabel(context) is { } label)
        {
            parts.Add(label);
        }

        if (state.Status is not null)
        {
            parts.Add(RenderValue(context, state.Status, state.StatusColor));
        }
        else if (state.Waiting)
        {
            parts.Add(RenderValue(context, WaitingForInputText ?? context.Translate("menu.input.waiting_chat"), WaitingForInputColor));
        }
        else
        {
            var value = GetValue(context.Session);

            parts.Add(string.IsNullOrEmpty(value)
                ? RenderValue(context, EmptyText ?? context.Translate("menu.input.empty"), DisabledColor)
                : RenderValue(context, value));
        }

        return new MenuLineNode(parts);
    }

    /// <inheritdoc/>
    public override async ValueTask<bool> HandleActionAsync( MenuActionContext context )
    {
        if (!Matches(context, MenuActions.Select))
        {
            return false;
        }

        return await ActivateAsync(context, ctx => {
            var state = ctx.Session.GetState<InputState>(this);

            if (state.Waiting)
            {
                Stop(state);
                ctx.Session.Invalidate();
                return ValueTask.CompletedTask;
            }

            state.Status = null;
            state.Waiting = true;
            state.Capture = ctx.Session.CaptureChat(message => OnChatMessage(ctx.Session, state, message, ctx.Translate));

            var hint = HintMessage ?? ctx.Translate("menu.input.chat_hint");

            if (!string.IsNullOrEmpty(hint))
            {
                ctx.Player.SendChat(hint);
            }

            ctx.Session.Invalidate();
            return ValueTask.CompletedTask;
        });
    }

    /// <inheritdoc/>
    protected override string Coerce( IMenuSession session, string value )
        => value.Length > MaxLength ? value[..MaxLength] : value;

    private bool OnChatMessage( IMenuSession session, InputState state, string message, Func<string, string> translate )
    {
        if (!state.Waiting)
        {
            return false;
        }

        Stop(state);

        var trimmed = message.Trim();
        var accepted = trimmed.Length > 0 && (Validator is null || Validator(Coerce(session, trimmed)));

        if (accepted)
        {
            _ = SetValue(session, trimmed);
        }

        state.Status = accepted ? AcceptedText ?? translate("menu.input.accepted") : InvalidText ?? translate("menu.input.invalid");
        state.StatusColor = accepted ? AcceptedColor : InvalidColor;
        state.StatusUntil = DateTime.UtcNow.AddMilliseconds(StatusDurationMs);

        session.Invalidate();
        return true;
    }

    private static void Stop( InputState state )
    {
        state.Waiting = false;
        state.Capture?.Dispose();
        state.Capture = null;
    }

    private sealed class InputState
    {
        public IDisposable? Capture { get; set; }

        public bool Waiting { get; set; }

        public string? Status { get; set; }

        public string? StatusColor { get; set; }

        public DateTime StatusUntil { get; set; }
    }
}
