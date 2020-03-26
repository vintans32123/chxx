using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 场景类型
/// </summary>
public enum GameScene
{
    Start,
    Lobby,
    Game,
    Over
}

/// <summary>
/// 游戏入口
/// </summary>
public class Game : MonoBehaviour
{
    private static StateMachine machine;
    private GameManage gameMan;

    private void Start()
    {
        gameMan = FindObjectOfType<GameManage>();//查找游戏管理器
        machine = new StateMachine();
        machine.AddState(new StartScene(GameScene.Start));
        machine.AddState(new LobbyScene(GameScene.Lobby));
        machine.AddState(new MainScene(GameScene.Game));
        machine.AddState(new OverScene(GameScene.Over));

        GoTo(GameScene.Start);
    }

    private void Update()
    {
        if (machine != null)
        {
            machine.OnUpdate();
        }
    }

    /// <summary>
    /// 切换场景
    /// </summary>
    /// <param name="scene">场景类型</param>
    public static void GoTo(GameScene scene)
    {
        machine.GoTo(scene.ToString());
    }
}
