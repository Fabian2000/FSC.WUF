namespace FSC.WUF.TEST
{
    public class People
    {
        public List<Person> GetPeople { get; set; } = new List<Person>();

        public People()
        {
            var person1 = new Person
            {
                Id = 0,
                FirstName = "Jack🤔",
                LastName = "Exampleman",
                Age = 30,
            };

            var person2 = new Person
            {
                Id = 1,
                FirstName = "Timmy",
                LastName = "Lalala",
                Age = 16,
            };

            var person3 = new Person
            {
                Id = 2,
                FirstName = "Cindy",
                LastName = "Stone",
                Age = 25,
            };

            GetPeople.Add(person1);
            GetPeople.Add(person2);
            GetPeople.Add(person3);
        }
    }
}
