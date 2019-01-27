using UnityEngine;

public class IngameMenu : UIWindow
{
    private string controllerID;

    private int currentIndex = 0;
    private int maxIndex;

    public override void OnOpen(params object[] args)
    {
        controllerID = (string)args[0];

        Time.timeScale = 0;
    }

    public override void OnClose()
    {
        Time.timeScale = 1;
    }

    private bool isStartButtonUp = true;
    private bool isSubmitButtonUp = true;

    private void Update()
    {
        if (Input.GetAxis("Start" + controllerID) == 0)
            isStartButtonUp = true;
        else if (isStartButtonUp)
        {
            isStartButtonUp = false;
            Close();
        }

        if (Input.GetAxis("Submit" + controllerID) == 0)
            isSubmitButtonUp = true;
        else if (isSubmitButtonUp)
        {
            isSubmitButtonUp = false;

            switch (currentIndex)
            {
                case 0:
                    Close();
                    break;

                case 1:
                    GameManager.Singleton.RestartMatch();
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
