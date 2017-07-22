using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YamagenLib
{
    public class SelectEnter : Enter
    {
        public override void SceneChange(int select)
        {
            select = select % System.Enum.GetValues(typeof(PlayStage)).Length;
            PlayStage change = (PlayStage)select;

            // プレイインストラクターにシーンを設定!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            PlayInstructor.m_nextScene = change;

            // 次のシーンに移動
            SceneInstructor.instance.LoadMainScene(GameScene.Play);
        }
    }
}
