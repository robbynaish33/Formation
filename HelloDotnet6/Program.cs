// See https://aka.ms/new-console-template for more information
using HelloDotnet6;

Console.WriteLine("Hello, World!");

Personne p1 = new Personne();
var p2 = new Personne();
Personne p3 = new();
Personne p4 = new(111);
Personne p5 = new() { Id = 222 };

Console.WriteLine(p1.Prenom?.ToUpper());

Console.WriteLine(p1.Prenom?.ToUpper() ?? "inconnu");

Console.WriteLine(p1.Nom.ToUpper());

p1.Prenom = null;

DoSomething(p1);

Console.WriteLine(p1.Prenom!.ToUpper());



void DoSomething(Personne p1)
{
    p1.Prenom = "toto";
}

Voiture v1 = new Voiture("aa-111-ee", "bmw", "e46", 500);
Voiture v2 = new Voiture("aa-111-ee", "bmw", "e46", 500);
Voiture v3 = new Voiture("bb-222-22", v1.Marque, v1.Modele, v1.Prix);
Voiture v4 = v1 with { Immatriculation = "new immat" };
Voiture v5 = v1; //copie reference
Voiture v6 = v1 with { }; //clone

int temperature = 12;
//if (temperature < -20 || temperature >50)
if (temperature is < -20 or > 50)
{
    Console.WriteLine("température incohérente");
}

string info = temperature switch
{
    < 0 => "froid",
    0 => "gele",
    > 0 and < 5 => "frais",
    < 20 and not 12 => "ok",
    _ => "chaud"
};


Console.WriteLine("Immatriculation " + v1.Immatriculation);
Console.WriteLine("Marque " + v1.Marque);
Console.WriteLine("Modele " + v1.Modele);
Console.WriteLine(v1);

Console.WriteLine("v3 : " + v3);