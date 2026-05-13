using TMPro;
using UnityEngine;

public class QuestOfferUI : MonoBehaviour
{
    [SerializeField] private GameObject root;
    [SerializeField] private TMP_Text questTitleText;
    [SerializeField] private TMP_Text questDescriptionText;

    public QuestSO2 currentQuestOffer;

    private void Start()
    {
        QuestManager2.Instance.OnQuestOfferReceived += ShowOffer;
        root.SetActive(false);
    }

    private void OnDisable()
    {
        QuestManager2.Instance.OnQuestOfferReceived -= ShowOffer;
    }

    private void ShowOffer(QuestSO2 quest)
    {
        if (root.activeSelf)
        {
            Debug.LogWarning("Quest offer already open.");
            return;
        }

        currentQuestOffer = quest;

        questTitleText.text = quest.questName;
        questDescriptionText.text = quest.questDescription;

        root.SetActive(true);
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
        root.SetActive(false);
    }
}

