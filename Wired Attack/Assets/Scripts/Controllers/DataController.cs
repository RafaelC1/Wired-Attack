using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;

public class DataController : MonoBehaviour {

    public GameController game_controller = null;

    private const string CUSTOM_LEVEL_FOLDER = "/CustomLevels";

    private const string DEFAULT_START_FILE_NAME = "MAP_";
    private const string DEFAULT_END_FILE_EXTENSION = ".txt";

    public const string START_OF_DESCRIPTION_SERIALIZED = "description";
    public const string START_OF_MACHINE_SERIALIZED = "machines";
    public const string START_OF_CONNECTION_SERIALIZED = "connections";
    public const string START_OF_DECORATION_SERIALIZED = "decorations";
    public const string START_OF_TIPS_SERIALIZED = "tips";
    public const string START_OF_TITLE_SERIALIZED = "title";

    public List<TextAsset> campaign_map_files = new List<TextAsset>();

    public List<MachineSerialized> map_file_machines = new List<MachineSerialized>();
    public List<ConnectionSerialized> map_file_connections = new List<ConnectionSerialized>();
    public List<DecorationSerialized> map_file_decorations = new List<DecorationSerialized>();

    public static string NEW_GAME_KEY = "create_a_new_level";

    private string full_path_of_custom_maps_directory = "";

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
        full_path_of_custom_maps_directory = Application.persistentDataPath + CUSTOM_LEVEL_FOLDER;
    }

    private void CreateCustomMapsDirectory()
    {
        if (!Directory.Exists(full_path_of_custom_maps_directory))
        {
            Directory.CreateDirectory(full_path_of_custom_maps_directory);
        }
    }

    public void SaveMap(Map map)
    {
        string map_name = map.file_name;
        string file_full_path = "";

        if (map_name == NEW_GAME_KEY)
        {
            file_full_path = AvailableFileName();
            File.Open(file_full_path, FileMode.OpenOrCreate, FileAccess.Write).Close();
        } else {
            file_full_path = fullPathOfFileInCustomMapsDirectory(map_name);
            File.Open(file_full_path, FileMode.Truncate, FileAccess.Write).Close();
        }

        using (TextWriter tw = new StreamWriter(file_full_path))
        {
            foreach (string line in map.Serialized())
            {
                tw.WriteLine(line);
            }
        }
    }

    private List<string> LoadRawCustonwMapData(string map_name)
    {
        List<string> file_lines = new List<string>();

        string line = "";
        string full_map_path = fullPathOfFileInCustomMapsDirectory(map_name);

        if (!File.Exists(full_map_path)) { return new List<string>(); }

        StreamReader file = new StreamReader(full_map_path);

        while ((line = file.ReadLine()) != null)
        {
            file_lines.Add(line);
        }

        file.Close();

        return file_lines;
    }

    private List<string> LoadRawCampaignMapData(TextAsset file)
    {
        char[] archDelim = new char[] { '\r', '\n' };
        return new List<string>(file.text.Split(archDelim, System.StringSplitOptions.RemoveEmptyEntries));
    }

    private List<string> AllCustomLevelsNames()
    {
        List<string> level_names = new List<string>();
        DirectoryInfo dir_info = new DirectoryInfo(full_path_of_custom_maps_directory);
    
        foreach(FileInfo file in dir_info.GetFiles())
        {
            string file_name = file.Name.Split('.')[0];
            level_names.Add(file_name);
        }
        return level_names;
    }

    public IDictionary<string, List<string>> AllRawDataOfAllCustomMaps()
    {
        IDictionary<string, List<string>> information_of_all_maps = new Dictionary<string, List<string>>();

        foreach(string level_name in AllCustomLevelsNames())
        {
            information_of_all_maps.Add(level_name, LoadRawCustonwMapData(level_name));
        }

        return information_of_all_maps;
    }

    public IDictionary<string, List<string>> AllRawDataOfAllCampaignMaps()
    {
        IDictionary<string, List<string>> information_of_all_maps = new Dictionary<string, List<string>>();

        foreach (TextAsset map in campaign_map_files)
        {
            information_of_all_maps.Add(map.name, LoadRawCampaignMapData(map));
        }

        return information_of_all_maps;
    }

    private string AvailableFileName()
    {
        int file_end_id = 0;
        string name = null;

        do
        {
            file_end_id ++;
            name = fullPathOfFileInCustomMapsDirectory(FileNameFormat(file_end_id));
        } while (File.Exists(name));

        return name;
    }

    private string fullPathOfFileInCustomMapsDirectory(string file_name)
    {
        return string.Format("{0}/{1}{2}",
                             full_path_of_custom_maps_directory,
                             file_name,
                             DEFAULT_END_FILE_EXTENSION);
    }

    private string FileNameFormat(int id)
    {
        return string.Format("{0}{1}", DEFAULT_START_FILE_NAME, id.ToString());
    }
}
