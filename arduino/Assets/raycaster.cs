using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class raycaster : MonoBehaviour
{
    public string FileName = "data";
    public List<string> logInfo = new List<string>();

    public List<Vector3> pos = new List<Vector3>();
    public List<Vector3> rot = new List<Vector3>();

    public GameObject punchingBag;

    public bool editorMode;
    public Text punchForce;
    private Camera cam;
    public GameObject spawnableHit;
    private int count = 0;

    // Start is called before the first frame update
    void Start() {
        sReader = GetComponent<serialReader>();
        cam = GetComponent<Camera>();
        pos = new List<Vector3>();
        rot = new List<Vector3>();
        readFile();
    }
    int fileSize = 0;
    public void readFile() {
        int count = 0;
        if (!editorMode) {
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
                // Do Something with the input. 
                count++;
                if (count % 2 != 0) {
                    pos.Add(vec);
                    fileSize++;
                } else {
                    rot.Add(vec);
                }
            }

            reader.Close();
        }
      //  readFile();
    }

    public void writeToFile() {
        if (editorMode) {
            string dest = FileName + ".txt";
            StreamWriter writer = null;
            int count = 1;
            bool foundPath = false;
            while (foundPath == false) {
                if (File.Exists(dest)) {
                    dest = FileName + count + ".txt";
                    count++;
                } else {
                    writer = new StreamWriter(dest, true) as StreamWriter;
                    foundPath = true;
                }
            }
            print("Writing..");
            for (int i = 0; i < logInfo.Count; i++) {
                print(logInfo[i]);
                writer.Write(logInfo[i]);
                writer.WriteLine();
            }
            print("Writen to file:" + Application.dataPath);
            writer.Close();
        }
    }

    void OnApplicationQuit() {
        writeToFile();
    }
    public GameObject lastObject = null;
    private serialReader sReader;
    float timer = 0f;
    // Update is called once per frame
    private float lastHit = -1;
    void Update() {
        timer += Time.deltaTime;
        //print(timer);
        /*if (timer > 0.1f) {
            timer = 0;*/
       // Debug.Log(sReader.currVal);
        if (Input.GetMouseButtonDown(0) || sReader.currVal > sReader.vibrationHitCap) {
            if (editorMode) {
                RaycastHit hit;
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 100.0f)) {
                    GameObject cube = Instantiate(spawnableHit);
                    cube.transform.localScale = new Vector3(0.05f, 0.005f, 0.05f);
                    print("hit transform:" + hit.transform);
                    cube.transform.position = hit.point;
                    cube.transform.up = hit.normal;
                    //cube.transform.SetParent(hit.transform);
                    cube.transform.SetParent(punchingBag.transform, true);
                    cube.transform.GetChild(0).GetComponentInChildren<Text>().text = count.ToString();
                    Debug.Log("Hit " + hit.transform.name + " at " + hit.point);
                    logInfo.Add(cube.transform.position.x + "," + cube.transform.position.y + "," + cube.transform.position.z + "," + timer);
                    logInfo.Add(cube.transform.localEulerAngles.x + "," + cube.transform.localEulerAngles.y + "," + cube.transform.localEulerAngles.z);
                    count++;
                }
            } else if (count < fileSize && (lastHit == -1 || lastHit != sReader.currVal) || Input.anyKeyDown) {
                GameObject cube = Instantiate(spawnableHit);
                if (lastObject != null) {
                    lastObject.SetActive(false);
                }
                lastObject = cube;
                cube.transform.localScale = new Vector3(0.1f, 0.15f, 0.1f);
                lastHit = sReader.currVal;
                punchForce.text = "Punch Force:" + lastHit;
                cube.transform.position = pos[count];
                cube.transform.localEulerAngles = rot[count];
                cube.transform.SetParent(punchingBag.transform, true);
                cube.transform.GetChild(0).GetComponentInChildren<Text>().text = count.ToString();
                Debug.Log("Spawned " + cube.transform.name + " at " + pos[count]);
                count++;
                   // timer = 0f;
                //}
            }
        }
    }
}
