﻿using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Xiletrade.Library.Models.Serializable.SourceGeneration;

//[JsonSourceGenerationOptions(GenerationMode = JsonSourceGenerationMode.Serialization)] doesnt work
//[JsonSerializable(typeof(object))]
[JsonSerializable(typeof(AccountData))]
[JsonSerializable(typeof(Armour))]
[JsonSerializable(typeof(ArmourFilters))]
[JsonSerializable(typeof(BaseData))]
[JsonSerializable(typeof(BaseResult))]
[JsonSerializable(typeof(BaseResultData))]
[JsonSerializable(typeof(BulkData))]
[JsonSerializable(typeof(ChatCommands))]
[JsonSerializable(typeof(ConfigChecked))]
[JsonSerializable(typeof(ConfigData))]
[JsonSerializable(typeof(ConfigMods))]
[JsonSerializable(typeof(ConfigOption))]
[JsonSerializable(typeof(ConfigShortcut))]
[JsonSerializable(typeof(CurrencyEntrie))]
[JsonSerializable(typeof(CurrencyResult))]
[JsonSerializable(typeof(CurrencyResultData))]
[JsonSerializable(typeof(DivTiersData))]
[JsonSerializable(typeof(DivTiersResult))]
[JsonSerializable(typeof(ExchangeInfo))]
[JsonSerializable(typeof(FetchData))]
[JsonSerializable(typeof(FetchDataInfo))]
[JsonSerializable(typeof(FetchDataListing))]
[JsonSerializable(typeof(FilterData))]
[JsonSerializable(typeof(FilterResult))]
[JsonSerializable(typeof(FilterResultEntrie))]
[JsonSerializable(typeof(FilterResultOption))]
[JsonSerializable(typeof(FilterResultOptions))]
[JsonSerializable(typeof(Filters))]
[JsonSerializable(typeof(GemData))]
[JsonSerializable(typeof(GemResult))]
[JsonSerializable(typeof(GemResultData))]
[JsonSerializable(typeof(GemTransfigured))]
[JsonSerializable(typeof(ItemInfo))]
[JsonSerializable(typeof(JsonData))]
[JsonSerializable(typeof(LeagueData))]
[JsonSerializable(typeof(LeagueResult))]
[JsonSerializable(typeof(LicenceData))]
[JsonSerializable(typeof(Map))]
[JsonSerializable(typeof(MapFilters))]
[JsonSerializable(typeof(MinMax))]
[JsonSerializable(typeof(Misc))]
[JsonSerializable(typeof(MiscFilters))]
[JsonSerializable(typeof(ModOption))]
[JsonSerializable(typeof(NinjaCurDetails))]
[JsonSerializable(typeof(NinjaCurLines))]
[JsonSerializable(typeof(NinjaCurrencyContract))]
[JsonSerializable(typeof(NinjaItemContract))]
[JsonSerializable(typeof(NinjaItemLines))]
[JsonSerializable(typeof(NinjaValue))]
[JsonSerializable(typeof(OfferInfo))]
[JsonSerializable(typeof(OnlineStatus))]
[JsonSerializable(typeof(OptionTxt))]
[JsonSerializable(typeof(ParserData))]
[JsonSerializable(typeof(PoePrices))]
[JsonSerializable(typeof(PriceData))]
[JsonSerializable(typeof(Query))]
[JsonSerializable(typeof(ResultData))]
[JsonSerializable(typeof(Sanctum))]
[JsonSerializable(typeof(SocketFilters))]
[JsonSerializable(typeof(Socket))]
[JsonSerializable(typeof(Sort))]
[JsonSerializable(typeof(Armour))]
[JsonSerializable(typeof(Stats))]
[JsonSerializable(typeof(StatsFilters))]
[JsonSerializable(typeof(Trade))]
[JsonSerializable(typeof(TradeFilters))]
[JsonSerializable(typeof(TypeF))]
[JsonSerializable(typeof(TypeFilters))]
[JsonSerializable(typeof(Ultimatum))]
[JsonSerializable(typeof(UltimatumFilters))]
[JsonSerializable(typeof(Weapon))]
[JsonSerializable(typeof(WeaponFilters))]
[JsonSerializable(typeof(WordData))]
[JsonSerializable(typeof(WordResult))]
[JsonSerializable(typeof(WordResultData))]
[JsonSerializable(typeof(Exchange))]
[JsonSerializable(typeof(ExchangeData))]
[JsonSerializable(typeof(ExchangeStatus))]
public partial class SourceGenerationContext : JsonSerializerContext
{
    public static SourceGenerationContext ContextWithOptions { get; } = new(new JsonSerializerOptions
    {
        //ReferenceHandler = ReferenceHandler.IgnoreCycles,
        PropertyNameCaseInsensitive = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        AllowTrailingCommas = true
    });
}
