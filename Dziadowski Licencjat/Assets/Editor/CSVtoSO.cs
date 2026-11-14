using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public class CSVtoSO
{
    private static string cardCSVAPath = "/Editor/CSVs/CardCSVA.csv";
    private static string cardCSVPPath = "/Editor/CSVs/CardCSVP.csv";
    private static string cardCSVSPath = "/Editor/CSVs/CardCSVS.csv";
    private const string CsvSplitPattern = ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)";
    private const string Quote = "\"";
    private const string DoubleQuote = "\"\"";
    private const int QuoteLength = 1;
    private const int MinQuotedLength = 2;
    private const int PolishWindowsCodePage = 1250;

    [MenuItem("Utilities/Generate Attack Cards")]
    public static void GenerateAttackCards()
    {
        Encoding encoding = Encoding.GetEncoding(PolishWindowsCodePage);
        string[] allLines = File.ReadAllLines(Application.dataPath + cardCSVAPath, encoding);

        foreach (string s in allLines)
        {
            string[] splitData = Regex.Split(s, CsvSplitPattern);

            CardSO cardSO = ScriptableObject.CreateInstance<CardSO>();
            cardSO.cardSprite = null;
            cardSO.cardRarity = (CardRarity)int.Parse(splitData[1]);
            cardSO.cardType = (CardType)int.Parse(splitData[2]);
            cardSO.cardName = CleanField(splitData[3]);
            cardSO.cardDescription = CleanField(splitData[4]);
            cardSO.cardDamage = int.Parse(splitData[5]);
            cardSO.cardMana = int.Parse(splitData[6]);
            cardSO.cardBlock = int.Parse(splitData[7]);
            cardSO.isExhaust = bool.Parse(splitData[8]);
            cardSO.minionDamage = int.Parse(splitData[9]);
            cardSO.minionBlock = int.Parse(splitData[10]);
            cardSO.selfWeaknessTurns = int.Parse(splitData[11]);
            cardSO.enemyWeaknessTurns = int.Parse(splitData[12]);
            cardSO.enemyVulnerabilityTurns = int.Parse(splitData[13]);
            cardSO.summonAmount = int.Parse(splitData[14]);
            cardSO.scoopAmount = int.Parse(splitData[15]);
            cardSO.discardFromDrawPileAmount = int.Parse(splitData[16]);

            AssetDatabase.CreateAsset(cardSO, $"Assets/Nora Doriana/ScriptableObjects/Cards/Minion Build/Attacks/{cardSO.cardName}.asset");
        }

        AssetDatabase.SaveAssets();
    }

    [MenuItem("Utilities/Generate Power Cards")]
    public static void GeneratePowerCards()
    {
        Encoding encoding = Encoding.GetEncoding(PolishWindowsCodePage);
        string[] allLines = File.ReadAllLines(Application.dataPath + cardCSVPPath, encoding);

        foreach (string s in allLines)
        {
            string[] splitData = Regex.Split(s, CsvSplitPattern);

            CardSO cardSO = ScriptableObject.CreateInstance<CardSO>();
            cardSO.cardSprite = null;
            cardSO.cardRarity = (CardRarity)int.Parse(splitData[1]);
            cardSO.cardType = (CardType)int.Parse(splitData[2]);
            cardSO.cardName = CleanField(splitData[3]);
            cardSO.cardDescription = CleanField(splitData[4]);
            cardSO.cardDamage = int.Parse(splitData[5]);
            cardSO.cardMana = int.Parse(splitData[6]);
            cardSO.cardBlock = int.Parse(splitData[7]);
            cardSO.isExhaust = bool.Parse(splitData[8]);
            cardSO.minionDamage = int.Parse(splitData[9]);
            cardSO.minionBlock = int.Parse(splitData[10]);
            cardSO.selfWeaknessTurns = int.Parse(splitData[11]);
            cardSO.enemyWeaknessTurns = int.Parse(splitData[12]);
            cardSO.enemyVulnerabilityTurns = int.Parse(splitData[13]);
            cardSO.summonAmount = int.Parse(splitData[14]);
            cardSO.scoopAmount = int.Parse(splitData[15]);
            cardSO.discardFromDrawPileAmount = int.Parse(splitData[16]);

            AssetDatabase.CreateAsset(cardSO, $"Assets/Nora Doriana/ScriptableObjects/Cards/Minion Build/Powers/{cardSO.cardName}.asset");
        }

        AssetDatabase.SaveAssets();
    }

    [MenuItem("Utilities/Generate Skill Cards")]
    public static void GenerateSkillCards()
    {
        Encoding encoding = Encoding.GetEncoding(PolishWindowsCodePage);
        string[] allLines = File.ReadAllLines(Application.dataPath + cardCSVSPath, encoding);

        foreach (string s in allLines)
        {
            string[] splitData = Regex.Split(s, CsvSplitPattern);

            CardSO cardSO = ScriptableObject.CreateInstance<CardSO>();
            cardSO.cardSprite = null;
            cardSO.cardRarity = (CardRarity)int.Parse(splitData[1]);
            cardSO.cardType = (CardType)int.Parse(splitData[2]);
            cardSO.cardName = CleanField(splitData[3]);
            cardSO.cardDescription = CleanField(splitData[4]);
            cardSO.cardDamage = int.Parse(splitData[5]);
            cardSO.cardMana = int.Parse(splitData[6]);
            cardSO.cardBlock = int.Parse(splitData[7]);
            cardSO.isExhaust = bool.Parse(splitData[8]);
            cardSO.minionDamage = int.Parse(splitData[9]);
            cardSO.minionBlock = int.Parse(splitData[10]);
            cardSO.selfWeaknessTurns = int.Parse(splitData[11]);
            cardSO.enemyWeaknessTurns = int.Parse(splitData[12]);
            cardSO.enemyVulnerabilityTurns = int.Parse(splitData[13]);
            cardSO.summonAmount = int.Parse(splitData[14]);
            cardSO.scoopAmount = int.Parse(splitData[15]);
            cardSO.discardFromDrawPileAmount = int.Parse(splitData[16]);

            AssetDatabase.CreateAsset(cardSO, $"Assets/Nora Doriana/ScriptableObjects/Cards/Minion Build/Skills/{cardSO.cardName}.asset");
        }

        AssetDatabase.SaveAssets();
    }

    private static string CleanField(string field)
    {
        string cleaned = field.Trim();

        if (cleaned.StartsWith(Quote) && cleaned.EndsWith(Quote) && cleaned.Length >= MinQuotedLength)
        {
            cleaned = cleaned.Substring(QuoteLength, cleaned.Length - MinQuotedLength);
        }

        return cleaned.Replace(DoubleQuote, Quote);
    }
}