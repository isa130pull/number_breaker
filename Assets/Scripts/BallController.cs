using Common.Isao;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

public class BallController : MonoBehaviour
{
    [SerializeField] private Collider2D collider = default;
    [SerializeField] private TMP_Text text = default;

    public Vector2 Speed;
    private float baseSpeed;
    public int attack = 1;
    private float collisionWaitTimer;

    private void Awake()
    {
        // TODO: 仮値
        this.baseSpeed = 4;
        this.Speed = new Vector2(Utility.IsTrueOrFalse() ? this.baseSpeed : -this.baseSpeed, this.baseSpeed);
    }

    private void Update()
    {
        this.collisionWaitTimer -= Time.deltaTime;
        var currentPosition = this.transform.localPosition;
        this.transform.localPosition = new Vector3(currentPosition.x + this.Speed.x, currentPosition.y + this.Speed.y);
    }

    private void ReflectionEnemy(Vector3 targetPosition)
    {
        var ballPosition = this.transform.localPosition;

        // 角判定
        const float cornerThreshold = 0.04f;
        var xDiff = ballPosition.x - targetPosition.x;
        var yDiff = ballPosition.y - targetPosition.y;
        var xyDiff = Mathf.Abs(Mathf.Abs(xDiff) - Mathf.Abs(yDiff));

        // 右下
        // if (xDiff > 0 && yDiff < 0 && xyDiff < cornerThreshold)
        // {
        //     if (this.Speed.x > 0 && this.Speed.y > 0) this.Speed = new Vector2(this.Speed.x, -this.Speed.y);
        //     else if (this.Speed.x < 0 && this.Speed.y < 0) this.Speed = new Vector2(-this.Speed.x, this.Speed.y);
        //     else this.Speed = -this.Speed;
        //     // Debug.Log($"右下 xDiff {xDiff} yDiff {yDiff} xyDiff {xyDiff}");
        // }
        // // 右上
        // else if (xDiff > 0 && yDiff > 0 && xyDiff < cornerThreshold)
        // {
        //     if (this.Speed.x > 0 && this.Speed.y < 0) this.Speed = new Vector2(this.Speed.x, -this.Speed.y);
        //     else if (this.Speed.x < 0 && this.Speed.y > 0) this.Speed = new Vector2(-this.Speed.x, this.Speed.y);
        //     else this.Speed = -this.Speed;
        //     // Debug.Log($"右上 xDiff {xDiff} yDiff {yDiff} xyDiff {xyDiff}");
        // }
        // // 左上
        // else if (xDiff < 0 && yDiff > 0 && xyDiff < cornerThreshold)
        // {
        //     if (this.Speed.x < 0 && this.Speed.y < 0) this.Speed = new Vector2(this.Speed.x, -this.Speed.y);
        //     else if (this.Speed.x > 0 && this.Speed.y > 0) this.Speed = new Vector2(-this.Speed.x, this.Speed.y);
        //     else this.Speed = -this.Speed;
        //     // Debug.Log($"左上 xDiff {xDiff} yDiff {yDiff} xyDiff {xyDiff}");
        // }
        // // 左下
        // else if (xDiff < 0 && yDiff < 0 && xyDiff < cornerThreshold)
        // {
        //     if (this.Speed.x < 0 && this.Speed.y > 0) this.Speed = new Vector2(this.Speed.x, -this.Speed.y);
        //     else if (this.Speed.x > 0 && this.Speed.y < 0) this.Speed = new Vector2(-this.Speed.x, this.Speed.y);
        //     else this.Speed = -this.Speed;
        //     // Debug.Log($"左下 xDiff {xDiff} yDiff {yDiff} xyDiff {xyDiff}");
        // }
        // 上下
        if (Mathf.Abs(xDiff) < Mathf.Abs(yDiff))
        {
            this.Speed = new Vector2(this.Speed.x, -this.Speed.y);
            // Debug.Log($"上下 xDiff {xDiff} yDiff {yDiff} xyDiff {xyDiff}");
        }
        // 左右
        else
        {
            this.Speed = new Vector2(-this.Speed.x, this.Speed.y);
            // Debug.Log($"左右 xDiff {xDiff} yDiff {yDiff} xyDiff {xyDiff}");
        }
    }

    private void ReflectionPlayer(GameObject player)
    {
        // プレイヤーバーに跳ね返るたびにスピードアップ
        this.baseSpeed *= 1.1f;
        if (this.baseSpeed > 10) this.baseSpeed = 10;

        var playerSize = player.GetComponent<PlayerManager>().GetSize();
        var ballPosition = this.transform.localPosition;
        var playerPosition = player.transform.localPosition;
        
        // 左右下
        if (ballPosition.y < playerPosition.y) this.Speed = new Vector2(-this.Speed.x, this.Speed.y);
        
        // あたった位置に応じて反射角を決める
        // TODO: 今の移動ベクトルも計算補正に入れる
        var rate = Mathf.Abs(ballPosition.x - playerPosition.x) / (playerSize.x / 2f);
        var angle = 45 * rate;
        angle = ballPosition.x > playerPosition.x ? -angle : angle;
        angle += 90;

        this.Speed = new Vector2(this.baseSpeed * Mathf.Cos(Utility.ToRadian(angle)), this.baseSpeed * Mathf.Sin(Utility.ToRadian(angle)));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var colliderName = other.gameObject.name;
        if (colliderName.Contains("Player")) ReflectionPlayer(other.gameObject);
        else if (colliderName.Contains("Left") || colliderName.Contains("Right")) this.Speed = new Vector2(-this.Speed.x, this.Speed.y);
        else if (colliderName.Contains("Top")) this.Speed = new Vector2(this.Speed.x, -this.Speed.y);
        else if (colliderName.Contains("Bottom"))
        {
            this.collider.enabled = false;
            this.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    // TODO: ボールカウントを減らす
                    Object.Destroy(this.gameObject);
                });
        }
        else if (colliderName.Contains("Enemy") && this.collisionWaitTimer <= 0)
        {
            ReflectionEnemy(other.transform.localPosition);
            other.gameObject.GetComponent<EnemyController>().CollisionBall(this.attack);
            this.collisionWaitTimer = 0.1f;
        }

//        Debug.Log($"OnTriggerEnter2D {other.gameObject.name}");
    }
}