using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;

public class DataController : MonoBehaviour {

    public GameController gameController = null;

    private const string CUSTOM_LEVEL_FOLDER = "/CustomLevels";

    private const string DEFAULT_START_FILE_NAME = "MAP_";
    private const string DEFAULT_END_FILE_EXTENSION = ".txt";

    public const string START_OF_DESCRIPTION_SERIALIZED = "description";
    public const string START_OF_MACHINE_SERIALIZED = "machines";
    public const string START_OF_CONNECTION_SERIALIZED = "connections";
    public const string START_OF_DECORATION_SERIALIZED = "decorations";
    public const string START_OF_TIPS_SERIALIZED = "tips";
    public const string START_OF_TITLE_SERIALIZED = "title";

    public List<TextAsset> campaignMapFiles = new List<TextAsset>();

    public List<MachineSerialized> mapFileMachines = new List<MachineSerialized>();
    public List<ConnectionSerialized> mapFileConnections = new List<ConnectionSerialized>();
    public List<DecorationSerialized> mapFileDecoration = new List<DecorationSerialized>();

    public static string NEW_GAME_KEY = "create_a_new_level";

    private string FullPathOfCustomMapsDirectory = "";

    void Start()
    {
        DefineFullPathOfCustomMapsDirectory();
        CreateCustomMapsDirectory();
    }
    void Update()
    {
    }

    private void DefineFullPathOfCustomMapsDirectory()
    {
        FullPathOfCustomMapsDirectory = Application.persistentDataPath + CUSTOM_LEVEL_FOLDER;
    }

    private void CreateCustomMapsDirectory()
    {
        if (!Directory.Exists(FullPathOfCustomMapsDirectory))
        {
            Directory.CreateDirectory(FullPathOfCustomMapsDirectory);
        }
    }

    public void SaveMap(Map map)
    {
        string mapName = map.fileName;
        string fileFullPath = "";

        if (mapName == NEW_GAME_KEY)
        {
            fileFullPath = AvailableFileName();
            File.Open(fileFullPath, FileMode.OpenOrCreate, FileAccess.Write).Close();
        } else {
            fileFullPath = fullPathOfFileInCustomMapsDirectory(mapName);
            File.Open(fileFullPath, FileMode.Truncate, FileAccess.Write).Close();
        }

        using (TextWriter tw = new StreamWriter(fileFullPath))
        {
            foreach (string line in map.Serialized())
            {
                tw.WriteLine(line);
            }
        }
    }

    private List<string> LoadRawCustonwMapData(string map_name)
    {
        List<string> fileLines = new List<string>();

        string line = "";
        string fullMapPath = fullPathOfFileInCustomMapsDirectory(map_name);

        if (!File.Exists(fullMapPath)) { return new List<string>(); }

        StreamReader file = new StreamReader(fullMapPath);

        while ((line = file.ReadLine()) != null)
        {
            fileLines.Add(line);
        }

        file.Close();

        return fileLines;
    }

    private List<string> LoadRawCampaignMapData(TextAsset file)
    {
        char[] archDelim = new char[] { '\r', '\n' };
        return new List<string>(file.text.Split(archDelim, System.StringSplitOptions.RemoveEmptyEntries));
    }

    private List<string> AllCustomLevelsNames()
    {
        List<string> levelNames = new List<string>();
        DirectoryInfo dirInfo = new DirectoryInfo(FullPathOfCustomMapsDirectory);
    
        foreach(FileInfo file in dirInfo.GetFiles())
        {
            string file_name = file.Name.Split('.')[0];
            levelNames.Add(file_name);
        }
        return levelNames;
    }

    public IDictionary<string, List<string>> AllRawDataOfAllCustomMaps()
    {
        IDictionary<string, List<string>> informationOfAllMaps = new Dictionary<string, List<string>>();

        foreach(string levelName in AllCustomLevelsNames())
        {
            informationOfAllMaps.Add(levelName, LoadRawCustonwMapData(levelName));
        }

        return informationOfAllMaps;
    }

    public IDictionary<string, List<string>> AllRawDataOfAllCampaignMaps()
    {
        IDictionary<string, List<string>> informationOfAllMaps = new Dictionary<string, List<string>>();

        foreach (TextAsset map in campaignMapFiles)
        {
            informationOfAllMaps.Add(map.name, LoadRawCampaignMapData(map));
        }

        return informationOfAllMaps;
    }

    private string AvailableFileName()
    {
        int fileEndId = 0;
        string name = null;

        do
        {
            fileEndId ++;
            name = fullPathOfFileInCustomMapsDirectory(FileNameFormat(fileEndId));
        } while (File.Exists(name));

        return name;
    }

    private string fullPathOfFileInCustomMapsDirectory(string fileName)
    {
        return string.Format("{0}/{1}{2}",
                             FullPathOfCustomMapsDirectory,
                             fileName,
                             DEFAULT_END_FILE_EXTENSION);
    }

    private string FileNameFormat(int id)
    {
        return string.Format("{0}{1}", DEFAULT_START_FILE_NAME, id.ToString());
    }
}
