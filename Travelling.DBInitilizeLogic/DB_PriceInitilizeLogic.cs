using Price;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Price.www.opentravel.org.OTA.Item2003.Item05;
using Model;
using DAL;
using Travelling.OpenApiLogic;
using Common.Logging;
namespace Travelling.DBInitilizeLogic
{
    public class DB_PriceInitilizeLogic
    {
        static ILog logger = LogManager.Adapter.GetLogger(typeof(DB_HotelInitilizeLogic));
        public static void Process(string hotelId, string hotelCode,string ratePlanCode="", DateTime? startDate=null, DateTime? endDate=null, bool isInitilize = true)
        {
          
            OTAHotelServiceLogic hotelService = new OTAHotelServiceLogic();
            bool ifNeedUpdateRatePlan = true;

            //全量获取数据
            if (isInitilize)
            {
                startDate = DateTime.Now;
                endDate = DateTime.Now.AddDays(28);

                //获取90天数据，分四次获取
                for (int i = 0; i < 4; i++)
                {

                    string xml = hotelService.GetHotelRatePlan(hotelCode, startDate.Value, endDate.Value);
                    ProcessPrice(hotelId, hotelCode, xml, ifNeedUpdateRatePlan,isInitilize);

                    ifNeedUpdateRatePlan = false;

                    startDate = endDate.Value.AddDays(1);
                    endDate = startDate.Value.AddDays(28);
                }
            }
            else
            {
                ifNeedUpdateRatePlan = false;
                //Update Rate plan Cache 
                string xml = hotelService.GetHotelRatePlan(hotelCode, startDate.Value, endDate.Value, ratePlanCode);
                ProcessPrice(hotelId, hotelCode, xml, ifNeedUpdateRatePlan,isInitilize);
            }
        }

        private static void LoggerHelper(object ratePlan, string message)
        {
            Console.WriteLine("RatePlan Code :" + ratePlan + "Message :" + message);
            logger.Info("RatePlan Code :" + ratePlan + "Message :" + message);
        }

        public static void ProcessPrice(string hotelId, string hotelCode, string xml, bool ifNeedUpdateRatePlan,bool isInitilize)
        {


            //XRoot root = XRoot.Load(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "realprice.xml"));
            XRoot root = XRoot.Parse(xml);

            var RatePlans = root.Response.HotelResponse[0].OTA_HotelRatePlanRS.RatePlans;

            foreach (var item in RatePlans)
            {
                if (ifNeedUpdateRatePlan)
                {
                    using (var context = new TravelDBContext())
                    {
                        EfRepository<RatePlan> ratePlanContext = new EfRepository<RatePlan>(context);

                        var rpCheck = (from r in ratePlanContext.Table where r.HoteID == hotelId select r).ToList();
                        if (rpCheck.Count > 0)
                            ratePlanContext.Delete(rpCheck);
                    }
                }
                // var hotelCode = item.HotelCode;
                ProcessRatePlan(hotelId, item.RatePlan, ifNeedUpdateRatePlan, isInitilize);
            }


            //Console.Read();
        }

        private static void ProcessRatePlan(string hoteId, IList<OTA_HotelRatePlanRS.RatePlansLocalType.RatePlanLocalType> ratePlans, bool ifNeedUpdateRatePlan, bool isInitilize)
        {
            if (Check(ratePlans))
            {

                foreach (var item in ratePlans)
                {
                    int RatePlanId = -1;
                    using (var context = new TravelDBContext())
                    {
                        int ratePlanCode = Convert.ToInt32(item.RatePlanCode);

                        EfRepository<RatePlan> ratePlanContext = new EfRepository<RatePlan>(context);

                        var rpCheck = (from r in ratePlanContext.Table where r.HoteID == hoteId && r.RatePlanCode==ratePlanCode select r).ToList();
                        if (rpCheck.Count > 0)
                        {
                            RatePlanId = rpCheck.First().Id;
                        }
                        else
                        {
                            RatePlan rp = new RatePlan();
                            rp.RatePlanCategory = item.RatePlanCategory;
                            rp.IsCommissionable = Convert.ToBoolean(item.IsCommissionable);
                            rp.LastModifyTime = DateTime.Now;
                            rp.MarketCode = item.MarketCode;
                            rp.RatePlanCode = ratePlanCode;
                            rp.RateReturn = Convert.ToBoolean(item.RateReturn);
                            rp.HoteID = hoteId;


                            ratePlanContext.Insert(rp);
                            RatePlanId = rp.Id;
                        }

                    }

                    InsertRate(RatePlanId, item.Rates, !isInitilize);
                    InsertOffers(RatePlanId, item.Offers);
                    InsertBookingRules(Convert.ToInt32(RatePlanId), item.BookingRules);
                }
            }

        }

        private static void InsertOffers(int ratePlanId, IList<OTA_HotelRatePlanRS.RatePlansLocalType.RatePlanLocalType.OffersLocalType> offers)
        {
            if (Check(offers))
            {
                using (var context = new TravelDBContext())
                {
                    EfRepository<Offer> offerContext = new EfRepository<Offer>(context);
                    EfRepository<OfferRule> offerRuleContext = new EfRepository<OfferRule>(context);

                    Int32 offerInserted = -1;
                    foreach (var offer in offers[0].Offer)
                    {
                        Offer offerModel = new Offer();
                        offerModel.OfferCode = Convert.ToInt32(offer.OfferCode);

                        var description = offer.OfferDescription;

                        if (Check(description))
                        {
                            var descriptionText = description[0].Text;
                            offerModel.OfferDescription = descriptionText;
                        }

                        var discounts = offer.Discount;
                        if (Check(discounts))
                        {
                            var discount = discounts[0];
                            offerModel.NightsDiscounted = Convert.ToInt32(discount.NightsDiscounted);
                            offerModel.NightsRequired = Convert.ToInt32(discount.NightsRequired);
                            offerModel.DiscountPattern = discount.DiscountPattern;
                        }
                        offerModel.ratePlanId = ratePlanId;

                        offerContext.Insert(offerModel);
                        LoggerHelper(ratePlanId, " Offer" + description + "Inserted");

                        offerInserted = offerModel.Id;

                        var offerRules = offer.OfferRules;
                        if (Check(offerRules) && Check(offerRules[0].OfferRule[0].DateRestriction))
                        {
                            foreach (var item in offerRules[0].OfferRule[0].DateRestriction)
                            {
                                OfferRule or = new OfferRule();
                                or.RestrictionDateCode = Convert.ToInt32(item.RestrictionDateCode);
                                or.LastModifyTime = DateTime.Now;
                                or.OfferId = offerInserted;
                                or.EndTime = item.End;
                                or.StartTime = item.Start;
                                or.RestrictionType = item.RestrictionType;

                                offerRuleContext.Insert(or);
                                LoggerHelper(ratePlanId, " Offer Rule" + or.RestrictionDateCode + "Inserted");

                            }


                        }
                    }
                }
            }
        }

        private static bool Check(IEnumerable<object> o)
        {
            if (o != null && o.Count() > 0)
                return true;

            return false;
        }


        private static int CheckRuleInserted(string key, EfRepository<BookRule> bookruleContext)
        {

            int inserted = -1;
            var keyCheck = (from k in bookruleContext.Table where k.Name == key select k).FirstOrDefault();
            if (keyCheck == null)
            {
                BookRule bookRule = new BookRule();
                bookRule.Name = key;
                bookruleContext.Insert(bookRule);
                inserted = bookRule.Id;
            }
            else
            {
                inserted = keyCheck.Id;
            }

            return inserted;
        }

        private static void InsertBookRuleMapping(int ratePlanId, string key, string parameter, EfRepository<RoomPlanBookRuleMapping> roomPlanBookMappingContext, EfRepository<BookRule> bookruleContext)
        {
            RoomPlanBookRuleMapping rbrm = new RoomPlanBookRuleMapping();
            rbrm.RatePlanId = ratePlanId;
            rbrm.Parameters = parameter;
            rbrm.LastModifyTime = DateTime.Now;
            rbrm.BookRuleId = CheckRuleInserted(key, bookruleContext);

            roomPlanBookMappingContext.Insert(rbrm);

        }

        private static void InsertBookingRules(int ratePlanId, IList<BookingRules> BooKRules)
        {
            if (Check(BooKRules))
            {

                using (var context = new TravelDBContext())
                {
                    EfRepository<BookRule> bookruleContext = new EfRepository<BookRule>(context);
                    EfRepository<RoomPlanBookRuleMapping> roomPlanBookMappingContext = new EfRepository<RoomPlanBookRuleMapping>(context);

                    var mappingCheck = (from m in roomPlanBookMappingContext.Table where m.RatePlanId == ratePlanId select m).ToList();
                    if (mappingCheck.Count > 0)
                    {
                        roomPlanBookMappingContext.Delete(mappingCheck);
                    }



                    foreach (var item in BooKRules[0].BookingRule)
                    {
                        if (item.MinAdvancedBookingOffset != null)
                        {
                            string key = "MinAdvancedBookingOffset";
                            InsertBookRuleMapping(ratePlanId, key, item.MinAdvancedBookingOffset, roomPlanBookMappingContext, bookruleContext);

                        }


                        if (Check(item.LengthsOfStay))
                        {
                            foreach (var los in item.LengthsOfStay)
                            {
                                string key = "LengthOfStay";
                                InsertBookRuleMapping(ratePlanId, key, los.LengthOfStay[0].Time, roomPlanBookMappingContext, bookruleContext);

                                Console.WriteLine(los.LengthOfStay[0].Time);
                            }
                        }

                        if (item.LaterReserveTime != null)
                        {
                            string key = "LaterReserveTime";
                            InsertBookRuleMapping(ratePlanId, key, item.LaterReserveTime, roomPlanBookMappingContext, bookruleContext);

                        }


                        if (Check(item.Viewerships))
                        {
                            string key = "Viewerships";
                            string customer = string.Empty;
                            foreach (var vs in item.Viewerships[0].Viewership)
                            {
                                var Customer = vs.Profiles[0].Profile[0].Customer[0].CustomerValue;
                                customer += Customer + ",";
                            }

                            InsertBookRuleMapping(ratePlanId, key, customer, roomPlanBookMappingContext, bookruleContext);
                        }

                    }
                }
            }
        }
        

        //增量获取不删除rateplan，直接更新此处
        public static void InsertRate(int ratePlanId, IList<OTA_HotelRatePlanRS.RatePlansLocalType.RatePlanLocalType.RatesLocalType> rates,bool ifNeedCheck = false)
        {
            if (Check(rates))
            {
                foreach (var item in rates[0].Rate)
                {
                    using (var context = new TravelDBContext())
                    {
                        int RateInserted = -1;
                        EfRepository<Rate> rateContext = new EfRepository<Rate>(context);
                        EfRepository<BaseByGuestAmt> baseByGuestContext = new EfRepository<BaseByGuestAmt>(context);
                        DateTime startDate = Convert.ToDateTime(item.Start);
                        DateTime endDate = Convert.ToDateTime(item.End);

                        if (ifNeedCheck)
                        {
                            var checkDataExists = (from r in rateContext.Table where r.RatePlanId == ratePlanId && r.Start.Value >= startDate && r.End.Value <= endDate select r).ToList();

                            if (checkDataExists.Count > 0)
                            {
                                LoggerHelper(ratePlanId.ToString(), "Rate" + startDate + "," + endDate + " Deleted");
                                rateContext.Delete(checkDataExists);
                            }
                        }

                        Rate rate = new Rate();
                        rate.RatePlanId = ratePlanId;
                        rate.Start =startDate;
                        rate.End =endDate;
                        rate.IsInstantConfirm = Convert.ToBoolean(item.IsInstantConfirm);
                        rate.Status = item.Status;
                        rate.NumberOfUnits = Convert.ToInt32(item.NumberOfUnits);

                        var mealIncluded = item.MealsIncluded;
                        if (Check(mealIncluded))
                        {
                            rate.IsBreakfast = Convert.ToBoolean(mealIncluded[0].Breakfast);
                            rate.BreakfastNumber = Convert.ToInt32(mealIncluded[0].NumberOfBreakfast);
                        }
                        rate.LastModifyTime = DateTime.Now;

                        rateContext.Insert(rate);

                        LoggerHelper(ratePlanId.ToString() , "RAte" + rate.Start + "," + rate.End + "Inserted");

                        RateInserted = rate.Id;

                        var BaseByGuestAmts = item.BaseByGuestAmts;
                        if (Check(BaseByGuestAmts))
                        {
                            var BaseByGuestAmtItem = BaseByGuestAmts[0].BaseByGuestAmt[0];
                            BaseByGuestAmt bga = new BaseByGuestAmt();
                            bga.AmountBeforeTax = Convert.ToDecimal(BaseByGuestAmtItem.AmountBeforeTax);
                            bga.CurrencyCode = BaseByGuestAmtItem.CurrencyCode;
                            bga.NumberOfGuests = Convert.ToInt32(BaseByGuestAmtItem.NumberOfGuests);

                            bga.RateId = RateInserted;
                            bga.ListPrice = Convert.ToDecimal(BaseByGuestAmtItem.ListPrice);


                            var tap_extension = BaseByGuestAmtItem.TPA_Extensions;
                            if (Check(tap_extension))
                            {
                                var otherCurrency = tap_extension[0].OtherCurrency[0];
                                var AmountPercentType = otherCurrency.AmountPercentType[0];

                                var Amount = AmountPercentType.Amount;

                                var CurrencyCode = AmountPercentType.CurrencyCode;

                                bga.OtherCurrency = Convert.ToDecimal(Amount);
                                bga.OtherCurrencyCode = CurrencyCode;

                            }
                            bga.LastModifyTime = DateTime.Now;
                            baseByGuestContext.Insert(bga);
                            LoggerHelper(ratePlanId.ToString() , "BaseByGuestAmts" + bga.AmountBeforeTax + "," + bga.CurrencyCode + "Inserted");
                          
                        }



                        var Fees = item.Fees;
                        if (Check(Fees))
                        {
                            EfRepository<Fee> feeContext = new EfRepository<Fee>(context);
                            foreach (var fee in Fees[0].Fee)
                            {
                                Fee f = new Fee();
                                f.Code = Convert.ToInt32(fee.Code);
                                f.Amount = Convert.ToDecimal(fee.Amount);
                                f.CurrencyCode = fee.CurrencyCode;
                                f.ChargeUnit = Convert.ToInt32(fee.ChargeUnit);
                                f.RateId = RateInserted;
                                f.LastModifyTime = DateTime.Now;
                                f.RateId = RateInserted;
                                var Description = fee.Description;
                                if (Check(Description))
                                {
                                    f.DescriptionText = Description[0].Text;
                                }
                                f.LastModifyTime = DateTime.Now;
                                var feeExtension = fee.TPA_Extensions;
                                if (Check(feeExtension))
                                {
                                    var otherCurrency = feeExtension[0].OtherCurrency[0];
                                    var AmountPercentType = otherCurrency.AmountPercentType[0];

                                    var feeAmount = AmountPercentType.Amount;
                                    var Currency = AmountPercentType.CurrencyCode;

                                    LoggerHelper(ratePlanId.ToString(), "Fee Other Currency :" + Currency + "," + feeAmount);


                                }

                                feeContext.Insert(f);

                                LoggerHelper(ratePlanId, "Fee Other Currency Finished");

                            }
                        }



                        var GuaranteePolicies = item.GuaranteePolicies;
                        if (Check(GuaranteePolicies))
                        {
                            EfRepository<GuaranteePolicy> guaranteePolicyContext = new EfRepository<GuaranteePolicy>(context);
                            GuaranteePolicy gp = new GuaranteePolicy();
                            gp.RateId = RateInserted;
                            gp.GuaranteeCode = Convert.ToInt32(GuaranteePolicies[0].GuaranteePolicy[0].GuaranteeCode);
                            var Holdtime = GuaranteePolicies[0].GuaranteePolicy[0].HoldTime;
                            if (!string.IsNullOrEmpty(Holdtime))
                                gp.HoldTime = Convert.ToDateTime(Holdtime);

                            gp.LastModifyTime = DateTime.Now;
                            guaranteePolicyContext.Insert(gp);
                        }

                        var CancelPolicies = item.CancelPolicies;
                        if (Check(CancelPolicies))
                        {
                            EfRepository<CancelPenalty> cancleContext = new EfRepository<CancelPenalty>(context);
                            CancelPenalty cp = new CancelPenalty();
                            var CancelPolicy = CancelPolicies[0].CancelPenalty[0];
                            cp.Start = Convert.ToDateTime(CancelPolicy.Start);
                            cp.End = Convert.ToDateTime(CancelPolicy.End);
                            cp.RateId = RateInserted;
                            var AmountPercent = CancelPolicy.AmountPercent[0];
                            cp.AmountPercent = Convert.ToDecimal(AmountPercent.Amount);
                            cp.CurrencyCode = AmountPercent.CurrencyCode;

                            var CancleExtension = CancelPolicy.TPA_Extensions;
                            if (Check(CancleExtension))
                            {
                                var otherCurrency = CancleExtension[0].OtherCurrency[0];
                                var AmountPercentType = otherCurrency.AmountPercentType[0];

                                cp.OtherCurrencyAmount = Convert.ToDecimal(AmountPercentType.Amount);
                                cp.OtherCurrencyCode = AmountPercentType.CurrencyCode;

                                cancleContext.Insert(cp);

                                LoggerHelper(ratePlanId, "Cancle Pennalty  " + cp.OtherCurrencyAmount + "Inserted");
                            }

                        }

                        var rateExtension = item.TPA_Extensions;
                        if (Check(rateExtension))
                        {
                            var RebatePromotion = rateExtension[0].RebatePromotion;
                            if (Check(RebatePromotion))
                            {
                                EfRepository<RateExtension> rateExtensionContext = new EfRepository<RateExtension>(context);

                                RateExtension re = new RateExtension();
                                foreach (var rebate in RebatePromotion)
                                {

                                    re.StartPeriod = Convert.ToDateTime(rebate.StartPeriod);
                                    re.EndPeriod = Convert.ToDateTime(rebate.EndPeriod);
                                    re.ProgramName = rebate.ProgramName;
                                    re.Amount = Convert.ToDecimal(rebate.Amount);
                                    re.CurrencyCode = rebate.CurrencyCode;
                                    re.Code = Convert.ToInt32(rebate.Code);
                                    re.RateId = RateInserted;
                                    if (Check(rebate.Description))
                                    {
                                        var des = rebate.Description[0].Text;
                                        re.DescriptionText = des;

                                        Console.WriteLine(des);
                                    }


                                }
                                re.LastModifyTime = DateTime.Now;
                                rateExtensionContext.Insert(re);

                            }

                            var PayChange = rateExtension[0].PayChange;



                            Console.WriteLine(PayChange);

                        }
                    }
                }

            }

        }
    }
}
