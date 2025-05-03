using UnityEngine;

public abstract class Collectable : MonoBehaviour
{
    public CollectableData data;
    public virtual void Awake() {}
    public virtual void Update() {}
}
