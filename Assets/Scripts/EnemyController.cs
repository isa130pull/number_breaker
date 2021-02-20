using DG.Tweening;
using TMPro;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private BoxCollider2D collider = default;
    [SerializeField] private TMP_Text text = default;

    public int Hp;

    public Vector2 GetSize()
    {
        // TODO: 回転を考慮する
        var size = this.collider.size;
        var scale = this.transform.localScale;
        return new Vector2(size.x * scale.x, size.y * scale.y);
    }


    private void Start()
    {
        this.text.text = this.Hp.ToString();
    }

    public void CollisionBall(int damage)
    {
        this.Hp -= damage;

        if (this.Hp <= 0)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            // ブロック削除処理
            this.text.transform.DOScale(0, 0.2f).SetEase(Ease.Linear)
                .OnComplete(() => Object.Destroy(this.gameObject));
            // SoundManager.Instance.PlaySe("collisionblock");
        }
        else
        {
            this.text.text = this.Hp.ToString();
            SetSize();
            // SoundManager.Instance.PlaySe("collisionblock");
        }
    }

    // HPに応じてColliderのサイズ変更
    private void SetSize()
    {
        Vector2 size;
        if (this.Hp >= 10) size = new Vector3(60, 47);
        else if (this.Hp > 1) size = new Vector3(30, 47);
        else size = new Vector3(20, 47);
        this.collider.size = size;
    }
    
}