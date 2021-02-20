using DG.Tweening;
using TMPro;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private BoxCollider2D collider = default;
    [SerializeField] private TMP_Text text = default;

    public int Hp;

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
            // SoundManager.Instance.PlaySe("collisionblock");
        }
    }
    
}
