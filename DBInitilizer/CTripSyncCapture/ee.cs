using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using www.opentravel.org.OTA.Item2003.Item05;

namespace real
{

    //class Program
    //{

    //    static void Main(string[] args)
    //    {
    //        string kk = "976c0c32-0e3b-4f1e-866d-f425e8b5c450".ToUpper();
    //        var root = XRoot.Load("real.xml");
    //        var hotels = root.Response[0].HotelResponse[0].OTA_HotelDescriptiveInfoRS.HotelDescriptiveContents[0].HotelDescriptiveContent;

    //        foreach (var item in hotels)
    //        {
    //            string brandCode = item.BrandCode;

    //            string hotelCode = item.HotelCode;

    //            string hotelCityCode = item.HotelCityCode;

    //            string hotelName = item.HotelName;

    //            string AreaID = item.AreaID;

    //            var hotelinfo = item.HotelInfo[0];

    //            string whenBuilt = hotelinfo.WhenBuilt;

    //            string lastUpdated = hotelinfo.LastUpdated;


    //            //var SEG = hotelinfo.CategoryCodes;
    //            //InsertSEG(hotelCode, SEG);


    //            //var Position = hotelinfo.Position;
    //            //InsertPosition(hotelCode, Position);


    //            //var Address = hotelinfo.Address;
    //            //InsertAddress(hotelCode, Address);


    //            //var Services = hotelinfo.Services;
    //            //InsertServices(hotelCode, Services);



    //            //var facility = item.FacilityInfo[0];



    //            //var GuestRooms = facility.GuestRooms;

    //            //InsertGuestRoom(hotelCode, GuestRooms);



    //            //var policies = item.Policies;

    //            //InsertPolicies(hotelCode, policies);



    //            //var areas = item.AreaInfo;

    //            //InsertAreaInfo(hotelCode, areas);



    //            //var affiliation = item.AffiliationInfo;

    //            //InsertAffiliation(hotelCode, affiliation);



    //            //var multimediadescriptions = item.MultimediaDescriptions;

    //            //InsertMultimediaDescription(hotelCode,multimediadescriptions);





    //            InsertHotelTapExtension(hotelCode, item);

    //        }



    //        Console.Read();

    //    }

    //    public static void InsertSEG(string hotelCode, IList<OTA_HotelDescriptiveInfoRS.HotelDescriptiveContentsLocalType.HotelDescriptiveContentLocalType.HotelInfoLocalType.CategoryCodesLocalType> SEG)
    //    {

    //        if (SEG != null)
    //        {

    //            foreach (var item in SEG)
    //            {

    //                Console.WriteLine("insert into SEG " + hotelCode + "," + item.SegmentCategory[0].Code);

    //            }

    //        }

    //    }



    //    public static void InsertPosition(string hoteCode, IList<OTA_HotelDescriptiveInfoRS.HotelDescriptiveContentsLocalType.HotelDescriptiveContentLocalType.HotelInfoLocalType.PositionLocalType> Position)
    //    {

    //        if (Position != null)
    //        {

    //            foreach (var item in Position)
    //            {

    //                string Latitude = item.Latitude;

    //                string Longitude = item.Longitude;

    //                string PositionTypeCode = item.PositionTypeCode;



    //                Console.WriteLine("Insert into Postion" + hoteCode + "," + Latitude);

    //            }

    //        }

    //    }



    //    public static void InsertAddress(string hoteCode, IList<OTA_HotelDescriptiveInfoRS.HotelDescriptiveContentsLocalType.HotelDescriptiveContentLocalType.HotelInfoLocalType.AddressLocalType> Address)
    //    {

    //        if (Address != null)
    //        {

    //            foreach (var item in Address)
    //            {

    //                string addressLine = item.AddressLine;

    //                string cityName = item.CityName;

    //                string postalCode = item.PostalCode;



    //                Console.WriteLine("insert Address " + addressLine);



    //                var zone = item.Zone;

    //                if (zone != null)
    //                {

    //                    foreach (var z in zone)
    //                    {

    //                        Console.WriteLine("insert zone " + z.ZoneCode + "," + z.ZoneName);

    //                    }

    //                }



    //                var extension = item.TPA_Extensions;

    //                if (extension != null && extension.Count > 0)
    //                {

    //                    foreach (var e in extension)
    //                    {

    //                        Console.WriteLine("AddressExternsion :" + e.RoadCross[0].DescriptionText);

    //                    }

    //                }



    //            }

    //        }

    //    }



    //    public static void InsertServices(string hotelCode, IList<OTA_HotelDescriptiveInfoRS.HotelDescriptiveContentsLocalType.HotelDescriptiveContentLocalType.HotelInfoLocalType.ServicesLocalType> Services)
    //    {



    //        foreach (var item in Services[0].Service)
    //        {

    //            string HACID = item.Code;

    //            string HATID = item.ID;

    //            string descriptiveText = item.DescriptiveText;



    //            Console.WriteLine("Insert into Service" + HACID + "," + HATID);

    //        }

    //    }



    //    public static void InsertGuestRoom(string hotelCode, IList<OTA_HotelDescriptiveInfoRS.HotelDescriptiveContentsLocalType.HotelDescriptiveContentLocalType.FacilityInfoLocalType.GuestRoomsLocalType> GuestRooms)
    //    {

    //        if (GuestRooms != null && GuestRooms.Count > 0)
    //        {

    //            foreach (var item in GuestRooms[0].GuestRoom)
    //            {

    //                string roomName = item.RoomTypeName;



    //                var typeName = item.TypeRoom;

    //                if (Check(typeName))
    //                {

    //                    var typeNameNode = typeName[0];

    //                    string StandardOccupancy = typeNameNode.StandardOccupancy;

    //                    string Size = typeNameNode.Size;

    //                    string Name = typeNameNode.Name;

    //                    string RoomTypeCode = typeNameNode.RoomTypeCode;

    //                    string Floor = typeNameNode.Floor;

    //                    string InvBlockCode = typeNameNode.InvBlockCode;

    //                    string BedTypeCode = typeNameNode.BedTypeCode;

    //                    string NonSmoking = typeNameNode.NonSmoking;

    //                    string HasWindow = typeNameNode.HasWindow;

    //                    string Quantity = typeNameNode.Quantity;

    //                    string RoomSize = typeNameNode.RoomSize;



    //                    Console.WriteLine("Guest Room" + Name);

    //                }



    //                var Amenities = item.Amenities;

    //                if (Check(Amenities))
    //                {

    //                    foreach (var amentity in Amenities[0].Amenity)
    //                    {

    //                        string RoomAmenityCode = amentity.RoomAmenityCode;

    //                        string DescriptiveText = amentity.DescriptiveText;



    //                        Console.WriteLine("Guest Amentity" + RoomAmenityCode + "," + DescriptiveText);

    //                    }

    //                }



    //                var TPA_Extensions = item.TPA_Extensions;

    //                if (Check(TPA_Extensions))
    //                {

    //                    foreach (var extension in TPA_Extensions[0].TPA_Extension)
    //                    {

    //                        string FacilityName = extension.FacilityName;

    //                        string FTypeName = extension.FTypeName;

    //                        string FacilityValue = extension.FacilityValue;



    //                        Console.WriteLine("Room Extensions " + FacilityName);

    //                    }

    //                }







    //            }

    //        }

    //    }



    //    public static bool Check(IEnumerable<object> o)
    //    {

    //        if (o != null && o.Count() > 0)
    //        {

    //            return true;

    //        }



    //        return false;

    //    }



    //    public static void InsertPolicies(string hotelCode, IList<OTA_HotelDescriptiveInfoRS.HotelDescriptiveContentsLocalType.HotelDescriptiveContentLocalType.PoliciesLocalType> policies)
    //    {

    //        if (Check(policies))
    //        {

    //            var policy = policies[0].Policy;

    //            InsertPolicy(hotelCode, policy);

    //        }

    //    }



    //    public static void InsertPolicy(string hotelCode, IList<OTA_HotelDescriptiveInfoRS.HotelDescriptiveContentsLocalType.HotelDescriptiveContentLocalType.PoliciesLocalType.PolicyLocalType> policy)
    //    {

    //        if (Check(policy))
    //        {

    //            var policyinfoCodes = policy[0].PolicyInfoCodes;



    //            if (Check(policyinfoCodes))
    //            {

    //                var PolicyInfoCode = policyinfoCodes[0].PolicyInfoCode;

    //                foreach (var item in PolicyInfoCode[0].Description)
    //                {

    //                    Console.WriteLine(item.Name + "," + item.Text);

    //                }

    //            }



    //            var policyinfo = policy[0].PolicyInfo;

    //            var CheckinTime = policyinfo[0].CheckInTime;

    //            var checkOutTime = policyinfo[0].CheckOutTime;



    //        }

    //    }



    //    public static void InsertAreaInfo(string hotelCode, IList<OTA_HotelDescriptiveInfoRS.HotelDescriptiveContentsLocalType.HotelDescriptiveContentLocalType.AreaInfoLocalType> areas)
    //    {

    //        if (Check(areas))
    //        {

    //            var refPoints = areas[0].RefPoints;



    //            foreach (var item in refPoints[0].RefPoint)
    //            {

    //                var Distance = item.Distance;

    //                var UnitOfMeasureCode = item.UnitOfMeasureCode;

    //                var Name = item.Name;

    //                var Latitude = item.Latitude;

    //                var Longitude = item.Longitude;

    //                var RefPointCategoryCode = item.RefPointCategoryCode;

    //                var RefPointName = item.RefPointName;

    //                var DescriptiveText = item.DescriptiveText;



    //                Console.WriteLine(Name + "," + DescriptiveText);

    //            }

    //        }

    //    }



    //    public static void InsertAffiliation(string hotelCode, IList<OTA_HotelDescriptiveInfoRS.HotelDescriptiveContentsLocalType.HotelDescriptiveContentLocalType.AffiliationInfoLocalType> affiliation)
    //    {

    //        if (Check(affiliation))
    //        {

    //            foreach (var item in affiliation[0].Awards[0].Award)
    //            {

    //                Console.WriteLine(item.Provider + "," + item.Rating);

    //            }

    //        }

    //    }



    //    public static void InsertMultimediaDescription(string hotelCode, IList<OTA_HotelDescriptiveInfoRS.HotelDescriptiveContentsLocalType.HotelDescriptiveContentLocalType.MultimediaDescriptionsLocalType> multimediadescriptions)
    //    {

    //        if (Check(multimediadescriptions))
    //        {

    //            foreach (var item in multimediadescriptions[0].MultimediaDescription)
    //            {

    //                var imageItems = item.ImageItems;



    //                if (Check(imageItems))
    //                {

    //                    foreach (var image in imageItems[0].ImageItem)
    //                    {

    //                        var url = image.ImageFormat[0].URL;

    //                        var caption = image.Description[0].Caption;

    //                        var category = image.Category;



    //                        var tap_extension = image.TPA_Extensions;

    //                        if (Check(tap_extension))
    //                        {

    //                            var invokeCode = tap_extension[0].InvBlockCode;

    //                        }



    //                        Console.WriteLine(url + "," + caption);

    //                    }

    //                }



    //                var textitems = item.TextItems;



    //                if (Check(textitems))
    //                {

    //                    foreach (var text in textitems[0].TextItem)
    //                    {

    //                        var category = text.Category;

    //                        var Description = text.Description;



    //                        Console.WriteLine(category + "," + Description);

    //                    }

    //                }

    //            }

    //        }

    //    }



    //    public static void InsertHotelTapExtension(string hotelCode, OTA_HotelDescriptiveInfoRS.HotelDescriptiveContentsLocalType.HotelDescriptiveContentLocalType content)
    //    {

    //        if (Check(content.TPA_Extensions))
    //        {

    //            foreach (var item in content.TPA_Extensions)
    //            {

    //                var CityImportantMessageType = item.CityImportantMessage[0].CityImportantMessageType[0];



    //                var StartDate = CityImportantMessageType.StartDate;

    //                var EndDate = CityImportantMessageType.EndDate;

    //                var MessageContent = CityImportantMessageType.MessageContent;



    //                var Roomquantity = item.Roomquantity;



    //                Console.WriteLine(Roomquantity + "," + MessageContent);



    //            }

    //        }

    //    }

    //}

}

