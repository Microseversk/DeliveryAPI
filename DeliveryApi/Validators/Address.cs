using DeliveryApi.Context;
using Microsoft.EntityFrameworkCore;

namespace DeliveryApi.Validators;

public static class Address
{
    public async static Task<bool> Isvalid(AddressContext context, Guid? addressId)
    {
        var addressValid = (await context.AsAddrObjs.FirstOrDefaultAsync(a => a.Objectguid == addressId) != null) ||
                           (await context.AsHouses.FirstOrDefaultAsync(h => h.Objectguid == addressId) != null);
        return addressValid;
    }
}