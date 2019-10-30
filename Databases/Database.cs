﻿using MySql.Data.MySqlClient;
using Rocket.Core.Logging;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;

namespace LPXRemastered
{
    public class DatabaseManager
    {
        
        internal DatabaseManager()
        {
            new I18N.West.CP1250(); //Workaround for database encoding issues with mono
            CheckSchema();
        }

        private MySqlConnection CreateConnection()
        {
            MySqlConnection connection = null;
            try
            {
                if (LPXRemastered.Instance.Configuration.Instance.DatabasePort == 0) LPXRemastered.Instance.Configuration.Instance.DatabasePort = 3306;
                connection = new MySqlConnection(String.Format("SERVER={0};DATABASE={1};UID={2};PASSWORD={3};PORT={4}; Convert Zero Datetime=True;", LPXRemastered.Instance.Configuration.Instance.DatabaseAddress, LPXRemastered.Instance.Configuration.Instance.DatabaseName, LPXRemastered.Instance.Configuration.Instance.DatabaseUsername, LPXRemastered.Instance.Configuration.Instance.DatabasePassword, LPXRemastered.Instance.Configuration.Instance.DatabasePort));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return connection;
        }
        public List<string> GetMembers(string group)
        {
            string members = "";
            List<string> Members = new List<string>();
            DataTable dt = new DataTable();
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "select `steamID` from `" + LPXRemastered.Instance.Configuration.Instance.DatabaseTableName + "` where `group` = '" + group + "'";
                //command.Parameters.AddWithValue("group", group);
                connection.Open();              
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                adapter.Fill(dt);
                connection.Close();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    members += dt.Rows[i].ItemArray[0].ToString() + " ";
                }
                members = members.Trim();
                Members = members.Split(' ').ToList();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return Members;
        }
        public string GetParentGroup(string group)
        {
            string PGroup = "";
            DataTable dt = new DataTable();
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "select `parentgroup` from `" + LPXRemastered.Instance.Configuration.Instance.DatabaseTableGroup+ "` where `name` = '" + group + "'";
                connection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                adapter.Fill(dt);
                connection.Close();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    PGroup += dt.Rows[i].ItemArray[0].ToString() + " ";
                }
                PGroup = PGroup.Trim();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return PGroup;
        }
        public string[] GetParentGroupString(string group)
        {
            string PGroup = "";
            string[] Pgroups = {};
            DataTable dt = new DataTable();
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "select `parentgroup` from `" + LPXRemastered.Instance.Configuration.Instance.DatabaseTableGroup + "` where `name` = '" + group + "'";
                connection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                adapter.Fill(dt);
                connection.Close();
                Pgroups = new string[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    PGroup += dt.Rows[i].ItemArray[0].ToString() + " ";
                }
                PGroup = PGroup.Trim();
                Pgroups = PGroup.Split(' ');
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return Pgroups;
        }
        public string[] GetPermission(string id)
        {
            string name;
            string[] permission = {};
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "select `group` from `" + LPXRemastered.Instance.Configuration.Instance.DatabaseTableName + "` where `steamId` = '" + id.ToString() + "'";
                connection.Open();
                object group = command.ExecuteScalar();
                if (group != null)
                    name = group.ToString();
                else
                    name = "default";
                command.CommandText = "select `permission` from `" + LPXRemastered.Instance.Configuration.Instance.DatabaseTableGroup + "` where `name` = '" + name + "';";
                group = command.ExecuteScalar();
                if (group != null)
                {
                    permission = (group.ToString()).Split(' ');
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return permission;
        }
        public string[] GetPermission2(string group)
        {
            string[] permission = { };
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();               
                if (group == null)
                    group = "default";
                command.CommandText = "select `permission` from `" + LPXRemastered.Instance.Configuration.Instance.DatabaseTableGroup + "` where `name` = '" + group + "';";
                connection.Open();
                object  obj = command.ExecuteScalar();
                connection.Close();
                if (group != null)
                {
                    permission = (obj.ToString()).Split(' ');
                }              
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return permission;
        }
        public bool CheckPermissionCopy(string group, string permission)
        {
            string[] permissions = { };
            bool CopyDetected = false;
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "select `permission` from `" + LPXRemastered.Instance.Configuration.Instance.DatabaseTableGroup + "` where `name` = '" + group + "';";
                connection.Open();
                object perm = command.ExecuteScalar();
                if (perm != null)
                {
                    permissions = (perm.ToString()).Split(' ');
                }
                connection.Close();
                for(int x = 0; x < permissions.Length; x ++)
                {
                    if (permissions[x] == permission)
                    {
                        CopyDetected = true;
                    }
                    else
                        CopyDetected = false;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return CopyDetected;
        }
        public bool CheckhaveAllPermission(string group)
        {
            string[] permissions = { };
            bool HasAllPermission = false;
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "select `permission` from `" + LPXRemastered.Instance.Configuration.Instance.DatabaseTableGroup + "` where `name` = '" + group + "';";
                connection.Open();
                object perm = command.ExecuteScalar();
                if (perm != null)
                {
                    permissions = (perm.ToString()).Split(' ');
                }
                connection.Close();
                for (int x = 0; x < permissions.Length; x++)
                {
                    if (permissions[x] == "*")
                    {
                        HasAllPermission = true;
                    }
                    else
                        HasAllPermission = false;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return HasAllPermission;
        }
        public string[] GetGroupPermission(string group)
        {
            string[] permission = { };
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "select `permission` from `" + LPXRemastered.Instance.Configuration.Instance.DatabaseTableGroup + "` where `name` = '" + group + "'";
                connection.Open();
                object obj = command.ExecuteScalar();
                if (obj != null)
                {
                    permission = (obj.ToString()).Split(' ');
                }
                connection.Close();
                string parentgroup = string.Join(" ", LPXRemastered.Instance.Database.GetParentGroupString(group)).Trim();
                string AllParentGroups = "";
                string[] pgroups;
                if(parentgroup != "")
                {                   
                    while (parentgroup.Trim() != " " && parentgroup.Trim() != "")
                    {
                        pgroups = parentgroup.Split(' ');
                        if (parentgroup.StartsWith(" "))
                            AllParentGroups += parentgroup;
                        else
                            AllParentGroups += " " + parentgroup;
                        parentgroup = "";
                        for (int i = 0; i < pgroups.Length; i++)
                        {
                            string[] ParentGroup = LPXRemastered.Instance.Database.GetParentGroupString(pgroups[i]);
                            for (int x = 0; x < ParentGroup.Length; x++)
                            {
                                if(parentgroup == "")
                                    parentgroup += ParentGroup[x].Trim();
                                else
                                    parentgroup += " " + ParentGroup[x].Trim();
                            }
                        }
                    }
                    AllParentGroups = AllParentGroups.Trim();
                    pgroups = AllParentGroups.Split(' ');
                    string[] pgroupPerm = new string[pgroups.Length];
                    string newPermission = "";
                    string[] FindColor;                  
                    for (int i = 0; i < pgroups.Length; i++)
                    {
                        string NoColor = "";
                        FindColor = GetParentGroupPermissionString(pgroups[i]).Split(' ');
                        for(int y = 0; y < FindColor.Length; y++)
                        {
                            if (!(FindColor[y].Contains("color.")))
                            {
                                if(FindColor[y] != "")
                                    NoColor += FindColor[y] + " ";
                            }
                            pgroupPerm[i] = NoColor;
                        }
                       pgroupPerm[i] = pgroupPerm[i].Trim();
                    }
                    for (int i = 0; i < pgroupPerm.Length; i++)
                    {
                        if(pgroupPerm[i] != "")
                            newPermission += " " + pgroupPerm[i].Trim();
                    }
                    newPermission = newPermission.Trim();
                    string oldpermission = obj.ToString();
                    oldpermission = oldpermission.Trim();
                    permission = (oldpermission +" " +newPermission).Split(' ');
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return permission;
        }     
        public string GetParentGroupPermissionString(string group)
        {
            string permission = "";
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "select `permission` from `" + LPXRemastered.Instance.Configuration.Instance.DatabaseTableGroup + "` where `name` = '" + group + "'";
                connection.Open();
                object obj = command.ExecuteScalar();
                if (obj != null)
                {
                    permission = obj.ToString();
                }
                connection.Close();

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return permission;
        }
        public bool AddUserIntoGroup(string id, string group)
        {
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "select `steamId` from `" + LPXRemastered.Instance.Configuration.Instance.DatabaseTableName + "` WHERE `steamId` = '" + id + "'";
                connection.Open();
                object result = command.ExecuteScalar();              
                connection.Close();
                if (result == null)
                {
                    command.CommandText = "insert into `" + LPXRemastered.Instance.Configuration.Instance.DatabaseTableName + "` (`steamId`,`group`) values('" + id + "', '" + group + "')";
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                else
                {
                    command.CommandText = "update `" + LPXRemastered.Instance.Configuration.Instance.DatabaseTableName + "` set `group` = '" + group + "' where `steamId` = '" + id + "'";
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return false;
            }
            return true;
        }
        public bool AddGroup(string group, string income,string parentgroup, string updategroup, int Updatetime, bool autoUpdate)
        {
            _ = decimal.Parse(income);
            int AutoUpdate = 0;
            if (autoUpdate)
                AutoUpdate = 1;
            bool added;
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "insert into `" + LPXRemastered.Instance.Configuration.Instance.DatabaseTableGroup + "` (`name`,`income`,`parentgroup`,`updategroup`,`updatetime`, `updateenable`) values('" + group + "', '" + income + "', '" + parentgroup + "', '" + updategroup + "', " + Updatetime + ", " + AutoUpdate + ")";
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
                added = true;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                added = false;
            }
            return added;
        }
        public bool RemoveGroup(string group)
        {
            bool removed;
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "Delete from `" + LPXRemastered.Instance.Configuration.Instance.DatabaseTableGroup + "` (`name`) where `name` = '" + group + "'";
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
                removed = true;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                removed = false;
            }
            return removed;
        }
        public bool RemoveUser(string Id)
        {
            bool added;
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "Delete from `" + LPXRemastered.Instance.Configuration.Instance.DatabaseTableName + "` where `steamId` = '" + Id + "'";
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
                added = true;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                added = false;
            }
            return added;
        }
        public bool AddGroupFreeItem(string group, string id)
        {
            bool added;
            string[] ItemID = GetGroupFreeItem(group);
            string Items = "";
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                if (ItemID.Length < 1)
                    command.CommandText = "insert into `" + LPXRemastered.Instance.Configuration.Instance.DatabaseTableGroup + "` (`freeitem`) values('" + id + "')";
                else
                {
                    for (int i = 0; i < ItemID.Length; i++ )
                    {
                        Items += (" " + ItemID[i]); 
                    }
                    Items = Items.Trim();
                    command.CommandText = "insert into `" + LPXRemastered.Instance.Configuration.Instance.DatabaseTableGroup + "` (`freeitem`) values('" + Items + "')";
                }
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
                added = true;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                added = false;
            }
            return added;
        }
        public bool AddPermissionIntoGroup(string permi, string group)
        {
            bool results;
            try
            {
                string permissions = "";
                string[] permission = { };
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "select `permission` from `" + LPXRemastered.Instance.Configuration.Instance.DatabaseTableGroup + "` WHERE `name` = '" + group + "'";
                connection.Open();
                object result = command.ExecuteScalar();
                connection.Close();
                if (result != null)
                {
                    permissions = result.ToString();
                    permission = permissions.Split(' ');
                }
                else
                    return false;
                if(permi.Contains("color."))
                {
                    string newpermissions = "";
                    for (int i = 0; i < (permission.Length); i++)
                    {
                        if (newpermissions.Length > 0 && !(permission[i].Contains("color.")))
                            newpermissions += " " + permission[i];
                        else if (!(permission[i].Contains("color.")))
                            newpermissions += permission[i];
                        if (permission[i].Contains("color."))
                        {                           
                            if (i == 0)
                                newpermissions += permi;
                            else
                                newpermissions += " " + permi;
                        }
                        else if (newpermissions.Length < 1)
                            newpermissions = permi;
                        else if(!(permissions.Contains("color.")))
                            newpermissions += " " + permi;                        
                    }
                    command.CommandText = "update `" + LPXRemastered.Instance.Configuration.Instance.DatabaseTableGroup + "` set `permission` = '" + newpermissions + "' where `name` = '" + group + "'";
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                    results = true;
                    return results;
                }
                if (permissions.Length < 1)
                {
                    command.CommandText = "update `" + LPXRemastered.Instance.Configuration.Instance.DatabaseTableGroup + "` set `permission` = '" + permi + "' where `name` = '" + group + "'";
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                else
                {
                    permissions += " " + permi;
                    command.CommandText = "update `" + LPXRemastered.Instance.Configuration.Instance.DatabaseTableGroup + "` set `permission` = '" + permissions + "' where `name` = '" + group + "'";
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                results = true;
            }
            catch (Exception ex)
            {
                results = false;
                Logger.LogException(ex);
            }
            return results;
        }
        public bool RemovePermissionFromGroup(string group, string permi)
        {
            bool results;
            try
            {
                string permissions = "";
                string[] permission = GetPermission2(group);
                for (int i = 0; i < permission.Length; i++)
                {
                    if (permission[i] != permi)
                        permissions += permission[i] + " ";
                }
                permissions = permissions.Trim();
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "update `" + LPXRemastered.Instance.Configuration.Instance.DatabaseTableGroup + "` set `permission` = '" + permissions + "' where `name` = '" + group + "'";
                connection.Open();
                object result = command.ExecuteScalar();
                connection.Close();
                results = true;
            }
            catch (Exception ex)
            {
                results = false;
                Logger.LogException(ex);
            }
            return results;
        }
        public bool CheckGroup(string group)
        {
            bool contain = false;
            string exist= "";
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "select `name` from `" + LPXRemastered.Instance.Configuration.Instance.DatabaseTableGroup + "` where `name` = '" + group + "'";
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null) exist = result.ToString();
                connection.Close();
                if (exist == "")
                    contain = false;
                else
                    contain = true;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return contain;
        }
        public void HoursOnline(string id, DateTime logouttime)
        {
            DateTime loginTime = DateTime.Now;
            decimal HoursOnline = 0M, newHoursOnline;
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "select `lastlogin` from `" + LPXRemastered.Instance.Configuration.Instance.DatabaseTableName + "` where `steamId` = '" + id + "'";
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null) loginTime = (DateTime)result;
                connection.Close();
                newHoursOnline = (decimal)(logouttime - loginTime).TotalHours;
                command.CommandText = "select `hours` from `" + LPXRemastered.Instance.Configuration.Instance.DatabaseTableName + "` where `steamId` = '" + id + "'";
                connection.Open();
                result = command.ExecuteScalar();
                if (result != null) HoursOnline = int.Parse(result.ToString());
                connection.Close();
                newHoursOnline += HoursOnline;
                command.CommandText = "update `" + LPXRemastered.Instance.Configuration.Instance.DatabaseTableName + "` set `hours` = " + newHoursOnline + " where `steamId` = '" + id + "'";
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
        public bool AddParentGroup(string group, string parentgroup)
        {
            bool added;
            string exist;
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "select `parentgroup` from `" + LPXRemastered.Instance.Configuration.Instance.DatabaseTableGroup + "` where `name` = '" + group + "'";
                connection.Open();
                object result = command.ExecuteScalar();
                connection.Close();
                if (result == null)
                {
                    connection.Open();
                    command.CommandText = "update `" + LPXRemastered.Instance.Configuration.Instance.DatabaseTableGroup + "` set `parentgroup` = '" + parentgroup + "' where `name` = '" + group + "'";
                    command.ExecuteScalar();
                    connection.Close();                   
                }
                else
                {
                    exist = result.ToString().Trim();
                    string combine = exist + " " + parentgroup.Trim();
                    connection.Open();
                    command.CommandText = "update `" + LPXRemastered.Instance.Configuration.Instance.DatabaseTableGroup + "` set `parentgroup` = '" + combine + "' where `name` = '" + group + "'";
                    command.ExecuteScalar();
                    connection.Close();
                }
                added = true;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return false;
            }
            return added;
        }
        public string CheckUserGroup(string id)
        {
            string group = "";
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "select `group` from `" + LPXRemastered.Instance.Configuration.Instance.DatabaseTableName + "` where `steamId` = '" + id + "'";
                connection.Open();
                object result = command.ExecuteScalar();
                connection.Close();
                if (result != null) group = result.ToString();
                else return "default";              
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return group;
        }
        
        public string CheckUserGroupByID(string GroupID)
        {
            string group = "";
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "select `group` from `" + LPXRemastered.Instance.Configuration.Instance.DatabaseTableGroup + "` where `GroupId` = '" + GroupID + "'";
                connection.Open();
                object result = command.ExecuteScalar();
                connection.Close();
                if (result != null) group = result.ToString();
                else return "default";              
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return group;
        }
        public DataTable GetGroup()
        {
            DataTable dt = new DataTable();
            try
            {               
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "select `name` from `" + LPXRemastered.Instance.Configuration.Instance.DatabaseTableGroup + "`";
                connection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);             
                adapter.Fill(dt);
                connection.Close();               
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return dt;
        }
        public decimal[] GetAllGroupIncome()
        {
            DataTable dt = new DataTable();
            decimal[] stringArr = new decimal[50];
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "select `income` from `" + LPXRemastered.Instance.Configuration.Instance.DatabaseTableGroup + "`";
                connection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                adapter.Fill(dt);
                connection.Close();
                stringArr = new decimal[dt.Rows.Count];
                for (int i = 0; i < (dt.Rows.Count); i++)
                {
                    stringArr[i] += decimal.Parse(dt.Rows[i].ItemArray[0].ToString());
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return stringArr;
        }
        public string[] GetFreeItem(string id)
        {
            string group = CheckUserGroup(id);
            if (group == "")
                group = "default";
            string exist="";
            string[] stringArr = {};
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "select `freeitems` from `" + LPXRemastered.Instance.Configuration.Instance.DatabaseTableGroup + "` where name = '" + group + "'";
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null) exist = result.ToString();
                stringArr = exist.Split(' ');
                connection.Close();              
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return stringArr;
        }
        public string[] GetGroupFreeItem(string group)
        {
            string exist = "";
            string[] stringArr = new string[50];
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "select `freeitems` from `" + LPXRemastered.Instance.Configuration.Instance.DatabaseTableGroup + "` where `name` = '" + group + "'";
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null) exist = result.ToString();
                stringArr = exist.Split(' ');
                connection.Close();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return stringArr;
        }
        public bool SetGroupIncome(string group, string income)
        {
            decimal pay = decimal.Parse(income);
            bool added;
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "update `" + LPXRemastered.Instance.Configuration.Instance.DatabaseTableGroup + "` set  `income` = '" + pay + "' where `name` = '" + group +"'";
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
                added = true;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                added = false;
            }
            return added;
        }
        public void LastLogin(string id)
        {
            MySqlConnection connection = CreateConnection();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = command.CommandText = "update `" + LPXRemastered.Instance.Configuration.Instance.DatabaseTableName + "` set `lastlogin` = now() where `steamId` = '" + id + "'";
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
        public void AutoRemove()
        {
            DataTable dt = new DataTable();
            string Date;
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "select `lastlogin`,`steamId` from `" + LPXRemastered.Instance.Configuration.Instance.DatabaseTableName + "`";
                connection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                adapter.Fill(dt);
                connection.Close();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Date = dt.Rows[i].ItemArray[0].ToString();
                    Date = Date.Trim();
                    Date.Replace("-", "/");
                    if ((DateTime.Now - (Convert.ToDateTime(Date))).Days > LPXRemastered.Instance.Configuration.Instance.AutoRemoveDays)
                    {     
                        string[] Permission = GetPermission(dt.Rows[i].ItemArray[1].ToString());
                        bool immunity = false;
                        for (int x = 0; x < Permission.Length; x ++ )
                        {
                            if (Permission[x].ToLower() == "lpx.autoremoveimmunity" || Permission[x].ToLower() == "lpx.autori")
                                immunity = true;
                        }
                        if(!immunity)
                            RemoveUser(dt.Rows[i].ItemArray[1].ToString());
                    }              
                }            
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
        public bool CheckEnablePromotion(string id)
        {
            bool enable = false;
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "select `updateenable` from `" + LPXRemastered.Instance.Configuration.Instance.DatabaseTableGroup + "` where `name` = '" + CheckUserGroup(id) + "'";
                connection.Open();
                object result = command.ExecuteScalar();
                connection.Close();
                if (result != null) bool.TryParse(result.ToString(), out enable);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return enable;
        }
        public decimal GetTotalOnlineHours(string id)
        {
            decimal hours = 0m;
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "select `hours` from `" + LPXRemastered.Instance.Configuration.Instance.DatabaseTableName + "` where `steamId` = '" + id + "'";
                connection.Open();
                object result = command.ExecuteScalar();
                connection.Close();
                if (result != null)
                {
                    decimal.TryParse(result.ToString(), out hours);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return hours;
        }
        public decimal GetUpdateTime(string id)
        {
            decimal hours = 0m;
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "select `updatetime` from `" + LPXRemastered.Instance.Configuration.Instance.DatabaseTableGroup + "` where `name` = '" + CheckUserGroup(id) + "'";
                connection.Open();
                object result = command.ExecuteScalar();
                connection.Close();
                if (result != null)
                {
                    decimal.TryParse(result.ToString(), out hours);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return hours;
        }
        public string GetUpdateGroup(string id)
        {
            string group = "";
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "select `updategroup` from `" + LPXRemastered.Instance.Configuration.Instance.DatabaseTableGroup + "` where `name` = '" + CheckUserGroup(id) + "'";
                connection.Open();
                object result = command.ExecuteScalar();
                connection.Close();
                if (result != null)
                {
                    group = result.ToString();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return group;
        }
        public bool SetUpdateGroup(string group, string updategroup)
        {
            bool added = false;
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "update `" + LPXRemastered.Instance.Configuration.Instance.DatabaseTableGroup + "` set `updategroup` = '" + updategroup + "' where `name` = '" + group + "'";
                connection.Open();
                object result = command.ExecuteScalar();
                connection.Close();
                added = true;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return added;
        }
        public bool SetUpdateTime(string group, int Time)
        {
            bool added = false;
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "update `" + LPXRemastered.Instance.Configuration.Instance.DatabaseTableGroup + "` set `updatetime` = " + Time + " where `name` = '" + group + "'";
                connection.Open();
                object result = command.ExecuteScalar();
                connection.Close();
                added = true;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return added;
        }
        public bool SetEnableUpdate(string group, bool enable)
        {
            bool added = false;
            try
            {
                int enabled = 0;
                if (enable)
                    enabled = 1;
                else
                    enabled = 0;
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "update `" + LPXRemastered.Instance.Configuration.Instance.DatabaseTableGroup + "` set `updateenable` = " + enabled + " where `name` = '" + group + "'";
                connection.Open();
                command.ExecuteScalar();
                connection.Close();
                added = true;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return added;
        }
        public bool CheckIfUserInAnyGroup(string id)
        {
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "select `steamId` from `" + LPXRemastered.Instance.Configuration.Instance.DatabaseTableName + "` WHERE `steamId` = '" + id + "'";
                connection.Open();
                object result = command.ExecuteScalar();
                connection.Close();
                if (result == null) return false;
                else return true;
            }
            catch (Exception ex)
            {              
                Logger.LogException(ex);
                return false;
            }

        }
        public string GetColor(string group)
        {
            string[] permission = { };
            string Color = "white";
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "select `permission` from `" + LPXRemastered.Instance.Configuration.Instance.DatabaseTableGroup + "` where `name` = '" + group + "'";
                connection.Open();
                object obj = command.ExecuteScalar();
                if (obj != null)
                {
                    permission = (obj.ToString()).Split(' ');
                }
                connection.Close();
                for(int x = 0; x < permission.Length; x++)
                {
                    if (permission[x].StartsWith("color."))
                    {
                        Color = "";
                        char[] buff = permission[x].ToCharArray();
                        for(int y = permission[x].IndexOf(".") + 1; y < permission[x].Length; y ++)
                        {
                            Color += buff[y];
                        }
                        Color = Color.Trim();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return Color;
        }
        public uint Cooldown(string group)
        {
            uint cooldown = 0;
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "select `cooldown` from `" + LPXRemastered.Instance.Configuration.Instance.DatabaseTableGroup + "` where `name` = '" + group + "'";
                command.Parameters.AddWithValue("name", group);
                connection.Open();
                object obj = command.ExecuteScalar();
                if (uint.TryParse(obj.ToString(), out cooldown) == false)
                    cooldown = 0;
                connection.Close();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return cooldown;
        }
        public void SetSteamName(string id, string steamName)
        {
            char[] buffer;
            try
            {
                while (steamName.IndexOf("'") >= 0)
                {
                    buffer = steamName.ToCharArray();
                    buffer[steamName.IndexOf("'")] = ' ';
                    steamName = "";
                    for(int j = 0; j < buffer.Length; j++)
                    {
                        steamName += buffer[j];
                    }
                }
                while(steamName.IndexOf("`") >= 0)
                {
                    buffer = steamName.ToCharArray();
                    buffer[steamName.IndexOf("`")] = ' ';
                    steamName = "";
                    for (int j = 0; j < buffer.Length; j++)
                    {
                        steamName += buffer[j];
                    }
                }
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "update `" + LPXRemastered.Instance.Configuration.Instance.DatabaseTableName + "` set `SteamName` = '" + steamName + "' where `steamId` = '" + id + "'";
                connection.Open();
                command.ExecuteScalar();
                connection.Close();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
        public bool CheckKit(string name)
        {
            bool exist = false;
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "select `name` from `" + LPXRemastered.Instance.Configuration.Instance.DatabaseKit + "` where `name` = '" + name + "'";
                command.Parameters.AddWithValue("name", name);
                connection.Open();
                object obj = command.ExecuteScalar();
                if (obj == null)
                    exist = false;
                else
                    exist = true;
                connection.Close();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return exist;
        }
        public double GetKitCooldown(string name)
        {
            double cooldown = 1.00;
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "select `cooldown` from `" + LPXRemastered.Instance.Configuration.Instance.DatabaseKit + "` where `name` = '" + name + "'";
                connection.Open();
                object obj = command.ExecuteScalar();
                if (obj != null)
                    double.TryParse(obj.ToString(),out cooldown);
                connection.Close();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return cooldown;
        }
        public string[] GetKitItems(string name)
        {
            string[] ItemIDNAmt = {};
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "select `itemId/Amt` from `" + LPXRemastered.Instance.Configuration.Instance.DatabaseKit + "` where `name` = '" + name + "'";
                connection.Open();
                object obj = command.ExecuteScalar();
                if (obj != null)
                    ItemIDNAmt = obj.ToString().Split(' ');
                connection.Close();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return ItemIDNAmt;
        }
        public string[] GetAllKitName()
        {
            DataTable dt = new DataTable();
            string[] AllKitName = {};
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "select `name` from `" + LPXRemastered.Instance.Configuration.Instance.DatabaseKit + "`";
                connection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                adapter.Fill(dt);
                connection.Close();
                AllKitName = new string[dt.Rows.Count];
                for (int i = 0; i < (dt.Rows.Count); i++)
                {
                    AllKitName[i] = dt.Rows[i].ItemArray[0].ToString();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            
            return AllKitName;
        }
        public string[] GetPlayerKitName(string group)
        {
            string[] PlayerKitName = {};
            string[] Permission;
            int Count = 0;
            Permission = GetGroupPermission(group);
            try
            {
                for (int i = 0; i < Permission.Length; i++)
                {
                    if (Permission[i].Contains("kit."))
                        Count++;
                }
                PlayerKitName = new string[Count];
                Count = 0;
                for (int i = 0; i < Permission.Length; i++)
                {
                    if (Permission[i].Contains("kit."))
                    {
                        PlayerKitName[Count] = Permission[i].Split('.')[1];
                        Count++;                       
                    }
                }
               
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return PlayerKitName;
        }
        public bool SetKitsCooldown(string name, double cooldown)
        {
            bool added = false;
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "update `" + LPXRemastered.Instance.Configuration.Instance.DatabaseKit + "` set `cooldown` = " + cooldown + " where `name` = '" + name + "'";
                connection.Open();
                command.ExecuteScalar();
                connection.Close();
                added = true;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return added;
        }
        public bool AddKit(string name,string itemid, double cooldown)
        {
            bool added = false;
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "insert into `" + LPXRemastered.Instance.Configuration.Instance.DatabaseKit + "` (`name`,`itemID/Amt`,`cooldown`) values('" + name + "', '" + itemid + "', " + cooldown + ")";
                connection.Open();
                command.ExecuteScalar();
                connection.Close();
                added = true;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return added;
        }
        public bool RemoveKit(string name)
        {
            bool removed = false;
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "Delete from `" + LPXRemastered.Instance.Configuration.Instance.DatabaseKit + "` where `name` = '" + name + "'";
                connection.Open();
                command.ExecuteScalar();
                connection.Close();
                removed = true;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return removed;
        }       
        internal void CheckSchema()
        {
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                connection.Open();
                object test;
                if (LPXRemastered.Instance.Configuration.Instance.LPXEnabled)
                {
                    command.CommandText = "show tables like '" + LPXRemastered.Instance.Configuration.Instance.DatabaseTableName + "'";                    
                    test = command.ExecuteScalar();
                    if (test == null)
                    {
                        command.CommandText = "CREATE TABLE `" + LPXRemastered.Instance.Configuration.Instance.DatabaseTableName + "` (`steamId` varchar(32) NOT NULL,`SteamName` varchar(46),`group` varchar(32),`lastlogin` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00' ON UPDATE CURRENT_TIMESTAMP,`hours` decimal(10) NOT NULL DEFAULT 0, `permission` varchar(100), PRIMARY KEY (`steamId`)) ";
                        command.ExecuteNonQuery();
                    }
                    else
                    {
                        command.CommandText = "SHOW COLUMNS FROM `" + LPXRemastered.Instance.Configuration.Instance.DatabaseTableName + "` LIKE 'SteamName'";
                        test = command.ExecuteScalar();
                        if (test == null)
                        {
                            command.CommandText = "ALTER TABLE `" + LPXRemastered.Instance.Configuration.Instance.DatabaseTableName + "` ADD `SteamName` VARCHAR(46) AFTER `steamId`";
                            command.ExecuteNonQuery();
                        }
                        command.CommandText = "SHOW COLUMNS FROM `" + LPXRemastered.Instance.Configuration.Instance.DatabaseTableName + "` LIKE 'permission'";
                        test = command.ExecuteScalar();
                        if (test == null)
                        {
                            command.CommandText = "ALTER TABLE `" + LPXRemastered.Instance.Configuration.Instance.DatabaseTableName + "` ADD `permission` VARCHAR(100) AFTER `hours`";
                            command.ExecuteNonQuery();
                        }
                    }

                    command.CommandText = "show tables like '" + LPXRemastered.Instance.Configuration.Instance.DatabaseTableGroup + "'";
                    test = command.ExecuteScalar();

                    if (test == null)
                    {
                        command.CommandText = "CREATE TABLE `" + LPXRemastered.Instance.Configuration.Instance.DatabaseTableGroup + "` (`groupID` int(6) NOT NULL,`name` varchar(32) NOT NULL,`permission` varchar(668),`income` decimal(10) NOT NULL DEFAULT 0.00,`freeitems` varchar(100),`parentgroup` varchar(32),`updategroup` varchar(32),`updatetime` decimal(10) NOT NULL DEFAULT 15.00,`updateenable` bool,`cooldown` varchar(255),PRIMARY KEY (`name`)) ";
                        command.ExecuteNonQuery();
                        command.CommandText = "insert into `" + LPXRemastered.Instance.Configuration.Instance.DatabaseTableGroup + "` (`groupID`,`name`,`income`,`updatetime`,`updateenable`) values(1,'default', 10,7,0)";
                        command.ExecuteNonQuery();
                        command.CommandText = "insert into `" + LPXRemastered.Instance.Configuration.Instance.DatabaseTableGroup + "` (`groupID`,`name`,`income`,`updatetime`,`updateenable`) values(2,'admin', 60,7,0)";
                        command.ExecuteNonQuery();
                    }
                    else
                    {
                        command.CommandText = "SHOW COLUMNS FROM `" + LPXRemastered.Instance.Configuration.Instance.DatabaseTableGroup + "` LIKE 'cooldown'";
                        test = command.ExecuteScalar();
                        if (test == null)
                        {
                            command.CommandText = "ALTER TABLE `" + LPXRemastered.Instance.Configuration.Instance.DatabaseTableGroup + "` ADD `cooldown` VARCHAR(255) AFTER `permission`";
                            command.ExecuteNonQuery();
                        }
                        command.CommandText = "SHOW COLUMNS FROM `" + LPXRemastered.Instance.Configuration.Instance.DatabaseTableGroup + "` LIKE 'freeitem'";
                        test = command.ExecuteScalar();
                        if (test != null)
                        {
                            command.CommandText = "ALTER TABLE `" + LPXRemastered.Instance.Configuration.Instance.DatabaseTableGroup + "` DROP COLUMN `freeitem`";
                            command.ExecuteNonQuery();
                        }
                        command.CommandText = "SHOW COLUMNS FROM `" + LPXRemastered.Instance.Configuration.Instance.DatabaseTableGroup + "` LIKE 'freeitems'";
                        test = command.ExecuteScalar();
                        if (test == null)
                        {
                            command.CommandText = "ALTER TABLE `" + LPXRemastered.Instance.Configuration.Instance.DatabaseTableGroup + "` ADD `freeitems` VARCHAR(100) AFTER `income`";
                            command.ExecuteNonQuery();
                        }
                        command.CommandText = "SHOW COLUMNS FROM `" + LPXRemastered.Instance.Configuration.Instance.DatabaseTableGroup + "` LIKE 'groupID'";
                        test = command.ExecuteScalar();
                        if (test == null)
                        {
                            command.CommandText = "ALTER TABLE `" + LPXRemastered.Instance.Configuration.Instance.DatabaseTableGroup + "` ADD `groupID` int(6) BEFORE `name`";
                            command.ExecuteNonQuery();
                        }
                    }
                }
                if (LPXRemastered.Instance.Configuration.Instance.KitsEnabled)
                {
                    command.CommandText = "show tables like '" + LPXRemastered.Instance.Configuration.Instance.DatabaseKit + "'";
                    test = command.ExecuteScalar();
                    if (test == null)
                    {
                        command.CommandText = "CREATE TABLE `" + LPXRemastered.Instance.Configuration.Instance.DatabaseKit + "` (`name` varchar(32) NOT NULL,`itemID/Amt` varchar(255),`cooldown` decimal(10) NOT NULL DEFAULT 10.00, PRIMARY KEY (`name`)) ";
                        command.ExecuteNonQuery();
                        command.CommandText = "insert into `" + LPXRemastered.Instance.Configuration.Instance.DatabaseKit + "` (`name`,`itemID/Amt`,`cooldown`) values('survival', '245/1 81/2 16/1', 60.00)";
                        command.ExecuteNonQuery();
                        command.CommandText = "insert into `" + LPXRemastered.Instance.Configuration.Instance.DatabaseKit + "` (`name`,`itemID/Amt`,`cooldown`) values('watcher', '109/1 111/3 236/1',60.00)";
                        command.ExecuteNonQuery();
                        command.CommandText = "insert into `" + LPXRemastered.Instance.Configuration.Instance.DatabaseKit + "` (`name`,`itemID/Amt`,`cooldown`) values('brute force', '112/1 113/3 254/1',60.00)";
                        command.ExecuteNonQuery();
                    }
                }
                command.CommandText = "ALTER TABLE `" + LPXRemastered.Instance.Configuration.Instance.DatabaseItemShop + "` CHANGE COLUMN `ItemName` `ItemName` VARCHAR(56) NOT NULL";
                command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
    }
}

