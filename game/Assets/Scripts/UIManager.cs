using System.Linq;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    Image hpBar;
    Text hpBarText;

    float hpMax = 100;

    private void Awake()
    {
        var images = GetComponentsInChildren<Image>();
        Text text = gameObject.GetComponentInChildren<Text>();

        hpBar = images.Single(x => x.name == "HpBar");
    }

    public void SetPopulation(float populationPercent)
    {
        if (populationPercent > hpMax)
            hpMax = populationPercent;

        hpBar.fillAmount = populationPercent / hpMax;

        hpBarText.text = $"Population { (int)(100 / hpBar.fillAmount) }";
    }
}