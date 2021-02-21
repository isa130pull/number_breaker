using System.Diagnostics;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [SerializeField] private Player player = default;
    [SerializeField] private BallManager ballManager = default;

    public Vector2 GetPlayerPosition()
    {
        return this.player.transform.localPosition;
    }
    
    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    public void Reset()
    {
        this.player.Reset();
    }
    
}
