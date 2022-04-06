using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    /**
     * VARS
     */
    public bool Player1 = false;
    public bool Player2 = false;

    /**
     * PLAYER 1
     */
    [System.Serializable]
    public class Player1Keybinds
    {
        public KeyCode upKey;
        public KeyCode downKey;
        public KeyCode leftKey;
        public KeyCode rightKey;
        public KeyCode jumpKey;
        public KeyCode boostKey;
        public KeyCode driftKey1;
        public KeyCode driftKey2;
        public KeyCode cycleLevelKey;
        public string verticalAxis;
        public string horizontalAxis;
    }

    [System.Serializable]
    public class Player1Input
    {
        public bool forwardInput;
        public bool backwardInput;
        public bool leftInput;
        public bool rightInput;
        public bool jumpInput;
        public bool boostInput;
        public bool driftInput;
        public bool cycleLevelInput;
        public float verticalAxis;
        public float horizontalAxis;
    }

    /**
     * PLAYER 2
     */
    public class Player2Input
    {

    }

    /**
     * Class Instanciation
     */
    public Player1Keybinds player1keyboard = new Player1Keybinds();
    public Player1Keybinds player1gamepad = new Player1Keybinds();
    public Player1Input player1Input = new Player1Input();


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Getters&Setters
    public void GetInput_Player1()
    {
        // Reset
        player1Input.forwardInput = false;
        player1Input.backwardInput = false;
        player1Input.leftInput = false;
        player1Input.rightInput = false;
        player1Input.jumpInput = false;
        player1Input.boostInput = false;
        player1Input.driftInput = false;
        player1Input.cycleLevelInput = false;

        // Keys
        if (Input.GetKey(player1keyboard.upKey) || Input.GetKey(player1gamepad.upKey))
        {
            player1Input.forwardInput = true;
        }
        if (Input.GetKey(player1keyboard.downKey) || Input.GetKey(player1gamepad.downKey))
        {
            player1Input.backwardInput = true;
        }
        if (Input.GetKey(player1keyboard.leftKey) || Input.GetKey(player1gamepad.leftKey))
        {
            player1Input.leftInput = true;
        }
        if (Input.GetKey(player1keyboard.rightKey) || Input.GetKey(player1gamepad.rightKey))
        {
            player1Input.rightInput = true;
        }
        if (Input.GetKey(player1keyboard.jumpKey) || Input.GetKey(player1gamepad.jumpKey))
        {
            player1Input.jumpInput = true;
        }
        if (Input.GetKeyDown(player1keyboard.boostKey) || Input.GetKeyDown(player1gamepad.boostKey))
        {
            player1Input.boostInput = true;
        }
        if (Input.GetKey(player1keyboard.driftKey1) || Input.GetKey(player1keyboard.driftKey2) || Input.GetKey(player1gamepad.driftKey1) || Input.GetKey(player1gamepad.driftKey2))
        {
            player1Input.driftInput = true;
        }
        if (Input.GetKeyDown(player1keyboard.cycleLevelKey) || Input.GetKeyDown(player1gamepad.cycleLevelKey))
        {
            player1Input.cycleLevelInput = true;
        }

        // Axis
        player1Input.verticalAxis = Input.GetAxis("Vertical");
        player1Input.horizontalAxis = Input.GetAxis("Horizontal");
    }

    public void GetInput_Player2()
    {
        Debug.LogWarning("GetInput_Player2 is not yet implemented");
    }
    #endregion
}
