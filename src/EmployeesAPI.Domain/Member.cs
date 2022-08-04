namespace EmployeesAPI.Domain
{
    public class Member
    {
        public string Key { get; }
        public string Name { get; }
        public string Surname { get; }
        public int Age { get; }
        public string OrganizationKey { get; }

        public Member(string key, string name, string surname, int age, string organizationKey)
        {
            Validator(name, organizationKey);

            Key = key;
            Name = name;
            Surname = surname;
            Age = age;
            OrganizationKey = organizationKey;
        }

        public Member(string name, string surname, int age, string organizationKey) :
            this(Guid.NewGuid().ToString(), name, surname, age, organizationKey)
        {
        }

        private void Validator(string name, string key)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("Имя сотрудника не валидно");

            if (!Guid.TryParse(key, out _))
                throw new ArgumentNullException("Id организации сотрудника не валиден");
        }
    }
}