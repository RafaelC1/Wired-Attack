using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoldableDescription : MonoBehaviour {

    public GameObject holdable_pre_fab = null;

    public Text object_name_text = null;
    public Text object_description_text = null;
    public Image object_image = null;

    void Start ()
    {
        DefineNameText();
        DefineDescriptionText();

        if (object_image != null)
            DefineImage();
    }

	void Update ()
    {
		
	}

    private Holdable Holdable()
    {
        return holdable_pre_fab.GetComponent<Holdable>();
    }

    private Machine Machine()
    {
        return holdable_pre_fab.GetComponent<Machine>();
    }

    private Decoration Decoration()
    {
        return holdable_pre_fab.GetComponent<Decoration>();
    }

    private Connection Connection()
    {
        return holdable_pre_fab.GetComponent<Connection>();
    }

    private void DefineNameText()
    {
        object_name_text.text = Holdable().title;
    }

    private void DefineDescriptionText()
    {
        string new_description = Holdable().description;

        if (Machine() != null)
            new_description += string.Format("| connection: {0}, storage: {1}, production: {2}",
                Machine().max_connections,
                Machine().max_bit_storage,
                Machine().process_time);

        if (Connection() != null)
            new_description += string.Format("| delay: {0}", Connection().travel_time);

        if (Decoration() != null)
            new_description += "(No use for.)";

        object_description_text.text = new_description;
    }

    private void DefineImage()
    {
        object_image.sprite = holdable_pre_fab.GetComponent<SpriteRenderer>().sprite;
    }
}
