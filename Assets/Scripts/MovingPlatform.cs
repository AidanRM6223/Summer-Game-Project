using UnityEngine;
 using System.Collections;
 
 public class MovingPlatform : MonoBehaviour 
 {
     public Transform[] Waypoints;
     public float speed = 2;
 
     public int CurrentPoint = 0;
    public bool isMovingHorizontally, isMovingHorizontallyLeft, isMovingHorizontallyRight;
    private float stopTime = 2f;
    private void Start() {
        transform.position = Waypoints[0].position;
    }
     void Update () 
     {
         if(transform.position.y != Waypoints[CurrentPoint].transform.position.y)
         {
             transform.position = Vector3.MoveTowards(transform.position, Waypoints[CurrentPoint].transform.position, speed * Time.deltaTime);
             if(transform.position.x < Waypoints[CurrentPoint].transform.position.x) {
                 isMovingHorizontallyRight = true;
                 isMovingHorizontallyLeft = false;
             }
             else if(transform.position.x > Waypoints[CurrentPoint].transform.position.x) {
                 isMovingHorizontallyRight = false;
                 isMovingHorizontallyLeft = true;
             }
         }
         if(transform.position.y == Waypoints[CurrentPoint].transform.position.y)
         {
             isMovingHorizontallyRight = false;
             isMovingHorizontallyLeft = false;
             stopTime -= Time.deltaTime;
             if(stopTime <= 0)
             {
                CurrentPoint +=1;
                stopTime = 2f;
             }
             
         }
         if( CurrentPoint >= Waypoints.Length)
         {
             CurrentPoint = 0; 
         }
     }
 }

