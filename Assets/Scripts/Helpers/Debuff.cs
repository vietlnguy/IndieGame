[System.Serializable]
public class Debuff {
    public string name;
    public int chanceToApply;
    public int turnsRemaining;

    public Debuff(string name, int chanceToApply, int turnsRemaining) {
        this.name = name;
        this.chanceToApply = chanceToApply;
        this.turnsRemaining = turnsRemaining;

    }


}