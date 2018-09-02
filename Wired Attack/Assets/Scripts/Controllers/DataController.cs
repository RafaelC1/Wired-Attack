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

    public const string START_OF_MACHINE_SERIALIZED = "machines";
    public const string START_OF_CONNECTION_SERIALIZED = "wires";

    public List<TextAsset> campaign_level_files = new List<TextAsset>();
    private IDictionary<string, TextAsset> campaign_levels = new Dictionary<string, TextAsset>();

    public List<MachineSerialized> map_file_machines = new List<MachineSerialized>();
    public List<WireSerialized> map_file_connections = new List<WireSerialized>();

    private string custom_level_path = "";

    void Start()
    {
        custom_level_path = Application.persistentDataPath + CUSTOM_LEVEL_FOLDER;

        LoadAllCampaignLevels();
        CreateCustomLevelDirectory();
    }
    void Update()
    {
    }

    private void CreateCustomLevelDirectory()
    {
        if (!Directory.Exists(CustomMapPath()))
        {
            Directory.CreateDirectory(custom_level_path);
        }
    }

    private void LoadAllCampaignLevels()
    {
        foreach (TextAsset text_file in campaign_level_files)
        {
            campaign_levels[text_file.name] = text_file;
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
        List<string> map_script_lines = new List<string>();
        string current_model_loaded = START_OF_MACHINE_SERIALIZED;

        map_file_machines.Clear();
        map_file_connections.Clear();

        //ESSA MERDA NÃO DEVERIA FUNCIONAR ASSIM, CONCERTAR
        game_controller = GameObject.Find("GameController").GetComponent<GameController>();

        switch (game_controller.current_status)
        {
            case GameController.GameStatus.GAME_MODE:
                {
                    map_script_lines = LoadCampaignMap(map_name);
                    break;
                }
            case GameController.GameStatus.EDIT_MODE:
                {
                    map_script_lines = LoadCustomMap(map_name);
                    break;
                }
        }        

        foreach(string line in map_script_lines)
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
    }

    private List<string> LoadCustomMap(string map_name)
    {
        List<string> file_lines = new List<string>();
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                {
                    break;
                }
            case RuntimePlatform.WindowsPlayer:
                {

                    string line = "";
                    string full_map_path = FilePathFormat(map_name);

                    if (!File.Exists(full_map_path)) { return new List<string>(); }

                    StreamReader file = new StreamReader(full_map_path);

                    while ((line = file.ReadLine()) != null)
                    {
                        file_lines.Add(line);
                    }

                    file.Close();
                    break;
                    break;
                }
        }

        return file_lines;
    }

    private List<string> LoadCampaignMap(string map_name)
    {
        TextAsset selected_level = campaign_levels[map_name];
        return new List<string>(Regex.Split(selected_level.text, "\n|\r|\r\n"));
    }

    public List<string> AllLevelNames(GameController.GameStatus game_status)
    {
        List<string> level_names = new List<string>();

        switch(game_status)
        {
            case GameController.GameStatus.GAME_MODE:
                {
                    level_names = AllCampaignLevelsNames();
                    break;
                }
            case GameController.GameStatus.EDIT_MODE:
                {
                    level_names = AllCustomLevelsNames();
                    break;
                }
        }

        return level_names;
    }

    private List<string> AllCampaignLevelsNames()
    {
        return new List<string>(campaign_levels.Keys);
    }

    private List<string> AllCustomLevelsNames()
    {
        List<string> level_names = new List<string>();
        DirectoryInfo dir_info = new DirectoryInfo(CustomMapPath());
    
        foreach(FileInfo file in dir_info.GetFiles())
        {
            string file_name = file.Name.Split('.')[0];
            level_names.Add(file_name);
        }
        return level_names;
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
        return custom_level_path;
    }

    private MachineSerialized LoadSerializedMachine(string json)
    {
        return JsonUtility.FromJson<MachineSerialized>(json);
    }

    private WireSerialized LoadSerializedConnection(string json)
    {
        return JsonUtility.FromJson<WireSerialized>(json);
    }
}
