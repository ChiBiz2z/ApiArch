namespace EmployeesAPI.Domain
{
    public class Organization
    {
        public string Key { get; }
        public string Name { get; }

        public Organization(string key, string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("Имя организации не валидно");
            }

            if (!Guid.TryParse(key, out var _))
            {
                throw new ArgumentNullException("Ключ организации не валидно");
            }
            Key = key;
            Name = name;
        }

        public Organization(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("Имя организации не валидно");
            }
            Key = Guid.NewGuid().ToString();
            Name = name;
        }
    }
}