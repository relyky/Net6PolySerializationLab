using Microsoft.Extensions.Configuration;
using Net6XmlSerializationLab.Models;
using Net6XmlSerializationLab.Services;
using System;
using System.Text.Json;

namespace Net6XmlSerializationLab;

internal class App
{
  readonly IConfiguration _config;
  readonly RandomService _randSvc;

  public App(IConfiguration config, RandomService randSvc)
  {
    _config = config;
    _randSvc = randSvc;
  }

  /// <summary>
  /// 取代原本 Program.Main() 函式的效用。
  /// </summary>
  public void Run(string[] args)
  {
    //## 準備測試資料
    DiagramRepo repo = new DiagramRepo() { NodeList = new() };
    repo.NodeList.Add(new InitialNode { Class = "Initial", Title = "填單" });
    repo.NodeList.Add(new EndNode { Class = "End", Title = "結案" });
    repo.NodeList.Add(new ActivityNode { Class = "Activity", Title = "關卡一", Type = "經辦處理" });
    repo.NodeList.Add(new ActivityNode { Class = "Activity", Title = "關卡二", Type = "經辦覆核" });

    //## JSON 多型序列化 ---------------------------------------------------------
    var options = new JsonSerializerOptions
    {
      Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping, // 中文字不編碼
      WriteIndented = true  // 換行與縮排
    };

    string json = JsonSerializer.Serialize(repo, options);
    Console.WriteLine("Serialize to JSON =>" + Environment.NewLine + json);

    //## JSON 多型反序列化 --------------------------------------------------------
    DiagramRepo derepo = JsonSerializer.Deserialize<DiagramRepo>(json, options);

    for (int i = 0; i < derepo.NodeList.Count; i++)
    {
      JsonElement element = (JsonElement)derepo.NodeList[i];
      string nodeClass = element.GetProperty("Class").GetString(); // 需先標記實體類別
      derepo.NodeList[i] = nodeClass switch
      {
        "Initial" => element.Deserialize<InitialNode>(),
        "End" => element.Deserialize<EndNode>(),
        "Activity" => element.Deserialize<ActivityNode>(),
        _ => element
      };

      Console.WriteLine($"Deserialize node => {derepo.NodeList[i]}");
    }

    Console.WriteLine("Press any key to continue.");
    //Console.ReadKey();
  }
}