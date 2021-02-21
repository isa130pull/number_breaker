using System;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;


public enum ItemType
{
    Player = 0,
    Ball,
    Attack,
}

public class Item : MonoBehaviour
{
    [SerializeField] private TMP_Text text = default;
    public ItemType Type { get; private set; }
    private Vector2 speed;

    public void Init()
    {
        this.Type = (ItemType) Enum.ToObject(typeof(ItemType),Random.Range(0, 3));
        switch (this.Type)
        {
            case ItemType.Player:
                this.text.color = Color.white;
                break;
            case ItemType.Ball:
                this.text.color = Color.cyan;
                break;
            case ItemType.Attack:
                this.text.color = Color.magenta;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        // this.text.color = Color.yellow;

        // TODO: とりあえず固定値
        this.speed = new Vector2(0, -4f);
    }

    private void Update()
    {

        var currentPosition = this.transform.localPosition;
        this.transform.localPosition = new Vector3(currentPosition.x + this.speed.x, currentPosition.y + this.speed.y);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name != "Player") return;

        switch (this.Type)
        {
            case ItemType.Player:
                GameManager.Instance.SetItemPlayer(1);
                break;
            case ItemType.Ball:
                GameManager.Instance.SetItemBall(1);
                break;
            case ItemType.Attack:
                GameManager.Instance.SetItemBallAttack(1);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        SoundManager.Instance.PlaySe("se_item");
        Object.Destroy(this.gameObject);
    }

}
