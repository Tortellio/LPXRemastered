using Rocket.API;
using Rocket.Unturned.Player;
using Rocket.Unturned.Chat;
using System.Collections.Generic;
using Rocket.API.Serialisation;

namespace LPXRemastered
{
    public class SQLPermission: IRocketPermissionsProvider
    {

        public RocketPermissionsProviderResult AddGroup(RocketPermissionsGroup group)
        {
            if(LPXRemastered.Instance.Database.AddGroup(group.DisplayName,"10",group.ParentGroup,"",99,false))
                return RocketPermissionsProviderResult.Success;
            else
                return RocketPermissionsProviderResult.UnspecifiedError;
        }
        public RocketPermissionsProviderResult AddPlayerToGroup(string groupId, IRocketPlayer player)
        {
            string group;
            try
            {
                group = LPXRemastered.Instance.Database.CheckUserGroupByID(groupId);
            }
            catch
            {
                return RocketPermissionsProviderResult.GroupNotFound;
            }
            if (LPXRemastered.Instance.Database.AddUserIntoGroup(player.Id, group))
                return RocketPermissionsProviderResult.Success;
            else
                return RocketPermissionsProviderResult.UnspecifiedError;
        }
        public RocketPermissionsProviderResult DeleteGroup(string groupId)
        {
            string group;
            try
            {
                group = LPXRemastered.Instance.Database.CheckUserGroupByID(groupId);
            }
            catch
            {
                return RocketPermissionsProviderResult.GroupNotFound;
            }
            if (group == "default")
                return RocketPermissionsProviderResult.UnspecifiedError;
            if (LPXRemastered.Instance.Database.RemoveGroup(group))
                return RocketPermissionsProviderResult.Success;
            else
                return RocketPermissionsProviderResult.UnspecifiedError;
        }
        public RocketPermissionsGroup GetGroup(string groupId)
        {
            string group;
            group = LPXRemastered.Instance.Database.CheckUserGroupByID(groupId);
            RocketPermissionsGroup RPG = new RocketPermissionsGroup(group, group, LPXRemastered.Instance.Database.GetParentGroup(group), LPXRemastered.Instance.Database.GetMembers(group), GetGroupPermission(groupId), LPXRemastered.Instance.Database.GetColor(group));
            return RPG;
        }

        public List<RocketPermissionsGroup> GetGroups(IRocketPlayer player , bool IncludeParentGroup)
        {
            List<RocketPermissionsGroup> Group = new List<RocketPermissionsGroup>();
            string group;
            group = LPXRemastered.Instance.Database.CheckUserGroup(player.Id);
            if (player.IsAdmin && player.Id != null)
                group = "admin";
            else if (group == null || group == "")
                group = "default";         
            RocketPermissionsGroup RPG = new RocketPermissionsGroup(group, group, LPXRemastered.Instance.Database.GetParentGroup(group), LPXRemastered.Instance.Database.GetMembers(group), GetGroupPermission(player), LPXRemastered.Instance.Database.GetColor(group));
            Group.Add(RPG);
            return Group;
        }

        public List<Permission> GetPermissions(IRocketPlayer player)
        {
            List<Permission> UserGroup = new List<Permission>();
            Permission Usergroup = new Permission(string.Join(" ", LPXRemastered.Instance.Database.GetGroupPermission(LPXRemastered.Instance.Database.CheckUserGroup(player.Id))), LPXRemastered.Instance.Database.Cooldown(LPXRemastered.Instance.Database.CheckUserGroup(player.Id)));
            UserGroup.Add(Usergroup);
            return UserGroup;
        }

        public List<Permission> GetPermissions(IRocketPlayer player, List<string> requestedPermissions)
        {
            List<Permission> UserGroup = new List<Permission>();
            Permission Usergroup = new Permission(string.Join(" ", LPXRemastered.Instance.Database.GetGroupPermission(LPXRemastered.Instance.Database.CheckUserGroup(player.Id))), LPXRemastered.Instance.Database.Cooldown(LPXRemastered.Instance.Database.CheckUserGroup(player.Id)));
            UserGroup.Add(Usergroup);
            return UserGroup;
        }
        
        public bool HasPermission(IRocketPlayer player, List<string> requestedPermissions)
        {
            bool hasPerm;
            hasPerm = LPXRemastered.Instance.CheckPermission(requestedPermissions, player.Id);
            if (player.IsAdmin || LPXRemastered.Instance.Database.CheckhaveAllPermission(LPXRemastered.Instance.Database.CheckUserGroup(player.Id)))
                hasPerm = true;
            return hasPerm;
        }

        public bool SetGroup(IRocketPlayer caller , string group)
        {
            bool Result;
            if (LPXRemastered.Instance.Database.CheckGroup(group))
            {
                UnturnedPlayer target = (UnturnedPlayer)caller;
                if (target != null)
                {
                    LPXRemastered.Instance.Database.AddUserIntoGroup(caller.Id, group);
                    UnturnedChat.Say(caller, LPXRemastered.Instance.DefaultTranslations.Translate("lpx_added_user", target.SteamName, group));
                    Result = true;
                }
                else
                {
                    UnturnedChat.Say(caller, LPXRemastered.Instance.DefaultTranslations.Translate("lpx_fail_nouser"));
                    Result = false;
                }
            }
            else
            {
                UnturnedChat.Say(caller, LPXRemastered.Instance.DefaultTranslations.Translate("lpx_fail_nogroup"));
                Result = false;
            } 
            return Result;                                          
        }
        
        public void Reload()
        {

        }

        public RocketPermissionsProviderResult RemovePlayerFromGroup(string groupId, IRocketPlayer player)
        {
            string group;
            try
            {
                group = LPXRemastered.Instance.Database.CheckUserGroupByID(groupId);
            }
            catch
            {
                return RocketPermissionsProviderResult.GroupNotFound;
            }
            if (group == "default")
                return RocketPermissionsProviderResult.UnspecifiedError;
            if (LPXRemastered.Instance.Database.RemoveUser(player.Id))
                return RocketPermissionsProviderResult.Success;
            else
                return RocketPermissionsProviderResult.UnspecifiedError;
        }
        public RocketPermissionsProviderResult SaveGroup(RocketPermissionsGroup group)
        {
            return RocketPermissionsProviderResult.Success;
        }

        private List<Permission> GetGroupPermission(IRocketPlayer player)
        {
            string group;
            group = LPXRemastered.Instance.Database.CheckUserGroup(player.Id);
            if (player.IsAdmin && player.Id != null)
                group = "admin";
            else if (group == null || group == "")
                group = "default";
            List<Permission> GroupPermission = new List<Permission>();
            Permission GpPer = new Permission(group, LPXRemastered.Instance.Database.Cooldown(group));
            GroupPermission.Add(GpPer);
            return GroupPermission;
        }
        private List<Permission> GetGroupPermission(string GroupID)
        {
            string group;
            group = LPXRemastered.Instance.Database.CheckUserGroupByID(GroupID);
            if (group == null || group == "")
                group = "default";
            List<Permission> GroupPermission = new List<Permission>();
            Permission GpPer = new Permission(group, LPXRemastered.Instance.Database.Cooldown(group));
            GroupPermission.Add(GpPer);
            return GroupPermission;
        }
    }                            
}
