using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour {

    public GameObject first_menu = null;

    public List<GameObject> menus = new List<GameObject>();
    private IDictionary<string, GameObject> menus_dictionary = new Dictionary<string, GameObject>();

    void Start ()
    {
        AddAllMenusToDictionary();
        OpenMenuByObjectCloseAll(first_menu);
    }
	
	void Update () { }

    public void OpenMenuByObjectCloseAll(GameObject menu)
    {
        CloseAllMenus();
        OpenMenuByObject(menu);
    }

    public void OpenMenuByNameCloseAll(string menu_name)
    {
        CloseAllMenus();
        OpenMenuByName(menu_name);
    }

    public void OpenMenuByObject(GameObject menu)
    {
        if (menus_dictionary.Values.Contains(menu))
        {
            menu.SetActive(true);
        }
    }

    public void CloseMenuByObject(GameObject menu)
    {
        if (menus_dictionary.Values.Contains(menu))
        {
            menu.SetActive(false);
        }
    }

    public void OpenMenuByName(string menu_name)
    {
        menus_dictionary[menu_name].SetActive(true);
    }

    public void CloseMenuByName(string menu_name)
    {
        menus_dictionary[menu_name].SetActive(false);
    }

    public void CloseAllMenus()
    {
        foreach (KeyValuePair<string, GameObject> menu in menus_dictionary)
        {
            menu.Value.SetActive(false);
        }
    }

    private void OnEnable()
    {
        CloseAllMenus();
        OpenMenuByObject(first_menu);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void AddAllMenusToDictionary()
    {
        foreach (GameObject menu in menus)
        {
            menus_dictionary.Add(menu.transform.name, menu);
        }
    }

}
