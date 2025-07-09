namespace repo_service.Helpers
{
    public static class Messages
    {
        // Employee messages
        public const string EmployeeCreated = "Employee created successfully.";
        public const string EmployeeRetrieved = "Employee retrieved successfully.";
        public const string EmployeesRetrieved = "Employees retrieved successfully.";
        public const string EmployeeUpdated = "Employee updated successfully.";
        public const string EmployeesBatchAdded = "All employees added in one transaction.";
        public const string EmployeeSearchResults = "Search results retrieved.";

        // Department messages
        public const string DepartmentsRetrieved = "Departments retrieved successfully.";

        // Validation / Error
        public const string InvalidDepartmentId = "Department with the given ID does not exist.";
        public const string InvalidRequest = "Invalid request. Please check the submitted data.";
        public const string MissingSearchParameter = "At least one search parameter (name or email) is required.";
        public const string ErrorRetrievingData = "Error retrieving data from the database.";
        public const string ErrorCreatingEmployee = "Error creating new employee record.";
        public const string ErrorUpdatingEmployee = "Error updating employee record.";
        public const string ErrorBatchInsert = "Error in batch insert.";
    }
}

