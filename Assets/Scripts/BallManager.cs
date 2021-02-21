using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class BallManager : MonoBehaviour
{
    [SerializeField] private Transform ballPool = default;
    private readonly List<Ball> balls = new List<Ball>();

    public void Reset()
    {
        foreach (var ball in this.balls) Object.Destroy(ball.gameObject);
        this.balls.Clear();
    }

    public int GetBallCount()
    {
        return this.balls.Count;
    }

    public void CreateBall(int count, Vector2 createPosition)
    {
        var ballObject = Resources.Load<Ball>("ball");
        var baseAngle = Random.Range(45f, 135f);

        switch (count)
        {
            case 2:
                baseAngle = 60;
                break;
            case 3:
                baseAngle = 45;
                break;
            case 4:
                baseAngle = 30;
                break;
            case 5:
                baseAngle = 20;
                break;
        }


        for (var i = 0; i < count; i++)
        {
            var ball = Object.Instantiate(ballObject, this.ballPool);
            ball.transform.localPosition = new Vector3(createPosition.x, createPosition.y + ball.GetSize().y * 1.5f);
            var angle = baseAngle;
            switch (count)
            {
                case 2:
                case 3:
                case 4:
                    angle = 45 + i * baseAngle;
                    break;
                case 5:
                    angle = 50 + i * baseAngle;
                    break;
            }

            ball.Init(angle);
            this.balls.Add(ball);
        }
    }

    private void Start()
    {
        CreateBall(1, GameManager.Instance.GetPlayerPosition());
    }

    private void Update()
    {
        foreach (var ball in this.balls.ToArray())
        {
            if (ball.IsBreak)
            {
                this.balls.Remove(ball);
                Object.Destroy(ball.gameObject);
            }
        }
    }
}