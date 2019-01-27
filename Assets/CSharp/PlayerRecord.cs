public struct PlayerRecord
{
    public readonly int playerId;
    public readonly string playerName;
    public readonly int playerScore;

    public PlayerRecord(Player player)
    {
        playerId = player.Id;
        playerName = player.Name;
        playerScore = player.Score;
    }

    public override string ToString()
    {
        return string.Format("[{0}] {1} ... {2}", playerId, playerName, playerScore);
    }
}
