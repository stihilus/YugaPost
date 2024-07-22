using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public int cash;
    public int photoCounter;
    
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        
        // Set the cursor to not be visible
        Cursor.visible = false;

        // Lock the cursor
        Cursor.lockState = CursorLockMode.Locked;
    }
}
