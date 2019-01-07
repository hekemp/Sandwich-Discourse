using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrollBG : MonoBehaviour {

    public SpriteRenderer background1;
    public SpriteRenderer background2;
    public SpriteRenderer background3;
    public SpriteRenderer background4;
    public SpriteRenderer background5;

    public float movementSpeed = 1f;
    public float backgroundSize = 10f;

    public bool isMoving;

    public GameObject personBodyPart;
    public float neutralPositionLimit;

    public GameObject backgroundSymbol;

    // Use this for initialization
    void Start () {
        backgroundSize = background1.size.x - .2f;
        Debug.Log(backgroundSize);
	}

    // Update is called once per frame
    void Update() {

        if(backgroundSymbol.transform.localRotation.y > 0)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        if (isMoving)
        {

            float deltaTime = Time.deltaTime;

            background1.transform.SetPositionAndRotation(new Vector3(background1.transform.position.x + movementSpeed * deltaTime * 2, background1.transform.position.y, background1.transform.position.z), background1.transform.rotation);
            background2.transform.SetPositionAndRotation(new Vector3(background2.transform.position.x + movementSpeed * deltaTime * 2, background2.transform.position.y, background2.transform.position.z), background2.transform.rotation);
            background3.transform.SetPositionAndRotation(new Vector3(background3.transform.position.x + movementSpeed * deltaTime * 2, background3.transform.position.y, background3.transform.position.z), background3.transform.rotation);
            background4.transform.SetPositionAndRotation(new Vector3(background4.transform.position.x + movementSpeed * deltaTime * 2, background4.transform.position.y, background4.transform.position.z), background4.transform.rotation);
            background5.transform.SetPositionAndRotation(new Vector3(background5.transform.position.x + movementSpeed * deltaTime * 2, background5.transform.position.y, background5.transform.position.z), background5.transform.rotation);

            if (background1.transform.position.x > 19.0f)
            {
                Vector3 background1Position = new Vector3(background2.transform.position.x - backgroundSize, background1.transform.position.y, background1.transform.position.z);
                background1.transform.position = background1Position;

            }

            if (background2.transform.position.x > 19.0f)
            {
                Vector3 background2Position = new Vector3(background3.transform.position.x - backgroundSize, background2.transform.position.y, background2.transform.position.z);
                background2.transform.position = background2Position;
            }

            if (background3.transform.position.x > 19.0f)
            {
                Vector3 background3Position = new Vector3(background4.transform.position.x - backgroundSize, background3.transform.position.y, background3.transform.position.z);
                background3.transform.position = background3Position;
            }

            if (background4.transform.position.x > 19.0f)
            {
                Vector3 background4Postiion = new Vector3(background5.transform.position.x - backgroundSize, background4.transform.position.y, background4.transform.position.z);
                background4.transform.position = background4Postiion;
            }

            if (background5.transform.position.x > 19.0f)
            {
                Vector3 background5Position = new Vector3(background1.transform.position.x - backgroundSize, background5.transform.position.y, background5.transform.position.z);
                background5.transform.position = background5Position;
            }
        }
    }
}
