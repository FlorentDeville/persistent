using AssemblyCSharp;
using System.Collections;
using System.Xml;
using System.IO;
using UnityEngine;

public class InteractionDialog : Interaction 
{
    public TextAsset m_assetDialog;
	
	private int m_currentPartId;

    private Core_XmlDialog m_innerDialog;
	
	public override void ExecuteAwake()
	{
		enabled = false;

        System.Xml.XmlReader reader = XmlReader.Create(new StringReader(m_assetDialog.text));
        System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(Core_XmlDialog));
        m_innerDialog = (Core_XmlDialog)ser.Deserialize(reader);
	}
	
	public override void ExecuteOnEnable()
	{
		m_currentPartId = 0;
	}
	
	public override void ExecuteUpdate()
	{
		string buttonName = InputHelper.GetButtonName(InputButton.INPUT_BUTTON_A);
		if(Input.GetButtonDown(buttonName))
			++m_currentPartId;

        if (m_currentPartId >= m_innerDialog.m_Text.Parts.Count)
			enabled = false;
	}
	
	public override void ExecuteEnd(){}
	
	public override void ExecuteReset(){}
	
	public override void ExecuteOnGUI()
	{
        Core_XmlDialogPart part = m_innerDialog.m_Text.Parts[m_currentPartId];
        Core_DialogActor actor = GameObjectHelper.getWarehouse().GetActor(part.ActorName);
        RendererDialog.Render(actor.m_avatar, actor.m_name, part.Text, InputButton.INPUT_BUTTON_A);
	}
	
}
