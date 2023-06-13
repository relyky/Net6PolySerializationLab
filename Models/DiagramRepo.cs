using System.Collections.Generic;

namespace Net6XmlSerializationLab.Models;

public class DiagramRepo
{
  public List<object> NodeList { get; set; }
}

public abstract class NodeBase
{
  public string Title { get; set; }
}

public class InitialNode : NodeBase
{
  public string Class { get; set; }
}

public class EndNode : NodeBase
{
  public string Class { get; set; }
}

public class ActivityNode : NodeBase
{
  public string Class { get; set; }
  public string Type { get; set; }
}
