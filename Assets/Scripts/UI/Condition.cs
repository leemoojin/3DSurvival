using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    // ���簪
    public float curValue;
    public float maxValue;
    //���۰�
    public float startValue;
    //�ֱ������� ���ϴ� �� (��,��) 
    public float regenRate;
    public float decayRate;
    //�̹����� �ִ� fill amount �� ��Ʈ�� (0~1)
    public Image uiBar;

    private void Start()
    {   
        // ����� �����͸� �ҷ��ü����ִ�
        curValue = startValue;
    }

    private void Update()
    {
        //ui ������Ʈ
        uiBar.fillAmount = GetPercentage();
    }

    public void Add(float amount)
    {
        // ���߿� �� ���� ���� ���� , maxValue �� �Ѿ�� �ʴ´�
        curValue = Mathf.Min(curValue + amount, maxValue);
    }

    // ü�� ���� ����
    public void Subtract(float amount)
    {
        curValue = Mathf.Max(curValue - amount, 0.0f);
    }

    // fill amount �� ��Ʈ�� (0~1)
    public float GetPercentage()
    {
        return curValue / maxValue;
    }
}
