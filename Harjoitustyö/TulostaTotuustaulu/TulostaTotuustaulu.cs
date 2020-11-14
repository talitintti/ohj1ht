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

        char[] muuttujat = ErotteleMuuttujat(formatoituLauseke);
        //muuttujat = BoolKombinaatiot(muuttujat);

        Console.WriteLine(formatoituLauseke);
    }


    /// <summary>
    /// Muuttaa käyttäjän antaman lausekkeen suoraan koodille annettavaksi lausekkeeksi
    /// </summary>
    /// <example>
    /// <pre name="test">
    /// Prosessoi("a && b") === "a && b";
    /// Prosessoi("a || b") === "a || b";
    /// Prosessoi("a & b") === "a && b";
    /// Prosessoi("a | b") === "a || b";
    /// Prosessoi("a && b !(c) NOT(c &&b)") === "a && b !(c) !(c &&b)";
    /// </pre>
    /// </example>
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
                    if (formatoitava[i] == formatoitava[++i]) break;
                    formatoitava.Insert(i, '&');
                    i++;
                    break;
                case '|':
                    if (formatoitava[i] == formatoitava[++i]) break;
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
        int montako1 = 0;
        int montako2 = 0;
        for (int i = 0; i < tarkistettava.Length; i++)
        {
            switch (tarkistettava[i])
            {
                case '(':
                    montako1++;
                    break;

                case ')':
                    montako2++;
                    break;
            }
        }

        if (montako1 != montako2) Console.WriteLine("Tarkista sulut!");
    }

    /// <summary>
    /// Palauttaa annetusta lausekkeesta taulukon, jossa on lausekkeen muuttujat
    /// </summary>
    /// <param name="lauseke"></param>
    /// <returns></returns>
    public static char[] ErotteleMuuttujat(string lauseke)
    {
        string aakkoset = @"[a-z]";
        MatchCollection loydetut = Regex.Matches(lauseke, aakkoset);
        char[] muuttujat = new char[loydetut.Count];
        for (int i = 0; i < muuttujat.Length; i++) muuttujat[i] = Convert.ToChar(loydetut[i]);
        return muuttujat;
    }

    /// <summary>
    /// Ottaa listan muuttujia ja palauttaa niiden kaikki kombinaatiot 
    /// </summary>
    /// <param name="lista"></param>
    /// <returns></returns>
    public static char[,] BoolKombinaatiot(char[] lista)
    {
        if (lista.Length == 1) return lista;
        int listanPituus = lista.Length;
        string[,] palautettava = new string[Kertoma(listanPituus), listanPituus];
        return null;
    }

    /// <summary>
    /// Antaa annetun kokonaisluvun kertoman
    /// </summary>
    /// <example>
    /// <pre name="test">
    /// Kertoma(0) === 0;
    /// Kertoma(1) === 1;
    /// Kertoma(2) === 2;
    /// Kertoma(3) === 6;
    /// Kertoma(4) === 24;
    /// </pre>
    /// </example>
    /// <param name="num"></param>
    /// <returns></returns>
    public static int Kertoma(int num)
    {
        int tulos = num;
        for (int i = num - 1; i > 0; i--) tulos *= i;
        return tulos;
    }
}