﻿// w metodzie Rate klasy Object zakladam ze beda tylko 2 parametry

//strategia gdzie blizej tam biore do dekodowania

using System.Text;
using System.Collections.Generic;
using Microsoft.VisualBasic;

class Object : ICloneable
{

    public string Parameter { get; set; }
    public double Rating { get; set; }
    

    public Object(int LBnP)
    {
        var random = new Random();
        StringBuilder sb = new(LBnP);
        for (int i = 0; i < LBnP; i++)
        {
            sb.Append(random.Next(2));
        }
        Parameter = sb.ToString();
    }

    public Object(string parameter)
    {
        Parameter = new string(parameter);
    }

    public Object(Object item)
    {
        Parameter = item.Parameter;
        Rating = item.Rating;
    }


    public object Clone()
    {
        return new Object(3)
        {
            Parameter = new string(this.Parameter),
            Rating = this.Rating
        };
    }

    public void Rate(Dictionary<string, double>bytesStringsValues, int LBnP)
    {
        // ocenianie, tutaj zakladam ze beda tylko 2 parametry
        string part1 = Parameter.Substring(0, LBnP);
        string part2 = Parameter.Substring(LBnP, LBnP);
        double x1 = bytesStringsValues[part1];
        double x2 =  bytesStringsValues[part2];
        Rating = Math.Sin(x1*0.05)+Math.Sin(x2*0.05)+0.4*Math.Sin(x1*0.15)*Math.Sin(x2*0.15);
    }

    public void Mutate()
    {
        var random = new Random();
        //mutacja
        int randomIndex = random.Next(Parameter.Length);
        char[] charList = Parameter.ToCharArray();
        if (charList[randomIndex] == '0')
            charList[randomIndex] = '1';
        else 
            charList[randomIndex] = '0';
        Parameter = new string(charList);
    }
}

class Program
{
    static int LBnP = 4;
    static int numberOfParameters = 3;
    static int zdmin = 0;
    static int zdmax = 100;
    static int numberOfObjects = 13;
    static float tournamentSize = 0.2f;

    static List<string> GenerateBytesStrings(int n)
    {
        int total = (int)Math.Pow(2, n);
        List<string> bytesStrings = [];

        for (int i = 0; i < total; i++)
        {
            bytesStrings.Add(Convert.ToString(i, 2).PadLeft(n, '0'));
        }

        return bytesStrings;
    }

    static Dictionary<string, double> GenerateValues(List<string> bytesStrings, int zdmax, int zdmin)
    {
        int range = zdmax - zdmin;
        double step = (double)range / (bytesStrings.Count-1);
        Dictionary<string, double> mapping = [];
        for (int i = 0; i < bytesStrings.Count ; i++)
        {
            mapping[bytesStrings[i]] = (double)zdmin + step*i;
        }
        mapping[bytesStrings[bytesStrings.Count-1]] = zdmax; 
        return mapping;
    }

    public static List<Object> CreateObjects()
    {
        List<Object> objects = [];
        for (int i=0; i<numberOfObjects; i++)
        {
            Object specimen = new(LBnP * numberOfParameters);
            objects.Add(specimen);
        }
        return objects;
    }

    public static Object TournamentSelection(List<Object> objects)
    {
        int numberOfSelectedObjects = (int)(numberOfObjects * tournamentSize);
        List<Object> selectedObjects = [];
        Random random = new();
        for (int i=0; i<numberOfSelectedObjects; i++)
        {
            selectedObjects.Add(objects[random.Next(objects.Count)]);
        }
        double bestRating = selectedObjects[0].Rating;
        Object bestObject = selectedObjects[0];
        foreach (Object item in selectedObjects)
        {
            if (item.Rating > bestRating)
            {
                bestRating = item.Rating;
                bestObject = item;
            }
        } 

        return new Object(bestObject);
    }

    public static Object Crossover(Object parent1, Object parent2)
    {
        Random rand = new Random();
        int crossoverPoint = rand.Next(LBnP*3 -2 );

        string child = parent1.Parameter.Substring(0, crossoverPoint) + parent2.Parameter.Substring(crossoverPoint);
        Console.WriteLine(parent1.Parameter);
        Console.WriteLine(parent2.Parameter);
        Console.WriteLine(child);
        return new Object(child);
    }

    public static List<Object> Cross(List<Object> objects)
    {
        List<Object> crossed_list = new();
        Object specimen1 = objects[0];
        Object specimen2 = objects[1];
        Object specimen3 = objects[2];
        Object specimen4 = objects[3];
        Object specimen9 = objects[8];
        Object specimen10 = objects[9];
        Object firstFromLast = objects[objects.Count - 2];
        Object last = objects[objects.Count -1];

        crossed_list.Add(Crossover(specimen1, specimen2));
        crossed_list.Add(Crossover(specimen3, specimen4));
        crossed_list.Add(Crossover(specimen9, specimen10));
        crossed_list.Add(Crossover(firstFromLast, last));
        return crossed_list;
    }

    public static Object HotDeckSelection(List<Object> objects)
    {
        double bestRating = objects[0].Rating;
        Object bestObject = objects[0];
        foreach (Object item in objects)
        {
            if (item.Rating > bestRating)
            {
                bestRating = item.Rating;
                bestObject = item;
            }
        } 

        return new Object(bestObject);
    }

    public static double GetMeanObjectValue(List<Object> objects)
    {
        double total = 0;
        foreach (Object item in objects)
        {
            total += item.Rating;
        }
        return total/objects.Count;
    }

    static void Main()
    {
        // Console.Write("Podaj liczbe bitow na parametr: ");
        // Program.LBnP = int.Parse(Console.ReadLine());


        List<string> bytesStrings = GenerateBytesStrings(LBnP);
        var bytesStringsValues = GenerateValues(bytesStrings, zdmax, zdmin);

        List<Object> objects = CreateObjects();
        List<Object> selectedObjects = [];
        selectedObjects = Cross(objects);
        // foreach (Object item in objects)
        //     {
        //         item.Rate(bytesStringsValues, LBnP);
        //     }
        // for (int j=0; j<20;j++)
        // {   
        //     for (int i=0; i < numberOfObjects-1;i++)
        //     {
        //         selectedObjects.Add(TournamentSelection(objects));
        //     }
        //     foreach (Object item in selectedObjects)
        //     {
        //         item.Mutate();
        //     }
        //     selectedObjects.Add(HotDeckSelection(objects));
        //     foreach (Object item in selectedObjects)
        //     {
        //         item.Rate(bytesStringsValues, LBnP);
        //     }
        //     Console.WriteLine($"Iteracja {j}, najlepszy {HotDeckSelection(selectedObjects).Rating}, srednia {GetMeanObjectValue(selectedObjects)}");
        //     objects.Clear();
        //     selectedObjects.ForEach((item)=>
        //     {
        //         objects.Add(new Object(item));
        //     });
        //     selectedObjects.Clear();
        // }
        


    
    }
}