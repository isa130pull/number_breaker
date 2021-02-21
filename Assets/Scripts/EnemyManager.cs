using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private readonly List<Enemy> enemies = new List<Enemy>();
    
    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        foreach (Transform child in this.transform)
        {
            var enemy = child.gameObject.GetComponent<Enemy>();
            if (enemy != null) this.enemies.Add(enemy);
        }

        GameManager.Instance.EnemyManagerObject = this;
    }

    private void Update()
    {
        foreach (var enemy in this.enemies.ToArray())
        {
            if (enemy.IsBreak)
            {
                this.enemies.Remove(enemy);
                
                if (this.enemies.Count == 0)
                {
                    // TODO: クリア処理
                }
            }
        }
        
        

    }
}
