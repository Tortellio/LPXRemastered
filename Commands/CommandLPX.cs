using Rocket.Unturned.Player;
using Rocket.Unturned.Chat;
using System;
using Rocket.API;
using System.Linq;
using System.Data;
using System.Collections.Generic;

namespace LPXRemastered
{
    public class CommandLPX : IRocketCommand
    {
        public string Help
        {
            get { return "LPX Admin commands"; }
        }

        public string Name
        {
            get { return "lpx"; }
        }

        public AllowedCaller AllowedCaller
        {
            get
            {
                return AllowedCaller.Both;
            }
        }

        public string Syntax
        {
            get { return "<player> <group>"; }
        }

        public List<string> Aliases
        {
            get { return new List<string>(); }
        }

        public List<string> Permissions
        {
            get
            {
                return new List<string>() { "lpx", "lpx.*"};
            }
        }

        public void Execute(IRocketPlayer caller,params string[] command)
        {
            if (!LPXRemastered.Instance.Configuration.Instance.LPXEnabled) return;
            string[] permission = { };
            bool hasPerm = false;
            bool console = (caller is ConsolePlayer);
            if (!console)
            {
                if (LPXRemastered.Instance.Configuration.Instance.LPXEnabled)
                {
                    permission = LPXRemastered.Instance.Database.GetGroupPermission(LPXRemastered.Instance.Database.CheckUserGroup(caller.Id));
                    for (int i = permission.Length - 1; i >= 0; i--)
                    {
                        if (permission[i] == "lpx" || permission[i] == "lpx.*" || permission[i] == "*")
                        {
                            hasPerm = true;
                        }
                    }
                }
                if (!hasPerm && !(caller.IsAdmin))
                {
                    UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_no_perm"));
                    return;
                }
            }
            string comd = String.Join(" ", command);          
            if (String.IsNullOrEmpty(comd.Trim()))
            {
                UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_help"));
                return;
            }
            else
            {
                string[] oper = comd.Split(' ');
                hasPerm = false;
                if (oper.Length == 1)
                {
                    switch (oper[0].ToLower())
                    {
                        case "adduser":                           
                            for (int i = permission.Length - 1; i >= 0; i--)
                            {
                                if (permission[i] == "lpx.adduser" || permission[i] == "lpx.*" || permission[i] == "lpx.addu" || permission[i] == "*")
                                    hasPerm = true;
                            }
                            if (!hasPerm && !console && !(caller.IsAdmin))
                            {
                                UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_no_perm"));
                                return;
                            }
                            else
                                UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_help_adduser"));
                            break;
                        case "removeuser":
                            for (int i = permission.Length - 1; i >= 0; i--)
                            {
                                if (permission[i] == "lpx.removeuser" || permission[i] == "lpx.*" || permission[i] == "lpx.ru" || permission[i] == "*")
                                    hasPerm = true;
                            }
                            if (!hasPerm && !console && !(caller.IsAdmin))
                            {
                                UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_no_perm"));
                                return;
                            }
                            else
                                UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_help_removeuser"));
                            break;
                        case "addgroup":
                            for (int i = permission.Length - 1; i >= 0; i--)
                            {
                                if (permission[i] == "lpx.addgroup" || permission[i] == "lpx.*" || permission[i] == "lpx.addg" || permission[i] == "*")
                                    hasPerm = true;
                            }
                            if (!hasPerm && !console && !(caller.IsAdmin))
                            {
                                UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_no_perm"));
                                return;
                            }
                            else
                                UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_help_addgroup"));
                            break;
                        case "removegroup":
                            for (int i = permission.Length - 1; i >= 0; i--)
                            {
                                if (permission[i] == "lpx.removegroup" || permission[i] == "lpx.*" || permission[i] == "lpx.rg" || permission[i] == "*")
                                    hasPerm = true;
                            }
                            if (!hasPerm && !console && !(caller.IsAdmin))
                            {
                                UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_no_perm"));
                                return;
                            }
                            else
                                UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_help_removegroup"));
                            break;
                        case "addpermission":
                            for (int i = permission.Length - 1; i >= 0; i--)
                            {
                                if (permission[i] == "lpx.addpermission" || permission[i] == "lpx.*" || permission[i] == "lpx.addp" || permission[i] == "*")
                                    hasPerm = true;
                            }
                            if (!hasPerm && !console && !(caller.IsAdmin))
                            {
                                UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_no_perm"));
                                return;
                            }
                            else
                                UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_help_addpermission"));                               
                            break;
                        case "removepermission":
                            for (int i = permission.Length - 1; i >= 0; i--)
                            {
                                if (permission[i] == "lpx.removepermission" || permission[i] == "lpx.*" || permission[i] == "lpx.rp" || permission[i] == "*")
                                    hasPerm = true;
                            }
                            if (!hasPerm && !console && !(caller.IsAdmin))
                            {
                                UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_no_perm"));
                                return;
                            }
                            else
                                UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_help_removepermission"));
                            break;
                        case "addgroupfreeitem":
                            for (int i = permission.Length - 1; i >= 0; i--)
                            {
                                if (permission[i] == "lpx.addgroupfreeitem" || permission[i] == "lpx.*" || permission[i] == "lpx.addgfi" || permission[i] == "*")
                                    hasPerm = true;
                            }
                            if (!hasPerm && !console && !(caller.IsAdmin))
                            {
                                UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_no_perm"));
                                return;
                            }
                            else
                                UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_help_addgroupfreeitem"));
                            break;
                        case "setgroupincome":
                            for (int i = permission.Length - 1; i >= 0; i--)
                            {
                                if (permission[i] == "lpx.setgroupincome" || permission[i] == "lpx.*" || permission[i] == "lpx.setgi" || permission[i] == "*")
                                    hasPerm = true;
                            }
                            if (!hasPerm && !console && !(caller.IsAdmin))
                            {
                                UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_no_perm"));
                                return;
                            }
                            else
                                UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_help_setgroupincome"));
                            break;
                        case "listgroup":
                            for (int i = permission.Length - 1; i >= 0; i--)
                            {
                                if (permission[i] == "lpx.listgroup" || permission[i] == "lpx.*" || permission[i] == "lpx.listg" || permission[i] == "*")
                                    hasPerm = true;
                            }
                            if (!hasPerm && !console && !(caller.IsAdmin))
                            {
                                UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_no_perm"));
                                return;
                            }
                            else
                            {
                                DataTable dt;
                                dt = LPXRemastered.Instance.Database.GetGroup();
                                string[] stringArr = new string[50];
                                string groups="";
                                for (int i = 0; i < (dt.Rows.Count); i++)
                                {
                                    stringArr[i] += dt.Rows[i].ItemArray[0].ToString();
                                    if (i == (dt.Rows.Count-1))
                                        groups += stringArr[i] + ".";
                                    else
                                        groups += stringArr[i] + ", ";
                                }
                                UnturnedChat.Say(caller, groups);                                
                            }
                            break;
                        case "listpermission":
                            for (int i = permission.Length - 1; i >= 0; i--)
                            {
                                if (permission[i] == "lpx.listpermission" || permission[i] == "lpx.*" || permission[i] == "lpx.listp" || permission[i] == "*")
                                    hasPerm = true;
                            }
                            if (!hasPerm && !console && !(caller.IsAdmin))
                            {
                                UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_no_perm"));
                                return;
                            }
                            else
                                UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_help_listpermission"));
                            break;
                        case "addparentgroup":
                            for (int i = permission.Length - 1; i >= 0; i--)
                            {
                                if (permission[i] == "lpx.addparentgroup" || permission[i] == "lpx.*" || permission[i] == "lpx.addpg" || permission[i] == "*")
                                    hasPerm = true;
                            }
                            if (!hasPerm && !console && !(caller.IsAdmin))
                            {
                                UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_no_perm"));
                                return;
                            }
                            else
                                UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_help_addparentgroup"));
                            break;
                        case "setpromotegroup":
                            for (int i = permission.Length - 1; i >= 0; i--)
                            {
                                if (permission[i] == "lpx.setpromotegroup" || permission[i] == "lpx.*" || permission[i] == "lpx.setpromog" || permission[i] == "*")
                                    hasPerm = true;
                            }
                            if (!hasPerm && !console && !(caller.IsAdmin))
                            {
                                UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_no_perm"));
                                return;
                            }
                            else
                                UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_help_setpromotegroup"));
                            break;
                        case "setpromotetime":
                            for (int i = permission.Length - 1; i >= 0; i--)
                            {
                                if (permission[i] == "lpx.setpromotetime" || permission[i] == "lpx.*" || permission[i] == "lpx.setpromot" || permission[i] == "*")
                                    hasPerm = true;
                            }
                            if (!hasPerm && !console && !(caller.IsAdmin))
                            {
                                UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_no_perm"));
                                return;
                            }
                            else
                                UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_help_setpromotetime"));
                            break;
                        case "enablepromote":
                            for (int i = permission.Length - 1; i >= 0; i--)
                            {
                                if (permission[i] == "lpx.enablepromote" || permission[i] == "lpx.*" || permission[i] == "lpx.enpromo" || permission[i] == "*")
                                    hasPerm = true;
                            }
                            if (!hasPerm && !console && !(caller.IsAdmin))
                            {
                                UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_no_perm"));
                                return;
                            }
                            else
                                UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_help_enablepromote"));
                            break;
                        default:
                            break;
                    }
                    return;
                }
                else
                {
                    string[] param = string.Join(" ", oper.Skip(1).ToArray()).Split(' ');
                    hasPerm = false;
                    switch (oper[0].ToLower())
                    {
                        case "adduser":
                            for (int i = permission.Length - 1; i >= 0; i--)
                            {
                                if (permission[i] == "lpx.adduser.*" || permission[i] == "lpx.*" || permission[i] == "*" || permission[i] == "lpx.addu.*" || permission[i] == ("lpx.adduser." + param[1]) || permission[i] == ("lpx.addu." + param[1]))
                                    hasPerm = true;
                            }
                            if (!hasPerm && !console && !(caller.IsAdmin))
                            {
                                UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_no_perm"));
                                return;
                            }
                            else
                            {
                                if (param.Length == 1)
                                {
                                    UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_help_adduser2"));
                                }
                                else if (param.Length == 2)
                                {
                                    if (LPXRemastered.Instance.Database.CheckGroup(param[1]))
                                    {
                                        UnturnedPlayer target = UnturnedPlayer.FromName(param[0]);
                                        if (target != null)
                                        {
                                            LPXRemastered.Instance.Database.AddUserIntoGroup(UnturnedPlayer.FromName(param[0]).Id, param[1]);
                                            UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_added_user", target.SteamName, param[1]));
                                            LPXRemastered.Instance.Database.LastLogin(target.Id);
                                        }
                                        else
                                        {
                                            UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_fail_nouser"));
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_fail_nogroup"));
                                        return;
                                    }
                                }
                            }
                            break;
                        case "removeuser":
                            for (int i = permission.Length - 1; i >= 0; i--)
                            {
                                if (permission[i] == "lpx.removeuser" || permission[i] == "lpx.*" || permission[i] == "lpx.ru" || permission[i] == "*")
                                    hasPerm = true;
                            }
                            if (!hasPerm && !console && !(caller.IsAdmin))
                            {
                                UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_no_perm"));
                                return;
                            }
                            else
                            {
                                if (param.Length == 1)
                                {
                                    try
                                    {
                                        UnturnedPlayer target = UnturnedPlayer.FromName(param[0]);                                       
                                        if (target != null)
                                        {
                                            string group = LPXRemastered.Instance.Database.CheckUserGroup(target.Id);
                                            if (LPXRemastered.Instance.Database.RemoveUser(target.Id))
                                                UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_removed_user", target.SteamName, group));
                                            else
                                                UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_fail_remove", target.SteamName, group));
                                        }
                                        else
                                        {
                                            UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_fail_nouser"));
                                            return;
                                        }
                                    }
                                    catch
                                    {
                                        UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_fail_nouser"));
                                        return;
                                    }

                                }
                            }
                            break;
                        case "addgroup":
                            for (int i = permission.Length - 1; i >= 0; i--)
                            {
                                if (permission[i] == "lpx.addgroup" || permission[i] == "lpx.*" || permission[i] == "lpx.addg" || permission[i] == "*")
                                    hasPerm = true;
                            }
                            if (!hasPerm && !console && !(caller.IsAdmin))
                            {
                                UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_no_perm"));
                                return;
                            }
                            else
                            {
                                if (LPXRemastered.Instance.Database.CheckGroup(param[0]))
                                {
                                    UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_group_exist", param[0]));
                                    return;
                                }
                                else if(param.Length >= 2)
                                {
                                    if (decimal.TryParse(param[1], out _))
                                    {
                                        string updategroup = "", parentgroup = "";
                                        int UpdateTime = 7;
                                        bool EnableAuto = false;
                                        if (param.Length == 6)
                                            bool.TryParse(param[5], out EnableAuto);
                                        if (param.Length >= 5)
                                            int.TryParse(param[4], out UpdateTime);
                                        if (param.Length >= 4)
                                        {
                                            if (LPXRemastered.Instance.Database.CheckGroup(param[3]) == false)
                                            {
                                                UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_group_notexist"));
                                                return;
                                            }
                                            else
                                                updategroup = param[3];
                                        }
                                        if (param.Length >= 3)
                                            parentgroup = param[2];
                                        if (LPXRemastered.Instance.Database.AddGroup(param[0], param[1],parentgroup, updategroup,UpdateTime,EnableAuto))
                                            UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_added_group", param[0]));
                                        else
                                            UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_failed_group", param[0]));
                                    }
                                    else
                                    {
                                        UnturnedChat.Say(caller, "Please only enter numbers in <income>");
                                        UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_help_addgroup"));
                                        return;
                                    }
                                }
                                else if(param.Length == 1)
                                {
                                    if (LPXRemastered.Instance.Database.AddGroup(param[0], "10",null,null, 7, false))
                                        UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_added_group", param[0]));
                                    else
                                        UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_failed_group", param[0]));
                                }
                                else
                                    UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_invaild_para"));                             
                            }
                            break;
                        case "removegroup":
                            for (int i = permission.Length - 1; i >= 0; i--)
                            {
                                if (permission[i] == "lpx.removegroup" || permission[i] == "lpx.*" || permission[i] == "lpx.rg" || permission[i] == "*")
                                    hasPerm = true;
                            }
                            if (!hasPerm && !console && !(caller.IsAdmin))
                            {
                                UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_no_perm"));
                                return;
                            }
                            else
                            {
                                if (!LPXRemastered.Instance.Database.CheckGroup(param[0]))
                                {
                                    UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_fail_nogroup", param[0]));
                                    return;
                                }
                                else
                                {
                                    if (LPXRemastered.Instance.Database.RemoveGroup(param[0]))
                                        UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_removed_group", param[0]));
                                    else
                                        UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_fail_removegroup", param[0]));
                                }
                            }
                            break;
                        case "addpermission":
                            for (int i = permission.Length - 1; i >= 0; i--)
                            {
                                if (permission[i] == "lpx.addpermission" || permission[i] == "lpx.*" || permission[i] == "lpx.addp" || permission[i] == "*")
                                    hasPerm = true;
                            }
                            if (!hasPerm && !console && !(caller.IsAdmin))
                            {
                                UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_no_perm"));
                                return;
                            }
                            else
                            {
                                if (param.Length == 1)
                                {
                                    UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_help_addpermission2"));
                                }
                                else if(param.Length == 2)
                                {
                                    if (LPXRemastered.Instance.Database.CheckGroup(param[0]))
                                    {
                                        if (!LPXRemastered.Instance.Database.CheckPermissionCopy(param[0],param[1]))
                                        {
                                            bool result;
                                            result = LPXRemastered.Instance.Database.AddPermissionIntoGroup(param[1], param[0]);
                                            if (result)
                                                UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_added_permission", param[1], param[0]));
                                            else
                                                UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_failed_permission", param[1], param[0]));
                                        }
                                        else
                                        {
                                            UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_failed_copypermission", param[1], param[0]));
                                            return;
                                        }

                                    }
                                    else
                                    {
                                        UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_fail_nogroup"));
                                        return;
                                    }
                                }
                            }
                            break;
                        case "removepermission":
                            for (int i = permission.Length - 1; i >= 0; i--)
                            {
                                if (permission[i] == "lpx.removepermission" || permission[i] == "lpx.*" || permission[i] == "lpx.rp" || permission[i] == "*")
                                    hasPerm = true;
                            }
                            if (!hasPerm && !console && !(caller.IsAdmin))
                            {
                                UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_no_perm"));
                                return;
                            }
                            else
                            {
                                if (param.Length == 1)
                                {
                                    UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_help_removepermission2"));
                                }
                                else if (param.Length == 2)
                                {
                                    if (LPXRemastered.Instance.Database.CheckGroup(param[0]))
                                    {
                                        bool result;
                                        result = LPXRemastered.Instance.Database.RemovePermissionFromGroup(param[0], param[1]);
                                        if (result)
                                            UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_remove_removepermission", param[1], param[0]));
                                        else
                                            UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_failed_removepermission", param[0]));
                                    }
                                    else
                                    {
                                        UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_fail_nogroup"));
                                        return;
                                    }
                                }
                            }
                            break;
                        case "listpermission":
                            string permissions = "";
                            for (int i = permission.Length - 1; i >= 0; i--)
                            {
                                if (permission[i] == "lpx.listpermission" || permission[i] == "lpx.*" || permission[i] == "lpx.listp" || permission[i] == "*")
                                    hasPerm = true;
                            }
                            if (!hasPerm && !console && !(caller.IsAdmin))
                            {
                                UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_no_perm"));
                                return;
                            }
                            else
                            {
                                if (param.Length == 1)
                                {
                                    permission = LPXRemastered.Instance.Database.GetGroupPermission(param[0]);
                                    if (permission.Length > 0)
                                    {
                                        for (int i = permission.Length - 1; i >= 0; i--)
                                        {
                                            permissions += " " + permission[i];
                                        }
                                        UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_listp_group", param[0]));
                                        UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_listp_permission", permissions));
                                    }
                                }
                                else
                                {
                                    UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_help_listpermission"));
                                    return;
                                }
                            }
                            break;
                        case "addgroupfreeitem":
                            for (int i = permission.Length - 1; i >= 0; i--)
                            {
                                if (permission[i] == "lpx.addgroupfreeitem" || permission[i] == "lpx.*" || permission[i] == "lpx.addgfi" || permission[i] == "*")
                                    hasPerm = true;
                            }
                            if (!hasPerm && !console && !(caller.IsAdmin))
                            {
                                UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_no_perm"));
                                return;
                            }
                            else
                            {
                                if (param.Length == 1)
                                {
                                    UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_help_addgroupfreeitem2"));
                                }
                                else if (param.Length == 2)
                                {
                                    if (LPXRemastered.Instance.Database.CheckGroup(param[0]))
                                    {
                                        bool result;
                                        result = LPXRemastered.Instance.Database.AddGroupFreeItem(param[0], param[1]);
                                        if (result)
                                            UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_added_permission", param[0], param[1]));
                                        else
                                            UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_failed_permission", param[0], param[1]));
                                    }
                                    else
                                    {
                                        UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_fail_nogroup"));
                                        return;
                                    }
                                }
                            }
                            break;
                        case "setgroupincome":
                            for (int i = permission.Length - 1; i >= 0; i--)
                            {
                                if (permission[i] == "lpx.setgroupincome" || permission[i] == "lpx.*" || permission[i] == "lpx.setgi" || permission[i] == "*")
                                    hasPerm = true;
                            }
                            if (!hasPerm && !console && !(caller.IsAdmin))
                            {
                                UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_no_perm"));
                                return;
                            }
                            else
                            {
                                if (LPXRemastered.Instance.Database.CheckGroup(param[0]))
                                {
                                    if (LPXRemastered.Instance.Database.SetGroupIncome(param[0], param[1]))
                                        UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_added_income", param[0], param[1]));
                                    else
                                        UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_failed_setincome", param[0]));
                                }
                                else
                                {
                                    UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_fail_nogroup"));
                                    return;
                                }
                            }
                            break;
                        case "addparentgroup":
                            for (int i = permission.Length - 1; i >= 0; i--)
                            {
                                if (permission[i] == "lpx.addparentgroup" || permission[i] == "lpx.*" || permission[i] == "lpx.addpg" || permission[i] == "*")
                                    hasPerm = true;
                            }
                            if (!hasPerm && !console && !(caller.IsAdmin))
                            {
                                UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_no_perm"));
                                return;
                            }
                            else
                            {
                                if (LPXRemastered.Instance.Database.CheckGroup(param[0]))
                                {
                                    if (param[0] == param[1])
                                    {
                                        UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_add_sameparentgroup"));
                                        return;
                                    }
                                    if (LPXRemastered.Instance.Database.AddParentGroup(param[0], param[1]))
                                        UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_added_parentgroup", param[0], param[1]));
                                    else
                                        UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_failed_parentgroup", param[0], param[1]));
                                }
                                else
                                {
                                    UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_fail_nogroup"));
                                    return;
                                }
                            }
                            break;
                        case "setpromotegroup":
                            for (int i = permission.Length - 1; i >= 0; i--)
                            {
                                if (permission[i] == "lpx.setpromotegroup" || permission[i] == "lpx.*" || permission[i] == "lpx.setpromog" || permission[i] == "*")
                                    hasPerm = true;
                            }
                            if (!hasPerm && !console && !(caller.IsAdmin))
                            {
                                UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_no_perm"));
                                return;
                            }
                            else
                            {
                                if (LPXRemastered.Instance.Database.CheckGroup(param[0]))
                                {
                                    if (LPXRemastered.Instance.Database.SetUpdateGroup(param[0], param[1]))
                                        UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_set_promotegroup", param[0], param[1]));
                                    else
                                        UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_failed_promotegroup", param[0], param[1]));
                                }
                                else
                                {
                                    UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_fail_nogroup"));
                                    return;
                                }
                            }
                            break;
                        case "setpromotetime":
                            for (int i = permission.Length - 1; i >= 0; i--)
                            {
                                if (permission[i] == "lpx.setpromotetime" || permission[i] == "lpx.*" || permission[i] == "lpx.setpromot" || permission[i] == "*")
                                    hasPerm = true;
                            }
                            if (!hasPerm && !console && !(caller.IsAdmin))
                            {
                                UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_no_perm"));
                                return;
                            }
                            else
                            {
                                if (LPXRemastered.Instance.Database.CheckGroup(param[0]))
                                {
                                    if (int.TryParse(param[1], out int updateTime))
                                        if (LPXRemastered.Instance.Database.SetUpdateTime(param[0], updateTime))
                                            UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_set_promotetime", param[0], param[1]));
                                        else
                                            UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_failed_promotetime", param[0]));
                                }
                                else
                                {
                                    UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_fail_nogroup"));
                                    return;
                                }
                            }
                            break;
                        case "enablepromote":
                            for (int i = permission.Length - 1; i >= 0; i--)
                            {
                                if (permission[i] == "lpx.enablepromote" || permission[i] == "lpx.*" || permission[i] == "lpx.enpromo" || permission[i] == "*")
                                    hasPerm = true;
                            }
                            if (!hasPerm && !console && !(caller.IsAdmin))
                            {
                                UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_no_perm"));
                                return;
                            }
                            else
                            {
                                if (LPXRemastered.Instance.Database.CheckGroup(param[0]))
                                {
                                    if (bool.TryParse(param[1], out bool enable))
                                        if (LPXRemastered.Instance.Database.SetEnableUpdate(param[0], enable))
                                            UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_set_enablepromote", param[0], param[1]));
                                        else
                                            UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_failed_enablepromote", param[0]));
                                }
                                else
                                {
                                    UnturnedChat.Say(caller, LPXRemastered.Instance.Translate("lpx_fail_nogroup"));
                                    return;
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
