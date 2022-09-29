[System.Serializable]
public class StateDvd
{
    public int stateId;
    public DialogueData data;

    public StateDvd(int stateId, DialogueData data)
    {
        this.stateId = stateId;
        this.data = data;
    }

}

