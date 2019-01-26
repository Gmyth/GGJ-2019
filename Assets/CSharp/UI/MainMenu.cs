using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : UIWindow
{
    [SerializeField] private Transform list;

    private List<Text> highlights = new List<Text>();

    private int currentIndex = 0;
    private int maxIndex;

    private void Highlight(int index)
    {
        highlights[index].color = Color.red;
    }

    private void Dehighlight(int index)
    {
        highlights[index].color = Color.white;
    }

    private void MoveIndexUp()
    {
        if (currentIndex < maxIndex - 1)
        {
            Dehighlight(currentIndex);
            currentIndex = Mathf.Min(currentIndex + 1, maxIndex);
            Highlight(currentIndex);
        }
    }

    private void MoveIndexDown()
    {
        if (currentIndex > 0)
        {
            Dehighlight(currentIndex);
            currentIndex = Mathf.Max(currentIndex - 1, 0);
            Highlight(currentIndex);
        }
    }

    bool isUpButtonUp = true;
    bool isDownButtonUp = true;
    bool isSubmitButtonUp = true;

    private void Start()
    {
        maxIndex = list.childCount - 1;

        for (int i = 0; i <= maxIndex; i++)
        {
            highlights.Add(list.GetChild(i).GetComponent<Text>());
            Dehighlight(i);
        }

        Highlight(currentIndex);
    }

    private void Update()
    {
        if (Input.GetKeyUp("Up"))
            isUpButtonUp = true;
        else if (isUpButtonUp)
            MoveIndexUp();

        if (Input.GetKeyUp("Down"))
            isDownButtonUp = true;
        else if (isDownButtonUp)
            MoveIndexDown();

        if (Input.GetKeyUp("Submit"))
            isSubmitButtonUp = true;
        else if (isSubmitButtonUp)
            switch (currentIndex)
            {
                case 0:
                    UIManager.Singleton.Open("MatchSetup");
                    Close();
                    break;

                case 1:
                    GameManager.Singleton.QuitGame();
                    break;
            }
    }
}
