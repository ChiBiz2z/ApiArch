﻿namespace EmployeesAPI.DAL.Interfaces
{
    public interface IEmployeeMongoDbSettings
    {
        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }

        public string OrganizationCollectionName { get; set; }

        public string MembersCollectionName { get; set; }
        public string UsersCollectionName { get; set; }
        public string VerificationCodesCollectionName { get; set; }

    }
}
