using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class trackerTransform : MonoBehaviour
{
    public bool assignTrackingOffset;
    public bool assignTrackingOffsetWZ;
    public GameObject markerRef;
    public GameObject cube;
    private Vector3 posTransform;
    public Vector3 trackerOffsetXY;
    public Vector3 trackerOffsetZ;
    string FileName = "calibration";

    public void readFile() {
        int count = 0;
        string file_path = FileName + ".txt";
        StreamReader reader = new StreamReader(file_path);
        while (!reader.EndOfStream) {
            string ln = reader.ReadLine();
            string[] lnSplit = ln.Split(',');
            Vector3 vec = new Vector3();
            for (int i = 0; i < lnSplit.Length; i++) {
                if (i == 0)
                    vec.x = float.Parse(lnSplit[i]);
                else if (i == 1)
                    vec.y = float.Parse(lnSplit[i]);
                else if (i == 2)
                    vec.z = float.Parse(lnSplit[i]);
            }
            if (count == 0) {
                trackerOffsetXY = vec;
            } else if (count == 1) {
                trackerOffsetZ = vec;
            }
            count++;
        }
    }

    private void Awake() {
        readFile(); //if file exists..
    }

    public Vector3 startPos = Vector3.zero;
    public float bagOffset = 0.46f;
    private Vector3 previous;
    public bool calculateVelocity;
    private float overallVelocity;
    public float bagMass = 50f;

    float calculateKineticEnergy(float mass, float velocity) {
        return 0.5f * mass * Mathf.Sqrt(velocity);
    }

    void CalcVelocity() {
        if (previous != Vector3.zero) {
            float velocity = ((this.transform.position - previous).magnitude) / Time.deltaTime;
            overallVelocity += velocity;
            previous = this.transform.position;
        } else {
            previous = this.transform.position;
        }
    }
    float timer = 0f;
    // Update is called once per frame
    void Update() {
        if (calculateVelocity) {
            timer += Time.deltaTime;
            if (timer > 0.5f) {
                calculateVelocity = false;
                timer = 0f;
                Debug.Log("Velocity:" + overallVelocity);
                Debug.Log("KE = " + calculateKineticEnergy(bagMass, overallVelocity) + "J");
                overallVelocity = 0f;
            }
            CalcVelocity();
        }
        posTransform = markerRef.transform.position;
        this.transform.position = posTransform;
        if (assignTrackingOffset) {
            cube.transform.position = new Vector3(posTransform.x - trackerOffsetXY.x, (posTransform.y - trackerOffsetXY.y) - bagOffset, posTransform.z);
            cube.transform.localEulerAngles = markerRef.transform.localEulerAngles;
        } else if (assignTrackingOffsetWZ) {
            Vector3 cubeTransform = new Vector3(posTransform.x - trackerOffsetXY.x, posTransform.y - trackerOffsetXY.y, posTransform.z);
            if (startPos == Vector3.zero) {
                startPos = cubeTransform;
            }
            float zOff = startPos.z - trackerOffsetZ.z;
            float getXOff = zOff / trackerOffsetZ.x;
            float getYOff = zOff / trackerOffsetZ.y;
            float currPos = startPos.z - cubeTransform.z;
            
            float finalOffX = getXOff * currPos;
            float finalOffY = getYOff * currPos;
            cube.transform.position = new Vector3(finalOffX * (cubeTransform.x - trackerOffsetXY.x), finalOffY * (cubeTransform.y - trackerOffsetXY.y), cubeTransform.z);
        }
    }
}
