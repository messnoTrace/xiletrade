﻿using System.Collections.Generic;
using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using Xiletrade.Library.Services;
using Xiletrade.Library.Shared;

namespace Xiletrade.Library.Models.Serializable;

[DataContract]
public sealed class JsonData
{
    [DataMember(Name = "query")]
    [JsonPropertyName("query")]
    public Query Query { get; set; } = new();

    [DataMember(Name = "sort")]
    [JsonPropertyName("sort")]
    public Sort Sort { get; set; } = new();

    internal JsonData(XiletradeItem xiletradeItem, ItemBaseName currentItem, bool useSaleType, string market)
    {
        OptionTxt optTrue = new("true"), optFalse = new("false");

        bool errorsFilters = false;
        string Inherit = currentItem.Inherits.Length > 0 ? currentItem.Inherits[0] : string.Empty;
        string Inherit2 = currentItem.Inherits.Length > 1 ? currentItem.Inherits[1] : string.Empty;

        Query.Stats = [];

        if (xiletradeItem.ChkArmour || xiletradeItem.ChkEnergy || xiletradeItem.ChkEvasion || xiletradeItem.ChkWard)
        {
            if (xiletradeItem.ArmourMin.IsNotEmpty())
                Query.Filters.Armour.Filters.Armour.Min = xiletradeItem.ArmourMin;
            if (xiletradeItem.ArmourMax.IsNotEmpty())
                Query.Filters.Armour.Filters.Armour.Max = xiletradeItem.ArmourMax;
            if (xiletradeItem.EnergyMin.IsNotEmpty())
                Query.Filters.Armour.Filters.Energy.Min = xiletradeItem.EnergyMin;
            if (xiletradeItem.EnergyMax.IsNotEmpty())
                Query.Filters.Armour.Filters.Energy.Max = xiletradeItem.EnergyMax;
            if (xiletradeItem.EvasionMin.IsNotEmpty())
                Query.Filters.Armour.Filters.Evasion.Min = xiletradeItem.EvasionMin;
            if (xiletradeItem.EvasionMax.IsNotEmpty())
                Query.Filters.Armour.Filters.Evasion.Max = xiletradeItem.EvasionMax;
            if (xiletradeItem.WardMin.IsNotEmpty())
                Query.Filters.Armour.Filters.Ward.Min = xiletradeItem.WardMin;
            if (xiletradeItem.WardMax.IsNotEmpty())
                Query.Filters.Armour.Filters.Ward.Max = xiletradeItem.WardMax;

            Query.Filters.Armour.Disabled = false;
        }
        else
        {
            Query.Filters.Armour.Disabled = true;
        }

        if (xiletradeItem.ChkDpsTotal || xiletradeItem.ChkDpsPhys || xiletradeItem.ChkDpsElem)
        {
            if (xiletradeItem.ChkDpsTotal)
            {
                if (xiletradeItem.DpsTotalMin.IsNotEmpty())
                    Query.Filters.Weapon.Filters.Damage.Min = xiletradeItem.DpsTotalMin;
                if (xiletradeItem.DpsTotalMax.IsNotEmpty())
                    Query.Filters.Weapon.Filters.Damage.Max = xiletradeItem.DpsTotalMax;
            }
            if (xiletradeItem.ChkDpsPhys)
            {
                if (xiletradeItem.DpsPhysMin.IsNotEmpty())
                    Query.Filters.Weapon.Filters.Pdps.Min = xiletradeItem.DpsPhysMin;
                if (xiletradeItem.DpsPhysMax.IsNotEmpty())
                    Query.Filters.Weapon.Filters.Pdps.Max = xiletradeItem.DpsPhysMax;
            }
            if (xiletradeItem.ChkDpsElem)
            {
                if (xiletradeItem.DpsElemMin.IsNotEmpty())
                    Query.Filters.Weapon.Filters.Edps.Min = xiletradeItem.DpsElemMin;
                if (xiletradeItem.DpsElemMax.IsNotEmpty())
                    Query.Filters.Weapon.Filters.Edps.Max = xiletradeItem.DpsElemMax;
            }

            Query.Filters.Weapon.Disabled = false;
        }
        else
        {
            Query.Filters.Weapon.Disabled = true;
        }

        if (xiletradeItem.ChkResolve || xiletradeItem.ChkMaxResolve || xiletradeItem.ChkInspiration || xiletradeItem.ChkAureus)
        {
            if (xiletradeItem.ChkResolve)
            {
                if (xiletradeItem.ResolveMin.IsNotEmpty())
                    Query.Filters.Sanctum.Filters.Resolve.Min = xiletradeItem.ResolveMin;
                if (xiletradeItem.ResolveMax.IsNotEmpty())
                    Query.Filters.Sanctum.Filters.Resolve.Max = xiletradeItem.ResolveMax;
            }

            if (xiletradeItem.ChkMaxResolve)
            {
                if (xiletradeItem.MaxResolveMin.IsNotEmpty())
                    Query.Filters.Sanctum.Filters.MaxResolve.Min = xiletradeItem.MaxResolveMin;
                if (xiletradeItem.MaxResolveMax.IsNotEmpty())
                    Query.Filters.Sanctum.Filters.MaxResolve.Max = xiletradeItem.MaxResolveMax;
            }

            if (xiletradeItem.ChkInspiration)
            {
                if (xiletradeItem.InspirationMin.IsNotEmpty())
                    Query.Filters.Sanctum.Filters.Inspiration.Min = xiletradeItem.InspirationMin;
                if (xiletradeItem.InspirationMax.IsNotEmpty())
                    Query.Filters.Sanctum.Filters.Inspiration.Max = xiletradeItem.InspirationMax;
            }

            if (xiletradeItem.ChkAureus)
            {
                if (xiletradeItem.AureusMin.IsNotEmpty())
                    Query.Filters.Sanctum.Filters.Aureus.Min = xiletradeItem.AureusMin;
                if (xiletradeItem.AureusMax.IsNotEmpty())
                    Query.Filters.Sanctum.Filters.Aureus.Max = xiletradeItem.AureusMax;
            }

            Query.Filters.Sanctum.Disabled = false;
        }
        else
        {
            Query.Filters.Sanctum.Disabled = true;
        }

        Query.Status = new(market);
        Sort.Price = "asc";

        Query.Filters.Trade.Disabled = DataManager.Config.Options.SearchBeforeDay == 0;

        if (DataManager.Config.Options.SearchBeforeDay != 0)
        {
            Query.Filters.Trade.Filters.Indexed = new(BeforeDayToString(DataManager.Config.Options.SearchBeforeDay));
        }
        if (useSaleType)
        {
            Query.Filters.Trade.Filters.SaleType = new("priced");
        }
        /*
        Query.Filters.Trade.Filters.Price.Min = 99999;
        Query.Filters.Trade.Filters.Price.Max = 99999;
        */
        if (xiletradeItem.PriceMin > 0 && xiletradeItem.PriceMin.IsNotEmpty())
        {
            Query.Filters.Trade.Filters.Price.Min = xiletradeItem.PriceMin;
        }

        Query.Filters.Socket.Disabled = xiletradeItem.ChkSocket != true;

        if (xiletradeItem.LinkMin.IsNotEmpty())
            Query.Filters.Socket.Filters.Links.Min = xiletradeItem.LinkMin;
        if (xiletradeItem.LinkMax.IsNotEmpty())
            Query.Filters.Socket.Filters.Links.Max = xiletradeItem.LinkMax;

        if (xiletradeItem.SocketMin.IsNotEmpty())
            Query.Filters.Socket.Filters.Sockets.Min = xiletradeItem.SocketMin;
        if (xiletradeItem.SocketMax.IsNotEmpty())
            Query.Filters.Socket.Filters.Sockets.Max = xiletradeItem.SocketMax;

        if (xiletradeItem.SocketColors)
        {
            Query.Filters.Socket.Filters.Sockets.Red = xiletradeItem.SocketRed;
            Query.Filters.Socket.Filters.Sockets.Blue = xiletradeItem.SocketBlue;
            Query.Filters.Socket.Filters.Sockets.Green = xiletradeItem.SocketGreen;
            Query.Filters.Socket.Filters.Sockets.White = xiletradeItem.SocketWhite;
        }

        if (xiletradeItem.ChkQuality)
        {
            if (xiletradeItem.QualityMin.IsNotEmpty())
                Query.Filters.Misc.Filters.Quality.Min = xiletradeItem.QualityMin;
            if (xiletradeItem.QualityMax.IsNotEmpty())
                Query.Filters.Misc.Filters.Quality.Max = xiletradeItem.QualityMax;
        }

        if (xiletradeItem.FacetorExpMin.IsNotEmpty())
            Query.Filters.Misc.Filters.StoredExp.Min = xiletradeItem.FacetorExpMin;
        if (xiletradeItem.FacetorExpMax.IsNotEmpty())
            Query.Filters.Misc.Filters.StoredExp.Max = xiletradeItem.FacetorExpMax;

        if (!(!xiletradeItem.ChkLv || Inherit is Strings.Inherit.Gems || Inherit is Strings.Inherit.Maps || Inherit2 is Strings.Inherit.Area))
        {
            if (Inherit is not Strings.Inherit.Sanctum)
            {
                if (xiletradeItem.LvMin.IsNotEmpty())
                    Query.Filters.Misc.Filters.Ilvl.Min = xiletradeItem.LvMin;
                if (xiletradeItem.LvMax.IsNotEmpty())
                    Query.Filters.Misc.Filters.Ilvl.Max = xiletradeItem.LvMax;
            }
        }

        if (xiletradeItem.ChkLv && Inherit is Strings.Inherit.Gems)
        {
            if (xiletradeItem.LvMin.IsNotEmpty())
                Query.Filters.Misc.Filters.Gem_level.Min = xiletradeItem.LvMin;
            if (xiletradeItem.LvMax.IsNotEmpty())
                Query.Filters.Misc.Filters.Gem_level.Max = xiletradeItem.LvMax;
        }

        if (Inherit is Strings.Inherit.Gems && xiletradeItem.AlternateQuality is not null)
        {
            Query.Filters.Misc.Filters.Gem_alternate = new(xiletradeItem.AlternateQuality);
        }

        bool influenced = xiletradeItem.InfShaper || xiletradeItem.InfElder || xiletradeItem.InfCrusader
            || xiletradeItem.InfRedeemer || xiletradeItem.InfHunter || xiletradeItem.InfWarlord;

        /*
        Query.Filters.Misc.Filters.Synthesis.Option = Strings.any;
        Query.Filters.Misc.Filters.Split.Option = Strings.any;
        Query.Filters.Misc.Filters.Mirrored.Option = Strings.any;
        */
        //Query.Filters.Misc.Filters.Synthesis.Option = Inherit != Strings.Inherit.Maps && itemOptions.SynthesisBlight ? "true" : Strings.any;

        //Query.Filters.Misc.Filters.Corrupted.Option = itemOptions.Corrupt == 1 ? "true" : (itemOptions.Corrupt == 2 ? "false" : "any");

        if (xiletradeItem.Corrupted is "true")
        {
            Query.Filters.Misc.Filters.Corrupted = optTrue;
        }
        else if (xiletradeItem.Corrupted is "false")
        {
            Query.Filters.Misc.Filters.Corrupted = optFalse;
        }

        Query.Filters.Misc.Disabled = !(
            xiletradeItem.FacetorExpMin.IsNotEmpty() || xiletradeItem.FacetorExpMax.IsNotEmpty()
            || xiletradeItem.ChkQuality || Inherit is not Strings.Inherit.Maps && influenced
            || xiletradeItem.Corrupted is not Strings.any || Inherit is not Strings.Inherit.Maps
            && xiletradeItem.ChkLv || Inherit is not Strings.Inherit.Maps
            && (xiletradeItem.SynthesisBlight || xiletradeItem.BlightRavaged)
        );

        Query.Filters.Map.Disabled = !(
            (Inherit == Strings.Inherit.Maps || Inherit2 is Strings.Inherit.Area
            || Inherit is Strings.Inherit.Sanctum) && (xiletradeItem.ChkLv || xiletradeItem.SynthesisBlight
            || xiletradeItem.BlightRavaged || xiletradeItem.Scourged || influenced)
        );

        if (xiletradeItem.ChkLv && Inherit is Strings.Inherit.Maps)
        {
            if (xiletradeItem.LvMin.IsNotEmpty())
                Query.Filters.Map.Filters.Tier.Min = xiletradeItem.LvMin;
            if (xiletradeItem.LvMax.IsNotEmpty())
                Query.Filters.Map.Filters.Tier.Max = xiletradeItem.LvMax;
        }

        if (xiletradeItem.ChkLv && (Inherit2 is Strings.Inherit.Area || Inherit is Strings.Inherit.Sanctum))
        {
            if (xiletradeItem.LvMin.IsNotEmpty())
                Query.Filters.Map.Filters.Area.Min = xiletradeItem.LvMin;
            if (xiletradeItem.LvMax.IsNotEmpty())
                Query.Filters.Map.Filters.Area.Max = xiletradeItem.LvMax;
        }

        if (Inherit is Strings.Inherit.Maps)
        {
            if (xiletradeItem.InfShaper)
            {
                Query.Filters.Map.Filters.Shaper = optTrue;
            }
            if (xiletradeItem.SynthesisBlight)
            {
                Query.Filters.Map.Filters.Blight = optTrue;
            }
            if (xiletradeItem.InfElder)
            {
                Query.Filters.Map.Filters.Elder = optTrue;
            }
            if (xiletradeItem.BlightRavaged)
            {
                Query.Filters.Map.Filters.BlightRavaged = optTrue;
            }

            if (xiletradeItem.ChkMapIiq)
            {
                if (xiletradeItem.MapItemQuantityMin.IsNotEmpty())
                    Query.Filters.Map.Filters.Iiq.Min = xiletradeItem.MapItemQuantityMin;
                if (xiletradeItem.MapItemQuantityMax.IsNotEmpty())
                    Query.Filters.Map.Filters.Iiq.Max = xiletradeItem.MapItemQuantityMax;
            }
            if (xiletradeItem.ChkMapIir)
            {
                if (xiletradeItem.MapItemRarityMin.IsNotEmpty())
                    Query.Filters.Map.Filters.Iir.Min = xiletradeItem.MapItemRarityMin;
                if (xiletradeItem.MapItemRarityMax.IsNotEmpty())
                    Query.Filters.Map.Filters.Iir.Max = xiletradeItem.MapItemRarityMax;
            }
            if (xiletradeItem.ChkMapPack)
            {
                if (xiletradeItem.MapPackSizeMin.IsNotEmpty())
                    Query.Filters.Map.Filters.PackSize.Min = xiletradeItem.MapPackSizeMin;
                if (xiletradeItem.MapPackSizeMax.IsNotEmpty())
                    Query.Filters.Map.Filters.PackSize.Max = xiletradeItem.MapPackSizeMax;
            }
        }

        if (xiletradeItem.Scourged)
        {
            Query.Filters.Map.Filters.ScourgeTier.Min = 1;
            //Query.Filters.Map.Filters.ScourgeTier.Max = 99999;
        }

        Query.Filters.Ultimatum.Disabled = true;
        if (xiletradeItem.RewardType is not null && xiletradeItem.Reward is not null)
        {
            if (xiletradeItem.RewardType is Strings.Reward.DoubleCurrency or Strings.Reward.DoubleDivCards or Strings.Reward.MirrorRare or Strings.Reward.ExchangeUnique) // ultimatum
            {
                Query.Filters.Ultimatum.Disabled = false;
                Query.Filters.Ultimatum.Filters.Reward = new(xiletradeItem.RewardType);
                if (xiletradeItem.RewardType is Strings.Reward.DoubleCurrency or Strings.Reward.DoubleDivCards)
                {
                    Query.Filters.Ultimatum.Filters.Input = new(xiletradeItem.Reward);
                }
                if (xiletradeItem.RewardType is Strings.Reward.ExchangeUnique)
                {
                    Query.Filters.Ultimatum.Filters.Output = new(xiletradeItem.Reward);
                }
            }
            if (xiletradeItem.RewardType is Strings.Reward.FoilUnique) // valdo box
            {
                Query.Filters.Map.Filters.MapReward = new(xiletradeItem.Reward);
            }
        }

        if (xiletradeItem.ItemFilters.Count > 0)
        {
            Query.Stats = new Stats[1];
            Query.Stats[0] = new()
            {
                Type = "and",
                Filters = new StatsFilters[xiletradeItem.ItemFilters.Count]
            };

            int idx = 0;

            for (int i = 0; i < xiletradeItem.ItemFilters.Count; i++)
            {
                string input = xiletradeItem.ItemFilters[i].Text;
                string id = xiletradeItem.ItemFilters[i].Id;
                string type = xiletradeItem.ItemFilters[i].Id.Split('.')[0];
                if (input.Trim().Length > 0)
                {
                    string type_name = GetAffixType(type);

                    if (type_name.Length is 0)
                    {
                        continue; // will create a bad request as intended (to detect new type) and not crash the app 
                    }

                    FilterResultEntrie filter = null;

                    var filterResult = DataManager.Filter.Result.FirstOrDefault(x => x.Label == type_name);
                    type_name = type_name.ToLowerInvariant();
                    input = Regex.Escape(input).Replace("\\+\\#", "[+]?\\#");

                    System.Globalization.CultureInfo cultureEn = new(Strings.Culture[0]);
                    System.Resources.ResourceManager rm = new(typeof(Resources.Resources));

                    // For weapons, the pseudo_adds_ [a-z] + _ damage option is given on attack
                    string pseudo = rm.GetString("General014_Pseudo", cultureEn);

                    //bool isShako = DataManager.Words.FirstOrDefault(x => x.NameEn is "Forbidden Shako").Name == Modifier.CurrentItem.Name;
                    //if (type_name == pseudo && Inherit is Strings.Inherit.Weapons && Regex.IsMatch(id, @"^pseudo.pseudo_adds_[a-z]+_damage$"))
                    if (type_name == pseudo && Inherit is Strings.Inherit.Weapons && RegexUtil.AddsDamagePattern().IsMatch(id))
                    {
                        id += "_to_attacks";
                    }/*
                            else if (type_name != pseudo && (Inherit is Strings.Inherit.Weapons or Strings.Inherit.Armours) && !isShako)
                            {
                                // Is the equipment only option (specific)
                                Regex rgx = new("^" + input + "$", RegexOptions.IgnoreCase);
                                filter = filterResult.Entries.FirstOrDefault(x => rgx.IsMatch(x.Text) && x.Type == type);
                            }*/

                    filter ??= filterResult.Entries.FirstOrDefault(x => x.ID == id && x.Type == type); // && x.Part == null

                    Query.Stats[0].Filters[idx] = new() { Value = new() };
                    //Query.Stats[0].Filters[idx].Value.Option = 99999;

                    if (filter is not null && filter.ID is not null && filter.ID.Trim().Length > 0)
                    {
                        Query.Stats[0].Filters[idx].Disabled = xiletradeItem.ItemFilters[i].Disabled == true;

                        if (xiletradeItem.ItemFilters[i].Option != 0 && xiletradeItem.ItemFilters[i].Option.IsNotEmpty())
                        {
                            Query.Stats[0].Filters[idx].Value.Option = xiletradeItem.ItemFilters[i].Option.ToString();
                        }
                        else
                        {
                            if (xiletradeItem.ItemFilters[i].Min.IsNotEmpty())
                                Query.Stats[0].Filters[idx].Value.Min = xiletradeItem.ItemFilters[i].Min;
                            if (xiletradeItem.ItemFilters[i].Max.IsNotEmpty())
                                Query.Stats[0].Filters[idx].Value.Max = xiletradeItem.ItemFilters[i].Max;
                        }
                        Query.Stats[0].Filters[idx++].Id = filter.ID;
                    }
                    else
                    {
                        errorsFilters = true;
                        xiletradeItem.ItemFilters[i].IsNull = true;

                        // Add anything on null to avoid errors
                        //Query.Stats[0].Filters[idx].Disabled = true;
                        //Query.Stats[0].Filters[idx++].Id = "error_id";
                    }
                }
            }
        }

        // Set category here
        if (Strings.dicInherit.TryGetValue(Inherit, out string option))
        {
            if (xiletradeItem.ByType && Inherit is Strings.Inherit.Weapons or Strings.Inherit.Armours)
            {
                string[] lInherit = currentItem.Inherits;

                if (lInherit.Length > 2)
                {
                    string gearType = lInherit[Inherit is Strings.Inherit.Armours ? 1 : 2].ToLowerInvariant();

                    if (Inherit is Strings.Inherit.Weapons)
                    {
                        gearType = gearType.Replace("hand", string.Empty);
                        gearType = gearType.Remove(gearType.Length - 1);
                        if (gearType is "stave" && lInherit.Length is 4)
                        {
                            gearType = "staff";
                        }
                        else if (gearType is "onethrustingsword")
                        {
                            gearType = "onesword";
                        }
                    }
                    else if (Inherit is Strings.Inherit.Armours && gearType is "shields" or "helmets" or "bodyarmours")
                    {
                        gearType = gearType is "bodyarmours" ? "chest" : gearType.Remove(gearType.Length - 1);
                    }
                    option += "." + gearType;
                }
            }

            if (!xiletradeItem.ByType && Inherit is Strings.Inherit.Currency)
            {
                if (currentItem.TypeEn is "Forbidden Tome") // to redo
                {
                    option = "sanctum.research";
                }
            }

            Query.Filters.Type.Filters.Category = new(option); // Item category
        }

        string rarityEn = GetEnglishRarity(xiletradeItem.Rarity);
        if (rarityEn.Length > 0)
        {
            rarityEn = rarityEn is "Any N-U" ? "nonunique"
                : rarityEn is "Foil Unique" ? "uniquefoil"
                : rarityEn.ToLowerInvariant();
            if (rarityEn is not Strings.any)
            {
                Query.Filters.Type.Filters.Rarity = new(rarityEn);
            }
        }

        if (xiletradeItem.ByType || currentItem.Name.Length == 0 ||
            xiletradeItem.Rarity != Resources.Resources.General006_Unique
            && xiletradeItem.Rarity != Resources.Resources.General110_FoilUnique)
        {
            if (!xiletradeItem.ByType && Inherit is not Strings.Inherit.Jewels
                || Inherit is Strings.Inherit.NecropolisPack)
            {
                bool isTransfiguredGem = Inherit is Strings.Inherit.Gems && Inherit2.Length > 0 && Inherit2.Contains("alt");
                Query.Type = !isTransfiguredGem ? currentItem.Type : new GemTransfigured(currentItem.Type, Inherit2);
            }
        }
        else
        {
            Query.Name = currentItem.Name;
            Query.Type = currentItem.Type;
        }
        /*
        if (Inherit is Strings.Inherit.Gems && Inherit2.Length > 0 && Inherit2.Contains("alt"))
        {
            Query.Disc = new(Inherit2);
        }*/
        if (xiletradeItem.ChaosDivOnly)
        {
            Query.Filters.Trade.Disabled = false;
            Query.Filters.Trade.Filters.Price.Option = new("chaos_divine");
        }

        if (errorsFilters)
        {
            int errorCount = 0;
            List<int> errors = new();
            for (int i = 0; i < xiletradeItem.ItemFilters.Count; i++)
            {
                if (xiletradeItem.ItemFilters[i].IsNull)
                {
                    errorCount++;
                    errors.Add(i + 1);
                }
            }
            throw new Exception(string.Format("{0} Mod error(s) detected: \r\n\r\nMod lines : {1}\r\n\r\n", errorCount, errors.ToString()));
        }
    }

    private static string BeforeDayToString(int day)
    {
        if (day < 3) return "1day";
        if (day < 7) return "3days";
        if (day < 14) return "1week";
        return "2weeks";
    }

    private static string GetEnglishRarity(string rarityLang)
    {
        System.Globalization.CultureInfo cultureEn = new(Strings.Culture[0]);
        System.Resources.ResourceManager rm = new(typeof(Resources.Resources));

        return rarityLang == Resources.Resources.General005_Any ? rm.GetString("General005_Any", cultureEn) :
            rarityLang == Resources.Resources.General110_FoilUnique ? rm.GetString("General110_FoilUnique", cultureEn) :
            rarityLang == Resources.Resources.General006_Unique ? rm.GetString("General006_Unique", cultureEn) :
            rarityLang == Resources.Resources.General007_Rare ? rm.GetString("General007_Rare", cultureEn) :
            rarityLang == Resources.Resources.General008_Magic ? rm.GetString("General008_Magic", cultureEn) :
            rarityLang == Resources.Resources.General009_Normal ? rm.GetString("General009_Normal", cultureEn) :
            rarityLang == Resources.Resources.General010_AnyNU ? rm.GetString("General010_AnyNU", cultureEn) : string.Empty;
    }

    private static string GetAffixType(string inputType)
    {
        return inputType is "pseudo" ? Resources.Resources.General014_Pseudo :
            inputType is "explicit" ? Resources.Resources.General015_Explicit :
            inputType is "fractured" ? Resources.Resources.General016_Fractured :
            inputType is "crafted" ? Resources.Resources.General012_Crafted :
            inputType is "implicit" ? Resources.Resources.General013_Implicit :
            inputType is "enchant" ? Resources.Resources.General011_Enchant :
            inputType is "monster" ? Resources.Resources.General018_Monster :
            inputType is "veiled" ? Resources.Resources.General019_Veiled :
            inputType is "delve" ? Resources.Resources.General020_Delve :
            inputType is "ultimatum" ? Resources.Resources.General069_Ultimatum :
            inputType is "scourge" ? Resources.Resources.General099_Scourge :
            inputType is "crucible" ? Resources.Resources.General112_Crucible :
            inputType is "necropolis" ? Resources.Resources.General131_Necropolis :
            inputType is "sanctum" ? Resources.Resources.General111_Sanctum : string.Empty;
    }
}
