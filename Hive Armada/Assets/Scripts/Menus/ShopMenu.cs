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

using System.Linq;
using System.Collections;
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
        [Header("References")]
        /// <summary>
        /// Reference to Menu Audio source.
        /// </summary>
        public AudioSource source;

        /// <summary>
        /// Clips to use with source.
        /// </summary>
    	public AudioClip[] clips;

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
        /// Reference to LexiconUnlockData.
        /// </summary>
        private LexiconUnlockData lexiconUnlockData;

        /// <summary>
        /// Reference to menu title text.
        /// </summary>
        public GameObject menuTitle;

        /// <summary>
        /// Reference to description text.
        /// </summary>
        public GameObject menuDescription;

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
        /// Reference to item name text.
        /// </summary>
        public GameObject itemName;

        /// <summary>
        /// Reference to item text text.
        /// </summary>
        public GameObject itemText;

        /// <summary>
        /// Reference to item cost text.
        /// </summary>
        public GameObject itemCost;

        /// <summary>
        /// Reference to iridium amount text.
        /// </summary>
        public GameObject iridiumAmount;

        /// <summary>
        /// Reference to buy button.
        /// </summary>
        public GameObject buyButton;

        /// <summary>
        /// Reference to environment object on top of table.
        /// </summary>
        public GameObject tableDecoration;

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
        /// References to prefabs used in powerup entries. Order must match Lexicon.txt.
        /// </summary>
        public GameObject[] weaponPrefabs;

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
        /// Item texts of currently open category.
        /// </summary>
        private List<string> currTexts = new List<string>();

        /// <summary>
        /// Item costs of currently open category.
        /// </summary>
        private List<int> currCosts = new List<int>();

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
        private bool itemOpen = false;

        /// <summary>
        /// State of whether a category is currently open.
        /// </summary>
        private bool categoryOpen = false;

        /// <summary>
        /// Disable game object near Shop area.
        /// </summary>
        private void OnEnable()
        {
            tableDecoration.SetActive(false);
        }

        // Find IridiumSystem.
        void Start()
        {
            iridiumSystem = FindObjectOfType<IridiumSystem>();
            lexiconUnlockData = FindObjectOfType<LexiconUnlockData>();
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
                if (i >= currNames.Count && tooFewEntries)
                {
                    GameObject itemButtonEmpty = Instantiate(itemButtonEmptyPrefab, contentGO.transform);
                    itemButtonEmpty.SetActive(false);
                }
                else
                {
                    GameObject itemButton = Instantiate(itemButtonPrefab, contentGO.transform);
                    itemButton.GetComponent<ShopItemButton>().id = i;
                    itemButton.GetComponent<ShopItemButton>().shopMenu = this;
                    itemButton.GetComponent<UIHover>().source = source;
                    itemButton.transform.Find("Name").gameObject.GetComponent<Text>().text = currNames[i];
                }
            }
        }

        /// <summary>
        /// Attempt to purchase currently selected item.
        /// </summary>
        public void BuyItem()
        {
            if (iridiumSystem.PayIridium(currCosts[currItemId]))
            {
                iridiumSystem.UnlockItem(currCategory, currNames[currItemId]);
                lexiconUnlockData.AddWeaponUnlock(currNames[currItemId]);
                currNames.RemoveAt(currItemId);
                currTexts.RemoveAt(currItemId);
                currCosts.RemoveAt(currItemId);
                currPrefabs.RemoveAt(currItemId);
                PressBack();
                GenerateContent();
            }
        }

        /// <summary>
        /// Back button pressed. If a category or item is open, close it. Else, transition
        /// to back menu.
        /// </summary>
        public void PressBack()
        {
            source.PlayOneShot(clips[1]);

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
                tableDecoration.SetActive(true);
                FindObjectOfType<RoomTransport>().Transport(backMenuTransform, gameObject,
                    backMenuGO);
            }
        }

        /// <summary>
        /// Open item view and fill item name and text with corresponding values.
        /// </summary>
        /// <param name="itemId">Index of selected item.</param>
        public void OpenItem(int itemId)
        {
            source.PlayOneShot(clips[0]);

            menuTitle.SetActive(false);
            scrollView.SetActive(false);
            itemName.SetActive(true);
            itemText.SetActive(true);
            itemCost.SetActive(true);
            iridiumAmount.SetActive(true);
            buyButton.SetActive(true);

            if (iridiumSystem.GetIridiumAmount() < currCosts[currItemId])
            {
                buyButton.GetComponent<BoxCollider>().enabled = false;
                //buyButton.tag = "InteractableUI";
                Color tempColor = buyButton.GetComponent<Image>().color;
                tempColor.a = 0.2f;
                buyButton.GetComponent<Image>().color = tempColor;
                tempColor = buyButton.GetComponentInChildren<Text>().color;
                tempColor.a = 0.2f;
                buyButton.GetComponentInChildren<Text>().color = tempColor;
            }
            else
            {
                buyButton.GetComponent<BoxCollider>().enabled = true;
                //buyButton.tag = "Untagged";
                Color tempColor = buyButton.GetComponent<Image>().color;
                tempColor.a = 1;
                buyButton.GetComponent<Image>().color = tempColor;
                tempColor = buyButton.GetComponentInChildren<Text>().color;
                tempColor.a = 1;
                buyButton.GetComponentInChildren<Text>().color = tempColor;
            }

            foreach (GameObject categoryButton in categoryButtons)
            {
                categoryButton.SetActive(false);
            }

            itemName.GetComponent<Text>().text = currNames[itemId];
            itemText.GetComponent<Text>().text = currTexts[itemId];
            itemCost.GetComponent<Text>().text = "Cost: " + currCosts[itemId].ToString();
            iridiumAmount.GetComponent<Text>().text = "Iridium: " + iridiumSystem.GetIridiumAmount().ToString();
            currItemId = itemId;
            currPrefab = Instantiate(currPrefabs[itemId], itemPrefabPoint);
  
            itemOpen = true;
        }

        /// <summary>
        /// Close item view and return to category view.
        /// </summary>
        private void CloseItem()
        {
            menuTitle.SetActive(true);
            itemName.SetActive(false);
            itemText.SetActive(false);
            itemCost.SetActive(false);
            iridiumAmount.SetActive(false);
            buyButton.SetActive(false);
            scrollView.SetActive(true);

            foreach (GameObject categoryButton in categoryButtons)
            {
                categoryButton.SetActive(true);
            }

            Destroy(currPrefab);

            itemOpen = false;
        }

        /// <summary>
        /// Set variables tracking currently open category.
        /// </summary>
        /// <param name="category">Name of category</param>
        private void SetCurrCategory(string category)
        {
            switch (category)
            {
                case "Weapons":
                    currCategory = category;
                    currNames = iridiumSystem.GetLockedItemNames(category);
                    currTexts = iridiumSystem.GetLockedItemTexts(category);
                    currCosts = iridiumSystem.GetLockedItemCosts(category);
                    currPrefabs = weaponPrefabs.ToList();

                    List<GameObject> prefabsToRemove = new List<GameObject>();

                    //Remove prefabs from currPrefabs corresponding to unlocked items.
                    bool itemPresent = false;
                    foreach (GameObject prefab in currPrefabs)
                    {
                        foreach(string name in currNames)
                        {
                            if (prefab.name.Contains(name))
                            {
                                itemPresent = true;
                            }
                        }

                        if (!itemPresent)
                        {
                            prefabsToRemove.Add(prefab);
                        }
                    }

                    foreach (GameObject prefab in prefabsToRemove)
                    {
                        currPrefabs.Remove(prefab);
                    }

                    break;
                default:
                    Debug.Log("ERROR: Lexicon menu category could not be identified.");
                    break;
            }
        }

        /// <summary>
        /// Open the category specified by parameter string.
        /// </summary>
        /// <param name="category">Name of category to open.</param>
        public void OpenCategory(string category)
        {
            source.PlayOneShot(clips[0]);

            menuDescription.SetActive(false);
            menuTitle.GetComponent<Text>().text = category;
            scrollView.SetActive(true);
            SetCurrCategory(category);
            GenerateContent();
            scrollBar.value = 1;
            categoryOpen = true;
        }

        /// <summary>
        /// Close currently open category.
        /// </summary>
        private void CloseCategory()
        {
            menuDescription.SetActive(true);
            menuTitle.GetComponent<Text>().text = "Shop";
            scrollView.SetActive(false);
            categoryOpen = false;
        }
    }
}

