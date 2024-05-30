using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    //// Slider로 값을 조절하기 위해 Range를 정해준다.    
    //time의 범위는 0~1(0% ~ 100%)
    [Range(0.0f, 1.0f)]
    public float time;
    //하루의 길이
    public float fullDayLength;
    //0.5f 일때 정오, 0.4는 하루중 40% 정도 시간이 지남 
    public float startTime = 0.4f;
    private float timeRate;
    //정오 , Vector 90 0 0, 자정의 각도
    public Vector3 noon;

    [Header("Sun")]
    public Light sun;
    //조명 Gradient 으로 서서히 색 변화를 표현, 그라데이션
    public Gradient sunColor;
    // AnimationCurve를 통해 그래프를 생성할 수 있다.
    // 해당 그래프에서 원하는 값들을 Time 값을 통해 꺼내올 수 있다.
    public AnimationCurve sunIntensity;

    [Header("Moon")]
    public Light moon;
    public Gradient moonColor;
    public AnimationCurve moonIntensity;

    [Header("Other Lighting")]
    //AnimationCurve 를 이용해서 조절
    //풍경광
    public AnimationCurve lightingIntensityMultiplier;
    //반사광
    public AnimationCurve reflectionIntensityMultiplier;

    private void Start()
    {
        timeRate = 1.0f / fullDayLength;
        time = startTime;
    }

    private void Update()
    {
        // time을 계속해서 증가시켜주는 데, time을 퍼센테이지로 사용하기 위해 1.0f의 나머지 연산을 해준다.
        time = (time + timeRate * Time.deltaTime) % 1.0f;

        UpdateLighting(sun, sunColor, sunIntensity);
        UpdateLighting(moon, moonColor, moonIntensity);

        //시간에 맞는 값을 넣어준다
        RenderSettings.ambientIntensity = lightingIntensityMultiplier.Evaluate(time);
        RenderSettings.reflectionIntensity = reflectionIntensityMultiplier.Evaluate(time);

    }

    //조명과 아더세팅쪽을 업데이트 해주는 함수가 필요

    void UpdateLighting(Light lightSource, Gradient colorGradiant, AnimationCurve intensityCurve)
    {   

        float intensity = intensityCurve.Evaluate(time);

        //각도 변화
        // time 이 0.5가 정오인 시간이고 각도는 90이 되어야한다
        // 360도의 0.5 는 180도라서 90도가 되도록 0.25 를 또 빼준다
        // 밤은 0.25를 추가로 뺐을때 정오였으니 반대가 되도록 거기에 0.5(180도)를 더한다 
        // 정오는 90 * noon 여기서 noon은 0.25 
        // 90 * 0.25 는 90이 아니기때문에 추가로 4를 곱한다 
        lightSource.transform.eulerAngles = (time - (lightSource == sun ? 0.25f : 0.75f)) * noon * 4.0f;
        lightSource.color = colorGradiant.Evaluate(time);
        lightSource.intensity = intensity;

        GameObject go = lightSource.gameObject;
        if (lightSource.intensity == 0 && go.activeInHierarchy)
            go.SetActive(false);
        else if (lightSource.intensity > 0 && !go.activeInHierarchy)
            go.SetActive(true);
    }
}