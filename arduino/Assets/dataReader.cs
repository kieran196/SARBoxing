using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class dataReader : MonoBehaviour {

    public bool startedMusic = false;
    public string FILENAME;
    public int timeStampIndex = 0;
    public List<float> timeStamps = new List<float>();
    public List<Vector3> timeStampPos = new List<Vector3>();
    public List<Vector3> timeStampRot = new List<Vector3>();

    void Start() {
        readFile();
    }

    private void readFile() {
        string file_path = FILENAME + ".txt";
        StreamReader reader = new StreamReader(file_path);
        int count = 0;
        while (!reader.EndOfStream) {
            string ln = reader.ReadLine();
            string[] parseVec = ln.Split(',');
            Vector3 vec = new Vector3(float.Parse(parseVec[0]), float.Parse(parseVec[1]), float.Parse(parseVec[2]));
            if (count % 2 == 0) {
                timeStampPos.Add(vec);
                Debug.Log(parseVec[3]);
                timeStamps.Add(float.Parse(parseVec[3]));
            } else {
                timeStampRot.Add(vec);
                //Debug.Log(ln);
            }
            count++;
        }
        reader.Close();
    }

    public GameObject targetPrefab;
    private GameObject lastObject;
    public GameObject punchingBag;

    void addTarget(Vector3 pos, Vector3 rot) {
        // Spawn target at position/rotation
        GameObject cube = Instantiate(targetPrefab);
        if (lastObject != null) {
            lastObject.SetActive(false);
        }
        lastObject = cube;
        cube.transform.localScale = new Vector3(0.1f, 0.15f, 0.1f);
        //lastHit = sReader.currVal;
        //punchForce.text = "Punch Force:" + lastHit;
        cube.transform.position = pos;
        cube.transform.localEulerAngles = rot;
        cube.transform.SetParent(punchingBag.transform, true);
        //cube.transform.GetChild(0).GetComponentInChildren<Text>().text = count.ToString();
        //Debug.Log("Spawned " + cube.transform.name + " at " + pos[count]);
    }

    void spawnNextTarget(float currentTime) {
        if (currentTime >= (timeStamps[timeStampIndex] - DOWN_TIME) && currentTime <= (timeStamps[timeStampIndex] - DOWN_TIME) + 0.09f) {
            //string posSplit = timeStamps[timeStampIndex].
            //Vector3 pos = 
            
            //timeStampIndex
            Debug.Log("Spawned at:" + timeStamps[timeStampIndex]);
            addTarget(timeStampPos[timeStampIndex], timeStampRot[timeStampIndex]);
            timeStampIndex ++;
        }
    }

    private float softTimer = 0f;
    private float timer = 0f;
    private float DOWN_TIME = 0f;

    private void Update() {
        if (startedMusic) {
            timer += Time.deltaTime;
            softTimer += Time.deltaTime;
            if (softTimer > 0.05f && timeStampIndex < timeStamps.Count) {
                spawnNextTarget(timer);
                softTimer = 0f;
            }
        }
    }
}
