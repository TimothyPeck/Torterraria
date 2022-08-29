using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Xml;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    // Public

    public Image[] tabSelection = new Image[20];

    public static List<string> ressourcesName = new List<string>();

    public Sprite[] tabSprites = new Sprite[20];

    public static int indexRessource = -1;
    public static int selectedRessource = 0;

    public static Dictionary<string, int> ressourcesNameNumber = new Dictionary<string, int>();
    public static Dictionary<string, int> recipe = new Dictionary<string, int>();

    public static TMP_Text text;

    public Image craftZoneLeft;
    public Image craftZoneRight;
    public Image craftZoneResult;

    public Button button;

    // Private
    public Canvas CanvasObject;

    private int cpt = 1;
    private int cptRecipe = 0;

    private bool alreadyAdded = false;
    private bool goesLeft = true;
    private bool isCorrectRecipe = false;

    // Start is called before the first frame update
    void Start()
    {
        CanvasObject = GetComponent<Canvas>();
        CanvasObject.enabled = !CanvasObject.enabled;

        // Add every ressources available

        ressourcesName.Add("wood");
        ressourcesName.Add("iron");
        ressourcesName.Add("stone");
        ressourcesName.Add("grass");
        ressourcesName.Add("dirt");

        ressourcesName.Add("legendarySword");
        ressourcesName.Add("legendaryAxe");
        ressourcesName.Add("legendaryPickaxe");
        ressourcesName.Add("legendaryShowel");

        ressourcesName.Add("enemy1");
        ressourcesName.Add("enemy2");
        ressourcesName.Add("enemy3");
        ressourcesName.Add("enemy4");

        ressourcesName.Add("sword");
        ressourcesName.Add("axe");
        ressourcesName.Add("pickaxe");
        ressourcesName.Add("showel");

        ressourcesName.Add("plank");
        ressourcesName.Add("crown");

        recipe.Add("sword_enemy4_legendarySword", 5);
        recipe.Add("showel_enemy1_legendaryShowel", 8);
        recipe.Add("pickaxe_enemy2_legendaryPickaxe", 7);
        recipe.Add("axe_enemy3_legendaryAxe", 6);

        recipe.Add("wood_dirt_plank", 17);

        recipe.Add("wood_plank_showel", 16);
        recipe.Add("wood_iron_sword", 13);
        recipe.Add("iron_stone_pickaxe", 15);
        recipe.Add("wood_stone_axe", 14);

        // Hide every slots

        foreach (Image selection in tabSelection)
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

            craftZoneLeft.sprite = null;
            craftZoneRight.sprite = null;
        }

        isCorrectRecipe = false;

        if (craftZoneLeft.sprite != null && craftZoneRight.sprite != null)
        {
            foreach (var recipes in recipe)
            {
                string[] left = recipes.Key.Split("_");

                if ((left[0] == craftZoneLeft.sprite.name && left[1] == craftZoneRight.sprite.name) ||
                    (left[1] == craftZoneLeft.sprite.name && left[0] == craftZoneRight.sprite.name))
                {
                    int cptRecipeTmp = 0;
                    cptRecipe = 0;
                    foreach (Sprite sprites in tabSprites)
                    {
                        if (sprites.name == left[2])
                        {
                            cptRecipe = cptRecipeTmp;
                            craftZoneResult.sprite = sprites;
                            isCorrectRecipe = true;
                            button.interactable = true;
                        }
                        cptRecipeTmp++;
                    }
                }
            }
        }

        if(!isCorrectRecipe)
        {
            craftZoneResult.sprite = null;
            button.interactable = false;
        }
    }

    /// <summary>
    /// CHange the selected item
    /// </summary>
    /// <param name="number"></param>
    public void ClickedSlot(int number)
    {
        selectedRessource = number;

        foreach (Image selection in tabSelection)
        {
            selection.enabled = false;
        }

        tabSelection[number].enabled = true;
    }

    /// <summary>
    /// Put the wanted item in the correct craft slot
    /// </summary>
    public void moveToCraft(GameObject slotToCraft)
    {
        text = slotToCraft.GetComponentInChildren(typeof(TMP_Text)) as TMP_Text;
        int number = Convert.ToInt32(text.text);

        if (goesLeft)
        {
            if (number >= 1)
            {
                craftZoneLeft.sprite = slotToCraft.GetComponent<Image>().sprite;
                goesLeft = false;
            }
        }
        else
        {
            if (number >= 1)
            {
                craftZoneRight.sprite = slotToCraft.GetComponent<Image>().sprite;
                goesLeft = true;

            }
        }
    }

    public void CraftInitiated()
    {
        foreach (Image selection in tabSelection)
        {
            GameObject childPlane;
            childPlane = selection.transform.Find("Plane").gameObject;

            Image slot = childPlane.GetComponentInChildren<Image>();

            string nameLeft = null;
            string nameRight = null;

            if (craftZoneLeft.sprite != null)
            {
                nameLeft = craftZoneLeft.sprite.name;
            }

            if (craftZoneRight.sprite != null)
            {
                nameRight = craftZoneRight.sprite.name;
            }

            if (nameLeft != null && slot.sprite.name == nameLeft)
            {
                text = slot.GetComponentInChildren(typeof(TMP_Text)) as TMP_Text;
                text.text = (Convert.ToInt32(text.text) - 1).ToString();
                ressourcesNameNumber[nameLeft] = Convert.ToInt32(text.text);
                craftZoneLeft.sprite = null;
            }

            if (nameRight != null && slot.sprite.name == nameRight)
            {
                text = slot.GetComponentInChildren(typeof(TMP_Text)) as TMP_Text;
                text.text = (Convert.ToInt32(text.text) - 1).ToString();
                ressourcesNameNumber[nameRight] = Convert.ToInt32(text.text);
                craftZoneRight.sprite = null;

                indexRessource = cptRecipe;
                CollectResource();

                craftZoneResult.sprite = null;
            }
        }

        isCorrectRecipe = false;
        button.interactable = false;
    }
    public void CollectResource()
    {
        // Check if a ressource is picked up

        if (indexRessource >= 0)
        {
            // Check if the ressource has already been added to the inventory before

            foreach (string ressource in ressourcesNameNumber.Keys)
            {
                if (ressource == ressourcesName[indexRessource])
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

                ressourcesNameNumber.Add(ressourcesName[indexRessource], 1);

                text = slot.GetComponentInChildren(typeof(TMP_Text)) as TMP_Text;
                text.enabled = true;
                text.text = ressourcesNameNumber[ressourcesName[indexRessource]].ToString();

                cpt++;

                alreadyAdded = false;
            }
            else
            {
                // If the ressource has already been added in the past, increments the number of the said ressource by one. The max is 99.

                if (!(ressourcesNameNumber[ressourcesName[indexRessource]] >= 99))
                {
                    ressourcesNameNumber[ressourcesName[indexRessource]]++;

                    int cpt2 = 0;
                    int index = 0;

                    foreach (string ressource in ressourcesNameNumber.Keys)
                    {

                        if (ressource == ressourcesName[indexRessource])
                        {
                            index = cpt2;
                        }

                        cpt2++;
                    }

                    GameObject childPlane;
                    childPlane = tabSelection[index].transform.Find("Plane").gameObject;

                    UnityEngine.UI.Image slot = childPlane.GetComponentInChildren<UnityEngine.UI.Image>();

                    text = slot.GetComponentInChildren(typeof(TMP_Text)) as TMP_Text;

                    text.text = ressourcesNameNumber[ressourcesName[indexRessource]].ToString();

                    alreadyAdded = false;
                }
            }

            indexRessource = -1;
        }
    }
}
