using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class getOffset : MonoBehaviour
{

    public GameObject trackedMarker;
    public bool calibrateMarkerXY = false;
    public bool calibrateMarkerZ = false;
    string FileName = "calibration";
    public List<string> logInfo = new List<string>();
    //internal Vector3 calibVecXY = Vector3.zero;
    //internal Vector3 calibVecZ = Vector3.zero;
    public void writeToFile() {
        string dest = FileName + ".txt";
        StreamWriter writer = null;
        for (int i = 0; i < logInfo.Count; i++) {
            print(logInfo[i]);
            writer.Write(logInfo[i]);
            writer.WriteLine();
        }
        print("Writen to file:" + Application.dataPath);
        writer.Close();
    }

    // Update is called once per frame
    void Update()
    {
        if (calibrateMarkerXY) {
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, trackedMarker.transform.position.z);
            //print("marker pos:" + trackedMarker.transform.position);
            float offsetX = trackedMarker.transform.position.x - this.transform.position.x;
            float offsetY = trackedMarker.transform.position.y - this.transform.position.y;
            float offsetZ = trackedMarker.transform.position.z - this.transform.position.z;
            logInfo.Add(offsetX+","+ offsetY+","+ offsetZ);
            writeToFile();
            print("offset xy:" + offsetX + " , " + offsetY + " , " + offsetZ);
            calibrateMarkerXY = false;
        }
        if (calibrateMarkerZ) {
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, trackedMarker.transform.position.z);
            //print("marker pos:" + trackedMarker.transform.position);
            float offsetX = trackedMarker.transform.position.x - this.transform.position.x;
            float offsetY = trackedMarker.transform.position.y - this.transform.position.y;
            float offsetZ = trackedMarker.transform.position.z - this.transform.position.z;
            logInfo.Add(offsetX + "," + offsetY + "," + offsetZ);
            writeToFile();
            print("offset xy:" + offsetX + " , " + offsetY + " , " + offsetZ);
            calibrateMarkerZ = false;
        }
    }
}
