using DeliveryApi.Migrations;

namespace DeliveryApi.Services.AddressService;

public interface IAddressService
{
    public Task<List<AddressDTO>> GetObjectChildren(int parentObjectId, string? query);
    public Task<List<AddressDTO>> GetAddressChain(long? objectId);
}