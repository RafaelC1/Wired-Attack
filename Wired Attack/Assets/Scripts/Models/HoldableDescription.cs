using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoldableDescription : MonoBehaviour {

    public GameObject holdablePreFab = null;

    public Text objectNameText = null;
    public Text objectDescriptionText = null;
    public Image objectImage = null;

    void Start ()
    {
        DefineNameText();
        DefineDescriptionText();

        if (objectImage != null)
            DefineImage();
    }

	void Update ()
    {
		
	}

    private Holdable Holdable()
    {
        return holdablePreFab.GetComponent<Holdable>();
    }

    private Machine Machine()
    {
        return holdablePreFab.GetComponent<Machine>();
    }

    private Decoration Decoration()
    {
        return holdablePreFab.GetComponent<Decoration>();
    }

    private Connection Connection()
    {
        return holdablePreFab.GetComponent<Connection>();
    }

    private void DefineNameText()
    {
        objectNameText.text = Holdable().title;
    }

    private void DefineDescriptionText()
    {
        string newDescription = Holdable().description;

        if (Machine() != null)
            newDescription += string.Format("| connection: {0}, storage: {1}, production: {2}",
                Machine().maxConnections,
                Machine().maxBitStorage,
                Machine().processTime);

        if (Connection() != null)
            newDescription += string.Format("| delay: {0}", Connection().travelTime);

        if (Decoration() != null)
            newDescription += "(No use for.)";

        objectDescriptionText.text = newDescription;
    }

    private void DefineImage()
    {
        objectImage.sprite = holdablePreFab.GetComponent<SpriteRenderer>().sprite;
    }
}
