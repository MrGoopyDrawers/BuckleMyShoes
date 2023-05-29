using MelonLoader;
using BTD_Mod_Helper;
using BuckleMyShoes;
using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models;
using Il2CppAssets.Scripts.Simulation.Objects;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppAssets.Scripts.Simulation.Towers.Weapons;
using UnityEngine;
using Random = System.Random;
using System.Collections.Generic;
using System.Linq;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.TowerSets;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Api.Towers;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.Display;
using BTD_Mod_Helper.Api.Display;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.GenericBehaviors;
using Il2CppAssets.Scripts.Simulation.SMath;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities;
using Il2CppAssets.Scripts.Models.Towers.TowerFilters;
using Il2CppAssets.Scripts.Models.Map;
using Il2CppAssets.Scripts.Models.Audio;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
using Il2CppAssets.Scripts.Models.Towers.Weapons.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using HarmonyLib;
using Il2Cpp;

[assembly: MelonInfo(typeof(BuckleMyShoes.BuckleMyShoes), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace BuckleMyShoes;

public class BuckleMyShoes : BloonsTD6Mod
{
    public override void OnApplicationStart()
    {
        ModHelper.Msg<BuckleMyShoes>("BuckleMyShoes loaded!");
    }
    public override void OnTowerCreated(Tower tower, Entity target, Model modelToUse)
    {
        if (tower.model.name.Contains("BuckleMyShoesTower"))
        {
            ModContent.GetAudioClip<BuckleMyShoes>("BuckleMyShoesSound").Play();
        }
    }
    public class BuckleMyShoesTower : ModTower
    {


        public override TowerSet TowerSet => TowerSet.Primary;
        public override bool Use2DModel => true;
        public override string BaseTower => TowerType.DartMonkey;
        public override int Cost => 500;

        public override int TopPathUpgrades => 5;
        public override int MiddlePathUpgrades => 0;
        public override int BottomPathUpgrades => 0;

        public override string Icon => "BuckleMyShoeIcon";

        public override string Portrait => "BuckleMyShoeIcon";
        public override string Description => "1 2 buckle my shoes! 3 4 buckle some more! 5 6 nike kicks! Ahaha THATS SO FIRE!!!";
        public override string DisplayName => "1 2 Buckle My Shoes";
        public override bool IsValidCrosspath(int[] tiers) => ModHelper.HasMod("Ultimate Crosspathing") ? true : base.IsValidCrosspath(tiers);
        public override void ModifyBaseTowerModel(TowerModel towerModel)
        {
            towerModel.range += 10;
            var attackModel = towerModel.GetAttackModel();
            attackModel.range += 10;
            attackModel.RemoveWeapon(attackModel.weapons[0]);
            attackModel.AddWeapon(Game.instance.model.GetTowerFromId("Gwendolin 4").GetAttackModel().weapons[0].Duplicate());
            attackModel.weapons[0].projectile.pierce += 2;
            attackModel.weapons[0].rate *= 1.2f;

        }
        public override string Get2DTexture(int[] tiers)
        {
            return "BuckleMyShoeIcon";
        }
        public class BetterKicks : ModUpgrade<BuckleMyShoesTower>
        {
            public override int Path => TOP;
            public override int Tier => 1;
            public override int Cost => 350;
            public override string Icon => "BuckleMyShoes1";
            public override string DisplayName => "Better Kicks";

            public override string Description => "Better Kicks make the shoes shoot more fire.";

            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                attackModel.weapons[0].emission = new ArcEmissionModel("ArcEmissionModel_", 3, 0.0f, 25.0f, null, false);
            }
        }
        public class MrBeastKicks : ModUpgrade<BuckleMyShoesTower>
        {
            public override int Path => TOP;
            public override int Tier => 2;
            public override int Cost => 750;
            public override string Icon => "buckleMyShoes2";
            public override string DisplayName => "MrBeast kicks";

            public override string Description => "MrBeast kicks are very expensive and drippy making damage go UP!";

            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                attackModel.weapons[0].projectile.GetDamageModel().damage *= 3;
            }
        }
        public class NerdyKicks : ModUpgrade<BuckleMyShoesTower>
        {
            public override int Path => TOP;
            public override int Tier => 3;
            public override int Cost => 1250;
            public override string Icon => "BuckleMyShoes3";
            public override string DisplayName => "Nerdy Kicks";

            public override string Description => "According to my Caculations.... I will commerate these bloons to ultimate fall!";

            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                attackModel.weapons[0].emission = new ArcEmissionModel("ArcEmissionModel_", 5, 0.0f, 25.0f, null, false);
                attackModel.weapons[0].projectile.AddBehavior(new WindModel("NerdKicks", 0, 200, 50, false, null, 0, null, 1));
            }
        }
        public class PickupTheSticks : ModUpgrade<BuckleMyShoesTower>
        {
            public override int Path => TOP;
            public override int Tier => 4;
            public override int Cost => 8888;
            public override string Icon => "bucklemyshoes4";
            public override string DisplayName => "Pick up the sticks!";

            public override string Description => "Pick up the sticks to burn for extra fire, that damages purple bloons!";

            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                attackModel.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                attackModel.weapons[0].projectile.GetDamageModel().damage *= 2;
                attackModel.weapons[0].rate *= 0.3f;
                attackModel.weapons[0].projectile.AddBehavior(new AddBonusDamagePerHitToBloonModel("ouchyNerd","_ouchyNerd",10f,10f,5,true,true,false));

            }
        }
        public class MetaverseShoes : ModUpgrade<BuckleMyShoesTower>
        {
            public override int Path => TOP;
            public override int Tier => 5;
            public override int Cost => 9000000;
            public override string Icon => "buckleShoesMeta";
            public override string DisplayName => "MetaverseShoes";

            public override string Description => "Shoes from the metaverse activate intense drip...";

            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var attackModel = towerModel.GetAttackModel();
                attackModel.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                attackModel.weapons[0].projectile.GetDamageModel().damage *= 2000;
                attackModel.weapons[0].rate *= 0.01f;
                attackModel.weapons[0].projectile.GetBehavior<AddBonusDamagePerHitToBloonModel>().perHitDamageAddition *= 29349;
                attackModel.weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("MOABtag", "Bad", 10000f, 100000f, false, true));
                attackModel.weapons[0].projectile.AddBehavior(new DamageModifierWrathModel("dmg",1,1000000,100000));
                attackModel.weapons[0].projectile.pierce *= 2340;
                attackModel.weapons[0].projectile.GetBehavior<WindModel>().affectMoab = true;
                towerModel.AddBehavior(Game.instance.model.GetParagonTower("MonkeyBuccaneer").GetAbility().Duplicate());
                towerModel.GetAbility().activateOnPreLeak = true;
                towerModel.GetAbility().cooldown *= 0.00001f;
                towerModel.AddBehavior(Game.instance.model.GetTowerFromId("Druid-050").GetAbility().Duplicate());
                towerModel.GetAbility(1).cooldown *= 0.000001f;
                towerModel.GetAbility(1).activateOnPreLeak = true;
                towerModel.GetAbility(1).GetBehavior<BonusLivesOnAbilityModel>().amount *= 12349f;
                towerModel.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
            }
        }
    }
   }