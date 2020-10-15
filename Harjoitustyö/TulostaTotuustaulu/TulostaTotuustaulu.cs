using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;

/// @author toloojxz
/// @version 15.10.2020
/// <summary>
/// Käyttäjä syöttää haluamansa boolen sääntöjen mukaisen lausekkeen
/// ja ohjelma tulostaa sitä vastaavan totuustaulun  
/// </summary>
public class TulostaTotuustaulu
{
    //TODO: help screen
    public static void Main()
    {
        Console.Write("Syötä lauseke tähän: ");
        string userInput = Console.ReadLine();

        string formatoituLauseke = Prosessoi(userInput);

        // short vastaava = VastaavaNumero(laskettuBool);

        // TulostaTaulukko(vastaava);
    }


    /// <summary>
    /// Muuttaa käyttäjän antaman lausekkeen suoraan koodille annettavaksi lausekkeeksi
    /// </summary>
    /// <param name="userInput"></param>
    /// <returns></returns>
    public static string Prosessoi(string userInput)
    {
        StringBuilder formatoitava = new StringBuilder(userInput);

        for (short i = 0; i < formatoitava.Length; i++)
        {
            switch (formatoitava[i])
            {
                case '&':
                    formatoitava.Insert(i, '&');
                    i++;
                    break;
                case '|':
                    formatoitava.Insert(i, '|');
                    i++;
                    break;
            }
        }

        formatoitava.Replace("NOT", "!").Replace("AND", "&&").Replace("OR", "||");

        Console.WriteLine(formatoitava);
        return "";
    }
}