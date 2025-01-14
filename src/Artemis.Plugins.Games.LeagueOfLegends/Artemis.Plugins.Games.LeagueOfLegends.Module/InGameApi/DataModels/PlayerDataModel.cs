﻿using Artemis.Core.ColorScience;
using Artemis.Core.Modules;
using Artemis.Plugins.Games.LeagueOfLegends.Module.InGameApi.DataModels.Enums;
using Artemis.Plugins.Games.LeagueOfLegends.Module.InGameApi.GameDataModels;
using Artemis.Plugins.Games.LeagueOfLegends.Module.Utils;
using System;
using SummonerSpell = Artemis.Plugins.Games.LeagueOfLegends.Module.InGameApi.DataModels.Enums.SummonerSpell;
using ChampionEnum = Artemis.Plugins.Games.LeagueOfLegends.Module.InGameApi.DataModels.Enums.Champion;
using Rune = Artemis.Plugins.Games.LeagueOfLegends.Module.InGameApi.DataModels.Enums.Rune;

namespace Artemis.Plugins.Games.LeagueOfLegends.Module.InGameApi.DataModels;

public class PlayerDataModel : DataModel
{
    public AbilityGroupDataModel Abilities { get; } = new();
    public PlayerStatsDataModel ChampionStats { get; } = new();
    public InventoryDataModel Inventory { get; } = new();
    public RunesDataModel Runes { get; } = new();
    public string SummonerName { get; set; }
    public int Level { get; set; }
    public float Gold { get; set; }
    public int Kills { get; set; }
    public int Deaths { get; set; }
    public int Assists { get; set; }
    public int CreepScore { get; set; }
    public float WardScore { get; set; }
    public float RespawnTimer { get; set; }
    public bool IsDead { get; set; }
    public Team Team { get; set; }
    public ChampionEnum Champion { get; set; }
    public Position Position { get; set; }
    public SummonerSpell SpellD { get; set; }
    public SummonerSpell SpellF { get; set; }
    public ColorSwatch ChampionColors { get; set; }

    [DataModelIgnore] public string ShortChampionName { get; set; }
    [DataModelIgnore] public int SkinID { get; set; }

    public void Update(RootGameData rootGameData)
    {
        var allPlayer = Array.Find(rootGameData.AllPlayers, p => p.SummonerName == rootGameData.ActivePlayer.SummonerName);
        if (allPlayer == null)
            return;

        SummonerName = rootGameData.ActivePlayer.SummonerName;
        Team = ParseEnum<Team>.TryParseOr(allPlayer.Team, Team.Unknown);
        Champion = ParseEnum<ChampionEnum>.TryParseOr(allPlayer.RawChampionName, ChampionEnum.Unknown);
        Position = ParseEnum<Position>.TryParseOr(allPlayer.Position, Position.Unknown);

        SkinID = allPlayer.SkinID;
        ShortChampionName = allPlayer.RawChampionName[27..];

        Abilities.Update(rootGameData.ActivePlayer.Abilities);
        ChampionStats.Update(rootGameData.ActivePlayer.ChampionStats);
        Inventory.Update(allPlayer.Items);
        Runes.Update(allPlayer.Runes);

        Level = rootGameData.ActivePlayer.Level;
        Gold = rootGameData.ActivePlayer.CurrentGold;

        Kills = allPlayer.Scores.Kills;
        Deaths = allPlayer.Scores.Deaths;
        Assists = allPlayer.Scores.Assists;
        CreepScore = allPlayer.Scores.CreepScore;
        WardScore = allPlayer.Scores.WardScore;
        RespawnTimer = allPlayer.RespawnTimer;
        IsDead = allPlayer.IsDead;

        SpellD = ParseEnum<SummonerSpell>.TryParseOr(allPlayer.SummonerSpells.SummonerSpellOne?.RawDisplayName, SummonerSpell.Unknown);
        SpellF = ParseEnum<SummonerSpell>.TryParseOr(allPlayer.SummonerSpells.SummonerSpellTwo?.RawDisplayName, SummonerSpell.Unknown);
    }
}

public class RunesDataModel : DataModel
{
    public Rune PrimaryTree { get; set; }
    public Rune SecondaryTree { get;set;  }
    public Rune Keystone { get; set; }
    
    public void Update(Runes runes)
    {
        //I'm sure it's fiiiiine
        PrimaryTree =  (Rune)runes.PrimaryRuneTree.Id;
        SecondaryTree = (Rune)runes.SecondaryRuneTree.Id;
        Keystone = (Rune)runes.Keystone.Id;
    }
}