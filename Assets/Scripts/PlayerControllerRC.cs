//using statements
using System;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerControllerRC : MonoBehaviour
{

    //used to hold ui game objects
    public GameObject nextLevel, score;
    public Image star1, star2, star3;
    public TextMeshProUGUI timeText;



    [SerializeField]
    private float rotationSpeed = 5f;

    [SerializeField]
    private float moveSpeed = 0.1f;

    float distance = 1.05f;
    public GameObject selectedGO;
    public bool checkCollision = false;
    public bool isMouseDown = false;

    //used to hold time
    private float time = 0;

    //used to hold music
AudioSource music;


    private int directionFlag = 1;
    public Camera mc;
    public Rigidbody rb;
    private float lx, ly;
    private Vector3 origin;
    void Start()
    {
        if (mc == null) mc = Camera.main;
        if (rb == null) rb = GetComponent<Rigidbody>();
        origin = transform.position;


        //disables next level button
        nextLevel.SetActive(false);
        
        //disables score panel
        score.SetActive(false);

    }

   
    void FixedUpdate()
    {

        //sets time
    time += Time.deltaTime;
    timer(time);

        Ray ray = new Ray(), negRay = new Ray();
        Vector3 direction = new Vector3();
                        Vector3 origin = transform.position;



            //sets distance
            if(gameObject.tag == "Ver"){
                
                distance = 1.55f;  
                ray = new Ray(origin, 1 * transform.right);
                negRay = new Ray(origin, -1 * transform.right);
                     
            }
            else if(gameObject.tag == "Hor" || gameObject.tag == "Player"){ 
                distance = 1.05f; 
                ray = new Ray(origin, 1 * transform.forward);
                negRay = new Ray(origin, -1 * transform.forward);
                
            }
            

            

            
            Debug.DrawRay(new Vector3(origin.x,origin.y,origin.z), (directionFlag * direction) * distance, Color.yellow);

        

        if(isMouseDown && (Physics.Raycast(ray, out RaycastHit h, distance)))
        {

            //gets object piece collided wtih
            GameObject go = h.collider.gameObject;

            //if piece collieds with something
            if (go.CompareTag("Wall") || go.CompareTag("Ver") || go.CompareTag("Hor") || go.CompareTag("Player") || go.CompareTag("Goal")) {
                checkCollision = true;
                if(go.CompareTag("Goal")){
                    //sets ui elements to active
                    nextLevel.SetActive(true);
                    score.SetActive(true);

                    //"animates" stars
                    StartCoroutine(animator());

                    //plays music
                    music = GetComponent<AudioSource>();
                    music.Play(0);

                    //displays the amount of time it took to complete the level
                    timeText.text = timer(time);

                }
            }
    
        }
        else if(isMouseDown && (Physics.Raycast(negRay, out RaycastHit h2, distance))){

            //gets object piece collided wtih
            GameObject go = h2.collider.gameObject;

            //if piece collieds with something
            if (go.CompareTag("Wall") || go.CompareTag("Ver") || go.CompareTag("Hor") || go.CompareTag("Player") || go.CompareTag("Goal")) {
                checkCollision = true;
                if(go.CompareTag("Goal")){
                    //sets ui elements to active
                    nextLevel.SetActive(true);
                    score.SetActive(true);

                    //"animates" stars
                    StartCoroutine(animator());

                    //plays music
                    music = GetComponent<AudioSource>();
                    music.Play(0);

                    //displays the amount of time it took to complete the level
                    timeText.text = timer(time);

                }
            }

        }

    }

    private void OnMouseDown()
    {
        RaycastHit hit = findingObjectRC();

        if (hit.collider.gameObject.CompareTag("Player") || hit.collider.gameObject.CompareTag("Hor") || hit.collider.gameObject.CompareTag("Ver"))
        {
            selectedGO = hit.collider.gameObject;
            Cursor.visible = false;

        }
        isMouseDown = true;

    }

    private void OnMouseDrag()
    {

        //sets direction
        if(gameObject.tag == "Ver"){ 

        if ((origin.x - transform.position.x) <= 0)
            directionFlag = 1;
        else
            directionFlag = -1;
        }
        else if(gameObject.tag == "Hor" || gameObject.tag == "Player"){ 

            if ((origin.z - transform.position.z) <= 0)
            directionFlag = 1;
            else
            directionFlag = -1;

        }


        directionFlag = 1;


        if (!checkCollision && selectedGO != null)
        {
            lx = Input.mousePosition.x;
            ly = Input.mousePosition.y;
            Vector3 position = new Vector3(lx, ly, mc.WorldToScreenPoint(selectedGO.transform.position).z);
            Vector3 worldPos = mc.ScreenToWorldPoint(position);

            Vector3 subVecPosition = new Vector3();
            if(gameObject.tag == "Ver"){ subVecPosition = new Vector3(worldPos.x, 0.1f, origin.z);  }
            else if(gameObject.tag == "Hor" || gameObject.tag == "Player"){ subVecPosition = new Vector3(origin.x, 0.1f, worldPos.z);  }


            rb.MovePosition(subVecPosition);

        }
    }

        private void OnMouseUp()
    {
       
            checkCollision = false;
            
            
            rb.MovePosition(new Vector3(rb.transform.position.x,
               origin.y, rb.transform.position.z/1.5f-directionFlag*0.2f));
                
            if(gameObject.tag == "Hor" || gameObject.tag == "Player"){
                rb.MovePosition(new Vector3(rb.transform.position.x,
                    origin.y,
                    Mathf.Round(rb.transform.position.z)));
            }
            else if(gameObject.tag == "Ver"){
                rb.MovePosition(new Vector3(Mathf.Round(rb.transform.position.x),
                    origin.y,
                    rb.transform.position.z));

            }
            selectedGO = null;
            Cursor.visible = true;


        isMouseDown = false;

    }



        private RaycastHit findingObjectRC()
    {
        Vector2 mPosition = Input.mousePosition;
        Ray ray = mc.ScreenPointToRay(mPosition);
        Physics.Raycast(ray, out RaycastHit h);
        return h;

    }


IEnumerator animator()
{


    //Apparently since you cant animate 2D images without a sprite sheet in unity, I "animated" the stars/score by gradually increasing the size
    if(time < 120){
        for(float i = 0; i < (float)1; i += (float)0.02){
                star1.rectTransform.localScale = new Vector2(i, i);

                float time = (float).01;

                    yield return new WaitForSeconds(time);

        }
        if(time < 60){
            for(float i = 0; i < (float)1; i += (float)0.02){

                    star2.rectTransform.localScale = new Vector2(i, i);

                    float time = (float).01;

                        yield return new WaitForSeconds(time);

            }
            if(time < 20){
                for(float i = 0; i < (float)1; i += (float)0.02){

                        star3.rectTransform.localScale = new Vector2(i, i);

                        float time = (float).01;

                            yield return new WaitForSeconds(time);

                }
        

    }
    }
    }

}


//returns the formatted time (used for displaying time when user creates a level)
string timer (float currentTime){

    currentTime += 1;
    float minutes = Mathf.FloorToInt(currentTime / 60);
    float seconds = Mathf.FloorToInt(currentTime % 60);

    return minutes + " Minutes\n" + seconds + " Seconds";


}





}
