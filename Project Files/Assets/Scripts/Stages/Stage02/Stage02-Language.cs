using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class StageManager02
{
    protected override void ApplyLanguage()
    {
        base.ApplyLanguage();
        if (Common.language == GameSettings.ENGLISH)
        {
            text_trap_1.text = "stepping on traps will instantly kill you";
            text_trap_2.text = "arms will return to body,\nas they touch the trap";
            text_magnet.text = "press \"space\" to pull objects below";

            text_trap_1.font = font_english;
            text_magnet.font = font_english;
        }
        if (Common.language == GameSettings.KOREAN)
        {
            text_trap_1.text = "함정을 밟지 마세요";
            text_trap_2.text = "팔은 함정에 닿으면 몸으로 복귀합니다";
            text_magnet.text = "스페이스 바를 눌러 자석 바로 아래의 물건을 들어올리세요";

            text_trap_1.font = font_korean;
            text_trap_2.font = font_korean;
            text_magnet.font = font_korean;
        }
    }
}