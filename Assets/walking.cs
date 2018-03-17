using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class walking : MonoBehaviour {

	public GameObject outsideArm;
	public GameObject insideArm;
	public GameObject outsideLeg;
	public GameObject insideLeg;

	public bool incomingItem;

	public bool outsideArmMovingForward;
	public bool insideArmMovingForward;
	public bool outsideLegMovingForward;
	public bool insideLegMovingForward;

	// Use this for initialization
	void Start () {
		incomingItem = false;
		outsideArmMovingForward = true;
		insideArmMovingForward = false;
		outsideLegMovingForward = false;
		insideArmMovingForward = true;
	}
	
	// Update is called once per frame
	void Update () {
		while (!incomingItem) {


			// 30 goes backwards, -30 goes forward

			Quaternion outsideArmRotation = outsideArm.transform.rotation;

			if (outsideArmMovingForward) {
				if (outsideArmRotation.z >= 30f) { //reset movingForward and move back
					outsideArmMovingForward = false;
					outsideArmRotation.z = outsideArmRotation.z - 1f * Time.deltaTime;
				} else { // continue moving forward
					outsideArmRotation.z = outsideArmRotation.z + 1f * Time.deltaTime;
				}
			} else { // Moving backwards
				if (outsideArmRotation.z <= -30f) { // reset to moving forward and move forward
					outsideArmMovingForward = true;
					outsideArmRotation.z = outsideArmRotation.z + 1f * Time.deltaTime;
				} else { // continue moving back
					outsideArmRotation.z = outsideArmRotation.z - 1f * Time.deltaTime;
				}
			}

			Quaternion insideArmRotation = insideArm.transform.rotation;

			if (insideArmMovingForward) {
				if (insideArmRotation.z >= 30f) { //reset movingForward and move back
					insideArmMovingForward = false;
					insideArmRotation.z = insideArmRotation.z - 1f * Time.deltaTime;
				} else { // continue moving forward
					insideArmRotation.z = insideArmRotation.z + 1f * Time.deltaTime;
				}
			} else { // Moving backwards
				if (insideArmRotation.z <= -30f) { // reset to moving forward and move forward
					insideArmMovingForward = true;
					insideArmRotation.z = insideArmRotation.z + 1f * Time.deltaTime;
				} else { // continue moving back
					insideArmRotation.z = insideArmRotation.z - 1f * Time.deltaTime;
				}
			}

			Quaternion outsideLegRotation = outsideLeg.transform.rotation;

			if (outsideLegMovingForward) {
				if (outsideLegRotation.z >= 30f) { //reset movingForward and move back
					outsideLegMovingForward = false;
					outsideLegRotation.z = outsideLegRotation.z - 1f * Time.deltaTime;
				} else { // continue moving forward
					outsideLegRotation.z = outsideLegRotation.z + 1f * Time.deltaTime;
				}
			} else { // Moving backwards
				if (outsideLegRotation.z <= -30f) { // reset to moving forward and move forward
					outsideLegMovingForward = true;
					outsideLegRotation.z = outsideLegRotation.z + 1f * Time.deltaTime;
				} else { // continue moving back
					outsideLegRotation.z = outsideLegRotation.z - 1f * Time.deltaTime;
				}
			}

			Quaternion insideLegRotation = insideLeg.transform.rotation;

			if (insideLegMovingForward) {
				if (insideLegRotation.z >= 30f) { //reset movingForward and move back
					insideLegMovingForward = false;
					insideLegRotation.z = insideLegRotation.z - 1f * Time.deltaTime;
				} else { // continue moving forward
					insideLegRotation.z = insideLegRotation.z + 1f * Time.deltaTime;
				}
			} else { // Moving backwards
				if (insideLegRotation.z <= -30f) { // reset to moving forward and move forward
					insideLegMovingForward = true;
					insideLegRotation.z = insideLegRotation.z + 1f * Time.deltaTime;
				} else { // continue moving back
					insideLegRotation.z = insideLegRotation.z - 1f * Time.deltaTime;
				}
			}

			outsideArm.transform.rotation = outsideArmRotation;
			insideArm.transform.rotation = insideArmRotation;
			outsideLeg.transform.rotation = outsideLegRotation;
			insideLeg.transform.rotation = insideLegRotation;

		}
	}
}
