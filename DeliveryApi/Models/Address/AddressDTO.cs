using DeliveryApi.Enums;

namespace DeliveryApi.Migrations;

public class AddressDTO
{
    public long ObjectId { get; set; } 
    public Guid ObjectGuid { get; set;}
    public string Text { get; set; }
    public string ObjectLevel { get; set; }
    public string ObjectLevelText { get; set; }
}