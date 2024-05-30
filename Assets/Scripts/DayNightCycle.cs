using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    //// Slider�� ���� �����ϱ� ���� Range�� �����ش�.    
    //time�� ������ 0~1(0% ~ 100%)
    [Range(0.0f, 1.0f)]
    public float time;
    //�Ϸ��� ����
    public float fullDayLength;
    //0.5f �϶� ����, 0.4�� �Ϸ��� 40% ���� �ð��� ���� 
    public float startTime = 0.4f;
    private float timeRate;
    //���� , Vector 90 0 0, ������ ����
    public Vector3 noon;

    [Header("Sun")]
    public Light sun;
    //���� Gradient ���� ������ �� ��ȭ�� ǥ��, �׶��̼�
    public Gradient sunColor;
    // AnimationCurve�� ���� �׷����� ������ �� �ִ�.
    // �ش� �׷������� ���ϴ� ������ Time ���� ���� ������ �� �ִ�.
    public AnimationCurve sunIntensity;

    [Header("Moon")]
    public Light moon;
    public Gradient moonColor;
    public AnimationCurve moonIntensity;

    [Header("Other Lighting")]
    //AnimationCurve �� �̿��ؼ� ����
    //ǳ�汤
    public AnimationCurve lightingIntensityMultiplier;
    //�ݻ籤
    public AnimationCurve reflectionIntensityMultiplier;

    private void Start()
    {
        timeRate = 1.0f / fullDayLength;
        time = startTime;
    }

    private void Update()
    {
        // time�� ����ؼ� ���������ִ� ��, time�� �ۼ��������� ����ϱ� ���� 1.0f�� ������ ������ ���ش�.
        time = (time + timeRate * Time.deltaTime) % 1.0f;

        UpdateLighting(sun, sunColor, sunIntensity);
        UpdateLighting(moon, moonColor, moonIntensity);

        //�ð��� �´� ���� �־��ش�
        RenderSettings.ambientIntensity = lightingIntensityMultiplier.Evaluate(time);
        RenderSettings.reflectionIntensity = reflectionIntensityMultiplier.Evaluate(time);

    }

    //����� �ƴ��������� ������Ʈ ���ִ� �Լ��� �ʿ�

    void UpdateLighting(Light lightSource, Gradient colorGradiant, AnimationCurve intensityCurve)
    {   

        float intensity = intensityCurve.Evaluate(time);

        //���� ��ȭ
        // time �� 0.5�� ������ �ð��̰� ������ 90�� �Ǿ���Ѵ�
        // 360���� 0.5 �� 180���� 90���� �ǵ��� 0.25 �� �� ���ش�
        // ���� 0.25�� �߰��� ������ ���������� �ݴ밡 �ǵ��� �ű⿡ 0.5(180��)�� ���Ѵ� 
        // ������ 90 * noon ���⼭ noon�� 0.25 
        // 90 * 0.25 �� 90�� �ƴϱ⶧���� �߰��� 4�� ���Ѵ� 
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