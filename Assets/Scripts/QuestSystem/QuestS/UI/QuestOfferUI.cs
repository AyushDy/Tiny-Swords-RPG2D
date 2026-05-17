using TMPro;
using UnityEngine;

public class QuestOfferUI : MonoBehaviour
{
    [SerializeField] private TMP_Text questTitleText;
    [SerializeField] private TMP_Text questDescriptionText;

    private CanvasGroup questOfferCanvasGroup;


    public QuestSO2 currentQuestOffer;

    private void Start()
    {
        QuestManager2.Instance.OnQuestOfferReceived += ShowOffer;
        questOfferCanvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnDisable()
    {
        QuestManager2.Instance.OnQuestOfferReceived -= ShowOffer;
    }

    private void ShowOffer(QuestSO2 quest)
    {
        if (questOfferCanvasGroup.alpha > 0)
        {
            Debug.LogWarning("Quest offer already open.");
            return;
        }

        currentQuestOffer = quest;

        questTitleText.text = quest.questName;
        questDescriptionText.text = quest.questDescription;

        questOfferCanvasGroup.alpha = 1;
        questOfferCanvasGroup.interactable = true;
        questOfferCanvasGroup.blocksRaycasts = true;
        Time.timeScale = 0f;
    }

    public void AcceptOffer()
    {
        QuestManager2.Instance.AcceptPendingQuestOffer();
        Close();
    }

    public void DeclineOffer()
    {
        QuestManager2.Instance.DeclinePendingQuestOffer();
        Close();
    }

    private void Close()
    {
        currentQuestOffer = null;
        Time.timeScale = 1f;
        questOfferCanvasGroup.alpha = 0;
        questOfferCanvasGroup.interactable = false;
        questOfferCanvasGroup.blocksRaycasts = false;
    }
}

