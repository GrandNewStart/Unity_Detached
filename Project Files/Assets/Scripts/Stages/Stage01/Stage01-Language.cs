
public partial class StageManager01
{
    protected override void ApplyLanguage()
    {
        base.ApplyLanguage();
        if (Common.language == GameSettings.ENGLISH)
        {
            text_jump.text = "Press \"Space\" / \"W\" / \"↑\" to jump";

            text_jump.font = font_english;
        }
        if (Common.language == GameSettings.KOREAN)
        {
            text_jump.text = "\"스페이스\" / \"W\" / \"↑\" 를 눌러 점프";

            text_jump.font = font_korean;
        }
    }
}