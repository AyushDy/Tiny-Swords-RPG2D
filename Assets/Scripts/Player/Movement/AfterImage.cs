using UnityEngine;

public class AfterImage : MonoBehaviour
{

    public float lifeTime = 0.25f;
    private float timeLeft;

    private SpriteRenderer sr;

    private Color startColor;



    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        startColor = sr.color;
        timeLeft = lifeTime;
    }

    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;

        float alpha = timeLeft / lifeTime;
        sr.color = new Color(startColor.r, startColor.g, startColor.b, alpha);

        if(timeLeft <= 0)
        {
            Destroy(gameObject);
        }
    }
}
