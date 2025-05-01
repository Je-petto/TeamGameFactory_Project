using UnityEngine;
public abstract class Ability
{
    // Ability 활성화
    public virtual void Activate(object obj = null) {}
    // Ability 비활성화
    public virtual void Deactivate() {}
    // Ability 활성화 및 갱신
    public virtual void Update() {}
    // 빠른 Update Ver
    public virtual void FixedUpdate() {}
}
public abstract class Ability<D> : Ability where D : AbilityData
{
    // 읽기 전용 데이터
    public D Data { get; }
    protected PlayerBehaviour player;

    // Ability에서 사용되는 owner를 사용한다는 의미
    // 덮어쓰기
    public Ability(D data, PlayerBehaviour player) 
    {
        this.Data = data;
        this.player = player;
    }
}
[CreateAssetMenu(menuName = "ScriptableObject/Abilities", fileName = "Abilities")]
public class AbilityData : ScriptableObject
{
    
}