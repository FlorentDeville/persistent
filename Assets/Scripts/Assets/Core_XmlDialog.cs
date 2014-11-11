using System.Collections.Generic;
using System.Xml.Serialization;

[XmlRoot("Dialog")]
public class Core_XmlDialog 
{
    public string Title;

    [XmlElement("Text")]
    public Core_XmlDialogText m_Text;

    public Core_XmlDialog()
    {
        m_Text = new Core_XmlDialogText();
    }
}

[XmlRoot("Test")]
public class Core_XmlDialogText
{
    [XmlElement("Part")]
    public List<Core_XmlDialogPart> Parts;

    public Core_XmlDialogText()
    {
        Parts = new List<Core_XmlDialogPart>();
    }
}

public class Core_XmlDialogPart
{
    [XmlAttribute("ActorName")]
    public string ActorName;

    [XmlText]
    public string Text;
}
