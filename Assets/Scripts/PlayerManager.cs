using GodTouches;
using TMPro;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private TMP_Text text = default;
    [SerializeField] private BoxCollider2D collider = default;

    private readonly Vector2 size = new Vector2(30, 50);

    private void Update()
    {
        if (!Input.GetMouseButton(0) && !Input.GetMouseButtonDown(0)) return;

        // プレイヤーを動かす
        var tapX = Input.mousePosition.x / Screen.width * 750f - 750f / 2f;
        var playerSizeHalf = this.size.x * this.transform.localScale.y / 2;
        tapX = Mathf.Max(tapX, -750 / 2f + playerSizeHalf);
        tapX = Mathf.Min(tapX, 750 / 2f - playerSizeHalf);

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