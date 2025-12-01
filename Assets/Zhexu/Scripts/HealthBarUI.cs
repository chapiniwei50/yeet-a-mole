using UnityEngine;
using UnityEngine.UI; 

public class HealthBarUI : MonoBehaviour
{
    [Header("References")]
    public WorldVariable worldVariable; 
    public Image fillImage;             // reference to the UI Image representing the health bar fill

    [Header("Settings")]
    public int maxHealth = 5;           // max health value

    void Start()
    {
        // if worldVariable is not assigned in the inspector, try to find it in the scene
        if (worldVariable == null)
            worldVariable = FindAnyObjectByType<WorldVariable>();
    }

    void Update()
    {
        if (worldVariable != null && fillImage != null)
        {
            // compute health percentage
            float hpPercent = (float)worldVariable.playerHealth / maxHealth;

            // limit fill amount between 0 and 1
            fillImage.fillAmount = Mathf.Clamp01(hpPercent);
            
            // change color based on health percentage
            if (hpPercent < 0.3f)
                fillImage.color = Color.red; // red for critical health
            else
                fillImage.color = Color.green; // green for healthy
        }
    }
}