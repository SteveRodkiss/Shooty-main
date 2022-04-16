using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class LevelMenuManager : MonoBehaviourPunCallbacks
{

    public GameObject menuCanvas;
    bool isEnabled = false;
    GameObject localPlayerGameObject;

    // Start is called before the first frame update
    void Start()
    {
        localPlayerGameObject = GetComponent<LevelManager>().localPlayerGameObject;
        SetCanvaseEnabled(false);
        SetCursor(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isEnabled = !isEnabled;
            SetMenuEnabled(isEnabled);
        }        
    }

    public void SetMenuEnabled(bool _enabled)
    {
        isEnabled = _enabled;
        SetCanvaseEnabled(isEnabled);
        SetCursor(isEnabled);
        localPlayerGameObject = GetComponent<LevelManager>().localPlayerGameObject;
        localPlayerGameObject.GetComponent<PlayerMovement>().enabled = !isEnabled;
    }

    void SetCanvaseEnabled(bool _enabled)
    {
        isEnabled = _enabled;
        menuCanvas.SetActive(isEnabled);
    }

    public void SetCursor(bool state)
    {
        Cursor.lockState = state ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = state;
    }

    public void OnQuitButtonClicked()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(0);        
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        SceneManager.LoadScene(0);
    }



}
