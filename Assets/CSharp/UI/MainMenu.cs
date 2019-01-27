using System.Collections.Generic;
using UnityEngine;

public class MainMenu : UIWindow
{
    [SerializeField] private Transform list;

    private List<GameObject> selections = new List<GameObject>();

    private int currentIndex = 0;
    private int maxIndex;

    public override void OnOpen(params object[] args)
    {
        maxIndex = list.childCount - 1;

        for (int i = 0; i <= maxIndex; i++)
        {
            selections.Add(list.GetChild(i).GetChild(0).gameObject);
            Deselect(i);
        }

        currentIndex = Mathf.Clamp((int)args[0], 0, maxIndex);
        Select(currentIndex);
    }

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
            Deselect(currentIndex--);
            Select(currentIndex);
        }
    }

    private void MoveIndexDown()
    {
        if (currentIndex < maxIndex)
        {
            Deselect(currentIndex++);
            Select(currentIndex);
        }
    }

    bool isUpButtonUp = false;
    bool isDownButtonUp = false;
    bool isSubmitButtonUp = false;

    private void Update()
    {
        float verticalInput = Input.GetAxis("Vertical");

        if (verticalInput <= 0.5f)
            isUpButtonUp = true;
        else if (isUpButtonUp)
        {
            isUpButtonUp = false;

            MoveIndexUp();
            AudioManager.Instance.PlaySoundEffect("ButtonClick3");
        }

        if (verticalInput >= -0.5f)
            isDownButtonUp = true;
        else if (isDownButtonUp)
        {
            isDownButtonUp = false;

            MoveIndexDown();
            AudioManager.Instance.PlaySoundEffect("ButtonClick3");
        }

        if (Input.GetAxis("Submit") == 0)
            isSubmitButtonUp = true;
        else if (isSubmitButtonUp)
        {
            AudioManager.Instance.PlaySoundEffect("ClickDown2");

            isSubmitButtonUp = false;

            switch (currentIndex)
            {
                case 0:
                    GameManager.Singleton.SetUpNewMatch();
                    break;

                case 1:
                    GameManager.Singleton.ShowGameGuide();
                    break;

                case 2:
                    GameManager.Singleton.QuitGame();
                    break;
            }
        }
    }
}
