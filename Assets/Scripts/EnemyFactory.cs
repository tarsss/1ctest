using UnityEngine;

public abstract class AbstractEnemyFactory : MonoBehaviour
{
    public abstract int GetNumEnemiesLeft();
    public abstract bool SpawnMore(out Enemy enemy);
}

public class EnemyFactory : AbstractEnemyFactory
{
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private Transform _enemiesContainer;
    [SerializeField] private Transform[] _spawnPoints;

    [SerializeField] private float _minCooldown;
    [SerializeField] private float _maxCooldown;

    [SerializeField] private float _minSpeed;
    [SerializeField] private float _maxSpeed;

    [SerializeField] private int _minEnemyCount;
    [SerializeField] private int _maxEnemyCount;

    private float _nextSpawnTime;
    private int _numEnemiesLeft;

    private void Awake()
    {
        _numEnemiesLeft = Random.Range(_minEnemyCount, _maxEnemyCount);
    }

    public override bool SpawnMore(out Enemy enemy)
    {
        if (Time.time > _nextSpawnTime && _numEnemiesLeft > 0)
        {
            _nextSpawnTime = Time.time + Random.Range(_minCooldown, _maxCooldown);
            var spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)];
            enemy = Instantiate(_enemyPrefab, spawnPoint.position, Quaternion.identity, _enemiesContainer);
            enemy.gameObject.SetActive(true);
            enemy.Speed = Random.Range(_minSpeed, _maxSpeed);
            _numEnemiesLeft--;
            return true;
        }

        enemy = null;
        return false;
    }

    public override int GetNumEnemiesLeft()
    {
        return _numEnemiesLeft;
    }
}