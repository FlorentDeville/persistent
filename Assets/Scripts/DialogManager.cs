using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Xml;

public class DialogManager 
{
    private static DialogManager m_instance = new DialogManager();

    private Dictionary<string, Core_XmlDialog> m_dialogs;

    private DialogManager()
    {
        m_dialogs = new Dictionary<string, Core_XmlDialog>();
    }

    public static DialogManager GetInstance()
    {
        return m_instance;
    }

    public Core_XmlDialog GetDialog(TextAsset _asset)
    {
        if (m_dialogs.ContainsKey(_asset.name))
            return m_dialogs[_asset.name];
        else
        {
            System.Xml.XmlReader reader = XmlReader.Create(new StringReader(_asset.text));
            System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(Core_XmlDialog));
            Core_XmlDialog newDialog = (Core_XmlDialog)ser.Deserialize(reader);
            m_dialogs.Add(_asset.name, newDialog);
            return newDialog;
        }
    }


	
}
