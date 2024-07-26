using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _health;
    [SerializeField] private float _speed;
    [SerializeField] private float _attackRange;
    [SerializeField] private float _attackCooldown;

    [SerializeField] private RectTransform _bounds;
    [SerializeField] private RectTransform _range;
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private Transform _bulletsContainer;
    [SerializeField] private Game _game;

    public System.Action<float> OnHealthChange;
    public System.Action OnDeath;
    public float Health => _health;

    private RectTransform _rectTransform;
    private float _lastAttackTime;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        Move();

        if (FindClosestEnemyTarget(out var target))
            Attack(target);

        _range.sizeDelta = Vector2.one * _attackRange * 2.0f;
    }

    private void Move()
    {
        var input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        var newPos = _rectTransform.anchoredPosition + Time.deltaTime * _speed * input;

        newPos = Vector2.Min(newPos, _bounds.rect.max - _rectTransform.rect.size / 2.0f);
        newPos = Vector2.Max(newPos, _bounds.rect.min + _rectTransform.rect.size / 2.0f);

        _rectTransform.anchoredPosition = newPos;
    }

    private bool FindClosestEnemyTarget(out Enemy closestEnemy)
    {
        closestEnemy = null;

        if (Time.time < _lastAttackTime + _attackCooldown)
            return false;

        var minDistSqrd = float.MaxValue;

        foreach (var enemyKvp in _game.Enemies)
        {
            var distSqrd = (enemyKvp.Value.transform.position - transform.position).sqrMagnitude;

            if (distSqrd < minDistSqrd)
            {
                minDistSqrd = distSqrd;
                closestEnemy = enemyKvp.Value;
            }
        }

        return closestEnemy && Mathf.Sqrt(minDistSqrd) < _attackRange;
    }

    private void Attack(Enemy enemy)
    {
        var bullet = Instantiate(_bulletPrefab, transform.position, Quaternion.identity, _bulletsContainer);
        bullet.gameObject.SetActive(true);
        bullet.Launch(transform.position, enemy);
        _lastAttackTime = Time.time;
    }

    public void DealDamage(float damage)
    {
        _health = Mathf.Max(0, _health - damage);

        OnHealthChange?.Invoke(_health);

        if (Mathf.Approximately(_health, 0.0f))
        {
            OnDeath?.Invoke();
        }
    }
}