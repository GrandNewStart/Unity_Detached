
public partial class StageManager01
{
    protected override void ApplyLanguage()
    {
        base.ApplyLanguage();
        if (Common.language == GameSettings.ENGLISH)
        {
            text_jump.text = "Press \"Space\" / \"W\" / \"↑\" to jump";
            text_show_hints.text = "press \"H\" to show hints";
            text_fire.text = "press \"F\" / \"L\" to fire";
            text_shift_control.text = "press \"Tab\" to shift control";
            text_retrieve.text = "press \"R\" to retrieve hands \n(only when you are controlling the body)";
            text_plug_in.text = "press \"Q\" to plug into a switch\n(only when you are controlling arms)";
            text_plug_out.text = "Hold \"Q\" to plug out from a switch\n(only when you are controlling arms)";
            text_hide_hints.text = "Press \"H\" to hide hints";
            text_telescope.text = "Activate telescope to get a wider view of the area";

            text_jump.font = font_english;
            text_show_hints.font = font_english;
            text_fire.font = font_english;
            text_shift_control.font = font_english;
            text_retrieve.font = font_english;
            text_plug_in.font = font_english;
            text_plug_out.font = font_english;
            text_hide_hints.font = font_english;
            text_telescope.font = font_english;
        }
        if (Common.language == GameSettings.KOREAN)
        {
            text_jump.text = "\"스페이스\" / \"W\" / \"↑\" 를 눌러 점프";
            text_show_hints.text = "\"H\" 를 눌러 힌트 보기";
            text_fire.text = "\"F\" / \"L\" 을 눌러 발사";
            text_shift_control.text = "\"Tab\" 을 눌러 시점 전환";
            text_retrieve.text = "\"R\" 을 눌러 팔 회수\n(본체 조종 중에만)";
            text_plug_in.text = "\"Q\" 를 눌러 스위치 플러그 인\n(팔 조종 중에만)";
            text_plug_out.text = "\"Q\" 를 길게 눌러 스위치 플러그 아웃\n(팔 조종 중에만)";
            text_hide_hints.text = "\"H\" 를 눌러 힌트 숨기기";
            text_telescope.text = "망원경을 작동시켜 해당 지역을\n넓게 볼 수 있습니다";

            text_jump.font = font_korean;
            text_show_hints.font = font_korean;
            text_fire.font = font_korean;
            text_shift_control.font = font_korean;
            text_retrieve.font = font_korean;
            text_plug_in.font = font_korean;
            text_plug_out.font = font_korean;
            text_hide_hints.font = font_korean;
            text_telescope.font = font_korean;
        }
    }
}