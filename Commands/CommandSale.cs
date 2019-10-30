using System.Collections.Generic;
using Rocket.API;
using Rocket.Core.Logging;
using Rocket.Unturned.Chat;

namespace LPXRemastered
{
    public class CommandSale : IRocketCommand
    {
        public AllowedCaller AllowedCaller
        {
            get
            {
                return AllowedCaller.Both;
            }
        }
        public string Name
        {
            get
            {
                return "sale";
            }
        }
        public string Help
        {
            get
            {
                return "Allows you to put items on sale in your shop.";
            }
        }
        public string Syntax
        {
            get
            {
                return "sale";
            }
        }
        public List<string> Aliases
        {
            get { return new List<string>(); }
        }
        public List<string> Permissions
        {
            get
            {
                return new List<string>() { "sale.*", "sale.start", "sale.stop", "sale" };
            }
        }
        public void Execute(IRocketPlayer caller, params string[] command)
        {
            if (!LPXRemastered.Instance.Configuration.Instance.EnableShop || !LPXRemastered.Instance.Configuration.Instance.SaleEnable)
            {
                UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("shop_disable"));
                return;
            }
            bool console = (caller == null) ? true : false;
            bool hasPerm = false;
            if (!console)
            {
                if (caller.HasPermission("sale.*") || caller.HasPermission("sale.start") || caller.HasPermission("sale.stop") || caller.HasPermission("sale") || caller.HasPermission("*"))
                {
                    hasPerm = true;
                }
            }
            if (!hasPerm && !console)
            {
                UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_no_perm"));
                return;
            }
            if (command.Length == 0)
            {
                LPXRemastered.Instance.sale.MsgSale(caller);
            }                       
            if (command.Length == 1)
            {
                switch (command[0])
                {
                    case "stop":
                        if (caller.HasPermission("sale.*") || caller.HasPermission("sale.stop") || caller.HasPermission("*"))
                        {
                            LPXRemastered.Instance.sale.ResetSale();
                            if (!console)
                                UnturnedChat.Say(caller, "You have stop the sales.");
                            else
                                Logger.Log("Sale have stop");
                            UnturnedChat.Say(LPXRemastered.Instance.Translate("sale_end"));
                        }
                        else
                        {
                            UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_no_perm"));
                            return;
                        }
                        break;
                    case "start":
                        if (caller.HasPermission("sale.*") || caller.HasPermission("sale.start") || caller.HasPermission("*"))
                        {
                            if (!LPXRemastered.Instance.sale.salesStart)
                            {
                                LPXRemastered.Instance.sale.StartSale();
                                if (!console)
                                    UnturnedChat.Say(caller, "You have started the sale!");
                                else
                                    Logger.Log("Sales have started");
                                UnturnedChat.Say(LPXRemastered.Instance.Translate("sale_started", LPXRemastered.Instance.Configuration.Instance.SaleTime));
                            }
                            else
                            {
                                if (!console)
                                    UnturnedChat.Say(caller, "Sales is still ongoing!");
                                else
                                    Logger.Log("Sales is still ongoing!");
                                return;
                            }
                        }
                        else
                        {
                            UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_no_perm"));
                            return;
                        }
                        break;
                    default: UnturnedChat.Say(caller, "/sale - To Check Sale time, /sale stop - To stop or reset sale");
                        break;
                }
            }
        }
    }
}