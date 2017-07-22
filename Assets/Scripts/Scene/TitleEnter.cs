using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YamagenLib
{
    public class TitleEnter : Enter
    {
        public override void SceneChange(int select){
            SceneInstructor.instance.LoadMainScene(GameScene.Select);
            SelectManager.instance.Initialize();
        }
    }
}