using AssemblyCSharp;
using CinemaDirector;
using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

public class Sequence_DialogPlayer : MonoBehaviour 
{
    public GameObject m_Sequence;

    public TextAsset m_AssetDialog;

    private int m_PartId;

    private Core_XmlDialog m_innerDialog;

    private bool m_ShowDialogPart;

    void OnEnable()
    {
        System.Xml.XmlReader reader = XmlReader.Create(new StringReader(m_AssetDialog.text));
        System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(Core_XmlDialog));
        m_innerDialog = (Core_XmlDialog)ser.Deserialize(reader);

        m_PartId = -1;
        m_ShowDialogPart = false;
    }

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (!m_ShowDialogPart)
            return;

        string buttonName = InputHelper.GetButtonName(InputButton.INPUT_BUTTON_A);
        if (Input.GetButtonDown(buttonName))
        {
			m_ShowDialogPart = false;
            Cutscene cutscene = m_Sequence.GetComponent<Cutscene>();
            cutscene.Play();
        }
	}

    public void OnGUI()
    {
        if (!m_ShowDialogPart)
            return;

        Core_XmlDialogPart part = m_innerDialog.m_Text.Parts[m_PartId];
        Core_DialogActor actor = GameObjectHelper.getWarehouse().GetActor(part.ActorName);
        RendererDialog.Render(actor.m_avatar, actor.m_name, part.Text, InputButton.INPUT_BUTTON_A);
    }

    public void ShowDialog()
    {
        ++m_PartId;
        m_ShowDialogPart = true;
        Cutscene cutscene = m_Sequence.GetComponent<Cutscene>();
        cutscene.Pause();
    }
}
