using DG.Tweening;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private BoxCollider2D collider = default;
    [SerializeField] private TMP_Text text = default;

    public int Hp;
    public bool IsBreak { get; private set; }

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

    public void Collision(int damage)
    {
        this.Hp -= damage;

        if (this.Hp <= 0)
        {
            SoundManager.Instance.PlaySe("se_block1");
            GetComponent<BoxCollider2D>().enabled = false;
            CreateItem();
            // ブロック削除処理
            var textScale = this.text.transform.localScale;
            DOTween.Sequence()
                .Append(this.text.transform.DOScale(textScale * 2f, 0.1f).SetEase(Ease.Linear))
                .Join(this.text.DOFade(0, 0.1f).SetEase(Ease.Linear))
                .OnComplete(() =>
                {
                    this.IsBreak = true;
                    Object.Destroy(this.gameObject);
                });
        }
        else
        {
            SoundManager.Instance.PlaySe("se_block2");
            this.text.text = this.Hp.ToString();
            SetSize();
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


    private void CreateItem()
    {
        // TODO: 確率は変動性に
        const int createRate = 25;
        if (Random.Range(0, 100) > createRate) return;

        var itemObject = Resources.Load<Item>("Item");
        var item = Object.Instantiate(itemObject, this.transform.parent);
        item.transform.localPosition = this.transform.localPosition;
        item.Init();
    }
}