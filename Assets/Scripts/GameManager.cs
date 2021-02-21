using System;
using System.Diagnostics;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [SerializeField] private Player player = default;
    [SerializeField] private BallManager ballManager = default;

    public EnemyManager EnemyManagerObject;

    public Vector2 GetPlayerPosition()
    {
        return this.player.transform.localPosition;
    }
    
    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    private void Update()
    {
        this.player.SetText(this.ballManager.GetBallCount().ToString());
    }

    public void Reset()
    {
        this.player.Reset();
    }

    public void SetItemPlayer(int param)
    {
        this.player.SetItemPlayer(param);
    }

    public void SetItemBall(int param)
    {
        this.ballManager.CreateBall(param, GetPlayerPosition());
    }

    public void SetItemBallAttack(int param)
    {
        this.ballManager.SetAttack(param);
    }
    
}
