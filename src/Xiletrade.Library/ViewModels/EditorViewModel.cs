using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Input;
using Xiletrade.Library.Models.Collections;
using Xiletrade.Library.Models.Serializable;
using Xiletrade.Library.Services;
using Xiletrade.Library.Services.Interface;
using Xiletrade.Library.Shared;
using Xiletrade.Library.ViewModels.Command;

namespace Xiletrade.Library.ViewModels;

public sealed class EditorViewModel : BaseViewModel
{
    private static IServiceProvider _serviceProvider;
    private AsyncObservableCollection<ConfigMods> dangerousMods = new();
    private AsyncObservableCollection<ConfigMods> rareMods = new();
    private AsyncObservableCollection<ModOption> parser = new();
    private AsyncObservableCollection<ModFilterViewModel> filter = new();
    private string configlocation;
    private string parserlocation;
    private string filterlocation;
    private string searchField;
    private string queryItemText;
    private readonly DelegateCommand saveChanges;
    private readonly DelegateCommand initVm;
    private readonly DelegateCommand searchFilter;
    private readonly DelegateCommand queryItem;

    public AsyncObservableCollection<ConfigMods> DangerousMods { get => dangerousMods; set => SetProperty(ref dangerousMods, value); }
    public AsyncObservableCollection<ConfigMods> RareMods { get => rareMods; set => SetProperty(ref rareMods, value); }
    public AsyncObservableCollection<ModOption> Parser { get => parser; set => SetProperty(ref parser, value); }
    public AsyncObservableCollection<ModFilterViewModel> Filter { get => filter; set => SetProperty(ref filter, value); }
    public string Configlocation { get => configlocation; set => SetProperty(ref configlocation, value); }
    public string ParserLocation { get => parserlocation; set => SetProperty(ref parserlocation, value); }
    public string Filterlocation { get => filterlocation; set => SetProperty(ref filterlocation, value); }
    public string SearchField { get => searchField; set => SetProperty(ref searchField, value); }

    public string QueryItemText { get => queryItemText;set=>SetProperty(ref queryItemText, value); }


    public ICommand SaveChanges => saveChanges;
    public ICommand InitVm => initVm;
    public ICommand SearchFilter => searchFilter;

    public ICommand QueryItem => queryItem;

    public EditorViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        saveChanges = new(OnSaveChanges, CanSaveChanges);
        initVm = new(OnInitVm, CanInitVm);
        searchFilter = new(OnSearchFilter, CanSearchFilter);
        queryItem = new(OnQueryItem,CanQueryItem);

        string dataPath = System.IO.Path.GetFullPath("Data\\");
        StringBuilder sb = new(dataPath);
        sb.Append("Lang\\")
          .Append(Strings.Culture[DataManager.Config.Options.Language])
          .Append("\\");

        Configlocation = dataPath + Strings.File.Config;
        ParserLocation = sb.ToString() + Strings.File.ParsingRules;
        Filterlocation = sb.ToString() + Strings.File.Filters;

        OnInitVm(null);
    }

    private bool CanSaveChanges(object commandParameter)
    {
        return true;
    }

    private void OnSaveChanges(object commandParameter)
    {
        DataManager.Parser.Mods = Parser.Where(x => x.Replace is "equals" or "contains" && x.Old.Length > 0 && x.New.Length > 0).ToArray();
        string fileToSave = Json.Serialize<ParserData>(DataManager.Parser);
        DataManager.Save_File(fileToSave, ParserLocation);

        DataManager.Config.DangerousMapMods = DangerousMods.Where(x => x.Id.Length > 0 && x.Id.Contains("stat_")).ToArray();
        DataManager.Config.RareItemMods = RareMods.Where(x => x.Id.Length > 0 && x.Id.Contains("stat_")).ToArray();
        fileToSave = Json.Serialize<ConfigData>(DataManager.Config);
        DataManager.Save_File(fileToSave, Configlocation);
    }

    private bool CanInitVm(object commandParameter)
    {
        return true;
    }

    private void OnInitVm(object commandParameter)
    {
        Parser.Clear();
        foreach (var modOption in DataManager.Parser.Mods)
        {
            ModOption mod = new()
            {
                Id = modOption.Id,
                New = modOption.New,
                Old = modOption.Old,
                Replace = modOption.Replace,
                Stat = modOption.Stat
            };
            Parser.Add(mod);
        }
        SearchField = string.Empty;
        Filter.Clear();

        //if (DataManager.Config.DangerousMods.FirstOrDefault(x => x.Text == ifilter.Text && x.ID.IndexOf(inherit + "/", StringComparison.Ordinal) > -1) != null)
        DangerousMods.Clear();
        foreach (var modOption in DataManager.Config.DangerousMapMods)
        {
            ConfigMods mod = new()
            {
                Id = modOption.Id,
                Text = modOption.Text
            };
            DangerousMods.Add(mod);
        }

        RareMods.Clear();
        foreach (var modOption in DataManager.Config.RareItemMods)
        {
            ConfigMods mod = new()
            {
                Id = modOption.Id,
                Text = modOption.Text
            };
            RareMods.Add(mod);
        }
    }
    private bool CanSearchFilter(object commandParameter)
    {
        return true;
    }

    private void OnSearchFilter(object commandParameter)
    {
        Filter.Clear();
        if (SearchField.Length > 0)
        {
            var entriesMerge =
                from result in DataManager.Filter.Result
                from Entrie in result.Entries
                select Entrie;
            if (entriesMerge.Any())
            {
                var entrieMatches =
                    from result in entriesMerge
                    where result.Text.Contains(SearchField, System.StringComparison.Ordinal)
                    select result;
                if (entrieMatches.Any())
                {
                    int nb = 0;
                    foreach (FilterResultEntrie match in entrieMatches)
                    {
                        ModFilterViewModel newEntrie = new()
                        {
                            Num = nb,
                            Id = match.ID,
                            Type = match.Type,
                            Text = match.Text
                        };
                        Filter.Add(newEntrie);
                        nb++;
                    }
                }
            }
        }
    }


    private bool CanQueryItem(object commadParameter) {
        return true;

    }

    private void OnQueryItem(object commandParameter) {

        if (string.IsNullOrEmpty(QueryItemText)) {

            DataManager.showTest("物品信息为空");
            return;
        }
        var vm = _serviceProvider.GetRequiredService<MainViewModel>();
        if (vm.Logic.Task.Price.CoolDown.IsEnabled)
        {
            _serviceProvider.GetRequiredService<INavigationService>().ShowMainView();
            return;
        }
        vm.Logic.Task.HandlePriceCheckSpam();

        try
        {

            ;
            string clipText = Encoding.UTF8.GetString(Convert.FromBase64String(QueryItemText));
            string clipTextAdvanced = clipText;
            var sub = clipText[..clipText.IndexOf(Strings.ItemInfoDelimiterCRLF)];
            clipText = sub + clipTextAdvanced.Remove(0, clipTextAdvanced.IndexOf(Strings.ItemInfoDelimiterCRLF));
            vm.Logic.Task.UpdateMainViewModel(clipText, true);
     
        }
        catch (COMException ex) // for now : do not re-throw exception
        {
            if (ex.Message.Contains("0x800401D0", StringComparison.Ordinal)) // CLIPBRD_E_CANT_OPEN 
            {
                //Shared.Util.Helper.Debug.Trace("Can not access clipboard : " + ex.Message);
                return;
            }
            //Shared.Util.Helper.Debug.Trace("COMException catched : " + ex.Message);
        }
        catch (Exception ex) // do not re-throw exception
        {
            DataManager.showTest(ex.Message);
            //Shared.Util.Helper.Debug.Trace("Exception while parsing data : " + ex.Message);
        }
    }
}
