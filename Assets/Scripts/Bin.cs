using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bin : MonoBehaviour
{
    public SpriteRenderer Visuals;
    public Text CoolDownLabel;
    public bool IsAvailable = true;
    public bool CanScore { get { return this.WashCoolDownRemaining >= (this.CoolDownTime - this.ScoringGracePeriod); } }
    public float CoolDownTime = 20f;
    public float ScoringGracePeriod = 2f;
    private float WashCoolDownRemaining;
    public void WashCycle()
    {
        this.Visuals.color = new Color(1f, 1f, 1f, 0.2f);
        this.WashCoolDownRemaining = this.CoolDownTime;
        this.IsAvailable = false;
        StartCoroutine(RefreshLabel());
    }


    IEnumerator RefreshLabel()
    {
        while (WashCoolDownRemaining > 0f)
        {
            this.CoolDownLabel.text = Mathf.CeilToInt(this.WashCoolDownRemaining).ToString();
            yield return new WaitForSeconds(1f);
        }
    }


    private void Update()
    {
        if (this.WashCoolDownRemaining > 0f)
        {
            this.WashCoolDownRemaining -= Time.deltaTime;

            if (this.WashCoolDownRemaining <= 0f)
            {
                this.IsAvailable = true;
                this.Visuals.color = new Color(1f, 1f, 1f, 1f);
                this.CoolDownLabel.text = "DropOff";
            }
        }
    }
}
