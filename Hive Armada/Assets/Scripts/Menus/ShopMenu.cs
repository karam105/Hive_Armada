﻿//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-440-1
// Group Project
//
// ShopMenu controls interactions with the Shop Menu. The player can spend
// Iridium to permanently unlock different unlockables, such as new weapons.
//
//=============================================================================

using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hive.Armada.Game;

namespace Hive.Armada.Menus
{
    /// <summary>
    /// Controls interactions with Shop Menu.
    /// </summary>
    public class ShopMenu : MonoBehaviour
    {
        private ReferenceManager reference;

        /// <summary>
        /// Reference to Menu Audio source.
        /// </summary>
        /// [Header("References")]
        public AudioSource source;

        public AudioSource zenaSource;

        /// <summary>
        /// Reference to Menu Transition Manager.
        /// </summary>
        public MenuTransitionManager transitionManager;

        /// <summary>
        /// Reference to menu to go to when back is pressed.
        /// </summary>
        public GameObject backMenuGO;

        /// <summary>
        /// Reference to player transform for Options Menu.
        /// </summary>
        public Transform backMenuTransform;

        /// <summary>
        /// Reference to IridiumSystem.
        /// </summary>
        private IridiumSystem iridiumSystem;

        /// <summary>
        /// Reference to BestiaryUnlockData.
        /// </summary>
        private BestiaryUnlockData BestiaryUnlockData;

        /// <summary>
        /// Reference to menu title text.
        /// </summary>
        public GameObject menuTitle;

        /// <summary>
        /// Reference to category buttons/tabs.
        /// </summary>
        public GameObject[] categoryButtons;

        /// <summary>
        /// Reference to menu ScrollView game object.
        /// </summary>
        public GameObject scrollView;

        /// <summary>
        /// Reference to scrollview vertical scrollbar.
        /// </summary>
        public Scrollbar scrollBar;

        /// <summary>
        /// Reference to vertical slider.
        /// </summary>
        public Slider verticalSlider;

        /// <summary>
        /// Reference to UI Cover GameObjects.
        /// </summary>
        public GameObject[] uiCovers;

        public GameObject previewLighting;

        [Header("Sections")]
        public GameObject itemSection;

        public GameObject iridiumSection;

        public GameObject purchaseSection;

        /// <summary>
        /// Reference to item name text.
        /// </summary>
        [Header("Text")]
        public Text itemName;

        /// <summary>
        /// Reference to item text text.
        /// </summary>
        public Text itemDescription;

        /// <summary>
        /// Reference to item cost text.
        /// </summary>
        public Text itemCost;

        /// <summary>
        /// Reference to iridium amount text.
        /// </summary>
        public Text iridiumAmount;

        public GameObject purchased;

        /// <summary>
        /// Reference to buy button.
        /// </summary>
        public GameObject buyButton;

        /// <summary>
        /// Reference to armada preview game object on top of table.
        /// </summary>
        public GameObject armadaPreviewGO;

        [Header("Content")]

        /// <summary>
        /// Prefab for item button.
        /// </summary>
        public GameObject itemButtonPrefab;

        /// <summary>
        /// Prefab for empty item button.
        /// </summary>
        public GameObject itemButtonEmptyPrefab;

        /// <summary>
        /// Reference to transform to use for preview prefab instantiation.
        /// </summary>
        public Transform itemPrefabPoint;

        /// <summary>
        /// Number of buttons that are completely visible in view at a time.
        /// </summary>
        public int numFittableButtons = 3;

        /// <summary>
        /// Reference to scroll view content GO.
        /// </summary>
        public GameObject contentGO;

        /// <summary>
        /// References to prefabs used in powerup entries. Order must match Iridium.txt.
        /// </summary>
        public List<GameObject> weaponPrefabs;

        /// <summary>
        /// Stat descriptions for each weapon. Order must match Iridium.txt.
        /// </summary>
        [TextArea]
        public List<string> weaponStats;

        /// <summary>
        /// Id of currently open item.
        /// </summary>
        private int currItemId;

        /// <summary>
        /// name of currently open category.
        /// </summary>
        private string currCategory;

        /// <summary>
        /// Item names of currently open category.
        /// </summary>
        private List<string> currNames = new List<string>();

        /// <summary>
        /// Item display names of currently open category.
        /// </summary>
        private List<string> currDisplayNames = new List<string>();

        /// <summary>
        /// Item texts of currently open category.
        /// </summary>
        private List<string> currTexts = new List<string>();

        /// <summary>
        /// Item costs of currently open category.
        /// </summary>
        private List<int> currCosts = new List<int>();

        /// <summary>
        /// Items not bought of currently open category.
        /// </summary>
        private List<bool> currNotBought = new List<bool>();

        /// <summary>
        /// Reference to currently displayed item prefab.
        /// </summary>
        private GameObject currPrefab;

        /// <summary>
        /// Item prefabs of currently open category.
        /// </summary>
        private List<GameObject> currPrefabs = new List<GameObject>();

        /// <summary>
        /// State of whether an item is currently open.
        /// </summary>
        private bool itemOpen;

        /// <summary>
        /// State of whether a category is currently open.
        /// </summary>
        private bool categoryOpen;

        private void Awake()
        {
            reference = FindObjectOfType<ReferenceManager>();
        }

        /// <summary>
        /// Disable game object near Shop area.
        /// </summary>
        private void OnEnable()
        {
            armadaPreviewGO.SetActive(false);
        }

        // Find IridiumSystem.
        private void Start()
        {
            iridiumSystem = FindObjectOfType<IridiumSystem>();
            BestiaryUnlockData = FindObjectOfType<BestiaryUnlockData>();

            //int shopIndex = Random.Range(3, clips.Length);
            ////ASSUMES FIRST VISIT TO THE SHOP
            //if (iridiumSystem.GetIridiumAmount() < 0)
            //{
            //    zenaSource.PlayOneShot(clips[2]);
            //}

            ////ASSUMES SUBSEQUENT VISIT TO SHOP
            ////NEED TO ADD RANDOM RANGE CHOOSER
            //else if (iridiumSystem.GetIridiumAmount() > 0)
            //{
            //    zenaSource.PlayOneShot(clips[shopIndex]);
            //}
        }

        /// <summary>
        /// Create item buttons as children of Content and destroy previous buttons.
        /// </summary>
        private void GenerateContent()
        {
            for (int i = 0; i < contentGO.transform.childCount; i++)
            {
                Destroy(contentGO.transform.GetChild(i).gameObject);
            }

            List<int> removalIndices = new List<int>();
            for (int i = 0; i < currNames.Count; i++)
            {
                if (currCosts[i] == 0)
                {
                    removalIndices.Add(i);
                }
            }

            foreach (int index in removalIndices)
            {
                Debug.Log("Removed: " + currNames[index]);
                currNames.RemoveAt(index);
                currDisplayNames.RemoveAt(index);
                currTexts.RemoveAt(index);
                currCosts.RemoveAt(index);
                currNotBought.RemoveAt(index);
                weaponPrefabs.RemoveAt(index);
                weaponStats.RemoveAt(index);
            }

            int items;
            bool tooFewEntries;
            if (currNames.Count <= numFittableButtons)
            {
                items = numFittableButtons + 1;
                tooFewEntries = true;
                scrollBar.gameObject.GetComponent<BoxCollider>().enabled = false;
                verticalSlider.gameObject.SetActive(false);
            }
            else
            {
                items = currNames.Capacity;
                tooFewEntries = false;
                scrollBar.gameObject.GetComponent<BoxCollider>().enabled = true;
                verticalSlider.gameObject.SetActive(true);
            }

            for (int i = 0; i < items; ++i)
            {
                if (i >= currDisplayNames.Count && tooFewEntries)
                {
                    GameObject itemButtonEmpty =
                        Instantiate(itemButtonEmptyPrefab, contentGO.transform);
                    itemButtonEmpty.SetActive(false);
                }
                else
                {
                    GameObject itemButton = Instantiate(itemButtonPrefab, contentGO.transform);
                    itemButton.GetComponent<ShopItemButton>().id = i;
                    itemButton.GetComponent<ShopItemButton>().shopMenu = this;
                    itemButton.GetComponent<UIHover>().source = source;
                    itemButton.transform.Find("Name").gameObject.GetComponent<Text>().text =
                        currDisplayNames[i];
                }
            }
        }

        /// <summary>
        /// Attempt to purchase currently selected item.
        /// </summary>
        public void BuyItem()
        {
            /* NEED TO ADD SOUNDS INTO THE ARRAY THEN UNCOMMENT
             * SOUNDS: (ACCORDING TO DIALOGUE DOC)
             *      TAKE THIS
             *      WONDERFUL
             *      A NICE REWARD
             *      YOU'LL LIKE THAT ONE
             *      
             * Don't forget to actually do work...
             */
            //int purchaseSound = Random.Range(2, clips.Length);
            if (iridiumSystem.PayIridium(currCosts[currItemId]))
            {
                //zenaSource.PlayOneShot(clips[purchaseSound]);
                source.PlayOneShot(reference.menuSounds.shopPurchaseSound);
                iridiumSystem.UnlockItem(currCategory, currNames[currItemId]);
                currNotBought[currItemId] = false;
                purchaseSection.SetActive(false);
                purchased.SetActive(true);
                iridiumAmount.text = iridiumSystem.GetIridiumAmount().ToString();
                //PressBack();
            }
        }

        /// <summary>
        /// Back button pressed. If a category or item is open, close it. Else, transition
        /// to back menu.
        /// </summary>
        public void PressBack()
        {
            source.PlayOneShot(reference.menuSounds.menuButtonSelectSound);

            if (itemOpen)
            {
                CloseItem();
            }
            else if (categoryOpen)
            {
                CloseCategory();
            }
            else
            {
                armadaPreviewGO.SetActive(true);
                FindObjectOfType<RoomTransport>().Transport(backMenuTransform, gameObject,
                                                            backMenuGO);
            }

            previewLighting.SetActive(false);
        }

        /// <summary>
        /// Open item view and fill item name and text with corresponding values.
        /// </summary>
        /// <param name="itemId"> Index of selected item. </param>
        public void OpenItem(int itemId)
        {
            source.PlayOneShot(reference.menuSounds.menuButtonSelectSound);

            menuTitle.SetActive(false);
            scrollView.SetActive(false);
            itemSection.SetActive(true);
            iridiumSection.SetActive(true);

            if (currNotBought[itemId])
            {
                purchaseSection.SetActive(true);
                purchased.SetActive(false);

                Color tempColor = buyButton.GetComponent<Image>().color;
                ;

                if (iridiumSystem.GetIridiumAmount() < currCosts[itemId])
                {
                    buyButton.GetComponent<BoxCollider>().enabled = false;
                    tempColor.a = 0.2f;
                    buyButton.GetComponent<Image>().color = tempColor;
                    tempColor = buyButton.GetComponentInChildren<Text>().color;
                    tempColor.a = 0.2f;
                    buyButton.GetComponentInChildren<Text>().color = tempColor;
                }
                else
                {
                    buyButton.GetComponent<BoxCollider>().enabled = true;
                    tempColor.a = 1;
                    buyButton.GetComponent<Image>().color = tempColor;
                    tempColor = buyButton.GetComponentInChildren<Text>().color;
                    tempColor.a = 1;
                    buyButton.GetComponentInChildren<Text>().color = tempColor;
                }
            }
            else
            {
                purchaseSection.SetActive(false);
                purchased.SetActive(true);
            }

            foreach (GameObject uiCover in uiCovers)
            {
                uiCover.SetActive(false);
            }

            foreach (GameObject categoryButton in categoryButtons)
            {
                categoryButton.SetActive(false);
            }

            itemName.text = currDisplayNames[itemId];

            if (currCategory == "Weapons")
            {
                itemDescription.text = currTexts[itemId] + "\n"
                                       + weaponStats[itemId];
            }
            else
            {
                itemDescription.text = currTexts[itemId];
            }

            itemCost.text = currCosts[itemId].ToString();
            iridiumAmount.text = iridiumSystem.GetIridiumAmount().ToString();
            currItemId = itemId;
            previewLighting.SetActive(true);
            currPrefab = Instantiate(currPrefabs[itemId], itemPrefabPoint);

            itemOpen = true;
        }

        /// <summary>
        /// Close item view and return to category view.
        /// </summary>
        private void CloseItem()
        {
            menuTitle.SetActive(true);

            itemSection.SetActive(false);
            purchaseSection.SetActive(false);
            iridiumSection.SetActive(false);
            purchased.SetActive(false);
            scrollView.SetActive(true);

            foreach (GameObject uiCover in uiCovers)
            {
                uiCover.SetActive(true);
            }

            foreach (GameObject categoryButton in categoryButtons)
            {
                categoryButton.SetActive(true);
            }

            Destroy(currPrefab);
            previewLighting.SetActive(false);

            itemOpen = false;
        }

        /// <summary>
        /// Set variables tracking currently open category.
        /// </summary>
        /// <param name="category"> Name of category </param>
        private void SetCurrCategory(string category)
        {
            switch (category)
            {
                case "Weapons":
                    currCategory = category;
                    currNames = iridiumSystem.GetItemNames(category);
                    currDisplayNames = iridiumSystem.GetItemDisplayNames(category);
                    currTexts = iridiumSystem.GetItemTexts(category);
                    currCosts = iridiumSystem.GetItemCosts(category);
                    currNotBought = iridiumSystem.GetItemsLocked(category);
                    currPrefabs = weaponPrefabs;
                    break;
                default:
                    Debug.Log("ERROR: Bestiary menu category could not be identified.");
                    break;
            }
        }

        /// <summary>
        /// Open the category specified by parameter string.
        /// </summary>
        /// <param name="category"> Name of category to open. </param>
        public void OpenCategory(string category)
        {
            source.PlayOneShot(reference.menuSounds.menuButtonSelectSound);

            menuTitle.GetComponent<Text>().text = category;
            scrollView.SetActive(true);
            SetCurrCategory(category);
            GenerateContent();
            scrollBar.value = 1;

            categoryButtons[0].GetComponent<UIHover>().Select();

            categoryOpen = true;
        }

        /// <summary>
        /// Close currently open category.
        /// </summary>
        private void CloseCategory()
        {
            menuTitle.GetComponent<Text>().text = "Shop";
            scrollView.SetActive(false);

            categoryButtons[0].GetComponent<UIHover>().EndSelect();

            categoryOpen = false;
        }
    }
}