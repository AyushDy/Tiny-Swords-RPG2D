using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestRewardSlot : MonoBehaviour
{
    public Image rewardImage;
    public TMP_Text rewardQuantityText;

    public void DisplayReward(Sprite sprite, int quantity)
    {
        rewardImage.sprite = sprite;
        rewardQuantityText.text = quantity.ToString();
    }
}
