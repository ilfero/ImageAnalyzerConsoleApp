
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static string imageFolder = "./images"; // percorso della cartella da analizzare
    static string csvFile = "image_analysis.csv"; // nome file .csv da generare
    static Random random = new Random(); // valore casuale per il parametro luminosità per il filtro di ricerca soglia

    static void Main()
    {
        Console.WriteLine("Image analyzer console app");
        Directory.CreateDirectory(imageFolder);
        
        // in base alla scelta dell'utente devo eseguire delle operazioni sulla cartella della immagini
        while (true)
        {
            Console.WriteLine("\nSeleziona una operazione da eseguire\n1. Carica immagini da cartella\n2. Leggi dati immagini\n3. Analizza immagini\n4. Filtra per luminosità\n5. Esci");
            Console.Write("Scelta: ");
            string scelta = Console.ReadLine();

            switch (scelta)
            {
                case "1": CaricaImmaginiDaPercorso(); break;
                case "2": VisualizzaDatiImmagini(); break;
                case "3": AnalizzaImmagini(); break;
                case "4": CercaPerLuminosita(); break;
                case "5": return;
                default: Console.WriteLine("Seleziona un valore tra quelli indicati"); break;
            }
        }
    }

    /// <summary>
    /// funzione per controllare la presenza della cartella e delle immagini del tipo che voglio analizzare
    /// </summary>
    /// <param name="immagini"></param>
    /// <returns></returns>
    static bool VerificaCartellaEImmagini(out IEnumerable<string> immagini)
    {
        //inizializzo la variaile per il controllo della presenza dell immagini
        immagini = Enumerable.Empty<string>();

        //verifico se esiste la directory delle immagini
        if (!Directory.Exists(imageFolder))
        {
            Console.WriteLine("La cartella delle immagini non esiste.");
            return false;
        }

        //recupero l'elenco delle immagini
        immagini = Directory.GetFiles(imageFolder, "*.jpg").Concat(Directory.GetFiles(imageFolder, "*.png"));

        //se non ci sono immagini valide esco con messaggio.
        if (!immagini.Any())
        {
            Console.WriteLine("Nessuna immagine trovata nella cartella.");
            return false;
        }

        return true;
    }

    /// <summary>
    /// Effettuo il caricamento delle immagini dal percorso indicato 
    /// </summary>
    static void CaricaImmaginiDaPercorso()
    {
        if (VerificaCartellaEImmagini(out var immagini))
        {
            Console.WriteLine($"Trovate {immagini.Count()} immagini nella cartella.");
        }

    }

    /// <summary>
    /// Recupero le informazioni relative alla immagini e le mostro a video
    /// </summary>
    static void VisualizzaDatiImmagini()
    {
        if (VerificaCartellaEImmagini(out var immagini))
        {
            //recupero i dati delle immagini e li mostro a video
            foreach (var img in immagini)
            {
                FileInfo fileInfo = new FileInfo(img);
                Console.WriteLine($"{fileInfo.Name} - {fileInfo.Length} bytes - Creato: {fileInfo.CreationTime}");
            }
        }
    }

    /// <summary>
    /// Per ogni immagine leggo i dati relativi alla luminosità
    /// </summary>
    static void AnalizzaImmagini()
    {
        if (VerificaCartellaEImmagini(out var immagini))
        {
            //creao un elenco di valori da popolare con i dati letti dalle immagini
            List<string> risultati = new List<string> { "Nome File, Dimensione (bytes), Luminosità Stimata, Data Analisi" };

            //leggo i dati delle immagini
            foreach (var img in immagini)
            {
                FileInfo fileInfo = new FileInfo(img);
                int luminosita = random.Next(0, 101);
                risultati.Add($"{fileInfo.Name}, {fileInfo.Length}, {luminosita}, {DateTime.Now}");
            }

            File.WriteAllLines(csvFile, risultati);
            Console.WriteLine("Analisi completata. Risultati salvati nel file {csvFile}");
        }
    }

    /// <summary>
    /// Effettuo la ricerca in base ai parametri impostati
    /// </summary>
    static void CercaPerLuminosita()
    {
        if (!File.Exists(csvFile))
        {
            Console.WriteLine("Nessun dato disponibile. Esegui prima l'analisi.");
            return;
        }

        //recupero da console il valore soglia per cui filtrare le immagini
        Console.Write("Inserisci soglia di luminosità: ");
        if (!int.TryParse(Console.ReadLine(), out int soglia))
        {
            Console.WriteLine("Valore non valido.");
            return;
        }

        //leggo le righe del file e seleziono quelle che rientrano del dati della soglia da prenere in considerazione
        var righe = File.ReadAllLines(csvFile).Skip(1);
        var risultati = righe.Where(r => int.Parse(r.Split(',')[2]) > soglia);

        if (!risultati.Any())
        {
            Console.WriteLine("Nessuna immagine con luminosità superiore alla soglia trovata.");
            return;
        }

        foreach (var r in risultati)
            Console.WriteLine(r);
    }
}


