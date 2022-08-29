using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Xml;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static System.Net.Mime.MediaTypeNames;

public class Inventory : MonoBehaviour
{
    // Public

    public UnityEngine.UI.Image[] tabSelection = new UnityEngine.UI.Image[20];

    public static List<string> RessourcesName = new List<string>();

    public Sprite[] tabSprites = new Sprite[20];

    public static int indexRessource = -1;
    public static int selectedRessource = 0;

    public static Dictionary<string, int> RessourcesNameNumber = new Dictionary<string, int>();

    public static TMP_Text text;

    // Private
    public Canvas CanvasObject;

    private int cpt = 1;

    private bool alreadyAdded = false;

    // Start is called before the first frame update
    void Start()
    {
        CanvasObject = GetComponent<Canvas>();
        CanvasObject.enabled = !CanvasObject.enabled;

        // Add every ressources available

        RessourcesName.Add("wood");
        RessourcesName.Add("iron");
        RessourcesName.Add("stone");
        RessourcesName.Add("grass");
        RessourcesName.Add("dirt");
        RessourcesName.Add("legendarySword");
        RessourcesName.Add("legendaryAxe");
        RessourcesName.Add("legendaryPickaxe");
        RessourcesName.Add("legendaryShowel");
        RessourcesName.Add("enemy1");
        RessourcesName.Add("enemy2");
        RessourcesName.Add("enemy3");
        RessourcesName.Add("enemy4");
        RessourcesName.Add("sword");
        RessourcesName.Add("axe");
        RessourcesName.Add("pickaxe");
        RessourcesName.Add("showel");
        RessourcesName.Add("plank");
        RessourcesName.Add("crown");

        // Hide every slots

        foreach (UnityEngine.UI.Image selection in tabSelection)
        {
            selection.enabled = !selection.enabled;

            GameObject childPlane;

            childPlane = selection.transform.Find("Plane").gameObject;
            childPlane.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Open and closes the inventory when e is pressed.
        if (Input.GetKeyUp("e"))
        {
            CanvasObject.enabled = !CanvasObject.enabled;
        }        
    }

    /// <summary>
    /// CHange the selected item
    /// </summary>
    /// <param name="number"></param>
    public void ClickedSlot(int number)
    {
        selectedRessource = number;

        foreach (UnityEngine.UI.Image selection in tabSelection)
        {
            selection.enabled = false;
        }

        tabSelection[number].enabled = true;
    }

    public void CollectResource()
    {
        // Check if a ressource is picked up

        if (indexRessource >= 0)
        {
            // Check if the ressource has already been added to the inventory before

            foreach (string ressource in RessourcesNameNumber.Keys)
            {
                if (ressource == RessourcesName[indexRessource])
                {
                    alreadyAdded = true;
                }
            }

            if (!alreadyAdded)
            {
                // If the ressource has never been added, fill a new slot with the new ressource

                tabSelection[0].enabled = true;

                GameObject childPlane;
                childPlane = tabSelection[cpt - 1].transform.Find("Plane").gameObject;
                childPlane.SetActive(true);

                UnityEngine.UI.Image slot = childPlane.GetComponentInChildren<UnityEngine.UI.Image>();

                slot.sprite = tabSprites[indexRessource];

                RessourcesNameNumber.Add(RessourcesName[indexRessource], 1);

                text = slot.GetComponentInChildren(typeof(TMP_Text)) as TMP_Text;
                text.enabled = true;
                text.text = RessourcesNameNumber[RessourcesName[indexRessource]].ToString();

                cpt++;

                alreadyAdded = false;
            }
            else
            {
                // If the ressource has already been added in the past, increments the number of the said ressource by one. The max is 99.

                if (!(RessourcesNameNumber[RessourcesName[indexRessource]] >= 99))
                {
                    RessourcesNameNumber[RessourcesName[indexRessource]]++;

                    int cpt2 = 0;
                    int index = 0;

                    foreach (string ressource in RessourcesNameNumber.Keys)
                    {

                        if (ressource == RessourcesName[indexRessource])
                        {
                            index = cpt2;
                        }

                        cpt2++;
                    }

                    GameObject childPlane;
                    childPlane = tabSelection[index].transform.Find("Plane").gameObject;

                    UnityEngine.UI.Image slot = childPlane.GetComponentInChildren<UnityEngine.UI.Image>();

                    text = slot.GetComponentInChildren(typeof(TMP_Text)) as TMP_Text;

                    text.text = RessourcesNameNumber[RessourcesName[indexRessource]].ToString();

                    alreadyAdded = false;
                }
            }

            indexRessource = -1;
        }
    }
}
