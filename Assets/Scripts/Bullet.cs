using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _damage;

    private Vector2 _dir;
    private float _impactTime;
    private Enemy _target;

    public void Launch(Vector3 position, Enemy target)
    {
        var t = Vector2.Distance(position, target.transform.position) / _speed;
        var nPos = target.transform.position + Vector3.down * 100;
        _dir = (nPos - position).normalized;
        _impactTime = Time.time + Vector2.Distance(position, nPos) / _speed;
        _target = target;
    }

    private void Update()
    {
        transform.position += Time.deltaTime * _speed * new Vector3(_dir.x, _dir.y, 0.0f);

        if (Time.time > _impactTime)
        {
            Destroy(gameObject);

            if (_target)
                _target.DealDamage(_damage);
        }
    }
}