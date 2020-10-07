using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Threading;

public class serialReader : MonoBehaviour
{

    public enum PORTS {COM1, COM2, COM3, COM4, COM5, COM6, COM7}
    public PORTS port;
    // Start is called before the first frame update
    private SerialPort sp = null;
    public float currVal = 0;
    public float vibrationHitCap = 1000;
    Thread myThread;
    void Start()
    {
        sp = new SerialPort(port.ToString(), 9600);
        sp.Open();

        myThread = new Thread(new ThreadStart(getArduino));
        myThread.Start();
        //sp.ReadTimeout = 1;
        
    }

    private void getArduino() {
        while (myThread.IsAlive) {
            currVal = float.Parse(sp.ReadLine());
        }
    }

    // Update is called once per frame
    void Update() {
        /*
        if (sp != null) {
            try {
                //print(sp.ReadLine() + " , " + float.Parse(sp.ReadLine()));
                currVal = float.Parse(sp.ReadLine());
            } catch (System.Exception) {
            }
        }*/
    }
}
