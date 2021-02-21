using System;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private TMP_Text text = default;
    [SerializeField] private BoxCollider2D collider = default;
    private int sizeLevel;

    private void Awake()
    {
        Reset();
    }

    public void Reset()
    {
        this.sizeLevel = 1;
        this.text.text = "1";
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

    private void Update()
    {
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