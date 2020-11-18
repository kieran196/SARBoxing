using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class getPuckData : MonoBehaviour
{
    private SteamVR_TrackedObject trackedObj;
    // Start is called before the first frame update
    void Start()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    public bool calculateVelocity;
    private float overallVelocity;
    public float bagMass = 50f;
    public float velocity;
    public float angularVelocity;
    public float velocityCustom;
    public float kineticEnergy;

    private Vector3 previous;

    float calculateKineticEnergy(float mass, float velocity) {
        return 0.5f * mass * Mathf.Sqrt(velocity);
    }

    float CalcVelocity() {
        if (previous != Vector3.zero) {
            float velocity = ((this.transform.position - previous).magnitude) / Time.deltaTime;
            overallVelocity += velocity;
            previous = this.transform.position;
        } else {
            previous = this.transform.position;
        }
        return velocity;
    }
    float timer = 0f;
    float prevAngularVelocity = -1f;
    float coolDownTimer = 0f;
    // Update is called once per frame
    void Update()
    {
        SteamVR_Controller.Device device = SteamVR_Controller.Input((int)trackedObj.index);
        coolDownTimer += Time.deltaTime;
        //CalcVelocity();
        //if (calculateKineticEnergy(bagMass, device.velocity.magnitude) > 50f) {
        velocityCustom = CalcVelocity();
        velocity = device.velocity.magnitude;
        //if (prevAngularVelocity != -1 && device.angularVelocity.magnitude-prevAngularVelocity)
        //Debug.Log(device.angularVelocity.magnitude - prevAngularVelocity);
        float diff = device.angularVelocity.magnitude - prevAngularVelocity;
        if (diff > 0.8f && coolDownTimer > 0.7f) {
            Debug.Log("Hit bag - " + diff);
            coolDownTimer = 0f;
        }
        prevAngularVelocity = device.angularVelocity.magnitude;
        angularVelocity = device.angularVelocity.magnitude;
        //Debug.Log("KE = " + calculateKineticEnergy(bagMass, overallVelocity) + "J");
        //}
        // overallVelocity = 0f;
        /*if (device.velocity.magnitude > 0.1f) {
            Debug.Log("Bag hit.");
        }*/
        /*if (calculateVelocity) {
            timer += Time.deltaTime;
            if (timer > 0.5f) {
                calculateVelocity = false;
                timer = 0f;
                Debug.Log("Velocity:" + overallVelocity);
                Debug.Log("KE = " + calculateKineticEnergy(bagMass, overallVelocity) + "J");
                overallVelocity = 0f;
            }
            CalcVelocity();
        }*/
        // Debug.Log(device.velocity.magnitude + "_" + device.angularVelocity);
    }
}
