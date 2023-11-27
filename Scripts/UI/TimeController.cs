using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeController : MonoBehaviour
{
    [SerializeField]
    private float timeMultiplier; //controla quão rápido o tempo passa

    [SerializeField]
    private float startHour; //hora em que o jogo começa

    [SerializeField]
    private TextMeshProUGUI timeText; //display do horário

    [SerializeField]
    private Light sunLight; //luz que representa o sol

    [SerializeField]
    private float sunriseHour; //hora do nascer do sol

    [SerializeField]
    private float sunsetHour; //hora do pôr-do-sol

    [SerializeField]
    private Color dayAmbientLight; //luz ambiente durante o dia

    [SerializeField]
    private Color nightAmbientLight; //luz ambiente durante a noite

    [SerializeField]
    private AnimationCurve lightChangeCurve; //curva de transição da luz

    [SerializeField]
    private float maxSunLightIntensity; //intensidade máxima da luz do sol

    [SerializeField]
    private Light moonLight; //luz da lua

    [SerializeField]
    private float maxMoonLightIntensity; //intensidade máxima da luz da lua

    private DateTime currentTime; //mantém gravada a hora atual

    private TimeSpan sunriseTime; //usado para cálculo

    private TimeSpan sunsetTime; //usado para cálculo

    // Start is called before the first frame update
    void Start()
    {
        currentTime = DateTime.Now.Date + TimeSpan.FromHours(startHour); //controla a hora do jogo

        sunriseTime = TimeSpan.FromHours(sunriseHour); //controla o nascer do sol com base na hora definida
        sunsetTime = TimeSpan.FromHours(sunsetHour); //controla o pôr-do-sol com base na hora definida
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimeOfDay();
        RotateSun();
        UpdateLightSettings();
    }

    private void UpdateTimeOfDay() //adiciona segundos à hora do jogo, fazendo o tempo passar, multiplicando pela taxa desejada
    {
        currentTime = currentTime.AddSeconds(Time.deltaTime * timeMultiplier);

        if (timeText != null)
        {
            timeText.text = currentTime.ToString("HH:mm"); //mostra o tempo na tela, caso haja um display
        }
    }

    private void RotateSun() //controla a rotação do sol com base no horário
    {
        float sunLightRotation;

        if (currentTime.TimeOfDay > sunriseTime && currentTime.TimeOfDay < sunsetTime) //verifica se a hora atual está entre o nascer e o pôr do sol
        {
            TimeSpan sunriseToSunsetDuration = CalculateTimeDifference(sunriseTime, sunsetTime); //calcula o tempo total entre o nascer e o pôr do sol
            TimeSpan timeSinceSunrise = CalculateTimeDifference(sunriseTime, currentTime.TimeOfDay); //calcula quanto tempo se passou desde o nascer do sol

            double percentage = timeSinceSunrise.TotalMinutes / sunriseToSunsetDuration.TotalMinutes; //calcula a porcentagem do tempo do dia que já se passou, dividindo o tempo passado desde o nascer  do sol pela distancia total do nascer ao por do sol

            sunLightRotation = Mathf.Lerp(0, 180, (float)percentage); //varia o ângulo do sol com base na porcentagem do dia que já se passou, usando "percentage" como valor de interpolação
        }
        else
        {
            TimeSpan sunsetToSunriseDuration = CalculateTimeDifference(sunsetTime, sunriseTime); //mesmo procedimento, mas se estiver de noite
            TimeSpan timeSinceSunset = CalculateTimeDifference(sunsetTime, currentTime.TimeOfDay);

            double percentage = timeSinceSunset.TotalMinutes / sunsetToSunriseDuration.TotalMinutes;

            sunLightRotation = Mathf.Lerp(180, 360, (float)percentage);
        }

        sunLight.transform.rotation = Quaternion.AngleAxis(sunLightRotation, Vector3.right); //aplica a rotação calculada ao Sol, faz ele rodar ao redor do eixo X
    }

    private void UpdateLightSettings()
    {
        float dotProduct = Vector3.Dot(sunLight.transform.forward, Vector3.down); //calcula o angulo entre o vetor frontal da luz solar e o vetor em direção ao chão (varia de -1 (sol apontando para baixo) a 1 (sol apontando para cima))
        sunLight.intensity = Mathf.Lerp(0, maxSunLightIntensity, lightChangeCurve.Evaluate(dotProduct)); //varia a intensidade da luz do sol com base na posição do sol (de 0 ao máximo), a curva faz uma transição não linear
        moonLight.intensity = Mathf.Lerp(maxMoonLightIntensity, 0, lightChangeCurve.Evaluate(dotProduct));// varia a intensidade da luz da lua, mesma coisa mas do máximo até 0
        RenderSettings.ambientLight = Color.Lerp(nightAmbientLight, dayAmbientLight, lightChangeCurve.Evaluate(dotProduct)); //dá a luz ambiente e varia entre a luz noturna e diurna
    }

    private TimeSpan CalculateTimeDifference(TimeSpan fromTime, TimeSpan toTime) //calcula a diferença entre dois horários
    {
        TimeSpan difference = toTime - fromTime; //diferença = horario1 - horario2

        if (difference.TotalSeconds < 0)
        {
            difference += TimeSpan.FromHours(24); //se a diferença resultar em um número negativo, adiciona 24 horas para representar a mudança do dia
        }

        return difference;
    }
}