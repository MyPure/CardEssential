namespace CardEssential.Monitor.Utils;

public class TextUtils
{
    public static string RichText(string text, string color)
    {
        return $"<color=#{color}>{text}</color>";
    }
}