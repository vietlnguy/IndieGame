[System.Serializable]
public class Buff {
    public string name;
    public int chanceToApply;
    public int turnsRemaining;

    public Buff(string name, int chanceToApply, int turnsRemaining) {
        this.name = name;
        this.chanceToApply = chanceToApply;
        this.turnsRemaining = turnsRemaining;

    }


}