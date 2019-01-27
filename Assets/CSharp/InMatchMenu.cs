using UnityEngine;

public class InMatchMenu : UIWindow
{
    [SerializeField] Transform list;

    private string controllerID;

    private int currentIndex = 0;
    private int maxIndex;

    private GameObject[] selections;

    public override void OnOpen(params object[] args)
    {
        controllerID = (string)args[0];

        Time.timeScale = 0;
    }

    public override void OnClose()
    {
        Time.timeScale = 1;
    }

    private void MoveSelectorUp()
    {
        if (currentIndex > 0)
        {
            Deselect(currentIndex--);
            Select(currentIndex);
        }
    }

    private void MoveSelectorDown()
    {
        if (currentIndex < maxIndex)
        {
            Deselect(currentIndex++);
            Select(currentIndex);
        }
    }

    private void Select(int index)
    {
        selections[index].SetActive(true);
    }

    private void Deselect(int index)
    {
        selections[index].SetActive(false);
    }

    private void Start()
    {
        int numListItem = list.childCount;

        selections = new GameObject[numListItem];
        maxIndex = numListItem - 1;

        for (int i = 0; i <= maxIndex; i++)
        {
            selections[i] = list.GetChild(i).GetChild(0).gameObject;
            Deselect(i);
        }

        Select(currentIndex);
    }

    private bool isUpButtonUp = true;
    private bool isDownButtonUp = true;
    private bool isStartButtonUp = false;
    private bool isSubmitButtonUp = true;

    private void Update()
    {
        float verticalInput = Input.GetAxisRaw("Vertical" + controllerID);

        if (verticalInput <= 0.5f)
            isUpButtonUp = true;
        else if (isUpButtonUp)
        {
            isUpButtonUp = false;
            MoveSelectorUp();
        }

        if (verticalInput >= -0.5f)
            isDownButtonUp = true;
        else if (isDownButtonUp)
        {
            isDownButtonUp = false;
            MoveSelectorDown();
        }

        if (Input.GetAxisRaw("Start" + controllerID) == 0)
            isStartButtonUp = true;
        else if (isStartButtonUp)
        {
            isStartButtonUp = false;

            Close();
            return;
        }

        if (Input.GetAxisRaw("Submit" + controllerID) == 0)
            isSubmitButtonUp = true;
        else if (isSubmitButtonUp)
        {
            isSubmitButtonUp = false;

            Close();

            switch (currentIndex)
            {
                case 1:
                    GameManager.Singleton.StartNewMatch();
                    break;

                case 2:
                    GameManager.Singleton.QuitMatch();
                    break;

                case 3:
                    GameManager.Singleton.QuitGame();
                    break;
            }
        }
    }
}
