using TMPro;

public class ScoreItem : UI_Base
{
    enum Texts
    {
        Rank,
        Name,
        Score
    }

    public override void Init()
    {
        Bind<TextMeshProUGUI>(typeof(Texts));
    }

    public void SetInfo(string name, int rank, int score)
    {
        Get<TextMeshProUGUI>((int)Texts.Name).text = name;
        Get<TextMeshProUGUI>((int)Texts.Rank).text = rank.ToString();
        Get<TextMeshProUGUI>((int)Texts.Score).text = score.ToString();
    }
}
