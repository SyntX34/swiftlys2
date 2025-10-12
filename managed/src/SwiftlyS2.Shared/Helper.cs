using SwiftlyS2.Shared.Schemas;

namespace SwiftlyS2.Shared;

public static class Helper
{

  private static readonly Dictionary<string, string> ColorCodes = new()
  {
    { "[default]", "\x01" },
    { "[/]", "\x01" },
    { "[white]", "\x01" },
    { "[darkred]", "\x02" },
    { "[lightpurple]", "\x03" },
    { "[green]", "\x04" },
    { "[olive]", "\x05" },
    { "[lime]", "\x06" },
    { "[red]", "\x07" },
    { "[gray]", "\x08" },
    { "[grey]", "\x08" },
    { "[lightyellow]", "\x09" },
    { "[yellow]", "\x09" },
    { "[silver]", "\x0A" },
    { "[bluegrey]", "\x0A" },
    { "[lightblue]", "\x0B" },
    { "[blue]", "\x0B" },
    { "[darkblue]", "\x0C" },
    { "[purple]", "\x0E" },
    { "[magenta]", "\x0E" },
    { "[lightred]", "\x0F" },
    { "[gold]", "\x10" },
    { "[orange]", "\x10" }
  };

  public static class ChatColors
  {
    public static string Default = "[/]";
    public static string White = "[white]";
    public static string DarkRed = "[darkred]";
    public static string Green = "[green]";
    public static string LightYellow = "[lightyellow]";
    public static string LightBlue = "[lightblue]";
    public static string Olive = "[olive]";
    public static string Lime = "[lime]";
    public static string Red = "[red]";
    public static string LightPurple = "[lightpurple]";
    public static string Purple = "[purple]";
    public static string Grey = "[grey]";
    public static string Yellow = "[yellow]";
    public static string Gold = "[gold]";
    public static string Silver = "[silver]";
    public static string Blue = "[blue]";
    public static string DarkBlue = "[darkblue]";
    public static string BlueGrey = "[bluegrey]";
    public static string Magenta = "[magenta]";
    public static string LightRed = "[lightred]";
    public static string Orange = "[orange]";
  }

  /// <summary>
  /// Replace the color codes in the text with the corresponding color codes.
  /// </summary>
  /// <param name="text">The text to replace the color codes in.</param>
  /// <returns>The text with the color codes replaced.</returns>
  public static string Colored(this string text)
  {
    if (text.StartsWith("["))
      text = " " + text;

    foreach (var color in ColorCodes)
    {
      text = text.Replace(color.Key, color.Value);
    }

    return text;
  }

  /// <summary>
  /// Convert the pointer to the schema class.
  /// </summary>
  /// <typeparam name="T">The schema class to convert to.</typeparam>
  /// <param name="ptr">The pointer to the schema class.</param>
  /// <returns>The schema class.</returns>
  public static T AsSchema<T>(nint ptr) where T : ISchemaClass<T>
  {
    return T.From(ptr);
  }
}