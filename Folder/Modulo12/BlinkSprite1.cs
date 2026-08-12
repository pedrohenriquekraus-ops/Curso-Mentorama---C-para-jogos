using UnityEngine;

public class BlinkSprite : MonoBehaviour
{
    public float interval;

    private SpriteRenderer _spriteRenderer;

    private float _nextStateChange;
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.enabled = true;
        _nextStateChange = Time.time + interval;

    }

    // Update is called once per frame
    void Update()
    {
        if (_nextStateChange < Time.time)
        {
            _spriteRenderer.enabled = !_spriteRenderer.enabled;
            _nextStateChange = Time.time + interval;
        }

    }
}
