using System;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class DataComparer : MonoBehaviour
{
    public Text progressText;  // UI Text for showing progress
    public GameObject hiddenPanel;  // Panel to be shown/hidden

    private async void Start()
    {
        string filePath = "path_to_your_file.txt"; // Replace with your txt file path
        string url = "https://github.com/soulcatdev/catsite/blob/C%23/checker.txt"; // Replace with your target URL

        // Step 1: Read numbers from the txt file
        var (a, b) = ReadNumbersFromFile(filePath);
        
        // Step 2: Fetch the webpage source and extract numbers a1 and b1
        var (a1, b1) = await FetchNumbersFromWebPage(url);

        // Step 3: Compare and decide what to do
        if (a < a1)
        {
            // If a < a1, show the hidden panel and stop further comparisons
            hiddenPanel.SetActive(true);
            return;
        }

        if (a == a1)
        {
            if (b < b1)
            {
                // If a == a1 and b < b1, show the hidden panel
                hiddenPanel.SetActive(true);
            }
        }
    }

    private (int, int) ReadNumbersFromFile(string filePath)
    {
        int a = 0, b = 0;
        string fileContent = File.ReadAllText(filePath);

        // Regex to match numbers between vision//...// and note//...//
        var visionMatch = Regex.Match(fileContent, @"vision\/\/(\d+)");
        var noteMatch = Regex.Match(fileContent, @"note\/\/(\d+)");

        if (visionMatch.Success)
        {
            a = int.Parse(visionMatch.Groups[1].Value);
        }
        if (noteMatch.Success)
        {
            b = int.Parse(noteMatch.Groups[1].Value);
        }

        return (a, b);
    }

    private async Task<(int, int)> FetchNumbersFromWebPage(string url)
    {
        int a1 = 0, b1 = 0;

        using (HttpClient client = new HttpClient())
        {
            try
            {
                string pageContent = await client.GetStringAsync(url);

                // Regex to match numbers between ✦...✦ and ●...●
                var a1Match = Regex.Match(pageContent, @"✦(\d+)✦");
                var b1Match = Regex.Match(pageContent, @"●(\d+)●");

                if (a1Match.Success)
                {
                    a1 = int.Parse(a1Match.Groups[1].Value);
                }
                if (b1Match.Success)
                {
                    b1 = int.Parse(b1Match.Groups[1].Value);
                }

                // Update progress text (simulating progress during the task)
                for (int i = 0; i <= 100; i++)
                {
                    progressText.text = $"Loading: {i}%";
                    await Task.Delay(50);  // Simulate some delay for the progress
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error fetching webpage: {ex.Message}");
            }
        }

        return (a1, b1);
    }
}

