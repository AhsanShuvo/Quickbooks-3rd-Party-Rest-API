using System.Configuration;

namespace QuickbooksApi.Repository
{
    public class BaseRepository
    {
       protected string connectionString  = ConfigurationManager.ConnectionStrings["QuickbooksDB"].ConnectionString;
    }
}