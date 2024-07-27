using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public int cash;
    public int photoCounter;
    
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();

        GameStateManager.SetGameState(GameState.InGame);
        
        // Set the cursor to not be visible
        Cursor.visible = false;

        // Lock the cursor
        Cursor.lockState = CursorLockMode.Locked;
    }
}
