using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Deck : MonoBehaviour {



    public List<string> cards2 =  new List<string>();
    private static System.Random rand = new System.Random();


   

    // Use this for initialization
    void Start ()
    {
        cards2.Add("Revolver");
        cards2.Add("Revolver");
        cards2.Add("Revolver");
        cards2.Add("At Last...");
        cards2.Add("At Last...");
        cards2.Add("At Last...");
        cards2.Add("Faith");
        cards2.Add("Faith");
        cards2.Add("Faith");
        cards2.Add("Get Back You Devils");
        cards2.Add("Get Back You Devils");
        cards2.Add("Get Back You Devils");
        cards2.Add("Baseball Bat");
        cards2.Add("Baseball Bat");
        cards2.Add("First Aid Kit");
        cards2.Add("First Aid Kit");
        cards2.Add("Keys");
        cards2.Add("Keys");
        cards2.Add("Ammo");
        cards2.Add("Ammo");
        cards2.Add("Pump Shotgun");
        cards2.Add("Pump Shotgun");
        cards2.Add("Recovery");
        cards2.Add("Recovery");
        cards2.Add("Just a Scratch");
        cards2.Add("Just a Scratch");
        cards2.Add("Meat Cleaver");
        cards2.Add("Signal Flare");
        cards2.Add("Pitchfork");
        cards2.Add("Fireax");
        cards2.Add("Crowbar");
        cards2.Add("Chainsaw");
        cards2.Add("Torch");
        cards2.Add("Welding Torch");
        cards2.Add("Deputy Taylor");
        cards2.Add("Doc Brody, Country Physician");
        cards2.Add("Farmer Sty");
        cards2.Add("Mr. Hyde, The Shop Teacher");
        cards2.Add("Jeb, The Grease Monkey");
        cards2.Add("Principal Gomez");



        Shuffle2<string>(cards2);
    }

    public static void Shuffle2<T>(List<T> list)
    {
        for (int i = 0; i < 40; i++)
        {
            int idx = rand.Next(i, 40);

            //swap elements
            T tmp = list[i];
            list[i] = list[idx];
            list[idx] = tmp;
        }
    }










    // Update is called once per frame
    void Update () {
	
	}
}
