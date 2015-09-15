
using System;

namespace HelloApp
{
    class Program
    {
        static void Main(string[] args)
        {
        	Console.WriteLine("hi");
            Console.ReadKey();
        }
    }
}






















/*
// Hello demo
using System;
using Demo;

namespace HelloApp
{
    class Program
    {
        static void Main(string[] args)
        {
            HelloClient client = new HelloClient();
            string result = client.Greeting();
            Console.WriteLine(result);
            Console.ReadKey();
        }
    }
}
*/

/*
// Rock demo
using System;
using Demo;
using Demo.Models;

namespace HelloApp
{
    class Program
    {
        static void Main(string[] args)
        {
            RockClient client = new RockClient();
            Rock rock = client.ExampleRock();
            Console.Write($"Rock {rock.Name} is {rock.Color} and {rock.Weight}");
            Console.ReadKey();
        }
    }
}
*/


/*
// Pet demo
using System;
using Demo;
using Demo.Models;

namespace HelloApp
{
	class Program
	{
		static void Main(string[] args) 
		{
			PetClient petClient = new PetClient();
			Pet pet = petClient.FindPetById(2);
			Dog dog = pet as Dog;
			if(dog != null)
			{
			    Console.WriteLine($"Dog {dog.Name} PackSize {dog.PackSize}");
			}
		}
	}
}

*/
