using UnityEngine;

public class GameGuide : UIWindow
{
    bool isCancelButtonUp = true;
	private void Update ()
    {
        if (Input.GetAxis("Cancel") == 0)
            isCancelButtonUp = true;
        else if (isCancelButtonUp)
        {
            isCancelButtonUp = true;
            GameManager.Singleton.ReturnToMainMenu();
        }
	}
}
