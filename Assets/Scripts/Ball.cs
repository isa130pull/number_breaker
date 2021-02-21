using Common.Isao;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

public class Ball : MonoBehaviour
{
    [SerializeField] private BoxCollider2D collider = default;
    [SerializeField] private TMP_Text text = default;

    public Vector2 Speed;
    public int Attack { get; private set; } = 1;
    public bool IsBreak {get; private set; }
    private float baseSpeed;
    private float enemyCollisionTimer;

    public Vector2 GetSize()
    {
        // TODO: 回転を考慮する
        var size = this.collider.size;
        var scale = this.transform.localScale;
        return new Vector2(size.x * scale.x, size.y * scale.y);
    }

    public void SetAttack(int addParam)
    {
        this.Attack += addParam;
        this.text.text = this.Attack.ToString();
    }

    public void Init(float angle)
    {
        SetSize();
        // TODO: 仮値
        this.baseSpeed = 10;
        this.Speed = new Vector2(this.baseSpeed * Mathf.Cos(Utility.ToRadian(angle)), this.baseSpeed * Mathf.Sin(Utility.ToRadian(angle)));
    }

    private void Update()
    {
        this.enemyCollisionTimer -= Time.deltaTime;
        var currentPosition = this.transform.localPosition;
        this.transform.localPosition = new Vector3(currentPosition.x + this.Speed.x, currentPosition.y + this.Speed.y);
    }

    private void ReflectionEnemy(GameObject enemy)
    {
        var ballPosition = this.transform.localPosition;
        // ボールと敵との位置差
        var posDiff = new Vector2(ballPosition.x - enemy.transform.localPosition.x, ballPosition.y - enemy.transform.localPosition.y);
        
        // ボールの移動方向と位置差で判断
        
        // 上下
        if (posDiff.x > 0 && this.Speed.x > 0 || posDiff.x < 0 && this.Speed.x < 0)
        {
            this.Speed = new Vector2(this.Speed.x, -this.Speed.y);
            return;
        }

        // 左右
        if (posDiff.y > 0 && this.Speed.y > 0 || posDiff.y < 0 && this.Speed.y < 0)
        {
            this.Speed = new Vector2(-this.Speed.x, this.Speed.y);
            return;
        }

        var ballSize = GetSize();
        var enemySize = enemy.GetComponent<Enemy>().GetSize();

        var sizeDiff = new Vector2(ballSize.x / 2f + enemySize.x / 2f, ballSize.y / 2f + enemySize.y / 2f);
        var rate = new Vector2(Mathf.Abs(posDiff.x) / sizeDiff.x, Mathf.Abs(posDiff.y) / sizeDiff.y);

        // 上下
        if (rate.x < rate.y)
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
    }

    private void ReflectionPlayer(GameObject player)
    {
        // プレイヤーバーに跳ね返るたびにスピードアップ
        this.baseSpeed *= 1.1f;
        if (this.baseSpeed > 30) this.baseSpeed = 30;

        var playerSize = player.GetComponent<Player>().GetSize();
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
        SoundManager.Instance.PlaySe("se_player");
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
                    this.IsBreak = true;
                    //Object.Destroy(this.gameObject);
                });
        }
        else if (colliderName.Contains("Enemy"))
        {
            // Debug.Log(this.enemyCollisionTimer);
            // 多段ヒット判定だと跳ね返り処理を行わない
            if (this.enemyCollisionTimer < 0)
            {
                ReflectionEnemy(other.gameObject);
                this.enemyCollisionTimer = 0.03f;
            }
            other.gameObject.GetComponent<Enemy>().Collision(this.Attack);
        }

//        Debug.Log($"OnTriggerEnter2D {other.gameObject.name}");
    }
    
    // 攻撃に応じてColliderのサイズ変更
    private void SetSize()
    {
        Vector2 size;
        if (this.Attack >= 10) size = new Vector3(60, 47);
        else if (this.Attack > 1) size = new Vector3(30, 47);
        else size = new Vector3(20, 47);
        this.collider.size = size;
    }

}