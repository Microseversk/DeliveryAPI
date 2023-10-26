using DeliveryApi.Context;
using DeliveryApi.Enums;
using DeliveryApi.Helpers;
using DeliveryApi.Migrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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
        int showCount = string.IsNullOrEmpty(query) ? 10 : Int32.MaxValue;


        var childrenDTO = (from AsAdmHierarchy in _context.AsAdmHierarchies
            where AsAdmHierarchy.Parentobjid == parentObjectId
            from AsHouses in _context.AsHouses.Where(h => AsAdmHierarchy.Objectid == h.Objectid && h.Isactual == 1)
                .DefaultIfEmpty()
            from AsAddrObj in _context.AsAddrObjs.Where(a => AsAdmHierarchy.Objectid == a.Objectid && a.Isactual == 1)
                .DefaultIfEmpty()
            where (AsAddrObj != null && (AsAddrObj.Name.Contains(query)) || (AsHouses != null &&
                                                                             AsHouses.Housenum.Contains(query))
                                                                         || string.IsNullOrEmpty(query))
            select new AddressDTO
            {
                ObjectId = AsAddrObj != null ? AsAddrObj.Objectid : AsHouses.Objectid,
                ObjectGuid = AsAddrObj != null ? AsAddrObj.Objectguid : AsHouses.Objectguid,
                Text = AsAddrObj != null ? AsAddrObj.Typename + " " + AsAddrObj.Name : AsHouses.Housenum,
                ObjectLevel =
                    AsAddrObj != null
                        ? ((GarAddressLevel)int.Parse(AsAddrObj.Level)).ToString()
                        : ((HouseType)AsHouses.Housetype).ToString(),
                ObjectLevelText = AsAddrObj != null
                    ? EnumHelper.GetDescription((GarAddressLevel)int.Parse(AsAddrObj.Level))
                    : EnumHelper.GetDescription((HouseType)AsHouses.Housetype)
            }).Take(showCount).ToList();

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
                    ObjectLevel = ((GarAddressLevel)int.Parse(objInfo.Level)).ToString(),
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
                    ObjectLevel = ((HouseType)buildingInfo.Housetype).ToString(),
                    ObjectLevelText = EnumHelper.GetDescription((HouseType)buildingInfo.Housetype)
                });
            }
        }

        parentListDTO.Reverse();
        return parentListDTO;
    }
}