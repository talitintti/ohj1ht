using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.CodeDom;
using System.Data;
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

        StringBuilder formatoituLauseke = Prosessoi(userInput);
        Console.WriteLine(formatoituLauseke);

        char[] muuttujat = ErotteleMuuttujat(formatoituLauseke);
        int[,] kombiTaulukko = BoolKombinaatiot(muuttujat);

        int[] vastaukset = PalautaTulokset(kombiTaulukko, muuttujat, formatoituLauseke);
        TulostaKombinaatiot(muuttujat, kombiTaulukko, vastaukset);
        //Console.WriteLine(formatoituLauseke);
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
    public static StringBuilder Prosessoi(string userInput)
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

        {
            int i = 0;
            while (i < formatoitava.Length)
            {
                if (formatoitava[i] == ' ')
                    formatoitava.Remove(i, 1);
                else i++;
            }
        }

        TarkistaSulut(formatoitava);

        return formatoitava
            .Replace("NOT", "!")
            .Replace("AND", "&&")
            .Replace("OR", "||");
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
    public static char[] ErotteleMuuttujat(StringBuilder sb)
    {
        string lauseke = sb.ToString();
        string aakkoset = @"[a-z]";
        MatchCollection loydetut = Regex.Matches(lauseke, aakkoset);

        var muuttujat = loydetut.OfType<Match>()
            .Select(loyto => loyto.Groups[0].Value)
            .Distinct()
            .ToArray();

        char[] muuttujatChar = new char[muuttujat.Length];
        int i = 0;
        foreach (string kirjain in muuttujat) muuttujatChar[i++] = Convert.ToChar(kirjain);

        return muuttujatChar;
    }

    /// <summary>
    /// Ottaa listan muuttujia ja palauttaa niiden kaikki kombinaatiot
    /// Tyhjän listan saadessa palautetaan {-1}
    /// </summary>
    /// <param name="lista"></param>
    /// <returns></returns>
    public static int[,] BoolKombinaatiot(char[] lista)
    {
        if (lista.Length == 0)
        {
            Console.WriteLine("Tarkista antamasi muuttujat");
            return new int[,] {{-1}};
        }


        int dimMaara0 = Convert.ToInt16(Math.Pow(2, lista.Length));
        int[,] binComb = new int[dimMaara0, lista.Length];

        for (int x = 0; x < lista.Length; x++)
        {
            double kerroin = 1 / Math.Pow(2, x + 1);
            double kierrosVakio = kerroin * dimMaara0;


            int n = 1;
            for (double inLoop = Math.Pow(2, x); inLoop > 0; inLoop--)
            {
                double y = kierrosVakio * n;
                double endPoint = kierrosVakio * (n + 1);
                while (y < endPoint)
                {
                    binComb[(int) y, x] = 1;
                    y++;
                }

                n += 2;
            }
        }

        return binComb;
    }

    public static void TulostaKombinaatiot(char[] muuttujat, int[,] taulukko, int[] vastaustaulukko)
    {
        Console.Write("====================");
        Console.Write("\n");
        foreach (var kirjain in muuttujat)
        {
            if (kirjain != muuttujat.Last()) Console.Write("{0} | ", kirjain);
            else Console.Write(kirjain);
        }

        Console.Write("\n");
        for (int i = 0; i < muuttujat.Length; i++) Console.Write("----");

        Console.Write("\n");
        for (int y = 0; y < taulukko.GetLength(0); y++)
        {
            for (int x = 0; x < taulukko.GetLength(1); x++)
            {
                Console.Write("{0}   ", taulukko[y, x]);
            }

            Console.Write("| {0}\n", vastaustaulukko[y]);
        }
    }

    public static int[] PalautaTulokset(int[,] eval, char[] muuttujat, StringBuilder alkLauseke)
    {
        int[] tulokset = new int[eval.GetLength(0)];
        StringBuilder lauseke = new StringBuilder(alkLauseke.ToString());

        for (int vastausRiveja = 0; vastausRiveja < tulokset.Length; vastausRiveja++)
        {
            for (int i = 0; i < lauseke.Length; i++)
                if (lauseke[i] == ')')
                    for (int j = i; j >= 0; j--)
                        if (lauseke[j] == '(')
                        {
                            if (j - 1 >= 0 && j + 2 == i && lauseke[j - 1] == '!')
                            {
                                int sijoitettavaIndeksi =
                                    Array.FindIndex(muuttujat, muuttuja => muuttuja == lauseke[j + 1]);
                                char sijoitettava;

                                if (sijoitettavaIndeksi != -1)
                                    if (eval[vastausRiveja, sijoitettavaIndeksi] == 0)
                                        sijoitettava = '1';
                                    else sijoitettava = '0';
                                else if (lauseke[j + 1] == '1') sijoitettava = '0';
                                else sijoitettava = '1';
                                lauseke.Remove(j - 1, 4).Insert(j - 1, sijoitettava);

                                i = 0;
                                break;
                            }

                            if (lauseke[j + 2] == lauseke[i])
                            {
                                if (j != 0 && lauseke[j - 1] == '!') lauseke.Remove(j - 1, 2).Remove(j, 1);
                                else lauseke.Remove(j, 1).Remove(j + 1, 1);

                                i = 0;
                                break;
                            }

                            bool[] boolMap = new[] {false, true};
                            string operaattori = String.Concat(lauseke[j + 2], lauseke[j + 3]);
                            bool answerBool = false;
                            int operoitava1, operoitava2;

                            var muuttujanIndeksi =
                                Array.FindIndex(muuttujat, muuttuja => muuttuja == lauseke[j + 1]); //tee tästä funktio
                            if (muuttujanIndeksi != -1)
                                operoitava1 = eval[vastausRiveja, muuttujanIndeksi];
                            else operoitava1 = Convert.ToInt16(Char.GetNumericValue(lauseke[j + 1]));

                            muuttujanIndeksi =
                                Array.FindIndex(muuttujat, muuttuja => muuttuja == lauseke[j + 4]); //tee tästä funktio
                            if (muuttujanIndeksi != -1)
                                operoitava2 = eval[vastausRiveja, muuttujanIndeksi];
                            else operoitava2 = Convert.ToInt16(Char.GetNumericValue(lauseke[j + 4]));


                            if (operaattori.Equals("&&")) answerBool = boolMap[operoitava1] && boolMap[operoitava2];
                            else if (operaattori.Equals("||"))
                                answerBool = boolMap[operoitava1] || boolMap[operoitava2];

                            char vastaavaChar;
                            if (answerBool) vastaavaChar = '1';
                            else vastaavaChar = '0';
                            lauseke.Remove(j + 1, 4).Insert(j + 1, vastaavaChar);

                            
                            if (lauseke.Length > 1 && lauseke[0] != '(' || lauseke[lauseke.Length-1] != ')')
                            {
                                for (int k = 1; k < lauseke.Length - 1; k++)
                                    if (lauseke[k] == '(' || lauseke[k] == ')')
                                        break;
                                lauseke.Insert(0, '(').Insert(lauseke.Length - 1, ')');
                            }

                            i = 0;
                            break;
                        }


            Console.WriteLine(lauseke);
            tulokset[vastausRiveja] = Convert.ToInt16(Char.GetNumericValue(lauseke[0]));
            lauseke.Clear().Insert(0, alkLauseke.ToString());
        }

        return tulokset;
    }
}