using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    // 현재값
    public float curValue;
    public float maxValue;
    //시작값
    public float startValue;
    //주기적으로 변하는 값 (증,감) 
    public float regenRate;
    public float decayRate;
    //이미지에 있는 fill amount 를 컨트롤 (0~1)
    public Image uiBar;

    private void Start()
    {   
        // 저장된 데이터를 불러올수도있다
        curValue = startValue;
    }

    private void Update()
    {
        //ui 업데이트
        uiBar.fillAmount = GetPercentage();
    }

    public void Add(float amount)
    {
        // 둘중에 더 작은 값을 리턴 , maxValue 를 넘어가지 않는다
        curValue = Mathf.Min(curValue + amount, maxValue);
    }

    // 체력 감소 로직
    public void Subtract(float amount)
    {
        curValue = Mathf.Max(curValue - amount, 0.0f);
    }

    // fill amount 를 컨트롤 (0~1)
    public float GetPercentage()
    {
        return curValue / maxValue;
    }
}
