using UnityEngine;

public class NightLights : MonoBehaviour
{
    private DayNightCycle dayNightCycle;
    public GameObject lightbulb;

    private void Start()
    {
        dayNightCycle = DayNightCycle.Instance;
        
        dayNightCycle.onNightStart.AddListener(EnableLights);
        dayNightCycle.onDayStart.AddListener(DisableLights);    
    }

    private void DisableLights()
    {
        lightbulb.SetActive(false);
    }

    private void EnableLights()
    {
        lightbulb.SetActive(true);
    }

    private void OnDestroy()
    {
        dayNightCycle.onNightStart.RemoveListener(EnableLights);
        dayNightCycle.onDayStart.RemoveListener(DisableLights);
    }
}
