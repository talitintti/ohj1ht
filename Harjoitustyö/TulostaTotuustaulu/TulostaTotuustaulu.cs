using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.CodeDom;
using System.Diagnostics;
using System.Text.RegularExpressions;

/// @author toloojxz
/// @version 28.10.2020
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

        string[] muuttujat = ErotteleMuuttujat(formatoituLauseke);
        bool[,] kombinaatiot = BoolKombinaatiot(muuttujat);
        
        
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

        TarkistaSulut(formatoitava);

        return formatoitava
            .Replace("NOT", "!")
            .Replace("AND", "&&")
            .Replace("OR", "||")
            .ToString()
            .Trim();
    }

    public static void TarkistaSulut(StringBuilder tarkistettava)
    {
        int montako = 0;
        for (i = 0; i < tarkistettava.Length; i++)
        {
            switch (tarkistettava[i])
            {
                case '(':
                    montako++;
                    break;

                case ')':
                    montako++;
                    break;
            }
        }

        if (montako % 2 != 0) Console.WriteLine("Tarkista sulut!");
    }

    /// <summary>
    /// Palauttaa annetusta lausekkeesta taulukon, jossa on lausekkeen muuttujat
    /// </summary>
    /// <param name="lauseke"></param>
    /// <returns></returns>
    public static string[] ErotteleMuuttujat(string lauseke)
    {
        Regex aakkoset = new Regex(@"[a-z]");
        MatchCollection matches = alphabet.Matches(aakkoset);
        var muuttujat = matches.Select(m => m.Groups[0].Value).ToArray();
        return muuttujat;
    }

    /// <summary>
    /// Ottaa listan muuttujia ja palauttaa niiden kaikki kombinaatiot 
    /// </summary>
    /// <param name="lista"></param>
    /// <returns></returns>
    public static bool[,] BoolKombinaatiot(string[] lista)
    {
        //TODO: rekursion ja heapin algoritmin avulla luo permutaatiot
        return null;
    }
}