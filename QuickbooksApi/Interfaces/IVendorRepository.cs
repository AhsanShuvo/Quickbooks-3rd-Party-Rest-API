using QuickbooksApi.Models;

namespace QuickbooksApi.Interfaces
{
    public interface IVendorRepository
    {
        void SaveVendorInfo(VendorInfo model);
        void DeleteVendor(string id);
    }
}
