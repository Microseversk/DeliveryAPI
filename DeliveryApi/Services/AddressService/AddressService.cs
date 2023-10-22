using DeliveryApi.Context;
using DeliveryApi.Enums;
using DeliveryApi.Helpers;
using DeliveryApi.Migrations;
using Microsoft.EntityFrameworkCore;

namespace DeliveryApi.Services.AddressService;

public class AddressService : IAddressService
{
    private readonly AddressContext _context;

    public AddressService(AddressContext context)
    {
        _context = context;
    }

    public async Task<List<AddressDTO>> GetObjectChildren(int parentObjectId, string? query)
    {
        var childrenDTO = (from AsAdmHierarchy in _context.AsAdmHierarchies
            join AsAddrObj in _context.AsAddrObjs on AsAdmHierarchy.Objectid equals AsAddrObj.Objectid
            where AsAdmHierarchy.Parentobjid == parentObjectId &&
                  (AsAddrObj.Name.Contains(query) || string.IsNullOrEmpty(query))
            select new AddressDTO
            {
                ObjectId = AsAddrObj.Objectid,
                ObjectGuid = AsAddrObj.Objectguid,
                Text = AsAddrObj.Typename + " " + AsAddrObj.Name,
                ObjectLevel = (GarAddressLevel)int.Parse(AsAddrObj.Level),
                ObjectLevelText = EnumHelper.GetDescription((GarAddressLevel)int.Parse(AsAddrObj.Level))
            }).Take(10).ToList();

        if (childrenDTO.Count == 0)
        {
            childrenDTO = (from AsAdmHierarchy in _context.AsAdmHierarchies
                join AsHouses in _context.AsHouses on AsAdmHierarchy.Objectid equals AsHouses.Objectid
                where AsAdmHierarchy.Parentobjid == parentObjectId &&
                      (AsHouses.Housenum.Contains(query) || string.IsNullOrEmpty(query))
                select new AddressDTO
                {
                    ObjectId = AsHouses.Objectid,
                    ObjectGuid = AsHouses.Objectguid,
                    Text = AsHouses.Housenum,
                    ObjectLevel = GarAddressLevel.Building,
                    ObjectLevelText = EnumHelper.GetDescription(GarAddressLevel.Building)
                }).Take(10).ToList();
        }

        return childrenDTO;
    }

    public async Task<List<AddressDTO>> GetAddressChain(long? objectId)
    {
        var parentListDTO = new List<AddressDTO>();
        var parentList = new List<AsAdmHierarchy>();
        var currentObjId = objectId;
        var parent = await _context.AsAdmHierarchies.FirstOrDefaultAsync(o => o.Objectid == currentObjId);

        while (parent != null)
        {
            parentList.Add(parent);
            currentObjId = parent.Parentobjid;
            parent = await _context.AsAdmHierarchies.FirstOrDefaultAsync(o => o.Objectid == currentObjId);
        }

        foreach (var obj in parentList)
        {
            var objInfo = await _context.AsAddrObjs.FirstOrDefaultAsync(c => c.Objectid == obj.Objectid);
            if (objInfo != null)
            {
                parentListDTO.Add(new AddressDTO
                {
                    ObjectId = objInfo.Objectid,
                    ObjectGuid = objInfo.Objectguid,
                    Text = objInfo.Typename + " " + objInfo.Name,
                    ObjectLevel = (GarAddressLevel)int.Parse(objInfo.Level),
                    ObjectLevelText = EnumHelper.GetDescription((GarAddressLevel)int.Parse(objInfo.Level))
                });
            }
            else
            {
                var buildingInfo = await _context.AsHouses.FirstOrDefaultAsync(b => b.Objectid == obj.Objectid);
                parentListDTO.Add(new AddressDTO
                {
                    ObjectId = buildingInfo.Objectid,
                    ObjectGuid = buildingInfo.Objectguid,
                    Text = buildingInfo.Housenum,
                    ObjectLevel = GarAddressLevel.Building,
                    ObjectLevelText = EnumHelper.GetDescription(GarAddressLevel.Building)
                });
            }
        }

        parentListDTO.Reverse();
        return parentListDTO;
    }
}