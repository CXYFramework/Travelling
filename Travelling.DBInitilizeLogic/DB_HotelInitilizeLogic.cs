using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using www.opentravel.org.OTA.Item2003.Item05;
using Model;
using DAL;
using System.Transactions;
using Travelling.Unitity;
using System.Data.SqlClient;

namespace Travelling.DBInitilizeLogic
{
    public class DB_HotelInitilizeLogic
    {
        public static void ProcessHotel()
        {

            //OTAHotelServiceLogic hotelService = new OTAHotelServiceLogic();
            //string hotelXml = hotelService.GetHotelByAreaId(1);

            //File.AppendAllText("D:\\ttt.xml", hotelXml);
            string hotelXml = System.IO.File.ReadAllText("real.xml");

            var root = XRoot.Parse(hotelXml);




            var hotels = root.Response.HotelResponse[0].OTA_HotelDescriptiveInfoRS.HotelDescriptiveContents[0].HotelDescriptiveContent;

            foreach (var item in hotels)
            {
                TransactionOptions transactionOption = new TransactionOptions();
                transactionOption.Timeout = new TimeSpan(0, 0, 600000);

                using (TransactionScope tran = new TransactionScope(TransactionScopeOption.Required, transactionOption))
                {
                    var hotelinfo = item.HotelInfo[0];


                    var hotelCode = item.HotelCode;
                    var hotelId = InsertHotel(item);

                    var SEG = hotelinfo.CategoryCodes;
                    InsertSEG(hotelId, hotelCode, SEG);

                    var Position = hotelinfo.Position;
                    InsertPosition(hotelId, hotelCode, Position);

                    var Address = hotelinfo.Address;
                    InsertAddress(hotelId, hotelCode, Address);

                    var Services = hotelinfo.Services;
                    InsertServices(hotelId, Services);

                    var facility = item.FacilityInfo[0];

                    var GuestRooms = facility.GuestRooms;
                    InsertGuestRoom(hotelId, GuestRooms);

                    var policies = item.Policies;
                    InsertPolicies(hotelId, policies);

                    var areas = item.AreaInfo;
                    InsertAreaInfo(hotelId, areas);

                    var affiliation = item.AffiliationInfo;
                    InsertAffiliation(hotelId, affiliation);

                    var multimediadescriptions = item.MultimediaDescriptions;
                    InsertMultimediaDescription(hotelId, multimediadescriptions);


                    // InsertHotelTapExtension(hotelCode, item);

                    tran.Complete();
                }

            }
            Console.WriteLine("Done");
            Console.Read();
        }
        
        private static string InsertHotel(OTA_HotelDescriptiveInfoRS.HotelDescriptiveContentsLocalType.HotelDescriptiveContentLocalType hotel)
        {
            string returnVal = string.Empty;
            using (var context = new TravelDBContext())
            {
                Hotel h = null;
                int hotelCode = Convert.ToInt32(hotel.HotelCode);
                var isHas = (from e in context.Hotels where e.HotelCode == hotelCode select e).ToList();
                if (isHas.Count > 0)
                {
                    returnVal = isHas[0].Id;
                    h = isHas[0];
                }
                else
                {
                    h = new Hotel();
                    h.Id = CommonUtil.GetHotelID(hotelCode);
                }
                
                h.BrandCode = Convert.ToInt32(hotel.BrandCode);
                h.LastMofifyTime = DateTime.Now;
                h.AreaID = Convert.ToInt32(hotel.AreaID);
                h.HotelCityCode = Convert.ToInt32(hotel.HotelCityCode);
                h.HotelCode = Convert.ToInt32(hotel.HotelCode);
                h.HotelName = hotel.HotelName;
                
                var hoteinfo = hotel.HotelInfo;
                if (Check(hoteinfo))
                {
                    h.LastUpdated = Convert.ToDateTime(hoteinfo[0].LastUpdated);
                    h.WhenBuilt = Convert.ToDateTime(hoteinfo[0].WhenBuilt);
                }

                if (isHas.Count == 0)
                {
                    returnVal = context.Hotels.Add(h).Id;
                }

                 context.SaveChanges();

                 return returnVal;
            }
        }
        private static void InsertSEG(string hotelId ,string hotelCode, IList<OTA_HotelDescriptiveInfoRS.HotelDescriptiveContentsLocalType.HotelDescriptiveContentLocalType.HotelInfoLocalType.CategoryCodesLocalType> SEG)
        {
            if (SEG != null)
            {
                using (var context = new TravelDBContext())
                {
                    EfRepository<CategoryCodeHotelMapping> EfContext = new EfRepository<CategoryCodeHotelMapping>(context);

                    var seg = (from s in EfContext.Table where s.HotelId == hotelId select s).ToList();

                    if (seg != null)
                        EfContext.Delete(seg, false);

                    IList<CategoryCodeHotelMapping> segs = new List<CategoryCodeHotelMapping>();

                    foreach (var item in SEG)
                    {
                        CategoryCodeHotelMapping s = new CategoryCodeHotelMapping();
                        int segID = Convert.ToInt32(item.SegmentCategory[0].Code);



                        s.HotelId = hotelId;
                        s.SEGID = segID;
                        s.LastMofifyTime = DateTime.Now;
                        segs.Add(s);
                        
                    }

                    EfContext.Insert(segs);
                    Console.WriteLine("EDG Inserted");

                   
                }
            }
        }

        private static void InsertPosition(string hotelId, string hoteCode, IList<OTA_HotelDescriptiveInfoRS.HotelDescriptiveContentsLocalType.HotelDescriptiveContentLocalType.HotelInfoLocalType.PositionLocalType> Position)
        {
            if (Position != null)
            {
                using (var context = new TravelDBContext())
                {
                    EfRepository<Position> EfContext = new EfRepository<Position>(context);

                    var positions = (from p in EfContext.Table select p).ToList();
                    if (positions != null)
                        EfContext.Delete(positions, false);

                    IList<Position> ps = new List<Position>();

                    foreach (var item in Position)
                    {
                        Position p = new Model.Position();
                        p.HotelID = hotelId;
                        p.Latitude = Convert.ToDecimal(item.Latitude);
                        p.Longitude = Convert.ToDecimal(item.Longitude);
                        p.PositionTypeCode = Convert.ToInt32(item.PositionTypeCode);
                        p.LastMofifyTime = DateTime.Now;

                        ps.Add(p);
                       
                    }

                    EfContext.Insert(ps);
                    Console.WriteLine("Position Updated");
                }
            }
        }

        private static void InsertAddress(string hotelId,string hoteCode, IList<OTA_HotelDescriptiveInfoRS.HotelDescriptiveContentsLocalType.HotelDescriptiveContentLocalType.HotelInfoLocalType.AddressLocalType> Address)
        {
            if (Address != null)
            {

                using (var context = new TravelDBContext())
                {

                    //using (TransactionScope tran = new TransactionScope())
                    {
                       
                            EfRepository<Address> addressContext = new EfRepository<Address>(context);
                            EfRepository<AddressExtension> addressExtensionContext = new EfRepository<AddressExtension>(context);
                            EfRepository<Zone> zoonContext = new EfRepository<Zone>(context);
                            EfRepository<ZoneHotelMapping> zonehoteMappingContext = new EfRepository<ZoneHotelMapping>(context);

                            var addressList = (from a in addressContext.Table where a.HotelID == hotelId select a).ToList();

                            if (addressList.Count > 0)
                            {
                                foreach (var al in addressList)
                                {

                                    var addressExtension = addressExtensionContext.Table.Where(a => a.AddressID.Value == al.Id).ToList();
                                    if (addressExtension.Count > 0)
                                    {
                                        addressExtensionContext.Delete(addressExtension);
                                        
                                    }

                                    var zoonhoteMapping = zonehoteMappingContext.Table.Where(z => z.HotelID == hotelId).ToList();
                                    if (zoonhoteMapping.Count > 0)
                                        zonehoteMappingContext.Delete(zoonhoteMapping);
                                }
                              

                                addressContext.Delete(addressList);
                            }




                            foreach (var item in Address)
                            {
                                string addressLine = item.AddressLine;
                                string cityName = item.CityName;
                                string postalCode = item.PostalCode;

                                Address address = new Model.Address();
                                address.HotelID = hotelId;
                                address.AddressLine = addressLine;
                                address.CityName = cityName;
                                address.PostalCode = postalCode;
                                address.LastModifyTime = DateTime.Now;

                                addressContext.Insert(address);

                                int addressPK = address.Id;


                                Console.WriteLine("insert Address " + addressLine);

                                var zone = item.Zone;
                                if (zone != null)
                                {
                                    foreach (var z in zone)
                                    {
                                        int zCode = Convert.ToInt32(z.ZoneCode);
                                        bool checkZoneExits = zoonContext.Table.Where(zm => zm.Id == zCode).Any();
                                        if (!checkZoneExits)
                                        {
                                            Zone zm = new Zone();
                                            zm.Id = zCode;
                                            zm.Name = z.ZoneName;
                                            zm.LastMofifyTime = DateTime.Now;
                                            zoonContext.Insert(zm);
                                        }

                                        ZoneHotelMapping zhm = new ZoneHotelMapping();
                                        zhm.HotelID = hotelId;
                                        zhm.ZoneID = zCode;
                                        zhm.LastMofifyTime = DateTime.Now;

                                        zonehoteMappingContext.Insert(zhm);

                                        Console.WriteLine("insert zone " + z.ZoneCode + "," + z.ZoneName);
                                    }
                                }

                                var extension = item.TPA_Extensions;
                                if (extension != null && extension.Count > 0)
                                {
                                    foreach (var e in extension)
                                    {
                                        Console.WriteLine("AddressExternsion :" + e.RoadCross[0].DescriptionText);

                                        AddressExtension ae = new AddressExtension();
                                        ae.AddressID = addressPK;
                                        ae.Description = e.RoadCross[0].DescriptionText;
                                        ae.LastModifyTime = DateTime.Now;

                                        addressExtensionContext.Insert(ae);
                                    }
                                }

                            }


                           // tran.Complete();
                      
                    }
                }
            }
        }
     
      

        private static void InsertServices(string hotelId, IList<OTA_HotelDescriptiveInfoRS.HotelDescriptiveContentsLocalType.HotelDescriptiveContentLocalType.HotelInfoLocalType.ServicesLocalType> Services)
        {
            if (Check(Services))
            {
                using (var context = new TravelDBContext())
                {
                    //using (TransactionScope tran = new TransactionScope())
                    {

                        EfRepository<HACHATMapping> hachatContext = new EfRepository<HACHATMapping>(context);
                        EfRepository<HAT> hatContext = new EfRepository<HAT>(context);
                        EfRepository<HAC> hacContext = new EfRepository<HAC>(context);

                        var hachatlist = (from h in hachatContext.Table where h.HotelID == hotelId select h).ToList();

                        if (hachatlist.Count > 0)
                        {
                            hachatContext.Delete(hachatlist);
                        }


                        foreach (var item in Services[0].Service)
                        {
                            int HACID = Convert.ToInt32(item.Code);
                            int HATID = Convert.ToInt32(item.ID);
                            string descriptiveText = item.DescriptiveText;


                            var hacList = (from h in hacContext.Table where h.Id == HACID select h).ToList();
                           
                            if (hacList.Count == 0)
                            {
                                HAC hac = new HAC();
                                hac.Id = HACID;
                                hac.Name = descriptiveText;

                                hac.LastModiyTime = DateTime.Now;

                                hacContext.Insert(hac);
                            }



                            var hatList = (from h in hatContext.Table where h.Id == HATID select h).ToList();

                            if (hatList.Count == 0)
                            {
                                HAT hac = new HAT();
                                hac.Id = HATID;
                                hac.Name = "NO";
                               
                                hac.LastModiyTime = DateTime.Now;

                                hatContext.Insert(hac);
                            }



                            HACHATMapping hachatmapping = new HACHATMapping();
                            hachatmapping.HACID = Convert.ToInt32(HACID);
                            hachatmapping.HATID = Convert.ToInt32(HATID);
                            hachatmapping.HotelID = hotelId;
                            hachatmapping.LastModifyTine = DateTime.Now;
                            hachatContext.Insert(hachatmapping);

                            //logger.Info("HotelID" + hotelId + " Service Inserted");

                            Console.WriteLine("Insert into Service" + HACID + "," + HATID);
                        }

                        //tran.Complete();
                    }
                }
            }
        }

        private static void InsertGuestRoom(string hotelId, IList<OTA_HotelDescriptiveInfoRS.HotelDescriptiveContentsLocalType.HotelDescriptiveContentLocalType.FacilityInfoLocalType.GuestRoomsLocalType> GuestRooms)
        {
            if (GuestRooms != null && GuestRooms.Count > 0)
            {
                using (var context = new TravelDBContext())
                {
                    // TransactionOptions transactionOption = new TransactionOptions();
                  //  transactionOption.Timeout = new TimeSpan(0, 0, 600);

                   // using (TransactionScope tran = new TransactionScope(TransactionScopeOption.Required, transactionOption))
                    {
                        EfRepository<GuestRoom> guestRoomContext = new EfRepository<GuestRoom>(context);
                     
                        EfRepository<RoomExtension> roomExtensionsContext = new EfRepository<RoomExtension>(context);
                        EfRepository<RMARoomMapping> rmaRoomMappingContext = new EfRepository<RMARoomMapping>(context);
                        EfRepository<RMA> rmaContext = new EfRepository<RMA>(context);

                        int Inserted = 0;
                        var guestRoomCheck = (from r in guestRoomContext.Table where r.HotelID == hotelId select r).ToList();

                        if (guestRoomCheck.Count > 0)
                        {
                            foreach (var item in guestRoomCheck)
                            {
                                var roomExtensions = (from e in roomExtensionsContext.Table where e.RoomID == item.Id select e).ToList();
                                if (roomExtensions.Count > 0)
                                    roomExtensionsContext.Delete(roomExtensions);

                                var rmaRoomMapping =(from r in rmaRoomMappingContext.Table where r.RoomID==item.Id select r).ToList();
                                if(rmaRoomMapping.Count>0)
                                    rmaRoomMappingContext.Delete(rmaRoomMapping);

                                guestRoomContext.Delete(item);
                                Console.WriteLine(item.Name + "deleted");

                            }
                        }

                        foreach (var item in GuestRooms[0].GuestRoom)
                        {
                            string roomName = item.RoomTypeName;

                            var typeName = item.TypeRoom;
                            if (Check(typeName))
                            {
                                var typeNameNode = typeName[0];
                                int StandardOccupancy = Convert.ToInt32(typeNameNode.StandardOccupancy);
                                string Size = typeNameNode.Size;
                                string Name = typeNameNode.Name;
                                string RoomTypeCode = typeNameNode.RoomTypeCode;
                                string Floor = typeNameNode.Floor;
                                string InvBlockCode = typeNameNode.InvBlockCode;
                                string BedTypeCode = typeNameNode.BedTypeCode;
                                string NonSmoking = typeNameNode.NonSmoking;
                                string HasWindow = typeNameNode.HasWindow;
                                string Quantity = typeNameNode.Quantity;
                                string RoomSize = typeNameNode.RoomSize;


                                GuestRoom gr = new GuestRoom();
                                gr.StandardOccupancy = StandardOccupancy;
                                gr.Size = Size;
                                gr.Name = Name;
                                gr.RoomTypeCode = Convert.ToInt32(RoomTypeCode);
                                gr.Floor = Floor;
                                gr.InvBlockCode = Convert.ToInt32(InvBlockCode);
                                gr.BedTypeCode = Convert.ToInt32(BedTypeCode);
                                gr.NonSmoking = NonSmoking == "false" ? 0 : 1;
                                gr.HasWindow = Convert.ToInt32(HasWindow);
                                gr.Quantity = Convert.ToInt32(Quantity);
                                gr.RoomSize = RoomSize;
                                gr.LastMofifyTime = DateTime.Now;
                                gr.HotelID = hotelId;


                                var feature = item.Features;
                                if (Check(feature))
                                {
                                    gr.FeatureDescription = feature[0].Feature[0].DescriptiveText;
                                }
                                guestRoomContext.Insert(gr);

                                Inserted = gr.Id;

                                Console.WriteLine("Guest Room" + Name);
                            }

                            var Amenities = item.Amenities;
                            if (Check(Amenities))
                            {
                                foreach (var amentity in Amenities[0].Amenity)
                                {
                                    int RoomAmenityCode =Convert.ToInt32( amentity.RoomAmenityCode);
                                    string DescriptiveText = amentity.DescriptiveText;

                                    var rmaCheck = (from r in rmaContext.Table where r.Id == RoomAmenityCode select r).ToList();
                                    if (rmaCheck.Count == 0)
                                    {
                                        RMA r = new RMA();
                                        r.Id = RoomAmenityCode;
                                        r.Name = DescriptiveText;
                                        r.LastModiyTime = DateTime.Now;

                                        rmaContext.Insert(r);
                                    }

                                    RMARoomMapping rmaRoomMapping = new RMARoomMapping();
                                    rmaRoomMapping.RMAID = RoomAmenityCode;
                                    rmaRoomMapping.RoomID = Inserted;
                                    rmaRoomMapping.LastMofifyTime = DateTime.Now;

                                    rmaRoomMappingContext.Insert(rmaRoomMapping);

                                    Console.WriteLine("Guest Amentity" + RoomAmenityCode + "," + DescriptiveText);
                                }
                            }

                            var TPA_Extensions = item.TPA_Extensions;
                            if (Check(TPA_Extensions))
                            {
                                foreach (var extension in TPA_Extensions[0].TPA_Extension)
                                {
                                    string FacilityName = extension.FacilityName;
                                    string FTypeName = extension.FTypeName;
                                    string FacilityValue = extension.FacilityValue;
                                    string isAvailable = extension.FacilityValue;
                                    RoomExtension re = new RoomExtension();
                                    re.FacilityName = FacilityName;
                                    re.FaciliTypeName = FTypeName;
                                    re.LastMofifyTime = DateTime.Now;
                                    re.IsAllAvailable = Convert.ToInt32(isAvailable);
                                    re.RoomID = Inserted;

                                    roomExtensionsContext.Insert(re);

                                    Console.WriteLine("Room Extensions Inserted" + FacilityName);
                                }
                            }
                        }

                        //tran.Complete();
                    }
                }
            }
        }

        private static bool Check(IEnumerable<object> o)
        {
            if (o != null && o.Count() > 0)
            {
                return true;
            }

            return false;
        }

        private static void InsertPolicies(string hotelId, IList<OTA_HotelDescriptiveInfoRS.HotelDescriptiveContentsLocalType.HotelDescriptiveContentLocalType.PoliciesLocalType> policies)
        {
            if (Check(policies))
            {
                var policy = policies[0].Policy;
                InsertPolicy(hotelId, policy);
            }
        }

        private static void InsertPolicy(string hotelId, IList<OTA_HotelDescriptiveInfoRS.HotelDescriptiveContentsLocalType.HotelDescriptiveContentLocalType.PoliciesLocalType.PolicyLocalType> policy)
        {
            if (Check(policy))
            {
                using (var context = new TravelDBContext())
                {
                    EfRepository<Policy> policyContext = new EfRepository<Policy>(context);
                    EfRepository<PolicyInfo> policyinfoContext = new EfRepository<PolicyInfo>(context);

                    var policyCheck = (from p in policyContext.Table where p.HotelID == hotelId select p).ToList();
                    if (policyCheck.Count > 0)
                        policyContext.Delete(policyCheck);

                    var policyinfoCheck = (from p in policyinfoContext.Table where p.HotelId == hotelId select p).ToList();
                    if (policyinfoCheck.Count > 0)
                        policyinfoContext.Delete(policyinfoCheck);



                    var policyinfoCodes = policy[0].PolicyInfoCodes;

                    if (Check(policyinfoCodes))
                    {
                        var PolicyInfoCode = policyinfoCodes[0].PolicyInfoCode;
                        foreach (var item in PolicyInfoCode[0].Description)
                        {
                            Console.WriteLine(item.Name + "," + item.Text);

                            Policy p = new Policy();
                            p.HotelID = hotelId;
                            p.Key = item.Name;
                            p.Value = item.Text;
                            p.LastMofifyTime = DateTime.Now;

                            policyContext.Insert(p);
                        }
                    }

                    var policyinfo = policy[0].PolicyInfo;
                    var CheckinTime = policyinfo[0].CheckInTime;
                    var checkOutTime = policyinfo[0].CheckOutTime;

                    PolicyInfo pi = new PolicyInfo();
                    pi.HotelId = hotelId;
                    pi.CheckIn = CheckinTime;
                    pi.CheckOut = checkOutTime;
                    pi.LastModifyTime = DateTime.Now;

                    policyinfoContext.Insert(pi);


                }
            }
        }

        private static void InsertAreaInfo(string hotelId, IList<OTA_HotelDescriptiveInfoRS.HotelDescriptiveContentsLocalType.HotelDescriptiveContentLocalType.AreaInfoLocalType> areas)
        {
            if (Check(areas))
            {
                using (var context = new TravelDBContext())
                {
                    EfRepository<RefPoint> refPointContext = new EfRepository<RefPoint>(context);
                    EfRepository<REF> refContext = new EfRepository<REF>(context);

                    var refPoingCheck = (from p in refPointContext.Table where p.HotelID == hotelId select p).ToList();
                    if (refPoingCheck.Count > 0)
                        refPointContext.Delete(refPoingCheck);

                    var refPoints = areas[0].RefPoints;

                    foreach (var item in refPoints[0].RefPoint)
                    {
                        var Distance = item.Distance;
                        int UnitOfMeasureCode = Convert.ToInt32(item.UnitOfMeasureCode);
                        var Name = item.Name;
                        var Latitude = item.Latitude;
                        var Longitude = item.Longitude;
                        int RefPointCategoryCode =Convert.ToInt32( item.RefPointCategoryCode);
                        var RefPointName = item.RefPointName;
                        var DescriptiveText = item.DescriptiveText;

                        var refCheck = (from f in refContext.Table where f.Id == RefPointCategoryCode select f).ToList();
                        if (refCheck.Count == 0)
                        {
                            REF r = new REF();
                            r.Id = RefPointCategoryCode;
                            r.Name = RefPointName;
                            r.LastModiyTime = DateTime.Now;
                            refContext.Insert(r);
                        }

                        RefPoint rp = new RefPoint();
                        rp.HotelID = hotelId;
                        rp.Distance = Convert.ToDecimal(Distance);
                        rp.Name = Name;
                        rp.REFINT = RefPointCategoryCode;
                        rp.UOMID = UnitOfMeasureCode;
                        rp.LastMofifyTime = DateTime.Now;

                        refPointContext.Insert(rp);


                        Console.WriteLine(Name + "," + DescriptiveText);
                    }
                }
            }
        }

        private static void InsertAffiliation(string hotelId, IList<OTA_HotelDescriptiveInfoRS.HotelDescriptiveContentsLocalType.HotelDescriptiveContentLocalType.AffiliationInfoLocalType> affiliation)
        {
            if (Check(affiliation))
            {
                using (var context = new TravelDBContext())
                {
                    EfRepository<AwardType> awardContext = new EfRepository<AwardType>(context);
                    EfRepository<HotelAwardMapping> awardMapping = new EfRepository<HotelAwardMapping>(context);

                    var awardMappingCheck = (from a in awardMapping.Table where a.HotelID == hotelId select a).ToList();
                    if (awardMappingCheck.Count > 0)
                        awardMapping.Delete(awardMappingCheck);

                    foreach (var item in affiliation[0].Awards[0].Award)
                    {
                        int AID =-1;
                        var awardCheck = (from a in awardContext.Table where a.Name == item.Provider select a).ToList();
                        if(awardCheck.Count==0)
                        {
                            AwardType at = new AwardType();
                            at.Name = item.Provider;
                            at.LastMofifyTime = DateTime.Now;

                            awardContext.Insert(at);
                            AID=at.Id;
                        }
                        else
                        {
                            AID=awardCheck.FirstOrDefault().Id;
                        }

                        HotelAwardMapping ham = new HotelAwardMapping();
                        ham.AwardID = AID;
                        ham.HotelID = hotelId;
                        ham.LastMofifyTime = DateTime.Now;
                        ham.Rating = Convert.ToDecimal(item.Rating);

                        awardMapping.Insert(ham);

                        Console.WriteLine(item.Provider + "," + item.Rating);


                    }
                }
            }
        }

        private static void InsertMultimediaDescription(string hotelId, IList<OTA_HotelDescriptiveInfoRS.HotelDescriptiveContentsLocalType.HotelDescriptiveContentLocalType.MultimediaDescriptionsLocalType> multimediadescriptions)
        {
            if (Check(multimediadescriptions))
            {
                foreach (var item in multimediadescriptions[0].MultimediaDescription)
                {
                    var imageItems = item.ImageItems;

                    if (Check(imageItems))
                    {
                        using (var context = new TravelDBContext())
                        {
                            EfRepository<ImageItem> imageItemContext =new EfRepository<ImageItem> (context);

                            var imageItemCheck = (from i in imageItemContext.Table where i.HotelId == hotelId select i).ToList();
                            if (imageItemCheck.Count > 0)
                                imageItemContext.Delete(imageItemCheck);

                            foreach (var image in imageItems[0].ImageItem)
                            {
                                ImageItem imageItem = new ImageItem();

                                var url = image.ImageFormat[0].URL;
                                var caption = image.Description[0].Caption;
                                var category = image.Category;


                                var tap_extension = image.TPA_Extensions;
                                if (Check(tap_extension))
                                {
                                    var invokeCode = tap_extension[0].InvBlockCode;
                                    imageItem.InvBlockCode = Convert.ToInt32(invokeCode);
                                }
                                imageItem.Category = Convert.ToInt32(category);
                                imageItem.URL = url;
                                imageItem.Description = caption;
                                imageItem.HotelId = hotelId;
                                imageItem.LastMofifyTime = DateTime.Now;

                                imageItemContext.Insert(imageItem);

                                Console.WriteLine(url + "," + caption);
                            }
                        }
                    }

                    var textitems = item.TextItems;

                    if (Check(textitems))
                    {
                        using (var context = new TravelDBContext())
                        {
                            EfRepository<TextItem> textItemContext = new EfRepository<TextItem>(context);
                            var itemCheck = (from i in textItemContext.Table where i.HotelId == hotelId select i).ToList();
                            if (itemCheck.Count > 0)
                                textItemContext.Delete(itemCheck);

                            foreach (var text in textitems[0].TextItem)
                            {
                                TextItem ti = new TextItem();
                                ti.HotelId = hotelId;
                            
                                var category = text.Category;
                                var Description = text.Description;
                                ti.Category = Convert.ToInt32(category);
                                ti.DescriptionText = Description != null ? Description : text.URL;
                                ti.LastModityTime = DateTime.Now;
                               
                                textItemContext.Insert(ti);

                                Console.WriteLine(category + "," + Description);
                            }
                        }
                    }
                }
            }
        }

        private static void InsertHotelTapExtension(string hotelCode, OTA_HotelDescriptiveInfoRS.HotelDescriptiveContentsLocalType.HotelDescriptiveContentLocalType content)
        {
            if (Check(content.TPA_Extensions))
            {
                foreach (var item in content.TPA_Extensions)
                {
                    var CityImportantMessageType = item.CityImportantMessage[0].CityImportantMessageType[0];

                    var StartDate = CityImportantMessageType.StartDate;
                    var EndDate = CityImportantMessageType.EndDate;
                    var MessageContent = CityImportantMessageType.MessageContent;

                    var Roomquantity = item.Roomquantity;

                    Console.WriteLine(Roomquantity + "," + MessageContent);

                }
            }
        }
    }
}

    

