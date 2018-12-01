using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour {

    public GameObject menuToFirstOpen = null;

    public List<GameObject> menus = new List<GameObject>();
    private IDictionary<string, GameObject> menusDictionary = new Dictionary<string, GameObject>();

    void Start ()
    {
        AddAllMenusToDictionary();
        OpenMenuByObjectCloseAll(menuToFirstOpen);
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
        if (menusDictionary.Values.Contains(menu))
        {
            menu.SetActive(true);
        }
    }

    public void CloseMenuByObject(GameObject menu)
    {
        if (menusDictionary.Values.Contains(menu))
        {
            menu.SetActive(false);
        }
    }

    public void OpenMenuByName(string menuName)
    {
        menusDictionary[menuName].SetActive(true);
    }

    public void CloseMenuByName(string menuName)
    {
        menusDictionary[menuName].SetActive(false);
    }

    public void CloseAllMenus()
    {
        foreach (KeyValuePair<string, GameObject> menu in menusDictionary)
        {
            menu.Value.SetActive(false);
        }
    }

    private void OnEnable()
    {
        CloseAllMenus();
        OpenMenuByObject(menuToFirstOpen);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void AddAllMenusToDictionary()
    {
        foreach (GameObject menu in menus)
        {
            menusDictionary.Add(menu.transform.name, menu);
        }
    }

}
