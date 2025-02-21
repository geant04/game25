using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerUIManager : MonoBehaviour
{
    public GameObject inGameDisplay;
    public VisualTreeAsset itemLabel;

    private UIDocument uiDocument;
    private PopUpBox centerBox; // used for text like "Press E to activate button"
    private PopUpBox sideBox; // used for text like "+10 ammo picked up, fade..."

    public void Awake()
    {
        if (!inGameDisplay) return;
        uiDocument = inGameDisplay.GetComponent<UIDocument>();

        if (!uiDocument) return;
        centerBox = new PopUpBox(
            uiDocument.rootVisualElement.Q("CenterBox") as GroupBox, 
            itemLabel, 
            this);

        // sidebox not integrated
        InsertToCenterBox("I'm", 4);
        InsertToCenterBox("jaking", 5);
        InsertToCenterBox("it", 6);
    }

    public void InsertToCenterBox(MessageUI message)
    {
        centerBox.InsertPopUp(message);
    }

    public void InsertToCenterBox(string text, float lifeTime)
    {
        MessageUI message;
        message.text = text;
        message.lifeTime = lifeTime;
        centerBox.InsertPopUp(message);
    }
}

public struct MessageUI // by default will fade
{
    public string text;
    public float lifeTime;
}

public class PopUpBox
{
    public List<UIPopUp> popUps = new List<UIPopUp>();
    public VisualTreeAsset itemLabel;
    public PlayerUIManager UImanager;
    public GroupBox groupBox;

    public PopUpBox(GroupBox groupBox, VisualTreeAsset itemLabel, PlayerUIManager UImanager)
    {
        this.groupBox = groupBox;
        this.itemLabel = itemLabel;
        this.UImanager = UImanager;
    }

    public void InsertPopUp(MessageUI message)
    {
        UIPopUp popUp = new UIPopUp(message.text, message.lifeTime, itemLabel, this);
        popUps.Insert(0, popUp);
    }
    public void RemovePopUp(UIPopUp item, TemplateContainer label)
    {
        groupBox.Remove(label);
        popUps.Remove(item);
    }
}

public class UIPopUp // make this a monobehavior, then move messageContainer Instantiation here, then things shouldn't be popped off mysteriously I hope
{
    private float lifeTime;
    private string text;
    private TemplateContainer root;
    private PopUpBox parent;

    public UIPopUp(string text, float lifeTime, VisualTreeAsset labelAsset, PopUpBox parent)
    {
        this.text = text;
        this.lifeTime = lifeTime;
        this.parent = parent;

        root = labelAsset.Instantiate();
        parent.groupBox.Add(root);
        parent.UImanager.StartCoroutine(ShowText(lifeTime));
    }

    public IEnumerator ShowText(float lifeTime)
    {
        Label label = root.Q<Label>("Msg");
        label.text = text;

        while (lifeTime > 0)
        {
            lifeTime -= Time.deltaTime;
            yield return null;
        }

        Debug.Log("I am deader");
        parent.RemovePopUp(this, root);
        yield return null;
    }
}

