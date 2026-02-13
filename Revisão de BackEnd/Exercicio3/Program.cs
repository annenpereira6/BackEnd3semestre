using System;
using Exercicios01;

namespace Exercicios01
{
    class Program
    {
        static void Main(string[] args)
        {
            Pessoa pessoa1 = new Pessoa();

            pessoa1.Nome = "Anne";
            pessoa1.Idade = 17; 

            Console.WriteLine("Nome: " + pessoa1.Nome);
            Console.WriteLine("Idade: " + pessoa1.Idade);

            
            pessoa1.Idade = -5; 
        }
    }
}
