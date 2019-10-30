using MySql.Data.MySqlClient;
using Rocket.Core.Logging;
using System;
using System.Data;

namespace LPXRemastered
{
    public class DatabaseManagerAuction
    {
        // The base code for this class comes from Uconomy itself.
        internal DatabaseManagerAuction()
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
        public bool AddAuctionItem(int id, string itemid, string itemname, decimal price, decimal shopprice, int quality, string sellerID)
        {
            bool added = false;
            try
            {
                MySqlConnection Connection = CreateConnection();
                MySqlCommand Command = Connection.CreateCommand();
                Command.CommandText = "Insert into `" + LPXRemastered.Instance.Configuration.Instance.DatabaseAuction + "` (`id`,`itemid`,`ItemName`,`Price`,`ShopPrice`,`Quality`,`SellerID`) Values('" + id.ToString() + "', '" + itemid + "', '" + itemname + "', '" + price + "', '" + shopprice + "', '" + quality + "', '"+ sellerID + "')";
                Connection.Open();
                Command.ExecuteNonQuery();
                Connection.Close();
                added = true;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return added;
        }
        public bool CheckAuctionExist(int id)
        {
            bool exist = false;
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "select `id` from `" + LPXRemastered.Instance.Configuration.Instance.DatabaseAuction + "` where `id` = "+ id +"";
                connection.Open();
                object obj = command.ExecuteScalar();
                connection.Close();
                if (obj != null)
                    exist = true;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return exist;
        }
        public int GetLastAuctionNo()
        {
            DataTable dt = new DataTable();
            int AuctionNo = 0;
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "select `id` from `" + LPXRemastered.Instance.Configuration.Instance.DatabaseAuction + "`";
                connection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                adapter.Fill(dt);
                connection.Close();
                for (int i = 0; i < (dt.Rows.Count); i++)
                {
                    if(dt.Rows[i].ItemArray[0].ToString() != i.ToString()) 
                    {
                        AuctionNo = i;
                        return AuctionNo;
                    }
                }
                AuctionNo = dt.Rows.Count;

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return AuctionNo;
        }
        public string[] GetAllItemNameWithQuality()
        {
            DataTable dt = new DataTable();
            DataTable quality = new DataTable();
            string[] ItemName = {};
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "select `ItemName` from `" + LPXRemastered.Instance.Configuration.Instance.DatabaseAuction + "`";
                connection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                adapter.Fill(dt);
                command.CommandText = "select `Quality` from `" + LPXRemastered.Instance.Configuration.Instance.DatabaseAuction + "`";
                adapter = new MySqlDataAdapter(command);
                adapter.Fill(quality);
                connection.Close();
                ItemName = new string[dt.Rows.Count];
                for (int i = 0; i < (dt.Rows.Count); i++)
                {
                    ItemName[i] = dt.Rows[i].ItemArray[0].ToString() + "(" + quality.Rows[i].ItemArray[0].ToString() + ")";
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return ItemName;
        }       

        public string[] FindAllItemNameWithQualityByID(string ItemID)
        {
            DataTable dt = new DataTable();
            DataTable quality = new DataTable();
            string[] ItemName = {};
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "select `ItemName` from `" + LPXRemastered.Instance.Configuration.Instance.DatabaseAuction + "` where `itemid` = " + ItemID + "";
                connection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                adapter.Fill(dt);
                command.CommandText = "select `Quality` from `" + LPXRemastered.Instance.Configuration.Instance.DatabaseAuction + "` where `itemid` = " + ItemID + "";
                adapter = new MySqlDataAdapter(command);
                adapter.Fill(quality);
                connection.Close();
                ItemName = new string[dt.Rows.Count];
                for (int i = 0; i < (dt.Rows.Count); i++)
                {
                    ItemName[i] = dt.Rows[i].ItemArray[0].ToString() + "(" + quality.Rows[i].ItemArray[0].ToString() + ")";
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return ItemName;
        }
        public string[] FindAllItemNameWithQualityByItemName(string Itemname)
        {
            DataTable dt = new DataTable();
            DataTable quality = new DataTable();
            string[] ItemName = { };
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "select `ItemName` from `" + LPXRemastered.Instance.Configuration.Instance.DatabaseAuction + "` where `ItemName` = " + Itemname + "";
                connection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                adapter.Fill(dt);
                command.CommandText = "select `Quality` from `" + LPXRemastered.Instance.Configuration.Instance.DatabaseAuction + "` where `ItemName` = " + Itemname + "";
                adapter = new MySqlDataAdapter(command);
                adapter.Fill(quality);
                connection.Close();
                ItemName = new string[dt.Rows.Count];
                for (int i = 0; i < (dt.Rows.Count); i++)
                {
                    ItemName[i] = dt.Rows[i].ItemArray[0].ToString() + "(" + quality.Rows[i].ItemArray[0].ToString() + ")";
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return ItemName;
        }
        public string[] GetAllAuctionID()
        {
            DataTable dt = new DataTable();
            string[] AuctionID = { };
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "select `id` from `" + LPXRemastered.Instance.Configuration.Instance.DatabaseAuction + "`";
                connection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                adapter.Fill(dt);
                connection.Close();
                AuctionID = new string[dt.Rows.Count];
                for (int i = 0; i < (dt.Rows.Count); i++)
                {
                    AuctionID[i] = dt.Rows[i].ItemArray[0].ToString();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return AuctionID;
        }
        public string[] FindAllItemPriceByID(string ItemID)
        {
            DataTable dt = new DataTable();
            string[] ItemPrice= {};
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "select `Price` from `" + LPXRemastered.Instance.Configuration.Instance.DatabaseAuction + "` where `itemid` = " + ItemID + "";
                connection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                adapter.Fill(dt);
                connection.Close();
                ItemPrice = new string[dt.Rows.Count];
                for (int i = 0; i < (dt.Rows.Count); i++)
                {
                    ItemPrice[i] = dt.Rows[i].ItemArray[0].ToString();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return ItemPrice;
        }
        public string[] FindAllItemPriceByItemName(string ItemName)
        {
            DataTable dt = new DataTable();
            string[] ItemPrice = { };
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "select `Price` from `" + LPXRemastered.Instance.Configuration.Instance.DatabaseAuction + "` where `ItemName` = " + ItemName + "";
                connection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                adapter.Fill(dt);
                connection.Close();
                ItemPrice = new string[dt.Rows.Count];
                for (int i = 0; i < (dt.Rows.Count); i++)
                {
                    ItemPrice[i] = dt.Rows[i].ItemArray[0].ToString();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return ItemPrice;
        }
        public string[] GetAllItemPrice()
        {
            DataTable dt = new DataTable();
            string[] ItemPrice= {};
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "select `Price` from `" + LPXRemastered.Instance.Configuration.Instance.DatabaseAuction + "`";
                connection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                adapter.Fill(dt);
                connection.Close();
                ItemPrice = new string[dt.Rows.Count];
                for (int i = 0; i < (dt.Rows.Count); i++)
                {
                    ItemPrice[i] = dt.Rows[i].ItemArray[0].ToString();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return ItemPrice;
        }
        public string[] AuctionBuy(int auctionID)
        {
            DataTable dt = new DataTable();
            string[] ItemInfo = new string [5];
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "select `itemid`,`ItemName`, `Price`, `Quality`, `SellerID` from `" + LPXRemastered.Instance.Configuration.Instance.DatabaseAuction + "` where `id` = " + auctionID.ToString() + "";
                connection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                adapter.Fill(dt);
                connection.Close();
                for (int i = 0; i < (dt.Columns.Count); i++)
                {
                    ItemInfo[i] = dt.Rows[0].ItemArray[i].ToString();
                }              
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return ItemInfo; 
        }
        public string[] AuctionCancel(int auctionID)
        {
            DataTable dt = new DataTable();
            string[] ItemInfo = new string[5];
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "select `itemid`, `Quality` from `" + LPXRemastered.Instance.Configuration.Instance.DatabaseAuction + "` where `id` = " + auctionID.ToString() + "";
                connection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                adapter.Fill(dt);
                connection.Close();
                for (int i = 0; i < (dt.Columns.Count); i++)
                {
                    ItemInfo[i] = dt.Rows[0].ItemArray[i].ToString();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return ItemInfo;
        }
        public void DeleteAuction(string auctionID)
        {
            try
            {
                MySqlConnection Connection = CreateConnection();
                MySqlCommand Command = Connection.CreateCommand();
                Command.CommandText = "delete from `" + LPXRemastered.Instance.Configuration.Instance.DatabaseAuction + "` where `id` = '" + auctionID + "'";
                Connection.Open();
                object obj = Command.ExecuteNonQuery();
                Connection.Close();
            }
            catch (Exception exception)
            {
                Logger.LogException(exception);
            }
        }
        public string GetOwner(int auctionID)
        {
            string ID = "";
            try
            {
                MySqlConnection Connection = CreateConnection();
                MySqlCommand Command = Connection.CreateCommand();
                Command.CommandText = "select `SellerID` from `" + LPXRemastered.Instance.Configuration.Instance.DatabaseAuction + "` where `id` = '" + auctionID + "'";
                Connection.Open();
                object obj = Command.ExecuteScalar();
                if (obj != null)
                    ID = obj.ToString().Trim();
                Connection.Close();
            }
            catch (Exception exception)
            {
                Logger.LogException(exception);
            }
            return ID;
        }
        public string[] FindItemByID(string ItemID)
        {
            DataTable dt = new DataTable();
            string[] AuctionID = { };
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "select `id` from `" + LPXRemastered.Instance.Configuration.Instance.DatabaseAuction + "` where `itemid` = '" + ItemID + "'";
                connection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                adapter.Fill(dt);
                connection.Close();
                AuctionID = new string[dt.Rows.Count];
                for (int i = 0; i < (dt.Rows.Count); i++)
                {
                    AuctionID[i] = dt.Rows[i].ItemArray[0].ToString();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return AuctionID;
        }
        public string[] FindItemByName(string ItemName)
        {
            DataTable dt = new DataTable();
            string[] AuctionID = { };
            try
            {
                MySqlConnection connection = CreateConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "select `id` from `" + LPXRemastered.Instance.Configuration.Instance.DatabaseAuction + "` where `ItemName` = '" + ItemName + "'";
                connection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                adapter.Fill(dt);
                connection.Close();
                AuctionID = new string[dt.Rows.Count];
                for (int i = 0; i < (dt.Rows.Count); i++)
                {
                    AuctionID[i] = dt.Rows[i].ItemArray[0].ToString();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return AuctionID;
        }
        internal void CheckSchema()
        {
            try
            {
                MySqlConnection Connection = CreateConnection();
                MySqlCommand Command = Connection.CreateCommand();
                Connection.Open();
                object test;
                if (LPXRemastered.Instance.Configuration.Instance.AllowAuction)
                {
                    Command.CommandText = "show tables like '" + LPXRemastered.Instance.Configuration.Instance.DatabaseAuction + "'";
                    test = Command.ExecuteScalar();
                    if (test == null)
                    {
                        Command.CommandText = "CREATE TABLE `" + LPXRemastered.Instance.Configuration.Instance.DatabaseAuction + "` (`id` int(6) NOT NULL,`itemid` int(7) NOT NULL,`ItemName` varchar(56) NOT NULL,`Price` decimal(15,2) NOT NULL DEFAULT '20.00',`ShopPrice` decimal(15,2) NOT NULL DEFAULT '0.00', `Quality` int(3) NOT NULL DEFAULT '50', `SellerID` varchar(20) NOT NULL, PRIMARY KEY (`id`))";
                        Command.ExecuteNonQuery();
                    }
                }
                Connection.Close();
            }
            catch (Exception exception)
            {
                Logger.LogException(exception);
            }
        }
    }
}