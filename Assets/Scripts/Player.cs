using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private TMP_Text text = default;
    [SerializeField] private BoxCollider2D collider = default;
    private Sequence scaleChangeAnimation;
    private float scaleChangeTime;
    private int sizeLevel;

    private void Awake()
    {
        Reset();
    }

    public void Reset()
    {
        this.text.text = "1";
        this.sizeLevel = 0;
    }

    public void SetText(string text)
    {
        this.text.text = text;
    }

    public Vector2 GetSize()
    {
        // 90度回転しているので座標系を反転させている
        var scale = this.transform.localScale;
        return new Vector2(this.collider.size.y * scale.y, this.collider.size.x * scale.x);
    }

    // プレイヤーのサイズを大きくする
    public void SetItemPlayer(int param)
    {
        this.sizeLevel += param;
        var scale = 8 + this.sizeLevel * 1.6f;
        scale = Mathf.Min(scale, 16);
        this.transform.localScale = new Vector3(this.transform.localScale.x, scale);

        switch (this.sizeLevel)
        {
            case 1:
                this.scaleChangeTime = 5;
                break;
            case 2:
                this.scaleChangeTime = 4;
                break;
            case 3:
                this.scaleChangeTime = 3.5f;
                break;
            case 4:
                this.scaleChangeTime = 3f;
                break;
            case 5:
                this.scaleChangeTime = 2.5f;
                break;
        }
        
        this.scaleChangeTime = 5f;
        this.scaleChangeAnimation?.Kill();
        this.scaleChangeAnimation = null;
        this.text.alpha = 1.0f;
    }

    private void Update()
    {
        this.scaleChangeTime -= Time.deltaTime;
        // プレイヤーのサイズを小さくするアニメーション
        if (this.scaleChangeTime < 0 && this.transform.localScale.y > 8f && this.scaleChangeAnimation == null)
        {
            this.scaleChangeAnimation = DOTween.Sequence()
                .Append(this.text.DOFade(0.5f, 0))
                .AppendInterval(0.3f)
                .Append(this.text.DOFade(1.0f, 0))
                .AppendInterval(0.3f)
                .Append(this.text.DOFade(0.5f, 0))
                .AppendInterval(0.3f)
                .Append(this.text.DOFade(1.0f, 0))
                .AppendInterval(0.3f)
                .Play()
                .OnComplete(() =>
                {
                    this.transform.localScale = new Vector3(this.transform.localScale.x, 8);
                    this.sizeLevel = 0;
                    this.scaleChangeAnimation = null;
                });
        }

        if (!Input.GetMouseButton(0) && !Input.GetMouseButtonDown(0)) return;

        // プレイヤーを動かす
        var tapX = Input.mousePosition.x / Screen.width * 750f - 750f / 2f;
        // 端で移動制限
        // var playerSizeHalf = this.size.x * this.transform.localScale.y / 2;
        // tapX = Mathf.Max(tapX, -750 / 2f + playerSizeHalf);
        // tapX = Mathf.Min(tapX, 750 / 2f - playerSizeHalf);

        var playerX = this.transform.localPosition.x;

        var diff = tapX - playerX;
        if (Mathf.Abs(diff) > 100) playerX += diff / 8f;
        else if (Mathf.Abs(diff) > 50) playerX += diff / 6f;
        else if (Mathf.Abs(diff) > 20) playerX += diff / 4f;
        else if (Mathf.Abs(diff) > 5) playerX += diff / 2f;
        else playerX += diff;

        this.transform.localPosition = new Vector3(playerX, this.transform.localPosition.y);
    }
}