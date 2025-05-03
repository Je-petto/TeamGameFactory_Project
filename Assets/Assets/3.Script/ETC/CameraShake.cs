using System.Collections; // Coroutine을 사용하려면 필요해
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    // 흔들림의 원래 위치 (흔들림이 끝난 후 돌아올 위치)
    private Vector3 initialPosition;

    void Awake()
    {
        // 스크립트가 시작될 때 카메라의 현재 위치를 저장해둬.
        initialPosition = transform.localPosition; // 부모 오브젝트가 있다면 LocalPosition을 사용하는 것이 좋아. 부모가 없다면 transform.position도 괜찮아.
    }

    // 화면 흔들림을 시작하는 public 메소드
    // 다른 스크립트에서 이 메소드를 호출해서 흔들림 효과를 트리거할 거야.
    public void Shake(float duration, float magnitude)
    {
        // 만약 이미 흔들림 코루틴이 실행 중이라면 이전 코루틴을 멈추고 새로 시작할 수도 있어.
        // StopAllCoroutines(); // 현재 이 스크립트에서 실행 중인 모든 코루틴을 멈춤
        StartCoroutine(ShakeCoroutine(duration, magnitude)); // 흔들림 코루틴 시작
    }

    // 실제 흔들림 효과를 구현하는 코루틴
    private IEnumerator ShakeCoroutine(float duration, float magnitude)
    {
        float elapsedTime = 0f; // 경과 시간

        // 코루틴이 시작될 때 현재 카메라의 로컬 위치를 다시 저장해둬.
        // Shake() 메소드가 호출될 때마다 이 위치에서 흔들림이 시작되게 하는 거지.
        Vector3 startPosition = transform.localPosition; // 또는 transform.position

        while (elapsedTime < duration)
        {
            // 0부터 1 사이의 무작위 벡터를 만들고 magnitude(강도)를 곱해서 흔들림 오프셋을 계산해.
            // Random.insideUnitSphere는 반지름 1인 구 안의 무작위 점을 반환해.
            Vector3 randomOffset = Random.insideUnitSphere * magnitude;

            // 카메라 위치를 시작 위치 + 무작위 오프셋으로 설정
            transform.localPosition = startPosition + randomOffset; // 또는 transform.position = startPosition + randomOffset;

            // 경과 시간 업데이트
            elapsedTime += Time.deltaTime;

            // 다음 프레임까지 대기
            yield return null;
        }

        // 흔들림 시간이 끝나면 카메라 위치를 원래 위치로 되돌려놔.
        transform.localPosition = startPosition; // 또는 transform.position = initialPosition;
    }

    // (선택 사항) 흔들림 강도를 시간에 따라 줄이고 싶다면 ShakeCoroutine 내부를 수정해야 해.
    // 예를 들어 magnitude를 시작 시점에서는 크게, 끝날 때는 0에 가깝게 Lerp 시키는 방식.
    // 이렇게 하면 흔들림이 점점 잦아드는 효과를 낼 수 있어.
    /*
    private IEnumerator ShakeCoroutineWithDecay(float duration, float startMagnitude)
    {
        float elapsedTime = 0f;
        Vector3 startPosition = transform.localPosition;

        while (elapsedTime < duration)
        {
            float currentMagnitude = Mathf.Lerp(startMagnitude, 0f, elapsedTime / duration); // 시간이 지날수록 강도를 줄여
            Vector3 randomOffset = Random.insideUnitSphere * currentMagnitude;

            transform.localPosition = startPosition + randomOffset;

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = startPosition; // 끝난 후 제자리로
    }
    */
}