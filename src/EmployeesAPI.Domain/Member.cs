namespace EmployeesAPI.Domain
{
    public class Member
    {
        public string Key { get; }
        public string Name { get; }
        public string Surname { get; }
        public int Age { get; }
        public string OrganizationKey { get; } //Id или Key?

        public Member(string key, string name, string surname, int age, string organizationKey)
        {
            Validator(name, organizationKey);
            
            Key = key;
            Name = name;
            Surname = surname;
            Age = age;
            OrganizationKey = organizationKey;
        }

        public Member(string name, string surname, int age, string organizationKey)
        {
            Validator(name, organizationKey);
            
            Name = name;
            Surname = surname;
            Age = age;
            OrganizationKey = organizationKey;
            Key = Guid.NewGuid().ToString();
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