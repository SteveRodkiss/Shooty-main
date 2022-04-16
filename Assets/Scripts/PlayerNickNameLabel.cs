using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerNickNameLabel : MonoBehaviourPun
{
    //the game object of the txt label so we can set the text correctly
    //and update it every fram to face the main camera
    public GameObject nickNameTextLabel;
    private Transform cam;

    // Start is called before the first frame update
    void OnEnable()
    {
        //if we are the local object we dont need out own label
        if (photonView.IsMine)
            nickNameTextLabel.SetActive(false);
        cam = Camera.main.transform;
        nickNameTextLabel.GetComponent<TMP_Text>().text = photonView.Owner.NickName;
    }

    // Update is called once per frame
    void Update()
    {
        if(nickNameTextLabel != null)
        {
            nickNameTextLabel.transform.LookAt(cam);
        }
    }
}
