using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    // Public

    public static List<string> ressourcesName = new List<string>();

    public static int indexRessource = -1;
    public static int selectedRessource = 0;

    public static Dictionary<string, int> ressourcesNameNumber = new Dictionary<string, int>();
    public Dictionary<string, int> recipe = new Dictionary<string, int>();

    public static TMP_Text text;

    public Sprite[] tabSprites = new Sprite[20];
    public Image[] tabSelection = new Image[20];

    public Image craftZoneLeft;
    public Image craftZoneRight;
    public Image craftZoneResult;

    public Button button;

    // Private

    public Canvas CanvasObject;

    private int cpt = 1;
    private int cptRecipe = 0;
    int cptRecipeTmp;

    private bool alreadyAdded = false;
    private bool goesLeft = true;
    private bool isCorrectRecipe = false;

    private string[] left;

    // Start is called before the first frame update
    void Start()
    {
        // Hides the inventory

        CanvasObject = GetComponent<Canvas>();
        CanvasObject.enabled = !CanvasObject.enabled;

        // Adds every ressources and recipes available

        ressourcesName.Add("wood");
        ressourcesName.Add("iron");
        ressourcesName.Add("stone");
        ressourcesName.Add("grass");
        ressourcesName.Add("dirt");

        ressourcesName.Add("legendarySword");
        ressourcesName.Add("legendaryAxe");
        ressourcesName.Add("legendaryPickaxe");
        ressourcesName.Add("legendaryShovel");

        ressourcesName.Add("enemy1");
        ressourcesName.Add("enemy2");
        ressourcesName.Add("enemy3");
        ressourcesName.Add("enemy4");

        ressourcesName.Add("sword");
        ressourcesName.Add("axe");
        ressourcesName.Add("pickaxe");
        ressourcesName.Add("shovel");

        ressourcesName.Add("plank");
        ressourcesName.Add("crown");

        ressourcesName.Add("platform");

        recipe.Add("sword_enemy4_legendarySword", 5);
        recipe.Add("shovel_enemy1_legendaryShovel", 8);
        recipe.Add("pickaxe_enemy2_legendaryPickaxe", 7);
        recipe.Add("axe_enemy3_legendaryAxe", 6);

        recipe.Add("wood_dirt_plank", 17);

        recipe.Add("wood_plank_shovel", 16);
        recipe.Add("wood_iron_sword", 13);
        recipe.Add("iron_stone_pickaxe", 15);
        recipe.Add("wood_stone_axe", 14);

        recipe.Add("grass_wood_platform", 19);

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
        // Open and closes the inventory when e is pressed. If closed, empty the craft zone

        if (Input.GetKeyUp("e"))
        {
            CanvasObject.enabled = !CanvasObject.enabled;

            craftZoneLeft.sprite = null;
            craftZoneRight.sprite = null;
        }

        isCorrectRecipe = false;

        // Checks if the craft zones are occupied and a recipe is about to be made

        if (craftZoneLeft.sprite != null && craftZoneRight.sprite != null)
        {
            foreach (var recipes in recipe)
            {
                left = recipes.Key.Split("_");

                if ((left[0] == craftZoneLeft.sprite.name && left[1] == craftZoneRight.sprite.name) ||
                    (left[1] == craftZoneLeft.sprite.name && left[0] == craftZoneRight.sprite.name))
                {
                    cptRecipeTmp = 0;
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

        // Enables the craft button when a recipe is about to be made
        if(!isCorrectRecipe)
        {
            craftZoneResult.sprite = null;

            button.interactable = false;
        }
    }

    /// <summary>
    /// Change the selected item
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

        int cpt = 0;
        foreach(string key in ressourcesNameNumber.Keys)
        {
            if(cpt == selectedRessource)
            {
                GameObject.FindGameObjectWithTag("WeaponImage").GetComponent<Image>().enabled = true;
                GameObject.FindGameObjectWithTag("WeaponImage").GetComponent<Image>().sprite = tabSprites[ressourcesName.IndexOf(key)];
            }
            cpt++;
        }

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

    /// <summary>
    /// Empty the craft zone, decrements the correct ressources and adds the new ressource to the inventory
    /// </summary>
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

        SfxManager.instance.GetComponent<AudioSource>().PlayOneShot(SfxManager.instance.drop);

        isCorrectRecipe = false;
        button.interactable = false;
    }

    /// <summary>
    /// Pick up a ressources
    /// </summary>
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
                // If the ressource has already been added in the past, increments the number of the said ressource by one.
                // The max is 99.

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

                    Image slot = childPlane.GetComponentInChildren<UnityEngine.UI.Image>();

                    text = slot.GetComponentInChildren(typeof(TMP_Text)) as TMP_Text;

                    text.text = ressourcesNameNumber[ressourcesName[indexRessource]].ToString();

                    alreadyAdded = false;
                }
            }
            indexRessource = -1;
        }
    }
}
