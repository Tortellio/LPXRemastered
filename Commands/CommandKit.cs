using Rocket.Unturned.Player;
using Rocket.Unturned.Chat;
using System;
using Rocket.API;
using System.Linq;
using System.Data;
using System.Collections.Generic;
using Logger = Rocket.Core.Logging.Logger;

namespace LPXRemastered
{
    public class CommandKit : IRocketCommand
    {
        
        public string Help
        {
            get { return "Give Kits to player"; }
        }

        public string Name
        {
            get { return "kit"; }
        }

        public AllowedCaller AllowedCaller
        {
            get
            {
                return AllowedCaller.Player;
            }
        }

        public string Syntax
        {
            get { return "<Kit Name>"; }
        }

        public List<string> Aliases
        {
            get { return new List<string>(); }
        }

        public List<string> Permissions
        {
            get
            {
                return new List<string>() { "kit" , "kit.*"};
            }
        }

        public void Execute(IRocketPlayer caller,params string[] command)
        {
            if (!LPXRemastered.Instance.Configuration.Instance.KitsEnabled) return;
            string[] permission = { };
            bool hasPerm = false;
            UnturnedPlayer player= (UnturnedPlayer)caller;
            permission = LPXRemastered.Instance.Database.GetGroupPermission(LPXRemastered.Instance.Database.CheckUserGroup(caller.Id));
            if (command.Length == 0)
            {
                UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("kit_help"));
                return;
            }
            if (command.Length > 0)
            {
                string kitnamePermission = "";
                if (command.Length > 1)
                {                 
                    for (int x = 0; x < command.Length; x++)
                    {
                        kitnamePermission += command[x];
                    }
                }
                if (LPXRemastered.Instance.Configuration.Instance.LPXEnabled)
                {
                    for (int i = permission.Length - 1; i >= 0; i--)
                    {
                        if (permission[i] == "kit.*" || (permission[i] == ("kit." + command[0]) && command.Length == 1) || permission[i] == "*")
                        {
                            hasPerm = true;
                        }
                        if (command.Length > 1)
                        {
                            if (permission[i] == ("kit." + kitnamePermission))
                                hasPerm = true;
                        }
                    }
                }
                else
                {
                    if (player.HasPermission("kit.*") || player.HasPermission("kit." + kitnamePermission) || player.HasPermission("kit.*") || player.HasPermission("*"))
                    {
                        hasPerm = true;
                    }
                }
                string comd = String.Join(" ", command);
                if (!hasPerm && !(caller.IsAdmin))
                {
                    UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_no_perm"));
                    return;
                }                               
                else
                {
                    hasPerm = false;
                    KeyValuePair<string, DateTime> playerCooldown = LPXRemastered.PlayerCooldown.Where(z => z.Key == caller.Id).FirstOrDefault();
                    
                    if (command.Length == 1)
                    {
                        if (LPXRemastered.Instance.Database.CheckKit(command[0]))
                        {
                            double cooldown = LPXRemastered.Instance.Database.GetKitCooldown(command[0]);
                            if (LPXRemastered.Instance.Configuration.Instance.KitCooldownApplyToAll)
                            {
                                if (!playerCooldown.Equals(default(KeyValuePair<string, DateTime>)))
                                {
                                    double CooldownLeft = (DateTime.Now - playerCooldown.Value).TotalSeconds;
                                    if (CooldownLeft < cooldown)
                                    {
                                        UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("kit_cooldown", (int)(cooldown - CooldownLeft), command[0]));
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                KeyValuePair<string, DateTime> playerCooldownPerKit = LPXRemastered.PlayerCooldownPerKit.Where(z => z.Key == caller.Id + command[0]).FirstOrDefault();
                                if(!playerCooldownPerKit.Equals(default(KeyValuePair<string, DateTime>)))
                                {
                                    double CooldownLeft = (DateTime.Now - playerCooldownPerKit.Value).TotalSeconds;
                                    if (CooldownLeft < cooldown)
                                    {
                                        UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("kit_cooldown", (int)(cooldown - CooldownLeft), command[0]));
                                        return;
                                    }
                                }
                            }
                            string[] itemIDAmt = LPXRemastered.Instance.Database.GetKitItems(command[0]);
                            string[] itemID = new string[itemIDAmt.Length];
                            string[] Amount = new string[itemIDAmt.Length];
                            for (int x = 0; x < itemID.Length; x++)
                            {
                                itemID[x] = itemIDAmt[x].Split('/')[0];
                                Amount[x] = itemIDAmt[x].Split('/')[1];
                                try
                                {
                                    if (itemID[x].Contains("v."))
                                    {
                                        string vehicleID = itemID[x].Split('.')[1];
                                        player.GiveVehicle(ushort.Parse(vehicleID));
                                    }
                                    else
                                        player.GiveItem(ushort.Parse(itemID[x]), byte.Parse(Amount[x]));
                                }
                                catch
                                {
                                    Logger.Log("Error in Kit Name or Amount. Please check your database and correct them as soon as possible.");
                                }
                            }
                            UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("kit_given", command[0]));
                            if (LPXRemastered.Instance.Configuration.Instance.KitCooldownApplyToAll)
                            {
                                if (LPXRemastered.PlayerCooldown.ContainsKey(caller.Id))
                                    LPXRemastered.PlayerCooldown[caller.Id + command[0]] = DateTime.Now;
                                else
                                    LPXRemastered.PlayerCooldown.Add(caller.Id, DateTime.Now);
                            }
                            else
                            {
                                if (LPXRemastered.PlayerCooldownPerKit.ContainsKey(caller.Id))
                                    LPXRemastered.PlayerCooldownPerKit[caller.Id + command[0]] = DateTime.Now;
                                else
                                    LPXRemastered.PlayerCooldownPerKit.Add(caller.Id, DateTime.Now);
                            }
                        }
                        else
                        {
                            UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("kit_notexist", command[0]));
                            return;
                        }
                    }
                    else if (command.Length > 1)
                    {
                        string name = "";
                        for (int x = 0; x < command.Length; x++)
                        {
                            name += command[x] + " ";
                        }
                        name = name.Trim();
                        if (LPXRemastered.Instance.Database.CheckKit(name))
                        {
                            double cooldown = LPXRemastered.Instance.Database.GetKitCooldown(name);
                            if (LPXRemastered.Instance.Configuration.Instance.KitCooldownApplyToAll)
                            {
                                if (!playerCooldown.Equals(default(KeyValuePair<string, DateTime>)))
                                {
                                    double CooldownLeft = (DateTime.Now - playerCooldown.Value).TotalSeconds;
                                    if (CooldownLeft < cooldown)
                                    {
                                        UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("kit_cooldown", (int)(cooldown - CooldownLeft), name));
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                KeyValuePair<string, DateTime> playerCooldownPerKit = LPXRemastered.PlayerCooldownPerKit.Where(z => z.Key == caller.Id + name).FirstOrDefault();
                                if (!playerCooldownPerKit.Equals(default(KeyValuePair<string, DateTime>)))
                                {
                                    double CooldownLeft = (DateTime.Now - playerCooldownPerKit.Value).TotalSeconds;
                                    if (CooldownLeft < cooldown)
                                    {
                                        UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("kit_cooldown", (int)(cooldown - CooldownLeft), name));
                                        return;
                                    }
                                }
                            }
                            string[] itemIDAmt = LPXRemastered.Instance.Database.GetKitItems(name);
                            string[] itemID = new string[itemIDAmt.Length];
                            string[] Amount = new string[itemIDAmt.Length];
                            for (int x = 0; x < itemID.Length; x++)
                            {
                                itemID[x] = itemIDAmt[x].Split('/')[0];
                                Amount[x] = itemIDAmt[x].Split('/')[1];
                                try
                                {
                                    if (itemID[x].Contains("v."))
                                    {
                                        string vehicleID = itemID[x].Split('.')[1];
                                        player.GiveVehicle(ushort.Parse(vehicleID));
                                    }                                       
                                    else
                                        player.GiveItem(ushort.Parse(itemID[x]), byte.Parse(Amount[x]));
                                }
                                catch
                                {
                                    Logger.Log("Error in Kit Name or Amount. Please check your database and correct them as soon as possible.");
                                }
                            }
                            UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("kit_given", name));
                            if (LPXRemastered.Instance.Configuration.Instance.KitCooldownApplyToAll)
                            {
                                if (LPXRemastered.PlayerCooldown.ContainsKey(caller.Id))
                                    LPXRemastered.PlayerCooldown[caller.Id] = DateTime.Now;
                                else
                                    LPXRemastered.PlayerCooldown.Add(caller.Id, DateTime.Now);
                            }
                            else
                            {
                                if (LPXRemastered.PlayerCooldownPerKit.ContainsKey(caller.Id))
                                    LPXRemastered.PlayerCooldownPerKit[caller.Id + name] = DateTime.Now;
                                else
                                    LPXRemastered.PlayerCooldownPerKit.Add(caller.Id, DateTime.Now);
                            }
                        }
                        else
                        {
                            UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("kit_notexist", name));
                            return;
                        }

                    }
                }
            }
        }
    }
}
