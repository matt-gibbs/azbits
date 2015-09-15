
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace DemoServices.Controllers
{
    public class PetController : ApiController
    {
        // GET api/Pet/{id}
        /*
        public string Get(int id)
        {
            Dog d = new Dog() { Name = "Fido", PetType = "Dog", PackSize = 11 };
            return JsonConvert.SerializeObject(d);
        }
        */

        // GET api/Pet/{id}
        public Dog Get(int id)
        {
            Dog d = new Dog() { Name = "Fido", PackSize = id };
            return d;
        }

        public IList<Cat> Get()
        {
            Cat a = new Cat() { Name = "A", Breed = "Siamese" };
            Cat b = new Cat() { Name = "B", Breed = "Persian" };
            List<Cat> cats = new List<Cat>();
            cats.Add(a);
            cats.Add(b);
            return cats;
        }
    }

    public class Pet
    {
        public string Name { get; set; }
        public string PetType { get; set; }
    }

    public class Dog : Pet
    {
        public Dog()
        {
            PetType = "Dog";
        }
        public int PackSize { get; set; }
    }

    public class Cat : Pet
    {
        public Cat()
        {
            PetType = "Cat";
        }
        public string Breed { get; set; }
    }
}