using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour
{
    public Image image;
    // ui�� ���ڰŸ��� �ӵ�
    public float flashSpeed;
    //�ڷ�Ƽ�� �����ϱ� ���ؼ� �ڷ�ƾ ������ �������Ѵ�
    private Coroutine coroutine;

    private void Start()
    {
        CharacterManager.Instance.Player.condition.onTakeDamage += Flash;
    }

    public void Flash()
    {
        // �������� ������ ������ �� �־ �ڷ�ƾ�� �̹� ���� ���ϼ� ���ִ�
        // �������� �ڷ�ƾ�� ������ �����ϵ��� ������ �߰�
        if (coroutine != null)
        {   
            //�������� �ڷ�ƾ�� �ִٸ� ������
            StopCoroutine(coroutine);
        }

        image.enabled = true;
        image.color = new Color(1f, 105f / 255f, 105f / 255f);
        coroutine = StartCoroutine(FadeAway());
    }

    private IEnumerator FadeAway()
    {
        float startAlpha = 0.3f;
        //���İ��� ���� 
        float a = startAlpha;

        //�������� �Ծ����� �������� �����ٰ� ������ �������°�
        while (a > 0.0f)
        {               
            a -= (startAlpha / flashSpeed) * Time.deltaTime;
            image.color = new Color(1f, 105f / 255f, 105f / 255f, a);
            yield return null;
        }

        //�̹��� ui ����
        image.enabled = false;
    }
}