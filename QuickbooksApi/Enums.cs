namespace QuickbooksWeb
{
    public enum ActiveStatus
    {
        False = 0,
        True = 1
    }

    public enum Operations
    {
        Create,
        Update,
        Delete,
        Merge,
        Void
    }

    public enum EntityType
    {
        Account,
        Company,
        Customer,
        Item,
        Invoice,
        Payment,
        Vendor,
        Employee
    }
}