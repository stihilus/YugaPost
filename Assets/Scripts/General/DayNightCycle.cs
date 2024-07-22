using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class DayNightCycle : Singleton<DayNightCycle>
{
    public float cycleLength;
    public Vector2 fogStartEndDay;
    public Vector2 fogStartEndNight;
    public Color dayFogColor;
    public Color nightFogColor;
    public Color daySkyColor;
    public Color nightSkyColor;
    public Camera mainCamera;

    public UnityEvent onDayStart;
    public UnityEvent onDayEnd;
    public UnityEvent onNightStart;
    public UnityEvent onNightEnd;

    private bool isDay = true;

    public void Start()
    {
        SetDay();
        // Cycle();
    }

    private void SetDay()
    {
        isDay = true;
        RenderSettings.fogColor = dayFogColor;
        mainCamera.backgroundColor = daySkyColor;
        RenderSettings.fogStartDistance = fogStartEndDay.x;
        RenderSettings.fogEndDistance = fogStartEndDay.y;
    }

    private void Cycle()
    {
        if (isDay)
        {
            // Transition to night
            onDayEnd.Invoke();
            DOTween.To(() => RenderSettings.fogColor, x => RenderSettings.fogColor = x, nightFogColor, cycleLength / 2).SetEase(Ease.Linear);
            DOTween.To(() => mainCamera.backgroundColor, x => mainCamera.backgroundColor = x, nightSkyColor, cycleLength / 2).SetEase(Ease.Linear);
            DOTween.To(() => RenderSettings.fogStartDistance, x => RenderSettings.fogStartDistance = x, fogStartEndNight.x, cycleLength / 2).SetEase(Ease.Linear);
            DOTween.To(() => RenderSettings.fogEndDistance, x => RenderSettings.fogEndDistance = x, fogStartEndNight.y, cycleLength / 2).SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    isDay = false;
                    onNightStart.Invoke();
                    Cycle();
                });
        }
        else
        {
            // Transition to day
            onNightEnd.Invoke();
            DOTween.To(() => RenderSettings.fogColor, x => RenderSettings.fogColor = x, dayFogColor, cycleLength / 2).SetEase(Ease.Linear);
            DOTween.To(() => mainCamera.backgroundColor, x => mainCamera.backgroundColor = x, daySkyColor, cycleLength / 2).SetEase(Ease.Linear);
            DOTween.To(() => RenderSettings.fogStartDistance, x => RenderSettings.fogStartDistance = x, fogStartEndDay.x, cycleLength / 2).SetEase(Ease.Linear);
            DOTween.To(() => RenderSettings.fogEndDistance, x => RenderSettings.fogEndDistance = x, fogStartEndDay.y, cycleLength / 2).SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    isDay = true;
                    onDayStart.Invoke();
                    Cycle();
                });
        }
    }
}