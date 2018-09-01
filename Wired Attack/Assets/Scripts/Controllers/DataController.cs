using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataController : MonoBehaviour {

    private const string DEFAULT_START_FILE_NAME = "MAP_";
    private const string DEFAULT_END_FILE_EXTENSION = ".txt";

    private const string LEVEL_FOLDER = "/Maps";
    private const string CUSTOM_LEVEL_FOLDER = "/CustomLevels";
    private const string STREAMING_ASSETS_FOLDER_NAME = "/StreamingAssets";

    public const string START_OF_MACHINE_SERIALIZED = "machines";
    public const string START_OF_CONNECTION_SERIALIZED = "wires";

    public List<TextAsset> campaign_level_files = new List<TextAsset>();
    private IDictionary<string, StreamReader> campaign_level = new Dictionary<string, StreamReader>();
    private IDictionary<string, StreamWriter> custom_level_files = new Dictionary<string, StreamWriter>();

    public List<MachineSerialized> map_file_machines = new List<MachineSerialized>();
    public List<WireSerialized> map_file_connections = new List<WireSerialized>();

    private string persistent_path = "";
    private string streaming_assets_path = "";
    private string custom_level_path = "";

    void Start()
    {
        persistent_path = Application.persistentDataPath + CUSTOM_LEVEL_FOLDER;
        streaming_assets_path = Application.dataPath + STREAMING_ASSETS_FOLDER_NAME + LEVEL_FOLDER;

        LoadAllCustomLevels();
        CreateCustomLevelDirectory();
    }
    void Update() { }

    private void CreateCustomLevelDirectory()
    {
        if (!Directory.Exists(persistent_path))
        {
            Directory.CreateDirectory(custom_level_path);
        }
    }

    private void LoadAllCustomLevels()
    {
        custom_level_files.Clear();
        foreach (string file_name in Directory.GetFiles(CustomMapPath()))
        {
            custom_level_files[file_name] = new StreamWriter(file_name);
        }
        
    }

    private void LoadAllCampaignLevels()
    {
        foreach (TextAsset text in campaign_level_files)
        {
            campaign_level.Add(new StreamReader(text.pa));
        }
    }

    public void SaveMap(List<string> lines_to_save, string map_name)
    {
        string file_full_path = "";

        if (map_name == "")
        {
            file_full_path = AvailableFileName();
            File.Open(file_full_path, FileMode.OpenOrCreate, FileAccess.Write).Close();
        }
        else
        {
            file_full_path = FilePathFormat(map_name);
            File.Open(file_full_path, FileMode.Truncate, FileAccess.Write).Close();
        }

        using (TextWriter tw = new StreamWriter(file_full_path))
        {
            foreach (string line in lines_to_save)
            {
                tw.WriteLine(line);
            }
        }
    }

    public void LoadMap(string map_name)
    {
        string full_map_path = FilePathFormat(map_name);

        if (!File.Exists(full_map_path)) { return; }

        string line;
        string current_model_loaded = START_OF_MACHINE_SERIALIZED;

        StreamReader file = new StreamReader(full_map_path);
        while ((line = file.ReadLine()) != null)
        {
            if (line == START_OF_MACHINE_SERIALIZED ||
                line == START_OF_CONNECTION_SERIALIZED)
            {
                current_model_loaded = line;
                continue;
            }

            switch(current_model_loaded)
            {
                case START_OF_MACHINE_SERIALIZED:
                    {
                        map_file_machines.Add(LoadSerializedMachine(line));
                        break;
                    }
                case START_OF_CONNECTION_SERIALIZED:
                    {
                        map_file_connections.Add(LoadSerializedConnection(line));
                        break;
                    }
            }
        }

        file.Close();
    }

    private MachineSerialized LoadSerializedMachine(string json)
    {
        return JsonUtility.FromJson<MachineSerialized>(json);
    }

    private WireSerialized LoadSerializedConnection(string json)
    {
        return JsonUtility.FromJson<WireSerialized>(json);
    }

    public List<string> AllLevelNames(GameController.GameStatus game_status)
    {
        switch (game_status)
        {
            case GameController.GameStatus.GAME_MODE:
                {
                    List<string> file_names = new List<string>();
                    foreach(TextAsset text  in campaign_level_files)
                    {
                        file_names.Add(text.name);
                    }
                    return file_names;
                    break;
                }
            case GameController.GameStatus.EDIT_MODE:
                {
                    return new List<string>(custom_level_files.Keys);
                    break;
                }
            default:
                {
                    return new List<string>();
                }
        }
    }

    private string AvailableFileName()
    {
        int file_end_id = 0;
        string name = null;

        do
        {
            file_end_id ++;
            name = FilePathFormat(FileNameFormat(file_end_id));
        } while (File.Exists(name));

        return name;
    }

    private string FilePathFormat(string file_name)
    {
        return string.Format("{0}/{1}{2}",
                             CustomMapPath(),
                             file_name,
                             DEFAULT_END_FILE_EXTENSION);
    }

    private string FileNameFormat(int id)
    {
        return string.Format("{0}{1}",
                             DEFAULT_START_FILE_NAME,
                             id.ToString());
    }

    private string CustomMapPath()
    {
        return persistent_path;
    }
}
