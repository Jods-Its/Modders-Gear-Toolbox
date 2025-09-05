using Il2Cpp;
using Il2CppProCore.Decals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GearToolbox
{
    internal class TBFunctionalities
    {
        internal static string dismantleText;
        private static GameObject dismantleButton;
        internal static GearItem gunItem;
        internal static string gunName;

        internal static string replenishText;
        internal static GameObject replenishButton;
        internal static GearItem sewingKit;
        internal static void InitializeMTB(ItemDescriptionPage itemDescriptionPage)
        {
            dismantleText = Localization.Get("GAMEPLAY_MTB_DismantleLabel");
            replenishText = Localization.Get("GAMEPLAY_MTB_ReplenishLabel");

            GameObject equipButton = itemDescriptionPage.m_MouseButtonEquip;
            dismantleButton = UnityEngine.Object.Instantiate<GameObject>(equipButton, equipButton.transform.parent, true);
            dismantleButton.transform.Translate(0, -0.1f, 0);
            Utils.GetComponentInChildren<UILabel>(dismantleButton).text = dismantleText;

            replenishButton = UnityEngine.Object.Instantiate<GameObject>(equipButton, equipButton.transform.parent, true);
            replenishButton.transform.Translate(0.345f, 0, 0);
            Utils.GetComponentInChildren<UILabel>(replenishButton).text = replenishText;

            AddAction(dismantleButton, new System.Action(OnDismantleGun));
            AddAction(replenishButton, new System.Action(OnReplenishSK));

            SetDismantleGunActive(false);
            SetReplenishSkActive(false);

        }
        private static void AddAction(GameObject button, System.Action action)
        {
            Il2CppSystem.Collections.Generic.List<EventDelegate> placeHolderList = new Il2CppSystem.Collections.Generic.List<EventDelegate>();
            placeHolderList.Add(new EventDelegate(action));
            Utils.GetComponentInChildren<UIButton>(button).onClick = placeHolderList;
        }
        internal static void SetDismantleGunActive(bool active)
        {
            NGUITools.SetActive(dismantleButton, active);
        }
        internal static void SetReplenishSkActive(bool active)
        {
            NGUITools.SetActive(replenishButton, active);
        }
        private static void OnDismantleGun()
        {
            var thisGearItem = TBFunctionalities.gunItem;
            gunName = thisGearItem.gameObject.name;
            

            if (thisGearItem == null) return;

            if (GameManager.GetInventoryComponent().GearInInventory(ToolboxUtils.tools1Name, 1) || GameManager.GetInventoryComponent().GearInInventory(ToolboxUtils.tools2Name, 1))
            {
                GameAudioManager.PlayGuiConfirm();
                InterfaceManager.GetPanel<Panel_GenericProgressBar>().Launch(Localization.Get("GAMEPLAY_MTB_DismantleProgressBar"), 5f, 0f, 0f,
                                "Play_HarvestingGeneric", null, false, true, new System.Action<bool, bool, float>(OnDismantleGunFinished));
                GameManager.GetInventoryComponent().RemoveGearFromInventory(gunName, 1);
            }
            else
            {
                HUDMessage.AddMessage(Localization.Get("GAMEPLAY_MTB_NoDismantle"));
                GameAudioManager.PlayGUIError();
            }

        }
        private static void OnDismantleGunFinished(bool success, bool playerCancel, float progress)
        {
            if (gunName.ToLowerInvariant().Contains("revolver"))
            {
                GameManager.GetPlayerManagerComponent().InstantiateItemInPlayerInventory(ToolboxUtils.gunParts, 1);
                GameManager.GetPlayerManagerComponent().InstantiateItemInPlayerInventory(ToolboxUtils.scrap, 1);
                GameManager.GetPlayerManagerComponent().InstantiateItemInPlayerInventory(ToolboxUtils.wood1, 1);
            }
            else
            {
                GameManager.GetPlayerManagerComponent().InstantiateItemInPlayerInventory(ToolboxUtils.gunParts, 3);
                GameManager.GetPlayerManagerComponent().InstantiateItemInPlayerInventory(ToolboxUtils.scrap, 2);
                GameManager.GetPlayerManagerComponent().InstantiateItemInPlayerInventory(ToolboxUtils.wood2, 1);
            }
        }
        private static void OnReplenishSK()
        {
            var thisGearItem = TBFunctionalities.sewingKit;
            GameObject player = ToolboxUtils.GetPlayer();

            if (thisGearItem == null) return;
            if (thisGearItem.m_CurrentHP > 50)
            {
                HUDMessage.AddMessage(Localization.Get("GAMEPLAY_MTB_HighCondition"));
                GameAudioManager.PlayGUIError();
                return;
            }
            if (GameManager.GetInventoryComponent().GearInInventory(ToolboxUtils.bundleStringName, 1))
            {
                GameAudioManager.PlayGuiConfirm();
                thisGearItem.m_CurrentHP = thisGearItem.m_CurrentHP + 25;
                if (player != null)
                {
                    GameAudioManager.PlaySound("PLAY_SNDINVGENERICTINY", player);
                }
                GameManager.GetInventoryComponent().RemoveGearFromInventory(ToolboxUtils.bundleString.name, 1);
            }
            else
            {
                HUDMessage.AddMessage(Localization.Get("GAMEPLAY_MTB_NoString"));
                GameAudioManager.PlayGUIError();
            }

        }

    }
}
