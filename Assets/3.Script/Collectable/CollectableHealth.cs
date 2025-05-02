using UnityEngine;

public class CollectableHealth : Collectable
{
    public int gainHealth = 10;
    private Vector3 randomRotationAxis;
    public override void Awake()
    {
        base.Awake();
        randomRotationAxis = Random.insideUnitSphere.normalized;
        if (randomRotationAxis == Vector3.zero)
            randomRotationAxis = Vector3.up;
    }
    public override void Update()
    {
        base.Update();
        transform.Rotate(randomRotationAxis, data.rotationSpeed * Time.deltaTime);
    }  
}
