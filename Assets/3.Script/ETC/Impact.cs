using UnityEngine;
using UnityEngine.Pool;

public class Impact : MonoBehaviour
{
    private ParticleSystem particle;
    private MemoryPool memory;

    private void Awake()
    {
        if(!TryGetComponent(out particle))
            Debug.LogWarning("Impact ] ParticleSystem 이(가) 없음.");
    }
    public void Setup(MemoryPool pool)
    {
        memory = pool;
    }
    private void Update()
    {
        // Particle이 재생 중이 아니라면 비활성화
        if (!particle.isPlaying) memory.DeactivatePoolItem(gameObject);
        
        
    }
}