
using Newtonsoft.Json;
using System;
using System.Web.Http;

namespace DemoServices.Controllers
{
    public class Rock
    {
        public string Name { get; set; }
        public string Color { get; set; }
        public int Weight { get; set; }
    }

    public class RockController : ApiController
    {
        // GET api/Rock/
        public Rock Get()
        {
            Rock r = new Rock() { Name = "Stoic", Color = "gray", Weight = 5 };
            return r;
        }

        // GET api/Pet/{id}
        /*
        public string Get(int id)
        {
            Dog d = new Dog() { Name = "Fido", PetType = "Dog", PackSize = 11 };
            return JsonConvert.SerializeObject(d);
        }
        */
    }
}