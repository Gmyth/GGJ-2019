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
        if (currentIndex > 0)
        {
            Debug.Log("MoveIndexUp");
            Dehighlight(currentIndex);
            currentIndex = Mathf.Max(currentIndex - 1, 0);
            Highlight(currentIndex);
        }
    }

    private void MoveIndexDown()
    {
        if (currentIndex < maxIndex)
        {
            Debug.Log("MoveIndexDown");
            Dehighlight(currentIndex);
            currentIndex = Mathf.Min(currentIndex + 1, maxIndex);
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
        float verticalInput = Input.GetAxis("Vertical");

        if (verticalInput <= 0)
            isUpButtonUp = true;
        else if (isUpButtonUp)
        {
            isUpButtonUp = false;

            MoveIndexUp();
        }

        if (verticalInput >= 0)
            isDownButtonUp = true;
        else if (isDownButtonUp)
        {
            isDownButtonUp = false;

            MoveIndexDown();
        }

        if (Input.GetAxis("Submit") == 0)
            isSubmitButtonUp = true;
        else if (isSubmitButtonUp)
        {
            isSubmitButtonUp = false;

            switch (currentIndex)
            {
                case 0:
                    GameManager.Singleton.SetUpNewMatch();
                    Close();
                    break;

                case 1:
                    GameManager.Singleton.QuitGame();
                    break;
            }
        }
    }
}
