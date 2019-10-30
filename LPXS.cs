using System;
using Rocket.Unturned.Player;
using Rocket.Unturned.Chat;
using fr34kyn01535.Uconomy;
using System.Data;
using System.Collections.Generic;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;

namespace LPXRemastered
{
    public class LPXS : UnturnedPlayerComponent
    {
        private DateTime lastPaid;
        public delegate void PlayerPaidEvent(UnturnedPlayer player, decimal amount);
        public event PlayerPaidEvent OnPlayerPaid;
        public Dictionary<string, decimal> PayGroups = new Dictionary<string, decimal>();
        protected void Start()
        {
            lastPaid = DateTime.Now;
        }
        public void FixedUpdate()
        {
            if (LPXRemastered.Instance.Configuration.Instance.IncomeEnabled && (DateTime.Now - lastPaid).TotalSeconds >= LPXRemastered.Instance.Configuration.Instance.IncomeInterval)
            {
                PayGroups.Clear();
                lastPaid = DateTime.Now;
                decimal pay = 0.0m;
                string paygroup = "Player";
                DataTable dt;
                dt = LPXRemastered.Instance.Database.GetGroup();
                string[] stringArr = new string[dt.Rows.Count];
                List<string> plgroups = new List<string>();
                for (int i = 0; i < (dt.Rows.Count); i++)
                {
                    stringArr[i] += dt.Rows[i].ItemArray[0].ToString();
                    PayGroups.Add(stringArr[i], (LPXRemastered.Instance.Database.GetAllGroupIncome())[i]);
                    plgroups.Add(stringArr[i]);
                }
                decimal pay2 = 0.0m;
                foreach (string s in plgroups)
                {
                    PayGroups.TryGetValue(s, out pay2);
                    if (LPXRemastered.Instance.Database.CheckUserGroup(Player.Id).ToLower() == s.ToLower())
                    {
                        pay = pay2;
                        paygroup = s;
                        if (s == "defualt") paygroup = "Player";
                    }
                }
                if (pay == 0.0m)
                {
                    // We assume they are default group.
                    PayGroups.TryGetValue("default", out pay);
                    if (pay == 0.0m)
                    {
                        // There was an error.  End it.
                        Logger.Log(LPXRemastered.Instance.Translate("unable_to_pay_group_msg", new object[] { Player.CharacterName, "" }));
                        return;
                    }
                }
                if (paygroup == "default") paygroup = "Player";
                decimal bal = Uconomy.Instance.Database.IncreaseBalance(Player.CSteamID.ToString(), pay);
                OnPlayerPaid?.Invoke(Player, pay);
                this.Player.Player.gameObject.SendMessage("UEOnPlayerPaid", new object[] { Player.Player, pay });
                UnturnedChat.Say(Player.CSteamID, LPXRemastered.Instance.Translate("pay_time_msg", new object[] { pay, Uconomy.Instance.Configuration.Instance.MoneyName, paygroup }));
                if (bal >= 0.0m) UnturnedChat.Say(Player.CSteamID, LPXRemastered.Instance.Translate("new_balance_msg", new object[] { bal, Uconomy.Instance.Configuration.Instance.MoneyName }));
                PayGroups.Clear();
                string[] permission;
                string[] cmd;
                permission = LPXRemastered.Instance.Database.GetPermission(Player.Id);
                for (int i = permission.Length - 1; i >= 0; i--)
                {
                    if (permission[i].Contains("color.") && !(Player.IsAdmin))
                    {
                        cmd = permission[i].Split('.');
                        Color? color = UnturnedChat.GetColorFromName(cmd[1], Color.white);
                        this.Player.Color = color.Value;
                    }
                }
                
            }
        }
    }
}
