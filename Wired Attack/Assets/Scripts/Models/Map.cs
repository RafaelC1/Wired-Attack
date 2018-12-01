using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map {

    public string name = null;
    public string fileName = null;

    [TextArea]
    public string description = null;
    public List<string> tipTexts = new List<string>();
    public int mapStatus = -1;
    public float time = 0f;

    public List<MachineSerialized> machinesSerializeds = new List<MachineSerialized>();
    public List<ConnectionSerialized> connectionsSerializeds = new List<ConnectionSerialized>();
    public List<DecorationSerialized> decorationsSerializeds = new List<DecorationSerialized>();

    public enum MapStatus
    {
        BLOCKED,
        ALLOWED,
        PASSED
    }

    public Map() { }

    public Map(List<GameObject> machineGos, List<GameObject> connectionGos, List<GameObject> decorationGos)
    {
        ReplaceListOfMachines(machineGos);
        ReplaceListOfConnections(connectionGos);
        ReplaceListOfDecorations(decorationGos);

    }

    public Map(List<string> lines_of_file)
    {
        string currentModelToLoad = DataController.START_OF_MACHINE_SERIALIZED;
        foreach (string line in lines_of_file)
        {
            if (line == DataController.START_OF_MACHINE_SERIALIZED ||
                line == DataController.START_OF_CONNECTION_SERIALIZED ||
                line == DataController.START_OF_DECORATION_SERIALIZED ||
                line == DataController.START_OF_DESCRIPTION_SERIALIZED ||
                line == DataController.START_OF_TIPS_SERIALIZED ||
                line == DataController.START_OF_TITLE_SERIALIZED)
            {
                currentModelToLoad = line;
                continue;
            }
            switch (currentModelToLoad)
            {
                case DataController.START_OF_MACHINE_SERIALIZED:
                    {
                        machinesSerializeds.Add(LoadSerializedMachine(line));
                        break;
                    }
                case DataController.START_OF_CONNECTION_SERIALIZED:
                    {
                        connectionsSerializeds.Add(LoadSerializedConnection(line));
                        break;
                    }
                case DataController.START_OF_DECORATION_SERIALIZED:
                    {
                        decorationsSerializeds.Add(LoadSerializedDecoration(line));
                        break;
                    }
                case DataController.START_OF_DESCRIPTION_SERIALIZED:
                    {
                        description += line;
                        break;
                    }
                case DataController.START_OF_TIPS_SERIALIZED:
                    {
                        tipTexts.Add(line);
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

    public void ReplaceListOfMachines(List<GameObject> machinesGos)
    {
        machinesSerializeds.Clear();
        foreach (GameObject machineGo in machinesGos)
        {
            machinesSerializeds.Add(new MachineSerialized(machineGo.GetComponent<Machine>()));
        }
    }

    public void ReplaceListOfConnections(List<GameObject> connectionsGos)
    {
        connectionsSerializeds.Clear();
        foreach (GameObject connectionGo in connectionsGos)
        {
            connectionsSerializeds.Add(new ConnectionSerialized(connectionGo.GetComponent<Connection>()));
        }
    }

    public void ReplaceListOfDecorations(List<GameObject> decorationsGos)
    {
        decorationsSerializeds.Clear();
        foreach (GameObject decorationGo in decorationsGos)
        {
            decorationsSerializeds.Add(new DecorationSerialized(decorationGo.GetComponent<Decoration>()));
        }
    }

    public List<string> Serialized()
    {
        List<string> linesOfSerializedObjectsOnMap = new List<string>();

        linesOfSerializedObjectsOnMap.Add(DataController.START_OF_TITLE_SERIALIZED);
        linesOfSerializedObjectsOnMap.Add(name);

        linesOfSerializedObjectsOnMap.Add(DataController.START_OF_DESCRIPTION_SERIALIZED);
        linesOfSerializedObjectsOnMap.Add(description);

        linesOfSerializedObjectsOnMap.Add(DataController.START_OF_TIPS_SERIALIZED);
        foreach (string tip in tipTexts)
        {
            linesOfSerializedObjectsOnMap.Add(tip);
        }

        linesOfSerializedObjectsOnMap.Add(DataController.START_OF_MACHINE_SERIALIZED);

        foreach (MachineSerialized machineSerialized in machinesSerializeds)
        {
            linesOfSerializedObjectsOnMap.Add(machineSerialized.ToJson());
        }

        linesOfSerializedObjectsOnMap.Add(DataController.START_OF_CONNECTION_SERIALIZED);

        foreach (ConnectionSerialized connectionSerialized in connectionsSerializeds)
        {
            linesOfSerializedObjectsOnMap.Add(connectionSerialized.ToJson());
        }

        linesOfSerializedObjectsOnMap.Add(DataController.START_OF_DECORATION_SERIALIZED);

        foreach (DecorationSerialized deocrationSerializsed in decorationsSerializeds)
        {
            linesOfSerializedObjectsOnMap.Add(deocrationSerializsed.ToJson());
        }

        return linesOfSerializedObjectsOnMap;
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
