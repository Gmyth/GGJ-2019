using System.Collections.Generic;
using UnityEngine;

public class MainMenu : UIWindow
{
    [SerializeField] private Transform list;

    private List<GameObject> selections = new List<GameObject>();

    private int currentIndex = 0;
    private int maxIndex;

    private void Select(int index)
    {
        selections[index].SetActive(true);
    }

    private void Deselect(int index)
    {
        selections[index].SetActive(false);
    }

    private void MoveIndexUp()
    {
        if (currentIndex > 0)
        {
            Deselect(currentIndex);
            currentIndex = Mathf.Max(currentIndex - 1, 0);
            Select(currentIndex);
        }
    }

    private void MoveIndexDown()
    {
        if (currentIndex < maxIndex)
        {
            Deselect(currentIndex);
            currentIndex = Mathf.Min(currentIndex + 1, maxIndex);
            Select(currentIndex);
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
            selections.Add(list.GetChild(i).GetChild(0).gameObject);
            Deselect(i);
        }

        Select(currentIndex);
    }

    private void Update()
    {
        float verticalInput = Input.GetAxis("Vertical");

        if (verticalInput <= 0.5f)
            isUpButtonUp = true;
        else if (isUpButtonUp)
        {
            isUpButtonUp = false;

            MoveIndexUp();
        }

        if (verticalInput >= -0.5f)
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
                    break;

                case 1:
                    GameManager.Singleton.QuitGame();
                    break;
            }
        }
    }
}
