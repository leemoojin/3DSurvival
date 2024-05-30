using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour
{
    public Image image;
    // ui가 깜박거리는 속도
    public float flashSpeed;
    //코루티을 실행하기 위해서 코루틴 변수를 만들어야한다
    private Coroutine coroutine;

    private void Start()
    {
        CharacterManager.Instance.Player.condition.onTakeDamage += Flash;
    }

    public void Flash()
    {
        // 데미지를 여러번 받을수 도 있어서 코루틴이 이미 실행 중일수 도있다
        // 실행중인 코루틴이 끝나야 시작하도록 조건을 추가
        if (coroutine != null)
        {   
            //실행중인 코루틴이 있다면 끝낸다
            StopCoroutine(coroutine);
        }

        image.enabled = true;
        image.color = new Color(1f, 105f / 255f, 105f / 255f);
        coroutine = StartCoroutine(FadeAway());
    }

    private IEnumerator FadeAway()
    {
        float startAlpha = 0.3f;
        //알파값은 투명도 
        float a = startAlpha;

        //데미지를 입었을때 붉은색이 보였다가 서서히 옅어지는것
        while (a > 0.0f)
        {               
            a -= (startAlpha / flashSpeed) * Time.deltaTime;
            image.color = new Color(1f, 105f / 255f, 105f / 255f, a);
            yield return null;
        }

        //이미지 ui 끄기
        image.enabled = false;
    }
}