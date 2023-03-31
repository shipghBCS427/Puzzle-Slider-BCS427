//using statements
using System;
using System.Collections;
 using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DragComponent : MonoBehaviour {


//dragging is used for telling whether a given sprite is being dragged
//firstClick is used for telling when the user first clicks the item they want to drag 
public bool dragging = false, firstClick = false;

//used to hold x and y values
private float x = 0, y = 0;

//used to hold ui game objects
public GameObject nextLevel, score;
public Image star1, star2, star3;
public TextMeshProUGUI timeText;

//used to hold time
private float time = 0;

//used to hold music
AudioSource music;


void Start(){

    //disables next level button
    nextLevel.SetActive(false);
    
    //disables score panel
    score.SetActive(false);

}



// Update is called once per frame
void Update ()
{

    detectCollision();

    //sets time
    time += Time.deltaTime;
    timer(time);

    //checks for user input
    CheckForClicks();

    //if this is currently being dragged
    if (this.dragging)
    {

        //holds mouse position
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 position = new Vector3();

        
        //if piece is only meant to be moved horizontally, then restrict y movement
        if(gameObject.tag == "hor"){
            if(firstClick){
                position = new Vector3(mousePosition.x,mousePosition.y, 1);
                firstClick = false;
            }
            else{
                position = new Vector3(mousePosition.x, y, 1);
            }
        }

        //if piece is only meant to be moved vertically, then restrict x movement
        else if(gameObject.tag == "ver"){

            if(firstClick){
                position = new Vector3(mousePosition.x,mousePosition.y, 1);
                firstClick = false;
            }
            else{
                position = new Vector3(x, mousePosition.y, 1);
            }
        }
           

        
        //actually moves piece
        transform.position = position;
    }
}


//responsible for checking when the user clicks and what happens afterwards
private void CheckForClicks()
{
    //if player clicks left click
    if (Input.GetMouseButtonDown(0))
    {

        //get mouse position and cast ray in order to get object clicked by mouse
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        //if object has a collider, get object
        if (hit.collider != null)
        {
            if (hit.collider.gameObject == gameObject)
            {
                this.dragging = true;
                firstClick = true;

                //saves current x and y position of mouse
                x = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
                y = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
            }

        }

    }

    //if the player releases left click or presses right click, dragging is stopped
    else if(Input.GetMouseButtonDown(1) || Input.GetMouseButtonUp(0))
    {
        if (this.dragging)
        {
            this.dragging = false;

            //sets the position (x and y values) of the gameobject to the nearest integer. On a small 8 x 8 grid like mine, this has the ffect of "snapping" a piece in place
            transform.position = new Vector3(snap(Camera.main.ScreenToWorldPoint(Input.mousePosition).x), snap(Camera.main.ScreenToWorldPoint(Input.mousePosition).y), 1);

        }
    }
}



//when player collides with something
private void OnCollisionEnter2D(Collision2D collision)
{

    //if the game object collides with another gameobjects, it stops the player from dragging it
    if (collision.gameObject != gameObject)
    {
        this.dragging = false;

            //sets the position (x and y values) of the gameobject to the nearest integer. On a small 8 x 8 grid like mine, this has the ffect of "snapping" a piece in place
            transform.position = new Vector3(snap(Camera.main.ScreenToWorldPoint(Input.mousePosition).x), snap(Camera.main.ScreenToWorldPoint(Input.mousePosition).y), 1);

    }

    //if the game object collides with the goal, enable the next level button and play sound effect
    if (collision.gameObject.tag == "goal" && gameObject.name == "Player")
    {

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


//basically just rounds a deciaml to the nearest int, used for snapping a piece to the grid
private float snap(float coord){

        int rounded = (int)Math.Round(coord, 0);
        Debug.Log(rounded);
        return (float)rounded;

}


IEnumerator animator()
{


    //Apparently since you cant animate 2D images without a sprite sheet in unity, I "animated" the stars/score by gradually increasing the size
    if(time < 120){
        for(float i = 0; i < (float).5; i += (float)0.01){
                star1.rectTransform.localScale = new Vector2(i, i);

                float time = (float).01;

                    yield return new WaitForSeconds(time);

        }
        if(time < 60){
            for(float i = 0; i < (float).5; i += (float)0.01){

                    star2.rectTransform.localScale = new Vector2(i, i);

                    float time = (float).01;

                        yield return new WaitForSeconds(time);

            }
            if(time < 20){
                for(float i = 0; i < (float).5; i += (float)0.01){

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



private bool detectCollision(){

    Vector2 origin = transform.position;
    Vector2 direction = transform.forward + new Vector3(90, 0);   //UP: 0, 90, 0   //DOWN 0, 90, 0 //LEFT -90, 0, 0 //RIGHT 90, 0, 0
    float dangerDistance = 0.12f;
    Debug.DrawRay(origin, direction * dangerDistance, Color.red);

    RaycastHit2D h = Physics2D.Raycast(origin, direction * dangerDistance);

    //Debug.Log(h.collider.gameObject.CompareTag("goal"));

        

        if(h.collider.gameObject.CompareTag("goal") && h.collider.gameObject != gameObject){
         Debug.Log(gameObject);
        }
    

    return true;

}







}


