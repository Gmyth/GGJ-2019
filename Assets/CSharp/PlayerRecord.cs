public struct PlayerRecord
{
    public readonly int id;
    public readonly string name;
    public readonly int score;

    public PlayerRecord(Player player)
    {
        id = player.Id;
        name = player.name;
        score = player.Score;
    }
}
