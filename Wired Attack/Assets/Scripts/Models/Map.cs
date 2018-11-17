using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map {

    public string name = null;
    public string file_name = null;

    [TextArea]
    public string description = null;
    public List<string> tip_texts = new List<string>();
    public int map_status = -1;
    public float time = 0f;

    public List<MachineSerialized> serialized_machines = new List<MachineSerialized>();
    public List<ConnectionSerialized> serialized_connections = new List<ConnectionSerialized>();
    public List<DecorationSerialized> serialized_decorations = new List<DecorationSerialized>();

    public enum MapStatus
    {
        BLOCKED,
        ALLOWED,
        PASSED
    }

    public Map() { }

    public Map(List<GameObject> machine_gos, List<GameObject> connection_gos, List<GameObject> decoration_gos)
    {
        ReplaceListOfMachines(machine_gos);
        ReplaceListOfConnections(connection_gos);
        ReplaceListOfDecorations(decoration_gos);

    }

    public Map(List<string> lines_of_file)
    {
        string current_model_loaded = DataController.START_OF_MACHINE_SERIALIZED;
        foreach (string line in lines_of_file)
        {
            if (line == DataController.START_OF_MACHINE_SERIALIZED ||
                line == DataController.START_OF_CONNECTION_SERIALIZED ||
                line == DataController.START_OF_DECORATION_SERIALIZED ||
                line == DataController.START_OF_DESCRIPTION_SERIALIZED ||
                line == DataController.START_OF_TIPS_SERIALIZED ||
                line == DataController.START_OF_TITLE_SERIALIZED)
            {
                current_model_loaded = line;
                continue;
            }
            switch (current_model_loaded)
            {
                case DataController.START_OF_MACHINE_SERIALIZED:
                    {
                        serialized_machines.Add(LoadSerializedMachine(line));
                        break;
                    }
                case DataController.START_OF_CONNECTION_SERIALIZED:
                    {
                        serialized_connections.Add(LoadSerializedConnection(line));
                        break;
                    }
                case DataController.START_OF_DECORATION_SERIALIZED:
                    {
                        serialized_decorations.Add(LoadSerializedDecoration(line));
                        break;
                    }
                case DataController.START_OF_DESCRIPTION_SERIALIZED:
                    {
                        description += line;
                        break;
                    }
                case DataController.START_OF_TIPS_SERIALIZED:
                    {
                        tip_texts.Add(line);
                        break;
                    }
                case DataController.START_OF_TITLE_SERIALIZED:
                    {
                        name = line;
                        break;
                    }
            }
        }
    }

    public void ReplaceListOfMachines(List<GameObject> machine_gos)
    {
        serialized_machines.Clear();
        foreach (GameObject machine_go in machine_gos)
        {
            serialized_machines.Add(new MachineSerialized(machine_go.GetComponent<Machine>()));
        }
    }

    public void ReplaceListOfConnections(List<GameObject> connection_gos)
    {
        serialized_connections.Clear();
        foreach (GameObject connection_go in connection_gos)
        {
            serialized_connections.Add(new ConnectionSerialized(connection_go.GetComponent<Connection>()));
        }
    }

    public void ReplaceListOfDecorations(List<GameObject> decoration_gos)
    {
        serialized_decorations.Clear();
        foreach (GameObject decoration_go in decoration_gos)
        {
            serialized_decorations.Add(new DecorationSerialized(decoration_go.GetComponent<Decoration>()));
        }
    }

    public List<string> Serialized()
    {
        List<string> lines_of_serialized_model = new List<string>();

        lines_of_serialized_model.Add(DataController.START_OF_TITLE_SERIALIZED);
        lines_of_serialized_model.Add(name);

        lines_of_serialized_model.Add(DataController.START_OF_DESCRIPTION_SERIALIZED);
        lines_of_serialized_model.Add(description);

        lines_of_serialized_model.Add(DataController.START_OF_TIPS_SERIALIZED);
        foreach (string tip in tip_texts)
        {
            lines_of_serialized_model.Add(tip);
        }

        lines_of_serialized_model.Add(DataController.START_OF_MACHINE_SERIALIZED);

        foreach (MachineSerialized serialized_machine in serialized_machines)
        {
            lines_of_serialized_model.Add(serialized_machine.ToJson());
        }

        lines_of_serialized_model.Add(DataController.START_OF_CONNECTION_SERIALIZED);

        foreach (ConnectionSerialized serialized_connection in serialized_connections)
        {
            lines_of_serialized_model.Add(serialized_connection.ToJson());
        }

        lines_of_serialized_model.Add(DataController.START_OF_DECORATION_SERIALIZED);

        foreach (DecorationSerialized serialized_decoration in serialized_decorations)
        {
            lines_of_serialized_model.Add(serialized_decoration.ToJson());
        }

        return lines_of_serialized_model;
    }

    private MachineSerialized LoadSerializedMachine(string json)
    {
        return JsonUtility.FromJson<MachineSerialized>(json);
    }

    private ConnectionSerialized LoadSerializedConnection(string json)
    {
        return JsonUtility.FromJson<ConnectionSerialized>(json);
    }

    private DecorationSerialized LoadSerializedDecoration(string json)
    {
        return JsonUtility.FromJson<DecorationSerialized>(json);
    }
    
}
