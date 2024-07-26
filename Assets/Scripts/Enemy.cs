using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    Vector2 Velocity { get; }

    void DealDamage(float amount);
}

public class Enemy : MonoBehaviour, IEnemy
{
    [SerializeField] private float _health;
    [SerializeField] private RectTransform _playerBounds;
    [SerializeField] private Player _player;

    public Vector2 Velocity => Vector2.down * Speed;

    public float Speed { get; set; }

    public System.Action OnDeath;

    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        _rectTransform.anchoredPosition += Time.deltaTime * Velocity;

        if (transform.position.y < _playerBounds.sizeDelta.y)
        {
            _player.DealDamage(1);
            Die();
        }
    }

    public void DealDamage(float amount)
    {
        _health -= amount;

        if (_health <= Mathf.Epsilon)
            Die();
    }

    private void Die()
    {
        Destroy(gameObject);
        OnDeath?.Invoke();
    }
}