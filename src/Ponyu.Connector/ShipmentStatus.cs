using System.Runtime.Serialization;

namespace Ponyu.Connector
{
    public enum ShipmentStatus
    {
        [EnumMember(Value = "REQUESTED")]
        Requested,
        [EnumMember(Value = "ASSIGNED")]
        Assigned,
        [EnumMember(Value = "AT_PICKUP_SITE")]
        AtPickupSite,
        [EnumMember(Value = "PROGRESS")]
        Progress,
        [EnumMember(Value = "AT_DELIVERY_SITE")]
        AtDeliverySite,
        [EnumMember(Value = "COMPLETED")]
        Completed,
        [EnumMember(Value = "CANCELLED")]
        Cancelled,
        [EnumMember(Value = "WAITING")]
        Waiting,
        [EnumMember(Value = "TO_RETURN")]
        ToReturn,
        [EnumMember(Value = "AT_RETURN_SITE")]
        AtReturnSite,
        [EnumMember(Value = "RETURNED")]
        Returned,
    }
}
