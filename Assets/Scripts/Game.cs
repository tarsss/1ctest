using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private AbstractEnemyFactory _enemyFactory;

    public System.Action OnWin;
    public System.Action OnLose;

    public IReadOnlyDictionary<int, Enemy> Enemies => _enemies;

    private Dictionary<int, Enemy> _enemies = new Dictionary<int, Enemy>();
    private int _numEnemiesSpawned;

    private void Awake()
    {
        Time.timeScale = 1.0f;

        _player.OnDeath += () =>
        {
            OnLose?.Invoke();
            Time.timeScale = 0.0f;
        };
    }

    private void Update()
    {
        if (_enemyFactory.SpawnMore(out var enemy))
        {
            var id = _numEnemiesSpawned;
            _numEnemiesSpawned++;
            _enemies.Add(id, enemy);

            enemy.OnDeath += () =>
            {
                _enemies.Remove(id);
            };
        }

        if (_enemyFactory.GetNumEnemiesLeft() == 0 && _enemies.Count == 0)
        {
            OnWin?.Invoke();
            Time.timeScale = 0.0f;
        }
    }
}