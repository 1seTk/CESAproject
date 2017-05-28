using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YamagenLib
{
    public class TitleEnter : Enter
    {
        public override void SceneChange(int select){
            select = select % 1;
            // 次のシーンに移動  
            switch (select){
                case 0: SceneInstructor.instance.LoadMainScene(GameScene.Select); break;
                case 1: SceneInstructor.instance.LoadMainScene(GameScene.Endless); break;
            }
            SelectManager.instance.Initialize();
        }
    }
}