using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.CodeDom;
using System.Diagnostics;
using System.Text.RegularExpressions;

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

        string[] muuttujat = ErotteleMuuttujat(formatoituLauseke);
        List<bool> kombinaatiot = BoolKombinaatiot(muuttujat);
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

    public static string[] ErotteleMuuttujat(string lauseke)
    {
        Regex aakkoset = new Regex(@"[a-z]");
        MatchCollection matches = alphabet.Matches(aakkoset);
        var uusihomma = matches.Select(m => m.Groups[0].Value).ToArray();
        return uusihomma;
    }

    public static List<bool> BoolKombinaatiot(string[] lista)
    {
        return null;
    }
}