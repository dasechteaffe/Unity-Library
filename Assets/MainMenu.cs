using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Data;
using Mono.Data.Sqlite;
using TMPro; // Für TextMeshPro

public class MainMenu : MonoBehaviour
{
    public Button startButton;
    public Button databaseButton;
    public TextMeshProUGUI databaseText; // Geändert zu TextMeshProUGUI

    private string dbName = "URI=file:BookDatabase.db"; 
    private bool isDatabaseVisible = false; // Variable zum Überprüfen der Sichtbarkeit

    void Start()
    {
        startButton.onClick.AddListener(StartGame);
        databaseButton.onClick.AddListener(ToggleDatabaseVisibility);
        databaseText.gameObject.SetActive(false); // Anfangs ist das Textfeld nicht sichtbar
    }

    void StartGame()
    {
        SceneManager.LoadScene("Library"); 
    }

    void ToggleDatabaseVisibility()
    {
        if (isDatabaseVisible)
        {
            // Wenn das Textfeld sichtbar ist, verstecken
            databaseText.gameObject.SetActive(false);
            isDatabaseVisible = false;
        }
        else
        {
            // Wenn das Textfeld nicht sichtbar ist, Datenbankzugriff und anzeigen
            AccessDatabase();
            databaseText.gameObject.SetActive(true);
            isDatabaseVisible = true;
        }
    }

    void AccessDatabase()
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Books"; 
                using (IDataReader reader = command.ExecuteReader())
                {
                    string data = "";
                    while (reader.Read())
                    {
                        // Angenommen, Ihre Tabelle hat Spalten "Title" und "Author"
                        data += reader["Title"] + " von " + reader["Author"] + " - " + reader["Borrower"] + "\n";
                    }
                    databaseText.text = data; 
                }
            }
        }
    }
}
